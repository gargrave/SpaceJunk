using UnityEngine;

namespace gkh
{
    public class SplashScene : SceneScript
    {
        #region MonoBehaviour
        protected override void Start() 
        {
            base.Start();
            // just an extra test to make sure we get the loading
            // call when starting this scene from the editor
            if (firstLoad)
            {
                OnSceneLoaded(Scenes.Splash);
                firstLoad = false;
            }
        }
        #endregion


        #region scene mgmt
        public override void OnSceneLoaded(int scene)
        {
            if (scene == Scenes.Splash)
            {
                Globals.sceneMaster.CurrentScene = this;

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