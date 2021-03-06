﻿using ESRI.ArcGIS.Geometry;
using MilSpace.Core.DataAccess;
using MilSpace.DataAccess.DataTransfer;
using System.Collections.Generic;

namespace MilSpace.Visibility.ViewController
{
    public interface IObservationPointsView
    {
        void FillObservationPointList(IEnumerable<IObserverPoint> observationPoints,
                                      ValuableObservPointFieldsEnum filter,
                                      bool newSelection = false,
                                      bool clearUpdatedPointsList = true);

        void ClearObserverPointsList(bool isOPFromGdb, bool changeSet = true);
        void FillVisibilitySessionsList(IEnumerable<VisibilityTask> visibilitySessions, bool isNewSessionAdded, string newTaskName);
        void FillVisibilityResultsTree(IEnumerable<VisibilityCalcResults> visibilityResults);
        void FillObservationObjectsList(IEnumerable<ObservationObject> observationObjects);
        void FillObservationObjectsList(Dictionary<int, IGeometry> observationObjects);
        void FillSelectedOPFields(ObservationPoint point);
        void AddSelectedOO(IGeometry geometry, string title);
        void ChangeRecord(int id, ObservationPoint observationPoint);
        void AddRecord(ObservationPoint observationPoint);
        void RemoveSessionFromList(string id);
        void SetFieldsEditingAbility(bool areFiedlsReadOnly);
        void RemoveObserverPoint(int id);
        void RefreshOPGraphics(bool updateRelationLines);

        string ObservationPointsFeatureClass { get; }
       // string ObservationStationFeatureClass { get; }
        ValuableObservPointFieldsEnum GetFilter { get; }
        IEnumerable<string> GetTypes { get; }
        IEnumerable<string> GetAffiliation { get; }
    }
}
