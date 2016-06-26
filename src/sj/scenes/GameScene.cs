using UnityEngine;

namespace gkh
{
    public class GameScene : SceneScript
    {
        #region
        Pauser pauser;
        GameTimer gameTimer;
        SjCountdownMenu countdownMenu;
        SoundPlayer snd;
        float startTimer;
        int startTimerInt;
        bool onFinalTick;
        #endregion


        #region MonoBehaviour
        protected override void Start()
        {
            base.Start();

            // just an extra test to make sure we get the loading
            // call when starting this scene from the editor
            if (Application.isEditor && firstLoad)
            {
                firstLoad = false;
                OnSceneLoaded(Scenes.Game);
            }
        }

        protected override void Update()
        {
            base.Update();
            if (!FadeScreen.IsFinished)
                return;

            if (startTimer > 0)
                IncrementCountdown();
            // if the game-timer has expired, end the game in loss
            else if (gameTimer.Expired)
                Globals.gameState.EndGameInLoss();
        }
        #endregion


        #region game states
        void BeginGame()
        {
            countdownMenu.Hide();
            countdownMenu.Deactivate();

            // unpause all necessary game objects
            pauser.Activate();
            Globals.pieceSpawner.enabled = true;
            Globals.player.Unfreeze();
            gameTimer.StartTimer();
        }
        #endregion


        #region scene mgmt
        void IncrementCountdown()
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0)
                BeginGame();
            else if (startTimer > 1f)
                DoCountdownTick();
            else if (!onFinalTick)
                DoFinalTick();
        }

        void DoCountdownTick()
        {
            var timer = (int)Mathf.Ceil(startTimer - 1);
            countdownMenu.SetTitle(timer.ToString());

            // play the timer "ticking" sound
            if (startTimerInt != timer)
                snd.PlaySound(SjSounds.timerTick01, .5f);
            if (startTimerInt == 1)
                SjMusic.Snap1.TransitionTo(4f);
            startTimerInt = timer;
        }

        void DoFinalTick()
        {
            onFinalTick = true;
            snd.PlaySound(SjSounds.countdownEnd);
            countdownMenu.SetTitle("Go!");
        }

        public override void OnSceneLoaded(int scene)
        {
            if (scene == Scenes.Game)
            {
                GameMode.ResetTimeAttack();
                pauser = Globals.pauser;
                gameTimer = Globals.gameTimer;
                snd = GetComponent<SoundPlayer>();

                pauser.ResetState();
                pauser.Deactivate();
                Globals.sceneMaster.CurrentScene = this;

                // pre-pause all necessary game objects
                gameTimer.SetTimer(1, 30);
                gameTimer.StopTimer();
                Globals.pieceSpawner.enabled = false;
                Globals.asteroidSpawner.Deactivate();
                Globals.player.Freeze();

                // play the music
                var mcd = new MusicClipDetails(SjMusic.GetMusic());

                SjMusic.Snap2.TransitionTo(0.01f);
                Globals.music.PlayMusicAByDetails(mcd);

                // begin fade-in
                FadeScreen.SetFadeSpeed(FadeScreen.MAX_FADE_SPD);
                FadeScreen.FadeToClear();

                startTimer = 4f;
                startTimerInt = (int)Mathf.Ceil(startTimer);
                onFinalTick = false;

                snd.PlaySound(SjSounds.chargeUp, .45f);
                countdownMenu = GetComponentInChildren<SjCountdownMenu>();
                countdownMenu.SetTitle(Mathf.Ceil(startTimer - 1).ToString());
                countdownMenu.Show();
                countdownMenu.Activate();
            }
        }

        public override void OnSceneExit()
        {
            Globals.pauser.ResetState();
        }
        #endregion
    }
}