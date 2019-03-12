﻿using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using MilSpace.Configurations;
using MilSpace.Core.Actions;
using MilSpace.Core.Actions.ActionResults;
using MilSpace.Core.Actions.Base;
using MilSpace.Core.Actions.Interfaces;
using MilSpace.DataAccess.DataTransfer;
using MilSpace.DataAccess.Exceptions;
using MilSpace.DataAccess.Facade;
using MilSpace.Tools.Exceptions;
using MilSpace.Tools.SurfaceProfile.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MilSpace.Tools
{
    public class ProfileManager
    {
        private static readonly string FIRST_DIST_Field = "FIRST_DIST";
        private static readonly string FIRST_Z_Field = "FIRST_Z";
        private static readonly string LINE_ID_Field = "LINE_ID";

        private static readonly string WhereAllRecords = "OBJECTID > 0";


        public ProfileManager()
        { }

        public void GenerateProfile(string profileSource,
            IEnumerable<ILine> profileLines)
        {
            string profileSourceName = GdbAccess.Instance.AddProfileLinesToCalculation(profileLines);

            var action = new ActionParam<string>()
            {
                ParamName = ActionParamNamesCore.Action,
                Value = ActionsEnum.bsp.ToString()
            };


            string sdtnow = MilSpace.DataAccess.Helper.GetTemporaryNameSuffix();
            var resuTable = $"{MilSpaceConfiguration.ConnectionProperty.TemporaryGDBConnection}\\StackProfile{sdtnow}";
            var profileLineFeatureClass = GdbAccess.Instance.GetProfileLinesFeatureClass(profileSourceName);


            var prm = new List<IActionParam>
            {
                action,
                new ActionParam<string>() { ParamName = ActionParameters.FeatureClass, Value = profileLineFeatureClass },
                new ActionParam<string>() { ParamName = ActionParameters.ProfileSource, Value = profileSource },
                new ActionParam<string>() { ParamName = ActionParameters.DataWorkSpace, Value = resuTable},
                new ActionParam<string>() { ParamName = ActionParameters.OutGraphName, Value = ""}
            };


            var procc = new ActionProcessor(prm);
            var res = procc.Process<BoolResult>();

            if (!res.Result)
            {
                if (res.Exception != null)
                {
                    throw res.Exception;
                }
                //TODO: Log error
                throw new Exception(res.ErrorMessage);
            }

            //Teke the table and import the data


            try
            {
                ITable profiletable = GdbAccess.Instance.GetProfileTable($"StackProfile{sdtnow}");
                IFeatureClass lines = GdbAccess.Instance.GetCalcProfileFeatureClass(profileSourceName);

                IQueryFilter queryFilter = new QueryFilter()
                {
                    WhereClause = WhereAllRecords
                };

                ICursor featureCursor = profiletable.Search(queryFilter, true);
                IRow profileRow;

                int distanceFld = profiletable.FindField(FIRST_DIST_Field);
                int zFld = profiletable.FindField(FIRST_Z_Field);
                int idFld = profiletable.FindField(LINE_ID_Field);


                List<ProfileSurface> profileSurfaces = new List<ProfileSurface>();

                ProfileSession session = new ProfileSession()
                {
                    ProfileSurfaces = profileSurfaces.ToArray(),
                    ProfileLines = GetProfileLines(lines).ToArray()
                };


                Dictionary<int, List<ProfileSurfacePoint>> surface = new Dictionary<int, List<ProfileSurfacePoint>>();



                while ((profileRow = featureCursor.NextRow()) != null)
                {

                    int lineId = Convert.ToInt32(profileRow.Value[idFld]);

                    var profileLine = session.ProfileLines.FirstOrDefault(l => l.Id == lineId);

                    if (profileLine == null)
                    {
                        throw new MilSpaceProfileLineNotFound(lineId, profileLineFeatureClass);
                    }

                    List<ProfileSurfacePoint> points;
                    if (!surface.ContainsKey(lineId))
                    {
                        points = new List<ProfileSurfacePoint>();
                        surface.Add(lineId, points);
                    }
                    else
                    {
                        points = surface[lineId];
                    }

                    points.Add(new ProfileSurfacePoint
                    {
                        Distance = (double)profileRow.Value[distanceFld],
                        Z = (double)profileRow.Value[zFld],
                    });
                }

                //TODO: Clean memo using Marhsaling IRow

                session.ProfileSurfaces = surface.Select(r => new ProfileSurface
                {
                    LineId = r.Key,
                    ProfileSurfacePoints = r.Value.ToArray()
                }
                ).ToArray();


                XmlSerializer serializer = new XmlSerializer(typeof(ProfileSession));


                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, session);
                    try
                    {
                        string resSer = Encoding.UTF8.GetString((stream as MemoryStream).ToArray());
                    }
                    catch (Exception ex)
                    {
                        //TODO: log the error
                    }
                }

            }
            catch (MilSpaceDataException ex)
            {
                //TODO: Log error
                throw ex;
            }
            catch (Exception ex)
            {
                //TODO: Log error
                throw ex;
            }

        }

        private static IEnumerable<ProfileLine> GetProfileLines(IFeatureClass profileLines)
        {
            var result = new List<ProfileLine>();

            IQueryFilter queryFilter = new QueryFilter()
            {
                WhereClause = WhereAllRecords
            };

            var allrecords = profileLines.Search(queryFilter, true);

            IFeature line = null;
            while ((line = allrecords.NextFeature()) != null)
            {
                if (line.Shape is IPointCollection points)
                {
                    var from = points.Point[0];
                    var to = points.Point[points.PointCount - 1];

                    result.Add(new ProfileLine
                    {
                        PointFrom = new ProfilePoint { X = from.X, Y = from.Y },
                        PointTo = new ProfilePoint { X = to.X, Y = to.Y },
                        Id = line.OID
                    });
                }

            }

            return result;
        }
    }
}