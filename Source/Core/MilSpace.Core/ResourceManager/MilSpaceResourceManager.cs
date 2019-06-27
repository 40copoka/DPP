﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MilSpace.Core.MilSpaceResourceManager
{
    public class MilSpaceResourceManager
    {

        private static readonly string localizationregistryKey = "Localization";
        private static Logger logger = Logger.GetLoggerEx("MilSpaceResourceManager");
        private static string defailtValueNotLocalized = "<not localized>";

        ResourceManager innerObject;

        public MilSpaceResourceManager(string sourceName, CultureInfo cultireInfo)
        {

            var pathToAssembly = Helper.GetRegistryValue(localizationregistryKey);
            innerObject = ResourceManager.CreateFileBasedResourceManager(sourceName, $"{pathToAssembly}", null);

            System.Threading.Thread.CurrentThread.CurrentUICulture = cultireInfo;
        }

        public string GetLocalization(string key, string defailtcvalue = null)
        {
            try
            {
                var result = innerObject.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
                return result;
            }
            catch
            {
                defailtcvalue = string.IsNullOrEmpty(defailtcvalue) ? defailtValueNotLocalized : defailtcvalue;
                logger.ErrorEx($"There is no localization key \"{key}\". Default value {defailtcvalue} was returned.");
                return defailtcvalue;
            }
        }
    }
}