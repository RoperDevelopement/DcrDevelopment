using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public sealed class MasterCategories : SerializedObjectDictionary<MasterCategory>
    {
        public override string DirectoryPath
        {
            get
            { //return string.Format("{0}\\Local\\EdocsUsaBmC\\Config\\Master Categories", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)); } }
                return BinUtilities.BinMonMasterCategoriesFolder;
            }
        }
        //{ get { return @"Config\Master Categories"; } }

        static readonly MasterCategories _Instance = new MasterCategories();
        public static MasterCategories Instance
        { get { return _Instance; } }

        public const string ROUTINE_TITLE = "ROUTINE";
        public const string STAT_TITLE = "STAT";
        public const string READY_TITLE = "READY";
        public const string PROBLEM_TITLE = "PROBLEM";
    }
}
