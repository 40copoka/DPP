﻿using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MilSpace.Core.Tools
{
    public class MapLayersManager
    {
        private readonly List<ILayer> Layers;

        private esriGeometryType[] lineTypes = new esriGeometryType[] { esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryPolyline };
        private esriGeometryType[] pointTypes = new esriGeometryType[] { esriGeometryType.esriGeometryPoint };
        private esriGeometryType[] polygonTypes = new esriGeometryType[] { esriGeometryType.esriGeometryPolygon };
        private readonly IActiveView activeView;
        private static Logger logger = Logger.GetLoggerEx("MapLayersManager");

        public MapLayersManager(IActiveView activeView)
        {
            this.activeView = activeView;
            Layers = GetAllLayers();
        }

        private static IEnumerable<IRasterLayer> GetRasterLayers(ILayer layer)
        {
            var result = new List<IRasterLayer>();

            if (layer is IRasterLayer fLayer)
            {
                result.Add(fLayer);
            }

            if (layer is ICompositeLayer cLayer)
            {

                for (int j = 0; j < cLayer.Count; j++)

                {
                    if ((layer is IRasterLayer cRastreLayer))
                    {
                        result.Add(cRastreLayer);
                    }
                }

            }

            return result;
        }

        private static IEnumerable<ILayer> GetFeatureLayers(ILayer layer)
        {
            var result = new List<ILayer>();

            if (layer is IFeatureLayer fLayer)
            {
                var featureLayer = fLayer;
                var featureClass = featureLayer.FeatureClass;

                if (featureClass != null &&
                    ((featureClass.ShapeType == esriGeometryType.esriGeometryLine) ||
                    (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline) ||
                    (featureClass.ShapeType == esriGeometryType.esriGeometryPoint) ||
                    (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)))
                {
                    result.Add(fLayer);
                }
            }
            if (layer is ICompositeLayer cLayer)
            {
                for (int j = 0; j < cLayer.Count; j++)
                {
                    var curLauer = cLayer.Layer[j];
                    if ((curLauer is IFeatureLayer cfeatureLayer))
                    {
                        result.Add(cfeatureLayer);
                    }

                    if (curLauer is ICompositeLayer ccLayer)
                    {
                        // Here can be check by Tag in the Layer description
                        result.AddRange(GetFeatureLayers(curLauer as ILayer));
                    }
                }

            }

            return result;
        }

        internal List<ILayer> GetAllLayers()
        {
            var layersToReturn = new List<ILayer>();
            try
            {
                var layers = activeView.FocusMap.Layers;
                var layer = layers.Next();

                while (layer != null)
                {

                    layersToReturn.AddRange(GetRasterLayers(layer));
                    layersToReturn.AddRange(GetFeatureLayers(layer));

                    layer = layers.Next();
                }

                return layersToReturn;
            }
            catch (Exception ex)
            {
                logger.ErrorEx("Error: " + ex.ToString());
                return null;
            }
        }

        public IEnumerable<ILayer> RasterLayers => Layers.Where(layer => layer is IRasterLayer);

        public IEnumerable<ILayer> PointLayers => GetFeatureLayers(pointTypes);

        public IEnumerable<ILayer> LineLayers => GetFeatureLayers(lineTypes);

        public IEnumerable<ILayer> PolygonLayers => GetFeatureLayers(polygonTypes);


        private IEnumerable<ILayer> GetFeatureLayers(IEnumerable<esriGeometryType> geomType)
        {
            return GetAllLayers().Where(l => l is IFeatureLayer && geomType.Any(g => g == ((IFeatureLayer)l).FeatureClass.ShapeType));
        }
    }
}
