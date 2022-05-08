using System;

namespace ModInstalLogger.Management
{
    public class Moddata
    {
        public string ID { get; set; }
        public string Displayname { get; set; }
        public string Author { get; set; }
        public bool Enabled { get; set; }
        public Version Version { get; set; }
    }
}