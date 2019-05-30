﻿using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using MilSpace.ProjectionsConverter.Interfaces;
using System;
using System.Threading.Tasks;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using MilSpace.ProjectionsConverter.Models;
using MilSpace.ProjectionsConverter.ReferenceData;
using ESRI.ArcGIS.esriSystem;

namespace MilSpace.ProjectionsConverter
{
    public class BusinessLogic : IBusinessLogic
    {
        private const string NoMapExceptionMessage = "Can't get current Map Document.";
        private readonly IApplication _arcMapApp;
        private readonly IDataExport _dataExport;
        public BusinessLogic(IApplication arcMapApp, IDataExport dataExport)
        {
            _arcMapApp = arcMapApp ?? throw new ArgumentNullException(nameof(arcMapApp));
            _dataExport = dataExport;
        }

        public async Task<IPoint> ConvertFromMgrs(string mgrsInputValue, int falseOriginX = 0, int falseOriginY = 0, int scaleUnits = 1000)
        {
            return await Task.Run(() =>
            {
                var resultPoint = new Point();
                //Create Spatial Reference Factory
                var spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                //Create Spatial Reference
                ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                spatialReference.SetFalseOriginAndUnits(falseOriginX, falseOriginY, scaleUnits);
                resultPoint.SpatialReference = spatialReference;
                (resultPoint as IConversionMGRS).PutCoordsFromMGRS(mgrsInputValue, esriMGRSModeEnum.esriMGRSMode_Automatic);
                return resultPoint;
            });
        }

        public async Task<string> ConvertToMgrs(IPoint wgsInputPoint)
        {
            return await Task.Run(() =>
            {
                var conversionNotation = wgsInputPoint as IConversionNotation;
                //5 for 1m resolution
                return conversionNotation?.CreateMGRS(5, true, esriMGRSModeEnum.esriMGRSMode_Automatic);
            });
        }

        public void CopyCoordinatesToClipboard(PointModel pointModel)
        {
            Clipboard.Clear();
            Clipboard.SetData(nameof(PointModel), pointModel);
        }

        public async Task<IPoint> GetDisplayCenterAsync()
        {
            if (!(_arcMapApp.Document is IMxDocument currentDocument)) throw new Exception(NoMapExceptionMessage);
            var centerPoint = new Point();
            await Task.Run(() =>
            {
                var activeView = currentDocument.ActiveView;
                var envelope = activeView.Extent as IEnvelope;
                centerPoint.X = ((envelope.XMax - envelope.XMin) / 2) + envelope.XMin;
                centerPoint.Y = ((envelope.YMax - envelope.YMin) / 2) + envelope.YMin;
                centerPoint.SpatialReference = envelope.SpatialReference;
            });            
            return centerPoint;
        }

        public async Task<IPoint> GetSelectedPointAsync(int mousePositionX, int mousePositionY)
        {
            if (!(_arcMapApp.Document is IMxDocument currentDocument)) throw new Exception(NoMapExceptionMessage);

            IPoint resultPoint = new Point();
            await Task.Run(() => 
            {
                resultPoint = (currentDocument.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(mousePositionX, mousePositionY);
                
            });
            return resultPoint;
        }

        public async Task MoveToNewCoordinateAsync(double x, double y)
        {
            if (!(_arcMapApp.Document is IMxDocument currentDocument)) throw new Exception(NoMapExceptionMessage);
            
            await Task.Run(() =>
            {
                var activeView = currentDocument.ActiveView;
                var point = new Point();
                point.PutCoords(x, y);
                activeView.Extent.CenterAt(point);
                activeView.Refresh();                
            });            
        }

        public async Task<IPoint> ProjectPointAsync(IPoint inputPoint, SingleProjectionModel singleProjectionModel)
        {
            if (inputPoint == null) return null;
            await Task.Run(() =>
            {
                //Create Spatial Reference Factory
                var spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                //Projected Coordinate System to project into
                var projectedCoordinateSystem = spatialReferenceFactory.CreateProjectedCoordinateSystem(singleProjectionModel.ESRIWellKnownID);
                projectedCoordinateSystem.SetFalseOriginAndUnits(singleProjectionModel.FalseOriginX, singleProjectionModel.FalseOriginY, singleProjectionModel.Units);

                inputPoint.Project(projectedCoordinateSystem);
            });
            return inputPoint;
        }

        public async Task<IPoint> ProjectSelectedPointAsync(int targetCoordinateSystemType, int mousePositionX, int mousePositionY, double falseOriginX = 0, double falseOriginY = 0)
        {
            if (!(_arcMapApp.Document is IMxDocument currentDocument)) throw new Exception(NoMapExceptionMessage);

            var resultPoint = await Task.Run(() =>
            {
                var mouseMapPoint = (currentDocument.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(mousePositionX, mousePositionY);
                if (mouseMapPoint == null) return null;
                currentDocument.FocusMap.ClearSelection();
                // Select using the shape (point) to
                // select the feature(s) - false to select any intersecting, true to select just the first
                try
                {
                    currentDocument.FocusMap.SelectByShape(mouseMapPoint, (_arcMapApp as IMxApplication).SelectionEnvironment, false);
                }
                catch (NullReferenceException) { return null; }
                var selectedFeatures = (IEnumFeature)currentDocument.FocusMap.FeatureSelection;
                var selectedFeature = selectedFeatures.Next();
                if (selectedFeature == null) return null;
                var featuresCount = 1;
                double bufferXCoord = 0.0;
                double bufferYCoord = 0.0;
                do
                {
                    var geometry = selectedFeature.ShapeCopy;
                    switch (geometry.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            var point = geometry as IPoint;
                            bufferXCoord += point.X;
                            bufferYCoord += point.Y;
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            var polygon = geometry as IArea;
                            bufferXCoord += polygon.Centroid.X;
                            bufferYCoord += polygon.Centroid.Y;
                            break;
                        case esriGeometryType.esriGeometryPolyline:
                            var polyline = geometry as IPolyline;
                            bufferXCoord += (polyline.FromPoint.X + polyline.ToPoint.X) / 2;
                            bufferYCoord += (polyline.FromPoint.Y + polyline.ToPoint.Y) / 2;
                            break;
                        default:
                            break;
                    }
                    selectedFeature = selectedFeatures.Next();
                    featuresCount++;
                } while (selectedFeature != null);
                var bufferPoint = new Point();
                bufferPoint.PutCoords(bufferXCoord / featuresCount, bufferYCoord / featuresCount);
                return bufferPoint;
            });

            return await ProjectPointAsync(resultPoint, new SingleProjectionModel(targetCoordinateSystemType, falseOriginX, falseOriginY, currentDocument.FocusMap.MapScale));
        }

        public async Task SaveProjectionsToXmlFileAsync(PointModel pointModel, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            await _dataExport.ExportProjectionsToXmlAsync(pointModel, path);
        }
    }
}
