using System;
using System.Collections.Generic;
using System.IO;

namespace ModInstalLogger_BZ.Patches
{
    internal class business
    {
        internal const string SteamApiName = "steam_api64.dll";
        internal const int SteamApiLength = 220000;

        private static readonly string _folder = Environment.CurrentDirectory;

        internal static readonly HashSet<string> HappyFiles = new HashSet<string>()
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

        internal static bool crowofjudge()
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

        internal static bool itsok()
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
