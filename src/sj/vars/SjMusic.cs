using UnityEngine;
using UnityEngine.Audio;

namespace gkh
{
    public class SjMusic : MonoBehaviour
    {
        #region
        public AudioClip musicTitle01;
        public AudioClip music01;
        public AudioClip music02;
        public AudioClip music03;
        public AudioClip music04;

        public AudioMixerSnapshot snap1;
        public AudioMixerSnapshot snap2;
        #endregion


        #region
        static bool initialized;

        public static AudioClip Title01 { get; private set; }
        public static AudioClip Game01 { get; private set; }
        public static AudioClip Game02 { get; private set; }
        public static AudioClip Game03 { get; private set; }
        public static AudioClip Game04 { get; private set; }

        public static AudioMixerSnapshot Snap1 { get; private set; }
        public static AudioMixerSnapshot Snap2 { get; private set; }
        #endregion


        #region
        void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                Title01 = musicTitle01;
                Game01 = music01;
                Game02 = music02;
                Game03 = music03;
                Game04 = music04;
                Snap1 = snap1;
                Snap2 = snap2;
            }
        }
        #endregion

        static int last;
        public static AudioClip GetMusic()
        {
            var r = last++;
            switch (r)
            {
                case 1:
                    return Game02;
                case 2:
                    return Game03;
                case 3:
                    last = 0;
                    return Game04;
                default:
                    return Game01;
            }
        }
    }
}