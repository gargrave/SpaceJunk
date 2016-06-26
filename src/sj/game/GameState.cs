using UnityEngine;

namespace gkh
{
    public class GameState : MonoBehaviour
    {
        #region
        public AbstractMenu loseMenu;

        SoundPlayer snd;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            snd = GetComponent<SoundPlayer>();
        }
        #endregion


        #region
        public void EndGameInLoss()
        {
            if (loseMenu == null)
            {
                Debug.LogWarning("GameState.loseMenu is null!");
                return;
            }
            Globals.player.Deactivate();
            Globals.pauser.Unpause(false);
            Globals.pauser.Deactivate();
            Globals.gameTimer.StopTimer();
            Globals.gameTimer.ResetTimerState();

            // stop the music and play the "lose" cue
            Globals.music.FadeOutMusicA(MusicManager.MAX_FADE_SPD);
            snd.PlaySound(SjSounds.endGameLose01, 1f);

            loseMenu.Show(true);
            loseMenu.Activate();
        }
        #endregion
    }
}