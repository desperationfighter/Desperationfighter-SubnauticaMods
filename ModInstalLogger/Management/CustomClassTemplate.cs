using System.Collections.Generic;

namespace ModInstalLogger.Management
{
    public class LogSchema
    {
        public string Type { get; set; }
        public string Date { get; set; }
        public List<Moddata> Mods { get; set; }
    }

    public class Moddata
    {
        public string ID { get; set; }
        public string Displayname { get; set; }
        public string Author { get; set; }
        public string Version_Major { get; set; }
        public string Version_Minor { get; set; }
        public string Version_Build { get; set; }
        public string Version_Revision { get; set; }
        public string Enabled { get; set; }
        public string Version_combined { get => $"{Version_Major}.{Version_Minor}.{Version_Build}.{Version_Revision}"; }
    }
}
