


using System;

namespace Performance.EFCore
{
    public class AWBuildVersion
    {
        public byte SystemInformationID { get; set; }
        public string Database_Version { get; set; }
        public DateTime VersionDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
