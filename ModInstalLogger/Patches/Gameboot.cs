//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for List
using System.Collections.Generic;
//for File Operations
using System.IO;
//for Assembly info
using System.Reflection;
//for JSON Operation
using Oculus.Newtonsoft.Json;

namespace ModInstalLogger.Patches
{
    class Gameboot
    {
        public static string GetModPath()
        {
            //string path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Subnautica\\IMOSL_Test.json";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return path;
        }

        public static string GetModListFile()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game);
            return file;
        }

        public static string GetPath_ModListChange_Added()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game_Added);
            return file;
        }

        public static string GetPath_ModListChange_Removed()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game_Removed);
            return file;
        }

        public static void ModcheckforGame()
        {
            List<Moddata> mymodlist = LoggerLogic.GetrunningMods();
            /*
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            List<datamodel> _data = new List<datamodel>();
            _data.Add(new datamodel()
            {
                Type = "SN",
                Date = timeStamp,
                Mods = mymodlist
            });
            */

            if (File.Exists(GetModListFile()))
            {
                List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(GetModListFile()));
                LoggerLogic.ModCompare(ExistingModList, mymodlist, GetPath_ModListChange_Added(), GetPath_ModListChange_Removed(), "Gamewide");
            }

            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;
            string json = JsonConvert.SerializeObject(mymodlist, myformat);

            //write string to file
            System.IO.File.WriteAllText(GetModListFile(), json);

        }
    }
}
