using UnityEngine;

namespace gkh
{
    public class SjOptionsMenu : AbstractChildMenu
    {
        #region
        int itemFullscreen, itemMusicVol, itemSoundVol, itemReturn;
        #endregion


        #region
        protected override void Start()
        {
            base.Start();
            skipFirstFrame = true;
            title = "Options";
            isHidden = true;
            sndClick01 = SjSounds.menuClick01;
            Deactivate();
            Hide();

            skinItemUnselTop = Skins.Text01_Top;
            skinItemUnselBtm = Skins.Text01_Btm;
            skinItemSelTop = Skins.TextSel01_Top;
            skinItemSelBtm = Skins.TextSel01_Btm;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (!isActive || isHidden) return;

            if (skipFirstFrame)
            {
                skipFirstFrame = false;
                items[itemMusicVol] = "Music Vol: " + Globals.music.Volume;
                items[itemSoundVol] = "Sound Vol: " + Globals.sound.Volume;
            }
        }
        #endregion


        #region menu mgmt
        public override void Hide() 
        { 
            PlayerPrefs.Save();
            base.Hide();
        }

        protected override void BuildItemList()
        {
            int itemCounter = 0;
            itemFullscreen = itemCounter++;
            // add a spacer
            itemCounter++;
            itemMusicVol = itemCounter++;
            itemSoundVol = itemCounter++;
            // add a spacer
            itemCounter++;
            itemReturn = itemCounter++;

            items.Add("Toggle Fullscreen");
            items.Add(SPACER);
            items.Add("Music Vol: 100");
            items.Add("Sound Vol: 100");
            items.Add(SPACER);
            items.Add("Return");
        }

        protected override void OnItemSelected()
        {
            // toggle fullscreen
            if (sel == itemFullscreen)
                Screen.SetResolution(1280, 720, !Screen.fullScreen);
            // return to the previous screen
            else if (sel == itemReturn)
                parentScreen.HideChildMenu(this);
        }

        // called when the left key is pressed
        protected override void OnLeftPressed() 
        {
            // decrease music vol
            if (sel == itemMusicVol)
            {
                Globals.music.DecreaseVol();
                snd.PlaySound(sndClick01);
            }
            // decrease sound vol
            else if (sel == itemSoundVol)
            {
                Globals.sound.DecreaseVol();
                snd.PlaySound(sndClick01);
            }
            items[itemMusicVol] = "Music Vol: " + Globals.music.Volume;
            items[itemSoundVol] = "Sound Vol: " + Globals.sound.Volume;
        }
        // called when the right key is pressed
        protected override void OnRightPressed() 
        {
            // increase music vol
            if (sel == itemMusicVol)
            {
                Globals.music.IncreaseVol();
                snd.PlaySound(sndClick01);
            }
            // increase sound vol
            else if (sel == itemSoundVol)
            {
                Globals.sound.IncreaseVol();
                snd.PlaySound(sndClick01);
            }
            items[itemMusicVol] = "Music Vol: " + Globals.music.Volume;
            items[itemSoundVol] = "Sound Vol: " + Globals.sound.Volume;
        }
        #endregion
    }
}