﻿using ESRI.ArcGIS.Geometry;
using MilSpace.Core.DataAccess;
using MilSpace.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MilSpace.Core.ModalWindows
{
    public partial class ObservPointsForFunToPointsModalWindow : Form
    {
        private List<FromLayerPointModel> _points = new List<FromLayerPointModel>();
        public List<IGeometry> SelectedPoints;

        public ObservPointsForFunToPointsModalWindow(List<FromLayerPointModel> points)
        {
            InitializeComponent();
            LocalizeStrings();
            _points = points;
            FillPointsGrid();
        }

        private void LocalizeStrings()
        {
            this.Text = LocalizationContext.Instance.FindLocalizedElement("ModalObservPointsTitle", "Вибір точки з шару точок спостереження");
            btnChoosePoint.Text = LocalizationContext.Instance.ChooseText;
            dgvPoints.Columns["IdCol"].HeaderText = LocalizationContext.Instance.IdHeaderText;
            dgvPoints.Columns["TitleCol"].HeaderText = LocalizationContext.Instance.TitleHeaderText;
            lblLayer.Text = LocalizationContext.Instance.FindLocalizedElement("ObservPointsTypeText", "Пункти спостереження");
        }

        private void FillPointsGrid()
        {
            dgvPoints.Rows.Clear();

            foreach(var point in _points)
            {
                dgvPoints.Rows.Add(false, point.ObjId, point.DisplayedField, point.Point.X.ToFormattedString(), point.Point.Y.ToFormattedString());
            }
        }

        private void ChckAllPoints_CheckedChanged(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dgvPoints.Rows)
            {
                row.Cells[0].Value = chckAllPoints.Checked;
            }
        }

        private void BtnChoosePoint_Click(object sender, EventArgs e)
        {
            SelectedPoints = new List<IGeometry>();

            foreach(DataGridViewRow row in dgvPoints.Rows)
            {
                if((bool)row.Cells[0].Value)
                {
                    SelectedPoints.Add(_points.First(point => point.ObjId == (int)row.Cells["IdCol"].Value).Point);
                }
            }
        }
    }
}
