using UnityEngine;

namespace gkh
{
    public class MusicClipDetails
    {
        public const string SELF = "self";


        #region fields & properties
        // the clip to play from this instance
        public AudioClip clip;
        // whether the clip should fade in when it begins playing
        public bool FadesIn { get; private set; }
        // the name of the clip that should follow this one (if any)
        public string NextClip { get; private set; }

        // whether this instance has a track that will play when it ends
        // if not, this clip will loop
        public bool HasNextTrack { get { return NextClip != SELF; } }
        #endregion


        #region ctors
        public MusicClipDetails(AudioClip clip) :
            this(clip, false, SELF) {}

        public MusicClipDetails(AudioClip clip, bool fadesIn, string nextClip)
        {
            this.clip = clip;
            this.FadesIn = fadesIn;
            this.NextClip = nextClip;
        }
        #endregion
    }
}
