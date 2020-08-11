﻿using MilSpace.Configurations;
using MilSpace.Core;
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
        Logger log = Logger.GetLoggerEx("PrepareDemGenerateTileContreller");
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
            var tempFileName = $"{DataAccess.Helper.GetTemporaryNameSuffix()}.img";
            var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);
            messages = new List<string>();
            var tile = tiles.First(t => t.Name == view.SelectedTileDem);
            var resultFileName = Path.Combine(MilSpaceConfiguration.DemStorages.SentinelStorage, $"{tile.Name}.tif");

            log.InfoEx("Starting MosaicToRaster...");

            var list = checkedQuaziTiles.Select(r => GetQuaziTileFilePath(r)).Where(r => r != null);
            List<string> vttr = new List<string>();

            vttr.Add(list.First());
            vttr.Add(list.First());
            vttr.Add(list.First());
            vttr.Add(list.First());


            Processing = CalculationLibrary.MosaicToRaster(list, pathToTempFile, tempFileName, out messages);
            messages.ToList().ForEach(m => { if (Processing) log.InfoEx(m); else log.ErrorEx(m); });

            if (!Processing)
            { return false; }

            if (!File.Exists(tempFilePath))
            {
                Processing = false;
                (messages as List<string>).Add($"ERROR: File {tempFilePath} не було знайдено!");
                return false;
            }

            IEnumerable<string> messagesToClip;
            var res = CalculationLibrary.ClipRasterByArea(tempFilePath, resultFileName, tile, out messagesToClip);
            messagesToClip.ToList().ForEach(m => { if (res) log.InfoEx(m); else log.ErrorEx(m); });

            messages.Union(messagesToClip);
            Processing = false;

            try
            {
                //    File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                log.ErrorEx($"Cannot delete {tempFilePath}");
                log.ErrorEx(ex.Message);
            }

            return res;
        }
    }
}
