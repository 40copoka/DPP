﻿using MilSpace.AddDem.ReliefProcessing.Exceptions;
using MilSpace.AddDem.ReliefProcessing.GuiData;
using MilSpace.Configurations;
using MilSpace.Core;
using MilSpace.Core.Actions;
using MilSpace.Core.DataAccess;
using MilSpace.DataAccess.DataTransfer.Sentinel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MilSpace.AddDem.ReliefProcessing
{
    public partial class PrepareDem : Form, IPrepareDemViewSrtm, IPrepareDemViewSentinel,
        IPrepareDemViewSentinelPeocess, IPrepareDemViewGenerateTile
    {
        Logger log = Logger.GetLoggerEx("MilSpace.AddDem.ReliefProcessing.PrepareDem");
        PrepareDemControllerSrtm controllerSrtm = new PrepareDemControllerSrtm();
        PrepareDemControllerSentinel controllerSentinel = new PrepareDemControllerSentinel();
        PrepareDemControllerSentinelProcess controllerSentinelProcess = new PrepareDemControllerSentinelProcess();
        PrepareDemContrellerGenerateTile controllerGenerateTile = new PrepareDemContrellerGenerateTile();
        bool staredtFormArcMap;
        private IEnumerable<SentinelProduct> sentinelProducts = null;

        public PrepareDem(bool startFormArcMap = true)
        {
            var mode = startFormArcMap ? "ArcGis" : "Outside";
            log.InfoEx($"Starting Dem module in {mode} mode");
            staredtFormArcMap = startFormArcMap;
            controllerSrtm.SetView(this);
            controllerSentinel.SetView(this);
            controllerGenerateTile.SetView(this);
            controllerSentinelProcess.SetView(this);
            controllerSentinelProcess.OnProcessing += OnProcessing;
            controllerSentinelProcess.OnErrorProcessing += OnErrorProcessing;

            controllerSentinel.OnProductsDownloaded += OnProductsDownloaded;

            InitializeComponent();
            if (startFormArcMap)
            {
                tabControlTop.Controls.Remove(tabLoadTop);
                tabControlTop.Controls.Remove(tabPreprocessTop);
            }
            else
            {
                tabControlTop.Controls.Remove(srtmTabTop);
                tabControlTop.Controls.Remove(tabGenerateTileTop);
            }
            InitializeData();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if ((controllerSentinel.DownloadStarted || controllerGenerateTile.Processing) &&
                MessageBox.Show(
                    "Заватаження або обробка триває./n Дійсно бажаєте закрити вікно додатку?",
                    "Спостереження. Інформаційне повідомлення",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void OnProductsDownloaded(IEnumerable<SentinelProduct> products)
        {
            MessageBox.Show(
                "Обрані сцени S-1 завантажені",
                "Спостереження. Інформаційне повідомлення",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );

            var demPrepare = new DataAccess.Facade.DemPreparationFacade();
            var productRecords = products.ToList().Select(p => demPrepare.AddOrUpdateSentinelProduct(p));

            if (productRecords.Any(p => p == null))
            {
                log.ErrorEx("There was an error on adding Santinel product");
            }

            ShowButtons();
        }

        private void InitializeData()
        {
            log.InfoEx($"> InitializeData START");

            log.InfoEx($"InitializeData. controllerSrtm.ReadConfiguration()");
            controllerSrtm.ReadConfiguration();

            log.InfoEx($"InitializeData. controllerSentinel.ReadConfiguration()");
            controllerSentinel.ReadConfiguration();

            log.InfoEx($"InitializeData. Setting event lstSentilenProducts.DataSourceChanged");
            lstSentilenProducts.DataSourceChanged += LstSentilenProducts_DataSourceChanged;
            lstSentilenProducts.DisplayMember = "Identifier";

            log.InfoEx($"InitializeData. Setting event controllerSentinel.OnProductLoaded");
            controllerSentinel.OnProductLoaded += OnSentinelProductLoaded;

            log.InfoEx($"InitializeData. lstSentinelProductsToDownload.Items.Clear()");
            lstSentinelProductsToDownload.Items.Clear();

            log.InfoEx($"InitializeData. Setting toolStripLabelDB.Text:{0}", MilSpaceConfiguration.DemStorages.SentinelStorageDBExternal);
            toolStripLabelDB.Text = MilSpaceConfiguration.DemStorages.SentinelStorageDBExternal;

            ShowButtons();
            log.InfoEx($"> InitializeData END");
        }

        private void LstSentilenProducts_DataSourceChanged(object sender, EventArgs e)
        {
            var curSentineltile = SelectedTile;

            if (SelectedTile.DownloadingScenes != null)
            {
                foreach (var sg in SelectedTile.DownloadingScenes)
                {
                    AddSceneToDownload(sg);
                }
            }
        }

        private void OnSentinelProductLoaded(IEnumerable<SentinelProduct> products)
        {
            var selectedIndx = lstSentilenProducts.SelectedIndex;
            lstSentinelProductProps.Items.Clear();
            lstSentinelProductsToDownload.Items.Clear();

            var ttt = products.ToArray();
            lstSentilenProducts.DataSource = ttt;
            lstSentilenProducts.DisplayMember = "Identifier";
            lstSentilenProducts.Update();
            lstSentilenProducts.Refresh();

            if (products != null && products.Any())
            {
                if (products.Count() < selectedIndx)
                {
                    selectedIndx = products.Count() - 1;
                }
                lstSentilenProducts.SelectedItem = products.First();
            }

            lstSentilenProducts_SelectedIndexChanged(lstSentilenProducts, null);

            ShowButtons();
        }

        private void FillTileSource(ListBox tileList, IEnumerable<string> tiles)
        {

            tileList.Items.Clear();

            tiles?.ToList().ForEach(t => tileList.Items.Add(t));
            if (tileList.Items.Count > 0)
            {
                tileList.SelectedIndex = 0;
            }
            ShowButtons();
        }

        private void LstSrtmFiles_DataSourceChanged(object sender, EventArgs e)
        {

        }

        #region IPrepareDemViewSentinel
        public string SentinelSrtorage { get => lblSentinelStorage.Text; set => lblSentinelStorage.Text = value; }
        public string SentinelSrtorageExternal { get => lblCurrentSantinelDb.Text; set => lblCurrentSantinelDb.Text = value; }
        public IEnumerable<SentinelTile> TilesToImport { get => controllerSentinel.TilesToImport; }

        public SentinelTile SelectedTile => controllerSentinel.GetTileByName(lstTiles.SelectedItem?.ToString());
        #endregion
        public string TileLatitude { get => txtLatitude.Text; }
        public string TileLongitude { get => txtLongitude.Text; }


        public string TileLatitudeSrtm { get => txtLatitudeSrtm.Text; }

        public string TileLongitudeSrtm { get => txtLongitudeSrtm.Text; }

        public IEnumerable<SentinelProduct> SentinelProductsToDownload { get => sentinelProducts; set => sentinelProducts = value; }

        public DateTime SentinelRequestDate { get => dtSentinelProductes.Value; }

        #region IPrepareDemViewSrtm

        public string SrtmSrtorage { get => lblSrtmStorage.Text; set => lblSrtmStorage.Text = value; }

        public string SrtmSrtorageExternal { get => lblSrtmStorageExternal.Text; set => lblSrtmStorageExternal.Text = value; }
        public IEnumerable<FileInfo> SrtmFilesInfo { get; set; } = new List<FileInfo>();
        public IEnumerable<Tile> DownloadedTiles => controllerSentinelProcess.GetTilesFromDownloaded();
        #endregion
        public SentinelPairCoherence SentinelPairDem { get; set; }

        public SentinelProduct SelectedProductDem
        {
            get
            {
                if (lstSentinelProductsToProcess.SelectedItem != null)
                {
                    string productName = lstSentinelProductsToProcess.SelectedItem.ToString();
                    return controllerSentinelProcess.GetSentinelProductByIdentifier(productName);
                }
                return null;
            }
        }


        public IEnumerable<SentinelProduct> SentinelProductsFromDatabase => controllerSentinel.GetAllSentinelProduct();

        public IEnumerable<Tile> TilesToProcess => throw new NotImplementedException();


        #region IPrepareDemViewGenerateTile
        public string SentinelMetadataDb { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TileDemLatitude => txtLatitudeDem.Text;

        public string TileDemLongitude => txtLongitudeDem.Text;

        public string SelectedTileDem => lstTilesDem.SelectedItem?.ToString();

        public IEnumerable<string> QuaziTilesToGenerate =>
            listQuaziTiles.Items.Cast<string>();


        public string SelectedQuaziTile => listQuaziTiles.SelectedItems?.Cast<string>().First();
        #endregion

        private void btnImportSrtm_Click(object sender, EventArgs e)
        {
            if (controllerSrtm.CopySrtmFilesToStorage())
            {
                MessageBox.Show(
                    "Файли імпортовані успішно",
                    "Спостереження. Інформаційне повідомлення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Сталася помилка імпорту файлів /n Додаткова інформація у журналі додатку",
                    "Спостереження. Повідомлення про помилку",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSelectSrtm_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog selectFolder = new FolderBrowserDialog())
            {
                if (selectFolder.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(selectFolder.SelectedPath))
                {
                    string message = "The {0} files were found.";
                    MessageBoxIcon icon = MessageBoxIcon.Information;
                    try
                    {
                        controllerSrtm.ReadSrtmFilesFromFolder(selectFolder.SelectedPath);
                        lstTilesSrtm.Visible = true;
                        lstTilesSrtm.DataSource = SrtmFilesInfo;
                        lstTilesSrtm.Refresh();
                        lstTilesSrtm.Update();
                        message = message.InvariantFormat(SrtmFilesInfo.Count());

                        return;
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        log.ErrorEx(ex.Message);
                        message = ex.Message;
                        icon = MessageBoxIcon.Error;
                    }
                    catch (NothingToImportException ex)
                    {
                        log.ErrorEx(ex.Message);
                        message = ex.Message;
                        icon = MessageBoxIcon.Exclamation;
                    }
                    catch (Exception ex)
                    {
                        message = "Unexpected error";
                        log.ErrorEx(ex.Message);
                        icon = MessageBoxIcon.Error;
                    }
                    MessageBox.Show(
                        message,
                        "Спостереження. Повідомлення",
                        MessageBoxButtons.OK,
                        icon);
                }
            }
        }

        private bool CheckDouble(TextBox textBox, char keyChar)
        {
            return (char.IsNumber(keyChar) || keyChar == (char)Keys.Back || keyChar == (char)KeyCodesEnum.Minus
                            || (keyChar == (int)KeyCodesEnum.DecimalPoint && textBox.Text.IndexOf(".") == -1));
        }

        private void txtLongtitude_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !CheckDouble(sender as TextBox, e.KeyChar);
            if (tabControlTop.SelectedTab == tabLoadTop)
            {
                btnAddTileToList.Enabled = !e.Handled && controllerSentinel.GetTilesByPoint() != null;
            }
            else if (tabControlTop.SelectedTab == tabGenerateTileTop)
            {
                btnAddTileDem.Enabled = !e.Handled && controllerGenerateTile.GetTilesByPoint() != null;
            }
        }

        private void txtLongLatChecking(object sender, KeyPressEventArgs e)
        {
            e.Handled = !CheckDouble(sender as TextBox, e.KeyChar);
            if (tabControlTop.SelectedTab == tabLoadTop)
            {
                btnAddTileToList.Enabled = !e.Handled && controllerSentinel.GetTilesByPoint() != null;
            }
            else if (tabControlTop.SelectedTab == tabGenerateTileTop)
            {
                btnAddTileDem.Enabled = !e.Handled && controllerGenerateTile.GetTilesByPoint() != null;
            }
        }

        private void txtLatitude_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !CheckDouble(sender as TextBox, e.KeyChar);
            btnAddTileToList.Enabled = !e.Handled && controllerSentinel.GetTilesByPoint() != null;
        }

        private void btnAddTileToList_Click(object sender, EventArgs e)
        {
            controllerSentinel.AddTileForImport();
            FillTileSource(lstTiles, TilesToImport?.Select(t => t.ParentTile.Name));
        }

        private void btnGetScenes_Click(object sender, EventArgs e)
        {
            //lstSentilenProducts.Items.Clear();
            lstSentinelProductProps.Items.Clear();
            lstSentinelProductsToDownload.Items.Clear();

            controllerSentinel.GetScenes();
        }

        private void lstSentilenProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill Scene properties
            var props = controllerSentinel.GetSentinelProductProperties(lstSentilenProducts.SelectedItem as SentinelProduct);

            lstSentinelProductProps.Items.Clear();
            foreach (var prop in props)
            {
                var item = new ListViewItem(prop[0]);
                item.SubItems.Add(prop[1]);
                lstSentinelProductProps.Items.Add(item);
            }
            ShowButtons();
        }

        private void ShowButtons()
        {

            try
            {
                bool selectedProduct = controllerSentinel.CheckProductExistanceToDownload(lstSentilenProducts.SelectedItem as SentinelProduct);

                btnGetScenes.Enabled = SelectedTile != null;
                buttonDelTile.Enabled = btnGetScenes.Enabled;

                btnAddTileToList.Enabled = controllerSentinel.GetTilesByPoint() != null;

                btnAddSentinelProdToDownload.Enabled = lstSentilenProducts.SelectedItem != null && !selectedProduct;
                btnDownloadSentinelProd.Enabled = SelectedTile != null && SelectedTile.DownloadingScenes.Count() >= 2 && !controllerSentinel.DownloadStarted;

                btnChkCoherence.Enabled = lstPairDem.Items.Count == 2;
                //(SentinelPairDem != null && SentinelPairDem.Mean < 0);

                btnProcess.Enabled = SelectedProductDem != null;

                btnGenerateTile.Enabled = listQuaziTiles.CheckedItems.Count >= 0 && !controllerGenerateTile.Processing;

                //STRM 
                btnAddTileToListStrm.Enabled = controllerSrtm.GetTilesByPoint() != null;
                btnAddQTileToMapDem.Enabled = listQuaziTiles.SelectedIndex > 0;

            }
            catch (Exception ex)
            {
                log.InfoEx(ex.Message);
            }
        }

        private void btnAddSentinelProdToDownload_Click(object sender, EventArgs e)
        {
            var pg = lstSentilenProducts.SelectedItem as SentinelProduct;
            if (pg != null)
            {
                var pairs = controllerSentinel.GetScenePairProduct(pg);

                lstSentinelProductsToDownload.Items.Clear();
                var pgl = controllerSentinel.AddProductsToDownload(pairs);

                //if (pairs.Count() == 2)
                //{
                //    controllerSentinel.AddSentinelPairCoherence(pgl.First(), pgl.Last());
                //}

                foreach (var p in pgl)
                {
                    AddSceneToDownload(p);
                }
                ShowButtons();
            }
        }

        private void AddSceneToDownload(SentinelProductGui sentinelProduct)
        {
            if (sentinelProduct != null)
            {
                ListViewItem item = new ListViewItem(sentinelProduct.Identifier);
                lstSentinelProductsToDownload.Items.Add(item);
            }
        }

        private void btnDownloadSentinelProd_Click(object sender, EventArgs e)
        {
            btnDownloadSentinelProd.Enabled = false;
            toolStripLabelProcessing.Text = "Processing...";
            controllerSentinel.DownloadProducts();
            ShowButtons();
            toolStripLabelProcessing.Text = "";
            btnDownloadSentinelProd.Enabled = true;
        }

        private void FillScenesList()
        {
            var selectedTile = SelectedTile;
            if (selectedTile != null)
            {
                OnSentinelProductLoaded(selectedTile.TileScenes);
            }
        }

        private void lstTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTile = SelectedTile;
            if (selectedTile != null)
            {
                OnSentinelProductLoaded(selectedTile.TileScenes);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlTop.SelectedTab == tabPreprocessTop)
            {
                lstPreprocessTiles.Items.Clear();
                DownloadedTiles?.ToList().ForEach(t => lstPreprocessTiles.Items.Add(t.Name));
            }
        }

        private void lstPreprocessTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSentinelProductsToProcess.Items.Clear();
            controllerSentinelProcess.ClearSelected();
            var products = controllerSentinelProcess.GetProductsByTileName(lstPreprocessTiles.SelectedItem.ToString());

            products.ToList().ForEach(p => lstSentinelProductsToProcess.Items.Add(p.Identifier));
            lstProductInfoDem.Items.Clear();
            lstPairDem.Items.Clear();
            lblCoherenceVal.Text = string.Empty;
            ShowButtons();
        }

        public void OnErrorProcessing(string consoleMessage, ActironCommandLineStatesEnum state)
        {
            if (state == ActironCommandLineStatesEnum.Error)
            {
                if (!InvokeRequired)
                {
                    listLogMesages.Items.Add($"ERROR: {consoleMessage}");
                }
                else
                {
                    //Invoke(new ActionProcessCommandLineDelegate(FillMessages), new object[]
                    //    {consoleMessage, state});
                }
            }
        }

        public void FillMessages(string consoleMessage, ActironCommandLineStatesEnum state)
        {
            if (state == ActironCommandLineStatesEnum.Error)
            {
                listLogMesages.Items.Add($"ERROR: {consoleMessage}");
            }
            else if (state == ActironCommandLineStatesEnum.Output)
            {
                listLogMesages.Items.Add(consoleMessage);
            }
        }

        public void OnProcessing(string consoleMessage, ActironCommandLineStatesEnum state)
        {
            if (state == ActironCommandLineStatesEnum.Output)
            {
                if (!InvokeRequired)
                {
                    listLogMesages.Items.Add(consoleMessage);
                }
                else
                {
                    //Invoke(new ActionProcessCommandLineDelegate(FillMessages), new object[]
                    //    {consoleMessage, state});
                }
            }
        }

        private void btnChkCoherence_Click(object sender, EventArgs e)
        {
            btnChkCoherence.Enabled = false;
            toolStripLabelProcessing.Text = "Processing...";
            try
            {
                controllerSentinelProcess.CheckCoherence();
                MessageBox.Show(
                $"Сумісність була перевірена. Значення сумісності {SentinelPairDem.Mean.ToString("F3")}.",
                $"Спостереження. Інформаційне повідомлення",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(
                $"Перевірка сумісності закінчилась з помилкою.{Environment.NewLine}Папку {ex.Message} не знайдено.",
                $"Спостереження. Інформаційне повідомлення",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                log.ErrorEx(ex.Message);
                MessageBox.Show(
                $"Перевірка сумісності закінчилась з помилкою.{Environment.NewLine}Більш детальна інформація знаходиться у файлі журналу.",
                $"Спостереження. Інформаційне повідомлення",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }

            toolStripLabelProcessing.Text = "";
            ShowButtons();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            toolStripLabelProcessing.Text = "Processing...";
            controllerSentinelProcess.PairProcessing();
            MessageBox.Show(
                $"Обробку пари сцен S-1 завершено.",
                $"Спостереження. Інформаційне повідомлення",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            toolStripLabelProcessing.Text = "";
            ShowButtons();
        }

        private void lstPairsTOProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fil Scene properties
            var props = controllerSentinel.GetSentinelProductProperties(SelectedProductDem);

            lstProductInfoDem.Items.Clear();

            foreach (var prop in props)
            {
                var item = new ListViewItem(prop[0]);
                item.SubItems.Add(prop[1]);
                lstProductInfoDem.Items.Add(item);
            }

            //Get Scene pair
            lstPairDem.Items.Clear();
            var pair = controllerSentinelProcess.GetPairByProduct();


            if (pair.Count() == 2)
            {
                var coherenceVal = (SentinelPairDem != null && SentinelPairDem.Mean > 0) ? SentinelPairDem.Mean.ToString("F3") :
                        LocalizationContext.Instance.FindLocalizedElement("LblCoherenceValAbsent", "не розраховано");
                lblCoherenceVal.Text = $"{LocalizationContext.Instance.FindLocalizedElement("LblCoherenceVal", "Сумісність:")} " +
                    $"{coherenceVal}";

                lstPairDem.Items.AddRange(pair.Select(p => p.Identifier).ToArray());
            }
            else
            {
                lblCoherenceVal.Text = string.Empty;
            }

            ShowButtons();
        }

        private void tabGenerateTileTop_Click(object sender, EventArgs e)
        {

        }

        private void btnAddTaileDem_Click(object sender, EventArgs e)
        {
            var tileToSelect = controllerGenerateTile.AddTileToList();
            FillTileSource(lstTilesDem, controllerGenerateTile.Tiles?.Select(t => t.Name));
            if (tileToSelect != null)
                lstTilesDem.SelectedItem = tileToSelect.Name;

        }

        private void lstTilesDem_SelectedIndexChanged(object sender, EventArgs e)
        {
            listQuaziTiles.Items.Clear();
            controllerGenerateTile.GetQaziTilesByTileName(lstTilesDem.SelectedItem.ToString())?.
             ToList().ForEach(qt => listQuaziTiles.Items.Add(qt.QuaziTileName, true));
            btnGenerateTile.Enabled = listQuaziTiles.Items.Count > 0;
        }

        private void buttonrefreshlisttiles_Click(object sender, EventArgs e)
        {
            lstPreprocessTiles.Items.Clear();
            DownloadedTiles?.ToList().ForEach(t => lstPreprocessTiles.Items.Add(t.Name));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lstSentinelProductsToDownload.Items.Clear();
            btnAddSentinelProdToDownload.Enabled = true;
        }

        private void btnReadTilesFromFile_Click(object sender, EventArgs e)
        {
            var tiles = controllerSentinel.GetTilesFromFile();

            if (tiles == null) return;

            if (sender is Button btn)
            {
                var way = btnReadTilesFromFile == btn;
                ListBox lst = null;
                if (way)
                {
                    lst = lstTiles;
                    controllerSentinel.AddTilesForImport(tiles);
                }
                else if (btnReadTilesFromFileDem == btn)
                {
                    lst = lstTilesDem;
                    controllerGenerateTile.AddTilesToList(tiles);
                }
                else if (btnReadTilesFromFileSrtm == btn)
                {
                    lst = lstTilesSrtm;
                    controllerSrtm.AddTilesToList(tiles);
                }
                FillTileSource(lst, tiles.Select(t => t.Name));
            }
        }

        private void buttonDelTile_Click(object sender, EventArgs e)
        {
            var message = $"Ви дійсно бажаєте видалити {SelectedTile.ParentTile.Name}?";
            if (MessageBox.Show(
                message,
                "Спостереження. Запит",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedIndex = lstTiles.SelectedIndex;
                string tiletoRemove = SelectedTile.ParentTile.Name;
                controllerSentinel.RempoveTileFromImport(SelectedTile);
                lstTiles.Items.Remove(tiletoRemove);
                if (lstTiles.Items.Count <= selectedIndex)
                {
                    selectedIndex = lstTiles.Items.Count - 1;
                }
                lstTiles.SelectedIndex = selectedIndex;
            }
        }

        private void txtLatitude_Leave(object sender, EventArgs e)
        {
            ShowButtons();
        }

        private void btnSelectSentinelExternal_Click(object sender, EventArgs e)
        {
            controllerSentinel.SelectSentinelStorageExternal();
        }

        private void btnSelectSRTMExternal_Click(object sender, EventArgs e)
        {
            controllerSrtm.SelectSRTMStorageExternal();
        }

        private void btnGenerateTile_Click(object sender, EventArgs e)
        {
            if (listQuaziTiles.CheckedItems.Count == 0)
            {
                return;
            }

            lstGenerateTileMessages.Items.Clear();
            bool canGenerate = controllerGenerateTile.IsTIleCoveragedByQuaziTiles();
            if (!canGenerate && MessageBox.Show(
                $"Потрібний тайл ЦММ не має повного покриття.{Environment.NewLine}" +
                $"Бажаєте продовжити генерації тайлу ЦММ?",
                "Спостереження. Запит",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            IEnumerable<string> messages;

            btnGenerateTile.Enabled = false;
            //var res = controllerGenerateTile.GenerateTile(listQuaziTiles.CheckedItems.Cast<string>(), out messages);
            var res = controllerGenerateTile.GenerateTileClipped(listQuaziTiles.CheckedItems.Cast<string>(), out messages);

            ShowButtons();
            lstGenerateTileMessages.Items.AddRange(messages.ToArray());

            if (res)
            {
                MessageBox.Show(
                    "Тайл ЦММ для сховища даних сгенеровано успішно",
                    "Спостереження. Інформаційне повідомлення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Помилка генерації тайла ЦММ.{Environment.NewLine}Для отримання додаткової інформації зверніться до журналу додатку",
                    "Спостереження. Попередження",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void listQuaziTiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void listQuaziTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowButtons();
        }

        private void btnAddQTileToMapDem_Click(object sender, EventArgs e)
        {

            try
            {
                if (!controllerGenerateTile.AddQTileToMap())
                {
                    MessageBox.Show(LocalizationContext.Instance.FindLocalizedElement("MsgRasterWasNotAdded",
                        "Виникла помилка при додаванны квазыітайлу/nБільш детальна інформація знаходиться у файлі журналую")
                        , LocalizationContext.Instance.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }
            }
            catch (EntryPointNotFoundException)
            {
                MessageBox.Show(LocalizationContext.Instance.FindLocalizedElement("MsgAddDemModuleDoesnotExistText", "Модуль \"Спостереження. DEM\" не було підключено. Будь ласка додайте модуль до проекту, щоб мати можливість взаємодіяти з ним")
                    , LocalizationContext.Instance.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                log.ErrorEx(ex.Message);
            }

        }

        private void btnAddTileToListStrm_Click(object sender, EventArgs e)
        {
            var tileToSelect = controllerSrtm.AddTileToList();
            FillTileSource(lstTilesSrtm, controllerSrtm.Tiles?.Select(t => t.Name));
            if (tileToSelect != null)
                lstTilesSrtm.SelectedItem = tileToSelect.Name;
        }
    }

}
