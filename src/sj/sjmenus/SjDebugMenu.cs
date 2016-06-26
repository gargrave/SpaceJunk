using UnityEngine;

namespace gkh
{
    public class SjDebugMenu : AbstractMenu
    {
        #region
        int itemTimer, itemAsteroids;
        #endregion


        #region MonoBehaviour
        protected override void Start()
        {
            base.Start();
            title = "Debuggin'!";
            isHidden = true;
            sndClick01 = SjSounds.menuClick01;
            Deactivate();
            Hide();

            skinItemUnselTop = Skins.Text01_Top;
            skinItemUnselBtm = Skins.Text01_Btm;
            skinItemSelTop = Skins.TextSel01_Top;
            skinItemSelBtm = Skins.TextSel01_Btm;
        }
        #endregion


        #region menu mgmt
        protected override void BuildItemList()
        {
            int itemCounter = 0;
            itemTimer = itemCounter++;
            itemAsteroids = itemCounter++;

            items.Add(GetTimerString());
            items.Add(GetAsteroidString());
        }

        protected override void OnItemSelected()
        {
            // resume the game
            if (sel == itemAsteroids)
                ToggleAsteroidsEnabled();
            else if (sel == itemTimer)
                ToggleTimerDisabled();
        }
        #endregion


        #region switching methods
        public override void Show(bool resetSel)
        {
            base.Show(resetSel);
            UpdateStrings();
        }

        void UpdateStrings()
        {
            items[itemTimer] = GetTimerString();
            items[itemAsteroids] = GetAsteroidString();
            itemUnselRects.Clear();
            itemSelRects.Clear();
        }

        void ToggleTimerDisabled()
        {
            var stopTimer = Globals.gameTimer.running;
            if (stopTimer) Globals.gameTimer.StopTimer();
            else Globals.gameTimer.StartTimer();
            UpdateStrings();
        }

        void ToggleAsteroidsEnabled()
        {
            var asteroidsDisabled = !SjDebug.asteroidsEnabled;
            if (asteroidsDisabled) Globals.asteroidSpawner.Activate();
            else Globals.asteroidSpawner.Deactivate();
            UpdateStrings();
        }
        #endregion


        string GetTimerString() { return string.Format("Timer disabled: {0}", SjDebug.timerDisabled); }
        string GetAsteroidString() { return string.Format("Asteroids enabled: {0}", SjDebug.asteroidsEnabled); }
    }
}