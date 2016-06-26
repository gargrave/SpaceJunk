using UnityEngine;

namespace gkh
{
    public enum OS { Win, Mac, Linux }

    public class Settings : MonoBehaviour 
    {
        #region fields & properties
        public static string GameVersion { get; private set; }
        public static bool IsDevBuild { get; private set; }
        public static bool ShowWatermark { get; set; }
        public static OS CurrentOS { get; private set; }
        public static bool IsWinOrMac { get { return CurrentOS != OS.Linux; } }

        static bool initialized;

        public string gameVersion = "1.0";
        public bool isDevBuild;
        public bool showWatermark;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                GameVersion = gameVersion;
                IsDevBuild = isDevBuild;
                ShowWatermark = showWatermark;
                SetCurrentOS();
            }
        }
        #endregion


        static void SetCurrentOS()
        {
            string os = SystemInfo.operatingSystem;
            if (os.StartsWith("Win")) CurrentOS = OS.Win;
            else if (os.StartsWith("Mac")) CurrentOS = OS.Mac;

            if (Settings.IsDevBuild)
                Debug.Log(string.Format("OS: {0}\n", os));
        }
    }
}