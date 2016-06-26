using UnityEngine;
using UnityEngine.Audio;

namespace gkh
{
    public class MusicManager : MonoBehaviour
    {
        #region static members
        public const float DEFAULT_FADE_SPD = 1f;
        public const float MAX_FADE_SPD = 100f;

        // whether we have initialized static members
        static bool initialized;
        /* the current music volume in int value; this is used to give easy access in the options menu */
        static int volume = 100;
        /* the current music volume in float value */
        static float volumeF = 1f;
        #endregion


        #region fields & properties
        public AudioMixer mixer;
        // the two AudioSource instances
        public AudioSource audioA, audioB;
        // the volume at which the music level will be set at launch
        [Range(0, 100)]
        public int initialVolume = 70;
        // the volume mult for when music is in "ducked" mode
        [Range(0, 1)]
        public float duckedMult = .65f;


        public bool IsDucked { get; private set; }
        public int Volume { get { return volume; } }
        public bool NoMusicPlaying { get { return !audioA.isPlaying && !audioB.isPlaying; } }


        MusicClipDetails detailsA, detailsB;//, detailsC;
        AudioClip musicA, musicB;
        /* the multiplier to use for whether the audio is currently ducked */
        float currentDuckedMult = 1f;
        /* the speed of fade in/out operations */
        float fadeInSpeed = .5f, fadeOutSpeed = .5f;
        /* whether each music clip is currently fading in */
        bool fadingInMusicA, fadingInMusicB;
        /* whether each music clip is currently fading out */
        bool fadingOutMusicA, fadingOutMusicB;
        /* the current fade position for each audio clip */
        float musicACurrentFade = 1f, musicBCurrentFade = 1f;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            if (!initialized)
            {
                initialized = true;

                // try to get a previous volume setting from prefs
                int volumePref = (PlayerPrefs.GetInt(Prefs.MusicVol, -1));
                if (volumePref > -1)
                    volume =  volumePref;
                else
                {
                    volume = initialVolume;
                    PlayerPrefs.SetInt(Prefs.MusicVol, volume);
                }
            }
            UpdateVolume(0);
        }

        void Update()
        {
            if (Globals.Paused)
                return;

            UpdateFadesForMusicA();
            UpdateFadesForMusicB();
            UpdateVolume(0);
        }

        void UpdateFadesForMusicA()
        {
            if (fadingInMusicA)
            {
                musicACurrentFade += fadeInSpeed * Time.deltaTime;
                if (musicACurrentFade >= 1f)
                {
                    fadingInMusicA = false;
                    musicACurrentFade = 1f;
                }
            }
            else if (fadingOutMusicA)
            {
                musicACurrentFade -= fadeOutSpeed * Time.deltaTime;
                if (musicACurrentFade <= 0)
                {
                    fadingOutMusicA = false;
                    musicACurrentFade = 0;
                    audioA.Stop();
                }
            }
        }

        void UpdateFadesForMusicB()
        {
            if (fadingInMusicB)
            {
                musicBCurrentFade += fadeInSpeed * Time.deltaTime;
                if (musicBCurrentFade >= 1f)
                {
                    fadingInMusicB = false;
                    musicBCurrentFade = 1f;
                }
            }
            else if (fadingOutMusicB)
            {
                musicBCurrentFade -= fadeOutSpeed * Time.deltaTime;
                if (musicBCurrentFade <= 0)
                {
                    fadingOutMusicB = false;
                    musicBCurrentFade = 0;
                    audioB.Stop();
                }
            }
        }
        #endregion


        #region fade in
        public void FadeInMusicA(float mult)
        {
            if (audioB.isPlaying)
                FadeOutMusicB();

            musicACurrentFade = 0f;
            fadingInMusicA = true;
            fadingOutMusicA = false;
        }
        public void FadeInMusicA() { FadeInMusicA(DEFAULT_FADE_SPD); }

        public void FadeInMusicB(float mult)
        {
            if (audioA.isPlaying)
                FadeOutMusicA();

            musicBCurrentFade = 0f;
            fadingInMusicB = true;
            fadingOutMusicB = false;
        }

        public void FadeInMusicB() { FadeInMusicB(DEFAULT_FADE_SPD); }
        #endregion


        #region fade out
        public void FadeOutMusicA(float spd)
        {
            if (!audioA.isPlaying || fadingOutMusicA)
                return;

            fadeOutSpeed = spd;
            fadingInMusicA = false;
            fadingOutMusicA = true;
        }

        public void FadeOutMusicA() { FadeOutMusicA(DEFAULT_FADE_SPD); }

        public void FadeOutMusicB(float spd)
        {
            if (!audioB.isPlaying || fadingOutMusicB)
                return;

            fadeOutSpeed = spd;
            fadingInMusicB = false;
            fadingOutMusicB = true;
        }

        public void FadeOutMusicB() { FadeOutMusicB(DEFAULT_FADE_SPD); }

        public void FadeOutAllMusic()
        {
            FadeOutMusicA(DEFAULT_FADE_SPD);
            FadeOutMusicB(DEFAULT_FADE_SPD);
        }
        #endregion


        #region volume mgmt
        public void IncreaseVol() { UpdateVolume(1); }
        public void DecreaseVol() { UpdateVolume(-1); }

        void UpdateVolume(int mult)
        {
            volume = Mathf.Clamp(volume + (10 * mult), 0, 100);
            volumeF = (float)volume / 100.0f;

            audioA.volume = volumeF * musicACurrentFade * currentDuckedMult;
            audioB.volume = volumeF * musicBCurrentFade * currentDuckedMult;

            // store the updated volume in prefs
            PlayerPrefs.SetInt(Prefs.MusicVol, volume);
        }
        #endregion


        #region music mgmt
        public void SetDucked(bool d)
        {
            if (d == IsDucked)
                return;

            IsDucked = d;
            currentDuckedMult = IsDucked ? duckedMult : 1f;
            UpdateVolume(0);
        }

        public void PlayMusicA()
        {
            if (audioA.isPlaying) return;

            if (audioB.isPlaying)
                FadeOutMusicB();

            if (detailsA.FadesIn)
            {
                FadeInMusicA();
                audioA.volume = 0;
            }
            else
            {
                fadingInMusicA = false;
                musicACurrentFade = 1f;
            }

            fadingOutMusicA = false;
            audioA.loop = true;
            audioA.clip = musicA;
            audioA.Play();
        }

        public void PlayMusicB()
        {
            if (audioB.isPlaying) return;

            if (audioA.isPlaying)
                FadeOutMusicA();

            if (detailsB.FadesIn)
            {
                FadeInMusicB();
                audioB.volume = 0;
            }
            else
            {
                fadingInMusicB = false;
                musicBCurrentFade = 1f;
            }

            fadingOutMusicB = false;
            audioB.loop = true;
            audioB.clip = musicB;
            audioB.Play();
        }
        #endregion


        #region
        public void PlayMusicAByDetails(MusicClipDetails details)
        {
            detailsA = details;
            audioA.loop = !details.HasNextTrack;
            musicA = details.clip;
            PlayMusicA();
        }

        public void PlayMusicBByDetails(MusicClipDetails details)
        {
            detailsB = details;
            audioB.loop = !details.HasNextTrack;
            musicB = details.clip;
            PlayMusicB();
        }
        #endregion
    }
}