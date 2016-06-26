using UnityEngine;

namespace gkh
{
    public class SplashLogo : AbstractMenu 
    {
        #region 
        protected override void Start()
        {
            Activate();
            Show();
        }

        protected override void OnGUI(){}
        #endregion


        #region
        void OnSplashFinalFrame()
        {
            FadeScreen.SetFadeSpeed(FadeScreen.DEFAULT_FADE_SPD);
            FadeScreen.SetFinishDelay(.25f);
            FadeOut();
        }
        #endregion


        #region fade mgmt
        protected override void OnFadeFinished()
        {
            Globals.sceneMaster.ChangeScene(Scenes.Title);
        }
        #endregion


        #region menu mgmt
        protected override void BuildItemList(){}
        protected override void OnItemSelected(){}
        #endregion
    }
}