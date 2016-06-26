using UnityEngine;

namespace gkh
{
    public class TitleScene : SceneScript
    {
        #region MonoBehaviour
        protected override void Start() 
        {
            base.Start();
            // just an extra test to make sure we get the loading
            // call when starting this scene from the editor
            if (firstLoad)
            {
                OnSceneLoaded(Scenes.Title);
                firstLoad = false;
            }
        }
        #endregion


        #region scene mgmt
        public override void OnSceneLoaded(int scene)
        {
            if (scene == Scenes.Title)
            {
                Globals.sceneMaster.CurrentScene = this;
                MusicClipDetails mcd = new MusicClipDetails(SjMusic.Title01);
                SjMusic.Snap1.TransitionTo(0.01f);
                Globals.music.PlayMusicBByDetails(mcd);

                if (firstLoad)
                    FadeScreen.SetFadeSpeed(FadeScreen.MAX_FADE_SPD);
                FadeScreen.FadeToClear();
            }
        }

        public override void OnSceneExit()
        {
        }
        #endregion
    }
}