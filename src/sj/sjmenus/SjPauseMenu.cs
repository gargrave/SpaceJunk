using UnityEngine;
using System.Collections;

namespace gkh
{
    public class SjPauseMenu : AbstractMenu, IParentMenu
    {
        #region
        public AbstractChildMenu optionsMenu;
        public AbstractChildMenu aboutMenu;
        int itemResume, itemNewGame, itemOptions, itemAbout, itemQuit;
        #endregion


        #region MonoBehaviour
        protected override void Start()
        {
            base.Start();
            title = "Paused";
            isHidden = true;
            sndClick01 = SjSounds.menuClick01;
            Deactivate();
            Hide();

            skinItemUnselTop = Skins.Text01_Top;
            skinItemUnselBtm = Skins.Text01_Btm;
            skinItemSelTop = Skins.TextSel01_Top;
            skinItemSelBtm = Skins.TextSel01_Btm;

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
            }
            // otherwise, do the normal update
            else
            {
                if (isActive)
                    base.Update();
            }
        }
        #endregion


        #region menu mgmt
        protected override void BuildItemList()
        {
            int itemCounter = 0;
            itemResume = itemCounter++;
            itemOptions = itemCounter++;
            itemAbout = itemCounter++;
            itemQuit = itemCounter++;

            items.Add("Resume");
            items.Add("Options");
            items.Add("About");
            items.Add("Quit");
        }

        protected override void OnItemSelected()
        {
            // resume the game
            if (sel == itemResume)
                Globals.pauser.Unpause();
            // show the options menu
            else if (sel == itemOptions)
                ShowChildMenu(optionsMenu);
            else if (sel == itemAbout)
                ShowChildMenu(aboutMenu);
            // quit the game early
            else if (sel == itemQuit)
                Globals.gameState.EndGameInLoss();
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
            Globals.pauser.Deactivate();
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
            Globals.pauser.Activate();
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