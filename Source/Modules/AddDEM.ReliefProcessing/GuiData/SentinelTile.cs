﻿using MilSpace.Core.Geometry;
using MilSpace.DataAccess.DataTransfer.Sentinel;
using MilSpace.Tools.Sentinel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilSpace.AddDem.ReliefProcessing.GuiData
{
    public class SentinelTile
    {
        private List<SentinelProductGui> downloadingScenes = new List<SentinelProductGui>();
        private List<SentinelProduct> tileScenes = new List<SentinelProduct>();
        public bool Downloaded;
        public bool Downloading;

        public SentinelProductGui BaseScene;
        public Tile ParentTile;


        public IEnumerable<SentinelProductGui> DownloadingScenes => downloadingScenes;

        public IEnumerable<SentinelProduct> TileScenes => tileScenes;

        public void AddProducts(IEnumerable<SentinelProduct> products)
        {

            if (tileScenes.Count > 0)
            {
                tileScenes.Clear();
            }

            foreach (var p in products)
            {
                if (!tileScenes.Any(ts => ts.Identifier.Equals(p.Identifier)))
                {
                    tileScenes.Add(p);
                }
            }
        }

        public void AddProductsToDownload(IEnumerable<SentinelProductGui> products)
        {
            foreach (var p in products)
            {
                if (!downloadingScenes.Any(ts => ts.Identifier.Equals(p.Identifier)))
                {
                    downloadingScenes.Add(p);
                }
            }

          //  downloadingScenes.ForEach(p => p.BaseScene = p.Id == baseScene.Id);
        }
    }
}
