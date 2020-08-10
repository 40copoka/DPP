﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilSpace.Configurations.DemStorages
{
    public class DemStorages
    {
        private static string mdbFolder = "DB";
        private static string sourceFolder = "SRC";
        private static string processFolder = "Process";
        private static string scriptsFolder = "Scripts";



        public string SrtmStorage { get; internal set; }
        public string SrtmStorageExternal { get; set; }
        public string SentinelStorage { get; internal set; }
        public string SentinelStorageExternal { get; set; }
        public string SentinelDownloadStorage => Path.Combine(SentinelStorage, sourceFolder);
        public string SentinelDownloadStorageExternal => Path.Combine(SentinelStorageExternal, sourceFolder);
        public string SentinelStorageDBExternal => Path.Combine(SentinelStorageExternal, mdbFolder);
        public string ScihubMetadataApi { get; internal set; }
        public string ScihubProductsApi { get; internal set; }
        public string ScihubUserName { get; internal set; }
        public string ScihubPassword { get; internal set; }
        public string GptExecPath { get; internal set; }
        public string GptCommandsPath { get; internal set; }
        public string GdalInfoExecPath { get; internal set; }
        public string SnaphuExecPath { get; internal set; }

        public bool CheckFolderAsStorage(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            return Directory.Exists(Path.Combine(path, mdbFolder)) &&
                Directory.Exists(Path.Combine(path, sourceFolder)) &&
                Directory.Exists(Path.Combine(path, processFolder)) &&
                Directory.Exists(Path.Combine(path, scriptsFolder));
        }
    }
}
