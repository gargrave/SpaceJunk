using UnityEngine;

namespace gkh
{
    public class SjLoseMenu : AbstractMenu
    {
        const float HIGH_SCORE_NOTIFY_BLINK_INTERVAL = 1.4f;


        #region
        // skins for displaying the score string
        public GUISkin skinSubtitleTop;
        public GUISkin skinSubtitleBtm;
        public int scoreY = 460;

        int itemRestart, itemOptions, itemAbout, itemTitle;

        // high score display strings
        string highScoreNotifyStr;
        Rect highScoreNotifyRect;
        float highScoreNotifyBlinkTimer;
        bool highScoreNofityBlinkState = true;
        // score display strings
        string scoreStr;
        Rect scoreRect;
        // high score display strings
        string highScoreStr;
        Rect highScoreRect;
        #endregion


        #region MonoBehaviour
        protected override void Start()
        {
            base.Start();
            title = "You're Fired!";
            isHidden = true;
            sndClick01 = SjSounds.menuClick01;
            Deactivate();
            Hide();

            skinItemUnselTop = Skins.Text01_Top;
            skinItemUnselBtm = Skins.Text01_Btm;
            skinItemSelTop = Skins.TextSel01_Top;
            skinItemSelBtm = Skins.TextSel01_Btm;
        }

        protected override void Update()
        {
            base.Update();
            if (!isActive || isHidden) return;
        }
        #endregion


        #region GUI methods
        protected override void OnGUI()
        {
            if (!isActive || isHidden) return;
            base.OnGUI();

            if (highScoreNotifyBlinkTimer > 0)
            {
                highScoreNotifyBlinkTimer -= Time.deltaTime;
                if (highScoreNotifyBlinkTimer <= 0)
                {
                    highScoreNotifyBlinkTimer = HIGH_SCORE_NOTIFY_BLINK_INTERVAL;
                    highScoreNofityBlinkState = !highScoreNofityBlinkState;
                }
            }
            
            // check if we need to update dimensions
            if (scoreRect.width == 0)
                SetUpStrings();
            
            DrawBottomStrings();
            DrawTopStrings();
        }

        void DrawBottomStrings()
        {
            GUI.skin = skinSubtitleBtm;
            GUI.Label(scoreRect, scoreStr);
            GUI.Label(highScoreRect, highScoreStr);

            if (highScoreNofityBlinkState)
                GUI.Label(highScoreNotifyRect, highScoreNotifyStr);
        }

        void DrawTopStrings()
        {
            GUI.skin = skinSubtitleTop;
            GUI.Label(scoreRect, scoreStr);
            GUI.Label(highScoreRect, highScoreStr);

            if (highScoreNofityBlinkState)
                GUI.Label(highScoreNotifyRect, highScoreNotifyStr);
        }

        void SetUpStrings()
        {
            GUI.skin = skinSubtitleBtm;

            // set up the high score notification string
            var size = GUI.skin.label.CalcSize(new GUIContent(highScoreNotifyStr));
            highScoreNotifyRect.x = (Screen.width / 2) - (size.x / 2);
            highScoreNotifyRect.y = titleY + (size.y * .6f);
            highScoreNotifyRect.width = size.x;
            highScoreNotifyRect.height = size.y;

            // set up the current score string
            size = GUI.skin.label.CalcSize(new GUIContent(scoreStr));
            scoreRect.x = (Screen.width / 2) - (size.x / 2);
            scoreRect.y = scoreY - (size.y * .5f);
            scoreRect.width = size.x;
            scoreRect.height = size.y;

            // set up the high score string
            size = GUI.skin.label.CalcSize(new GUIContent(highScoreStr));
            highScoreRect.x = (Screen.width / 2) - (size.x / 2);
            highScoreRect.y = scoreRect.y + (size.y);
            highScoreRect.width = size.x;
            highScoreRect.height = size.y;
        }
        #endregion


        #region menu mgmt
        protected override void BuildItemList()
        {
            var itemCounter = 0;
            itemRestart = itemCounter++;
            itemTitle = itemCounter++;

            items.Add("Restart");
            items.Add("Return to Title");
        }

        public override void Show(bool resetSel)
        {
            base.Show(resetSel);

            var score = Globals.score.Current;
            var highScore = SjHighScore.CheckForNewHighScore(score);
            // set width to 0, so that it will be updated on next OnGUI() call
            scoreRect.width = 0;
            scoreStr = "Final Score: " + score;
            highScoreStr = "High Score: " + highScore;

            // if a new high score has been achieved, set that string
            if (score == highScore && score > 0)
            {
                highScoreNotifyStr = "New High Score!";
                highScoreNotifyBlinkTimer = HIGH_SCORE_NOTIFY_BLINK_INTERVAL;
            }
            else highScoreNotifyStr = "";
        }

        protected override void OnItemSelected()
        {
            // resume the game
            if (sel == itemRestart)
                Application.LoadLevel(Scenes.Game);
            // return to title screen
            else if (sel == itemTitle)
            {
                Globals.music.FadeOutMusicA(MusicManager.DEFAULT_FADE_SPD * .55f);
                FadeScreen.SetFadeSpeed(FadeScreen.DEFAULT_FADE_SPD * .5f);
                FadeOut();
            }
        }
        #endregion


        #region fade mgmt
        protected override void OnFadeFinished()
        {
            Globals.sceneMaster.ChangeScene(Scenes.Title);
        }
        #endregion
    }
}