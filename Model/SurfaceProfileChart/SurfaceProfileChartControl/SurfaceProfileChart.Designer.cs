﻿namespace SurfaceProfileChart.SurfaceProfileChartControl
{
    partial class SurfaceProfileChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.profileChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.profileChart)).BeginInit();
            this.SuspendLayout();
            // 
            // profileChart
            // 
            chartArea1.Name = "Default";
            this.profileChart.ChartAreas.Add(chartArea1);
            this.profileChart.Location = new System.Drawing.Point(0, 0);
            this.profileChart.Name = "profileChart";
            series1.ChartArea = "Default";
            series1.Name = "Series1";
            this.profileChart.Series.Add(series1);
            this.profileChart.Size = new System.Drawing.Size(664, 221);
            this.profileChart.TabIndex = 0;
            this.profileChart.Text = "chart1";
            this.profileChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Profile_MouseDown);
            // 
            // SurfaceProfileChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.profileChart);
            this.Name = "SurfaceProfileChart";
            this.Size = new System.Drawing.Size(664, 221);
            this.Load += new System.EventHandler(this.SurfaceProfileChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.profileChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart profileChart;
    }
}
