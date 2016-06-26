using UnityEngine;

namespace gkh
{
    public class Pauser : MonoBehaviour
    {
        #region
        // the menu to show when the game is paused
        public AbstractMenu pauseMenu;
        // the menu to show for dev-options pause
        public AbstractMenu devMenu;

        // the sound(s) to player when the game is (un)paused
        public AudioClip sndPause;
        public AudioClip sndUnpause;

        public bool IsActive { get; private set; }

        bool keyLock = false;

        SoundPlayer snd;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            IsActive = true;

            snd = GetComponent<SoundPlayer>();
            if (snd == null)
                Debug.LogWarning("Pauser does not have a SoundPlayer instance.");

            if (pauseMenu == null)
                Debug.LogWarning("Pauser does not have a pauseMenu.");
            if (devMenu == null)
                Debug.LogWarning("Pauser does not have a devMenu.");
        }

        void Update()
        {
            if (!IsActive) 
                return;

            if (!keyLock && InputWrapper.Pressed(Control.Pause))
            {
                if (Globals.Paused)
                    Unpause();
                else Pause();
            }
            else if (!keyLock && InputWrapper.Pressed(Control.DevOptions))
            {
                if (Globals.Paused)
                    Unpause();
                else PauseWithDevMenu();
            }
            else if (keyLock && !InputWrapper.Pressed(Control.Pause))
                keyLock = false;
        }
        #endregion


        #region state mgmt
        public void Activate()
        {
            IsActive = true;
            keyLock = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Pause() { Pause(true); }

        public void Pause(bool showPauseMenu)
        {
            snd.PlaySound(SjSounds.menuPause01);
            if (showPauseMenu && pauseMenu != null)
            {
                pauseMenu.Activate();
                pauseMenu.Show(true);
            }
            Globals.music.SetDucked(true);
            Globals.Paused = true;
        }

        public void PauseWithDevMenu()
        {
            if (!Settings.IsDevBuild || devMenu == null)
                return;

            Pause(false);
            devMenu.Activate();
            devMenu.Show(true);
        }

        public void Unpause(bool playSound = true)
        {
            if (playSound)
                snd.PlaySound(SjSounds.menuUnpause01);
            ResetState();
        }

        public void ResetState()
        {
            if (pauseMenu != null)
            {
                pauseMenu.Deactivate();
                pauseMenu.Hide();
            }
            if (devMenu != null)
            {
                devMenu.Deactivate();
                devMenu.Hide();
            }
            Globals.music.SetDucked(false);
            Globals.Paused = false;
        }
        #endregion
    }
}