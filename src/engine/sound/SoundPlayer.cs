using UnityEngine;
using UnityEngine.Audio;

namespace gkh
{
    public class SoundPlayer : MonoBehaviour
    {
        #region
        AudioSource audioSource;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0f;
        }
        #endregion

        #region sound mgmt
        public void PlaySound(AudioClip clip) { PlaySound(clip, 1f); }

        public void PlaySound(AudioClip clip, float vol)
        {
            if (Globals.sound.IsMuted) return;
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip, vol * Globals.sound.VolumeF);
        }
        #endregion
    }
}