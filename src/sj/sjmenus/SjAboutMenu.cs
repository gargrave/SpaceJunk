using UnityEngine;

namespace gkh
{
    public class SjAboutMenu : AbstractChildMenu
    {
        #region const strings
        const string copyString = "(c) 2015 We Did It Games";

        const string devString = "Designed and Developed by:  We Did It Games  (@wediditgames)";
        const string wwString = "Art Stuff:  Wayne Watrach  (@wwatrach)";
        const string ghString = "Code Stuff & Sound Editing:  Gabe Hargrave  (@gkhargrave)";

        const string musicString = "Musical Goodness:  Chip Hessner";
        const string musicURL = "(chiphessner.bandcamp.com)";

        const string returnString = "Press any key to return";
        #endregion


        #region
        string versionString;
        Rect versionRect;
        Rect returnRect;
        Rect copyRect;
        #endregion


        #region
        protected override void Start()
        {
            base.Start();
            title = "About  Space  Junk";
            isHidden = true;
            hasSelectableItems = false;

            skinItemUnselTop = Skins.Text01_Top;
            skinItemUnselBtm = Skins.Text01_Btm;
            skinItemSelTop = Skins.TextSel01_Top;
            skinItemSelBtm = Skins.TextSel01_Btm;

            Deactivate();
            Hide();
        }

        protected override void Update()
        {
            if (!isActive || isHidden) return;

            if (!controlsAreLocked && Input.anyKeyDown)
                parentScreen.HideChildMenu(this);
            else if (controlsAreLocked && !Input.anyKeyDown)
                controlsAreLocked = false;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (!isActive || isHidden) return;

            GUI.depth = GuiDepth.TextMenuTop;
            if (Mathf.Approximately(versionRect.width , 0))
                SetUpCopyString();

            /********************************************************
             * set up the "return" string
             ********************************************************/
            if (Mathf.Approximately(returnRect.width , 0))
            {
                Vector3 returnStrSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(returnString));
                float x, y, w, h;
                x = (Screen.width / 2) - (returnStrSize.x / 2);
                y = Screen.height - returnStrSize.y;
                w = returnStrSize.x;
                h = returnStrSize.y;
                returnRect = new Rect(x, y, w, h);
            }

            // bottom level strings
            GUI.skin = Skins.Text01_Btm;
            GUI.Label(copyRect, copyString);
            GUI.Label(versionRect, versionString);
            GUI.Label(returnRect, returnString);
            // top-level strings
            GUI.skin = Skins.Text01_Top;
            GUI.Label(versionRect, versionString);
            GUI.Label(copyRect, copyString);
            GUI.Label(returnRect, returnString);
        }
        #endregion


        #region menu mgmt
        void SetUpCopyString()
        {
            GUI.skin = Skins.Text01_Btm;
            // "version" string
            versionString = "v. " + Settings.GameVersion;
            var versionStrSize = GUI.skin.label.CalcSize(new GUIContent(versionString));
            float x, y, w, h;
            x = (Screen.width / 2) - (versionStrSize.x / 2);
            y = titleRect.y + (titleRect.height / 2) + versionStrSize.y;
            w = versionStrSize.x;
            h = versionStrSize.y;
            versionRect = new Rect(x, y, w, h);

            // "copyright" string
            var copyStrSize = GUI.skin.label.CalcSize(new GUIContent(copyString));
            x = (Screen.width / 2) - (copyStrSize.x / 2);
            y = versionRect.y + (copyStrSize.y * .75f);
            w = copyStrSize.x;
            h = copyStrSize.y;
            copyRect = new Rect(x, y, w, h);
        }

        protected override void BuildItemList()
        {
            items.Add(devString);
            items.Add(wwString);
            items.Add(ghString);
            items.Add(SPACER);
            items.Add(musicString);
            items.Add(musicURL);
        }

        protected override void OnItemSelected(){}
        #endregion
    }
}