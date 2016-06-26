using UnityEngine;
using UnityEngine.Audio;

namespace gkh
{
    public enum SoundCategory
    {
        General, Player, Enemy, Objects, Gui
    }

    public class SoundManager : MonoBehaviour
    {
        #region static members
        // whether we have initialized static members
        static bool initialized;
        /* the current music volume in int value; this is used to give easy access in the options menu */
        static int volume = 100;
        /* the current music volume in float value */
        static float volumeF = 1f;
        #endregion


        #region fields & properties
        public AudioMixer mixer;
        // the volume at which the sound fx level will be set at launch
        [Range(0, 100)]
        public int initialVolume = 100;


        public float Volume { get { return volume; } }
        public float VolumeF { get { return volumeF; } }
        public bool IsMuted { get { return Volume == 0; } }
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            if (!initialized)
            {
                initialized = true;

                // try to get a previous volume setting from prefs
                int volumePref = (PlayerPrefs.GetInt(Prefs.SoundVol, -1));
                if (volumePref > -1) 
                    volume =  volumePref;
                else
                {
                    volume = initialVolume;
                    PlayerPrefs.SetInt(Prefs.SoundVol, volume);
                }
            }
            ChangeVolume(0);
        }
        #endregion


        #region volume mgmt
        public void IncreaseVol() { ChangeVolume(1); }
        public void DecreaseVol() { ChangeVolume(-1); }

        void ChangeVolume(int mult)
        {
            volume = Mathf.Clamp(volume + (10 * mult), 0, 100);
            volumeF = (float)volume / 100.0f;

            // store the updated volume in prefs
            PlayerPrefs.SetInt(Prefs.SoundVol, volume);
        }
        #endregion
    }
}