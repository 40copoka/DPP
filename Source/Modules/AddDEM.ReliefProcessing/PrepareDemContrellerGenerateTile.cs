﻿using MilSpace.Configurations;
using MilSpace.Core;
using MilSpace.Core.ModulesInteraction;
using MilSpace.Core.Tools;
using MilSpace.DataAccess.DataTransfer.Sentinel;
using MilSpace.DataAccess.Facade;
using MilSpace.Tools.Sentinel;
using MilSpace.Tools.SurfaceProfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilSpace.AddDem.ReliefProcessing
{
    public class PrepareDemContrellerGenerateTile
    {
        static Logger log = Logger.GetLoggerEx("PrepareDemGenerateTileContreller");
        IPrepareDemViewGenerateTile view;

        List<SentinelTilesCoverage> quaziTiles = new List<SentinelTilesCoverage>();
        public bool Processing;


        public PrepareDemContrellerGenerateTile()
        { }

        public void SetView(IPrepareDemViewGenerateTile view)
        {
            this.view = view;
        }

        private List<Tile> tiles = new List<Tile>();

        public IEnumerable<Tile> Tiles => tiles;

        public void AddTilesToList(IEnumerable<Tile> newTiles)
        {
            tiles.Clear();
            newTiles.ToList().
                ForEach(tile =>
               {
                   if (!tiles.Any(t => t.Equals(tile)))
                   {
                       tiles.Add(tile);
                   }
               });
        }

        public void RemoveTileFromList(string tileName)
        {
            var tileToRemove = new Tile(tileName);
            if (!tileToRemove.IsEmpty)
            {
                tiles.Remove(tileToRemove);
            }
        }

        public Tile AddTileToList()
        {
            var latString = view.TileDemLatitude;
            var lonString = view.TileDemLongitude;
            double latDouble;
            double lonDouble;
            Tile tile = null;

            if (latString.TryParceToDouble(out latDouble) && lonString.TryParceToDouble(out lonDouble))
            {
                int lat = Convert.ToInt32(latDouble);
                int lon = Convert.ToInt32(lonDouble);
                tile = new Tile { Lat = lat, Lon = lon };
                if (!tiles.Any(t => t.Equals(tile)))
                {
                    tiles.Add(tile);
                }
                return tile;
            }

            return null;
        }

        public IEnumerable<SentinelTilesCoverage> GetQaziTilesByTileName(string tileName)
        {

            var tile = tiles.FirstOrDefault(t => t.Name == tileName);
            if (tile != null)
            {
                var pathToProcessFolder = MilSpaceConfiguration.DemStorages.SentinelProcessFolder;
                var facede = new DemPreparationFacade();
                quaziTiles =
                 facede.GeTileCoveragesHaveGeometry().
                    Where(c => c.Geometry.Intersects(tile.Geometry)
                    && File.Exists(Path.Combine(pathToProcessFolder, c.DEMFilePath))
                    ).ToList();
                return quaziTiles;

            }


            return null;
        }

        public Tile GetTilesByPoint()
        {
            var latString = view.TileDemLatitude;
            var lonString = view.TileDemLongitude;
            double latDouble;
            double lonDouble;
            Tile testTile = null;

            if (latString.TryParceToDouble(out latDouble) && lonString.TryParceToDouble(out lonDouble))
            {
                int lat = Convert.ToInt32(latDouble);
                int lon = Convert.ToInt32(lonDouble);

                if (!Tiles.Any(t => t.Lat == lat && t.Lon == lon))
                {
                    testTile = new Tile
                    {
                        Lat = lat,
                        Lon = lon
                    };

                }
            }

            return testTile;
        }

        public bool IsTIleCoveragedByQuaziTiles()
        {
            var tile = tiles.First(t => t.Name == view.SelectedTileDem);
            return SantinelExportDemToTileManager.CheckTileCompleteness(tile, quaziTiles);
        }

        public RasterInfo FillRasterProperties()
        {
            //var tile = tiles.First(t => t.Name == view.SelectedTileDem);
            return EsriTools.GetRasterProperties(GetQuaziTileFilePath(view.SelectedQuaziTile), view.ActiveView);
        }

        public string GetQuaziTileFilePath(string quaziTileName)
        {
            var qt = quaziTiles.FirstOrDefault(q => q.QuaziTileName == quaziTileName);

            if (qt != null)
            {
                return Path.Combine(MilSpaceConfiguration.DemStorages.SentinelProcessFolder, qt.DEMFilePath);
            }

            return null;
        }

        public bool GenerateTile(IEnumerable<string> checkedQuaziTiles, out IEnumerable<string> messages)
        {
            Processing = true;
            var pathToTempFile = Path.Combine(MilSpaceConfiguration.DemStorages.SentinelStorage, "Temp");
            if (!Directory.Exists(pathToTempFile))
            {
                Directory.CreateDirectory(pathToTempFile);
            }
            //var pathToTempFile = Path.GetTempPath();
            messages = new List<string>();
            var tile = tiles.First(t => t.Name == view.SelectedTileDem);
            var resultFileName = Path.Combine(MilSpaceConfiguration.DemStorages.SentinelStorage, $"{tile.FullName}.tif");

            log.InfoEx("Starting MosaicToRaster...");

            var list = checkedQuaziTiles.Select(r => GetQuaziTileFilePath(r)).Where(r => r != null).ToList();

            var tempFilesToDelete = new List<string>();
            var commonMessages = new List<string>();

            var tempFilePath = string.Empty;
            var tileount = 2;

            while (list.Count > 0)
            {
                var tempFileName = $"{DataAccess.Helper.GetTemporaryNameSuffix()}.tif";
                tempFilePath = Path.Combine(pathToTempFile, tempFileName);
                tempFilesToDelete.Add(tempFilePath);

                var temp = list.Take(list.Count < tileount ? list.Count : tileount);
                list = list.Except(temp).ToList();
                if (list.Count > 0)
                {
                    list.Add(tempFilePath);
                }

                Processing = CalculationLibrary.MosaicToRaster(temp, pathToTempFile, tempFileName, out messages, pathToTempFile);
                commonMessages.AddRange(messages);
                if (!Processing)
                {
                    break;
                }
            }

            commonMessages.ForEach(m => { if (Processing) log.InfoEx(m); else log.ErrorEx(m); });

            if (!Processing)
            { return false; }

            if (!File.Exists(tempFilePath))
            {
                Processing = false;
                (messages as List<string>).Add($"ERROR:  Об'єднаний файл  {tempFilePath} не було знайдено!");
                return false;
            }

            IEnumerable<string> messagesToClip;
            var res = CalculationLibrary.ClipRasterByArea(tempFilePath, resultFileName, tile, out messagesToClip, pathToTempFile);

            commonMessages.AddRange(messagesToClip);

            messagesToClip.ToList().ForEach(m => { if (res) log.InfoEx(m); else log.ErrorEx(m); });

            Processing = false;

            if (res && File.Exists(resultFileName))
            {
                var s1Tile = AddDemFacade.GetS1GridByTile(tile);
                if (s1Tile == null)
                {
                    commonMessages.Add($"ERROR:Cannot find tile {tile.FullName} in the S1 grid table");
                }
                else
                {
                    s1Tile.Loaded = true;
                    if (!AddDemFacade.UpdateS1Grid(s1Tile))
                    {
                        commonMessages.Add($"ERROR:The tile {tile.FullName} in the S1 grid table was not updated");
                    }
                }
            }

            messages = commonMessages;

            tempFilesToDelete.ForEach(f =>
           {
               FileInfo fl = new FileInfo(f);
               string templateToDel = $"{fl.Name.Substring(0, fl.Name.Length - 3)}*";

               fl.Directory.GetFiles(templateToDel).ToList().ForEach(fd =>
               {
                   try
                   {
                       File.Delete(fd.FullName);
                   }
                   catch (Exception ex)
                   {
                       log.ErrorEx($"Cannot delete {fd.FullName}");
                       log.ErrorEx(ex.Message);
                   }
               });
           });

            return res;
        }

        public bool GenerateTileClipped(IEnumerable<string> checkedQuaziTiles, out IEnumerable<string> messages)
        {
            Processing = true;
            var pathToTempFile = Path.Combine(MilSpaceConfiguration.DemStorages.SentinelStorage, "Temp");
            if (!Directory.Exists(pathToTempFile))
            {
                Directory.CreateDirectory(pathToTempFile);
            }
            //var pathToTempFile = Path.GetTempPath();
            messages = new List<string>();
            var tile = tiles.First(t => t.Name == view.SelectedTileDem);
            var resultFileName = Path.Combine(MilSpaceConfiguration.DemStorages.SentinelStorage, $"{tile.FullName}.tif");

            log.InfoEx("Starting MosaicToRaster...");

            var list = checkedQuaziTiles.Select(r => GetQuaziTileFilePath(r)).Where(r => r != null).ToList();

            var commonMessages = new List<string>();
            var clippedImages = new List<string>();


            IEnumerable<string> messagesToClip;
            var tempFilePath = string.Empty;
            var tempFileName = string.Empty;
            foreach (var t in list)
            {
                tempFileName = $"{DataAccess.Helper.GetTemporaryNameSuffix()}.tif";
                tempFilePath = Path.Combine(pathToTempFile, tempFileName);

                clippedImages.Add(tempFilePath);

                Processing = CalculationLibrary.ClipRasterByArea(t, tempFilePath, tile, out messagesToClip, pathToTempFile);
                commonMessages.AddRange(messagesToClip);

                if (!Processing)
                {
                    messages = commonMessages;
                    break;
                }
            };




            var tempFilesToDelete = new List<string>(clippedImages);


            bool res = true;

            if (Processing)
            {
                var tileount = 10;
                list = clippedImages;
                while (list.Count > 0)
                {
                    tempFileName = $"{DataAccess.Helper.GetTemporaryNameSuffix()}.tif";
                    tempFilePath = Path.Combine(pathToTempFile, tempFileName);
                    tempFilesToDelete.Add(tempFilePath);

                    var temp = list.Take(list.Count < tileount ? list.Count : tileount);
                    list = list.Except(temp).ToList();
                    if (list.Count > 0)
                    {
                        list.Add(tempFilePath);
                    }

                    Processing = CalculationLibrary.MosaicToRaster(temp, pathToTempFile, tempFileName, out messages, pathToTempFile);
                    commonMessages.AddRange(messages);
                    if (!Processing)
                    {
                        res = false;
                        break;
                    }
                }
            }

            commonMessages.ForEach(m => { if (Processing) log.InfoEx(m); else log.ErrorEx(m); });

            if (!Processing)
            { return false; }

            //
            Processing = CalculationLibrary.ClipRasterByArea(tempFilePath, resultFileName, tile, out messagesToClip, pathToTempFile);
            //

            if (!Processing)
            { return false; }

            if (!File.Exists(tempFilePath))
            {
                Processing = false;
                (messages as List<string>).Add($"ERROR:  Об'єднаний файл  {tempFilePath} не було знайдено!");
                return false;
            }

            Processing = false;

            if (File.Exists(resultFileName))
            {
                var s1Tile = AddDemFacade.GetS1GridByTile(tile);
                if (s1Tile == null)
                {
                    commonMessages.Add($"ERROR:Cannot find tile {tile.FullName} in the S1 grid table");
                }
                else
                {
                    s1Tile.Loaded = true;
                    if (!AddDemFacade.UpdateS1Grid(s1Tile))
                    {
                        commonMessages.Add($"ERROR:The tile {tile.FullName} in the S1 grid table was not updated");
                    }
                }
            }

            messages = commonMessages;

            tempFilesToDelete.ForEach(f =>
            {
                FileInfo fl = new FileInfo(f);
                string templateToDel = $"{fl.Name.Substring(0, fl.Name.Length - 3)}*";

                fl.Directory.GetFiles(templateToDel).ToList().ForEach(fd =>
                {
                    try
                    {
                        File.Delete(fd.FullName);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorEx($"Cannot delete {fd.FullName}");
                        log.ErrorEx(ex.Message);
                    }
                });
            });

            return res;
        }

        public bool AddQTileToMap()
        {

            var addDemModule = GetAddDemModule();
            if (addDemModule == null)
            {
                throw new EntryPointNotFoundException();
            }

            var qTileName = view.SelectedQuaziTile;

            if (string.IsNullOrEmpty(qTileName))
                return false;

            var tile = GetQuaziTileFilePath(qTileName);

            if (string.IsNullOrEmpty(tile))
                return false;


            return addDemModule.AddDemToMap(tile);
        }

        private static IAddDemInteraction GetAddDemModule()
        {
            var addDemModule = ModuleInteraction.Instance.GetModuleInteraction<IAddDemInteraction>(out bool changes);

            if (!changes && addDemModule == null)
            {
                log.ErrorEx($"> GetTargetObservPoints Exception: {LocalizationContext.Instance.FindLocalizedElement("MsgObservPointscModuleDoesnotExistText", "Модуль \"Видимість\" не було підключено. Будь ласка додайте модуль до проекту, щоб мати можливість взаємодіяти з ним")}");
                return null;
            }
            return addDemModule;
        }
    }
}
