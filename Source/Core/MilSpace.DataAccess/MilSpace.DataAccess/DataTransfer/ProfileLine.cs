﻿using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace MilSpace.DataAccess.DataTransfer
{
    public class ProfileLine
    {
        public ProfilePoint PointFrom;
        public ProfilePoint PointTo;
        public int Id;
        public double Angel;
        public double Azimuth;
        public double Length;
        public bool Visible;

        [XmlIgnore]
        public IPolyline Line;

        [XmlIgnore]
        public ISpatialReference SpatialReference;

        [XmlIgnore]
        public int SessionId;
    }
}
