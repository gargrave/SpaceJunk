using UnityEngine;

namespace gkh
{
    public class HUD : MonoBehaviour
    {
        #region fields & properties
        GameTimer timer;

        bool ScoreNeedsUpdating { get { return Globals.score.Current != prevScoreValue; } }
        bool TimeNeedsUpdating { get { return timer.Sec != prevSecValue; } }
        bool LevelNeedsUpdating { get { return GameMode.CurrentLevel != prevLevelValue; } }
        bool PowerNeedsUpdating { get { return Globals.player.PowerLevel != prevPowerValue; } }

        bool showTimeInRed = false;
        SoundPlayer snd;
        #endregion


        #region score-text settings
        public int scoreX = 10;
        public int scoreY = -5;

        string scoreText;
        string prevScoreText;
        Rect scoreRect = new Rect();
        Vector2 scoreTextSize;
        int prevScoreValue = -1;
        #endregion


        #region timer-text settings
        public int timerY = -5;

        string timerText;
        string prevTimerText;
        Rect timerRect = new Rect();
        Vector2 timerTextSize;
        int prevSecValue = -1;

        public int TimerRectY { get { return (int)timerRect.y; } }
        #endregion


        #region level-text settings
        public int levelX = 10;
        public int levelY = -5;
        
        string levelText;
        string prevLevelText;
        Rect levelRect = new Rect();
        Vector2 levelTextSize;
        int prevLevelValue = -1;
        #endregion


        #region level-text settings
        public int powerY = -5;
        
        string powerText = "Engines: 100%";
        string prevPowerText;
        Rect powerRect = new Rect();
        Vector2 powerTextSize;
        int prevPowerValue = 100;
        #endregion


        #region MonoBehaviour
        void Start()
        {
            timer = Globals.gameTimer;
            UpdateScoreString();
            UpdateTimeString();
            snd = GetComponent<SoundPlayer>();
        }

        void Update()
        {
            if (ScoreNeedsUpdating)
                UpdateScoreString();
            if (TimeNeedsUpdating)
                UpdateTimeString();
            if (LevelNeedsUpdating)
                UpdateLevelString();
            if (PowerNeedsUpdating)
                UpdatePowerString();
        }

        void OnGUI()
        {
            GUI.depth = GuiDepth.HUD;
            GUI.skin = Skins.Text01_Btm;

            // set up score text
            if (scoreText != prevScoreText)
                scoreTextSize = GUI.skin.label.CalcSize(new GUIContent(scoreText));
            prevScoreText = scoreText;
            scoreRect.x = scoreX;
            scoreRect.y = scoreY;
            scoreRect.width = scoreTextSize.x;
            scoreRect.height = scoreTextSize.y;

            // set up level text
            if (levelText != prevLevelText)
                levelTextSize = GUI.skin.label.CalcSize(new GUIContent(levelText));
            prevLevelText = levelText;
            levelRect.x = levelX;
            levelRect.y = levelY;
            levelRect.width = levelTextSize.x;
            levelRect.height = levelTextSize.y;


            // set up timer text
            if (timerText != prevTimerText)
            {
                GUI.skin = showTimeInRed ? Skins.Timer01_BtmRed : Skins.Timer01_Btm;
                timerTextSize = GUI.skin.label.CalcSize(new GUIContent(timerText));
                prevTimerText = timerText;
                timerRect.width = timerTextSize.x;
                timerRect.height = timerTextSize.y;
                timerRect.x = Screen.width / 2 - timerRect.width/2;
                timerRect.y = timerY;
                GUI.skin = Skins.Text01_Btm;
            }


            // set up "power" text
            if (powerText != prevPowerText)
                powerTextSize = GUI.skin.label.CalcSize(new GUIContent(powerText));
            prevPowerText = powerText;
            powerRect.width = powerTextSize.x;
            powerRect.height = powerTextSize.y;
            powerRect.x = Screen.width - powerRect.width - 6;
            powerRect.y = powerY;


            // draw bottom strings
            GUI.Label(scoreRect, scoreText);
            GUI.Label(levelRect, levelText);
            GUI.Label(powerRect, powerText);
            // draw top strings
            GUI.skin = Skins.Text01_Top;
            GUI.Label(scoreRect, scoreText);
            GUI.Label(levelRect, levelText);
            GUI.Label(powerRect, powerText);


            // draw the timer string
            GUI.skin = showTimeInRed ? Skins.Timer01_BtmRed : Skins.Timer01_Btm;
            GUI.Label(timerRect, timerText);
            GUI.skin = showTimeInRed ? Skins.Timer01_TopRed : Skins.Timer01_Top;
            GUI.Label(timerRect, timerText);
        }
        #endregion


        #region string methods
        void UpdateScoreString()
        {
            var score = Globals.score.Current;
            scoreText = string.Format("Score: {0}", score);
            prevScoreValue = score;
        }

        void UpdateTimeString()
        {
            var sec = timer.Sec;
            // check if we are nearing the end of the time limit
            // if so, we should switch to the red font
            showTimeInRed = (timer.Min == 0 && sec < 20 && sec % 2 != 0);
            if (showTimeInRed)
                snd.PlaySound(SjSounds.timerTick02, .65f);

            timerText = string.Format("{0}:{1}",
                timer.Min,
                Utils.PadString(sec.ToString(), 2, "0"));
            prevSecValue = sec;
        }

        void UpdateLevelString()
        {
            var lvl = GameMode.CurrentLevel;
            levelText = "Level: " + lvl;
            prevLevelValue = lvl;
        }

        void UpdatePowerString()
        {
            prevPowerValue = Globals.player.PowerLevel;
            powerText = string.Format("Engines: {0}%", prevPowerValue);
        }
        #endregion
    }
}