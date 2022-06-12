using System;
using System.Collections.Generic;
using System.IO;

namespace SharedSNCode
{
    internal class business
    {
        public static bool itsok => finalcheck();
        /*
        public static bool itsok {
            get {
                if (runonce)
                {
                    return itsok;
                }
                else
                {
                    itsok = finalcheck();
                    runonce = true;
                    return itsok;
                }
            }
            set => itsok = value;
        }

        private static bool runonce;
        */

        private const string SteamApiName = "steam_api64.dll";
        private const int SteamApiLength = 220000;

        private static readonly string _folder = Environment.CurrentDirectory;

        private static readonly HashSet<string> HappyFiles = new HashSet<string>()
        {
            "steam_api64.cdx",
            "steam_api64.ini",
            "steam_emu.ini",
            "valve.ini",
            "SmartSteamEmu.ini",
            "Subnautica_Data/Plugins/steam_api64.cdx",
            "Subnautica_Data/Plugins/steam_api64.ini",
            "Subnautica_Data/Plugins/steam_emu.ini",
            "Profile/SteamUserID.cfg",
            "Profile/Stats/Achievements.Bin",
            "Profile/VALVE/SteamUserID.cfg",
            "Profile/VALVE/Stats/Achievements.Bin",
            "launcher.bat",
            "chuj.cdx",
        };

        private static bool crowofjudge()
        {
            string steamDll = Path.Combine(_folder, SteamApiName);
            bool steamStore = File.Exists(steamDll);
            if (steamStore)
            {
                FileInfo fileInfo = new FileInfo(steamDll);
                if (fileInfo.Length > SteamApiLength)
                {
                    return true;
                }
            }

            foreach (string file in HappyFiles)
            {
                if (File.Exists(Path.Combine(_folder, file)))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool finalcheck()
        {
            if (crowofjudge())
            {
                return false;
            }

            if (IsSteam())
            {
                return true;
            }

            if (IsEpic())
            {
                return true;
            }

            if (IsMSStore())
            {
                return true;
            }

            return false;
        }

        private static bool IsSteam()
        {
            return PlatformServicesUtils.IsRuntimePluginDllPresent("CSteamworks");
        }

        private static bool IsEpic()
        {
            return Array.IndexOf(Environment.GetCommandLineArgs(), "-EpicPortal") != -1;
        }

        private static bool IsMSStore()
        {
            return PlatformServicesUtils.IsRuntimePluginDllPresent("XGamingRuntimeThunks");
        }
    }
}
