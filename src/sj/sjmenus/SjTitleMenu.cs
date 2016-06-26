using UnityEngine;

namespace gkh
{
    public class SjTitleMenu : AbstractMenu, IParentMenu
    {
        #region
        public AbstractChildMenu optionsMenu;
        public AbstractChildMenu aboutMenu;
        public Texture2D titleLogo;
        public Transform titleLogoTrans;

        public float logoCenter = 1.6f;
        public float logoDriftSpeed = 1.3f;
        public float logoDriftRange = .1f;

        int itemNewGame, itemOptions, itemAbout, itemExit;
        float angle, logoY, currentY;
        #endregion


        #region
        protected override void Start()
        {
            base.Start();
            title = "";
            sndClick01 = SjSounds.menuClick01;
            Activate();
            Show();

            skinItemUnselTop = Skins.TitleMenu01_Top;
            skinItemUnselBtm = Skins.TitleMenu01_Btm;
            skinItemSelTop = Skins.TitleMenuSel01_Top;
            skinItemSelBtm = Skins.TitleMenuSel01_Btm;

            if (optionsMenu != null)
                optionsMenu.SetParentMenu(this);
            if (aboutMenu != null)
                aboutMenu.SetParentMenu(this);
        }

        protected override void Update()
        {
            // if we are showing the 'about' screen,
            // check for any key press to hide it
            if (isHidden)
            {
                if (optionsMenu.isActive && 
                    InputWrapper.Pressed(Control.MenuBack))
                    HideChildMenu(optionsMenu);
                else if (aboutMenu.isActive && Input.anyKeyDown)
                    HideChildMenu(aboutMenu);
            }
            // otherwise, do the normal update
            else
            {
                if (isActive)
                {
                    base.Update();


                }
            }
            // current position
            var v3 = titleLogoTrans.position;
            // update the logo's position
            titleLogoTrans.position = new Vector3(
                v3.x, currentY, v3.z);

            // update the sine wave
            currentY = logoCenter + Mathf.Cos(angle) * logoDriftRange;
            angle += logoDriftSpeed * Time.deltaTime;
        }
        #endregion


        #region menu mgmt
        protected override void BuildItemList()
        {
            int itemCounter = 0;
            itemNewGame = itemCounter++;
            itemOptions = itemCounter++;
            itemAbout = itemCounter++;
            itemExit = itemCounter++;

            items.Add("New Game");
            items.Add("Options");
            items.Add("About");
            items.Add("Exit");
        }

        protected override void OnItemSelected()
        {
            // start a new game
            if (sel == itemNewGame)
            {
                snd.PlaySound(SjSounds.pieceClear01);
                Globals.effectMgr.DoScreenShake();
                Globals.music.FadeOutMusicB(MusicManager.DEFAULT_FADE_SPD * .5f);
                FadeScreen.SetFadeSpeed(FadeScreen.DEFAULT_FADE_SPD * .45f);
                FadeScreen.SetPrefadeDelay(.5f);
                FadeScreen.SetFinishDelay(.75f);
                FadeOut();
            }
            // show the options menu
            else if (sel == itemOptions)
                ShowChildMenu(optionsMenu);
            // show the 'about' screen
            else if (sel == itemAbout)
                ShowChildMenu(aboutMenu);
            // exit the game
            else if (sel == itemExit)
                Application.Quit();
        }
        #endregion


        #region IParentScreen
        public void ShowChildMenu(AbstractChildMenu menu)
        {
            if (menu == null)
            {
                Debug.Log("'menu' is null.");
                return;
            }
            if (!menu.ParentIsSet())
            {
                Debug.Log("Parent is not set for menu: " + menu);
                return;
            }
            Hide();
            menu.Show(true);
            menu.Activate();
        }

        public void HideChildMenu(AbstractChildMenu menu)
        {
            if (menu == null)
            {
                Debug.Log("menu is null");
                return;
            }
            if (!menu.ParentIsSet())
            {
                Debug.Log("Parent is not set for menu: " + menu);
                return;
            }
            controlsAreLocked = true;
            menu.Hide();
            menu.Deactivate();
            Show();
        }
        #endregion


        #region fade mgmt
        protected override void OnFadeFinished()
        {
            SjMusic.Snap2.TransitionTo(0.01f);
            Globals.sceneMaster.ChangeScene(Scenes.Game);
        }
        #endregion
    }
}