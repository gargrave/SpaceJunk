using UnityEngine;

namespace gkh
{
    public class SjCountdownMenu : AbstractMenu
    {
        #region fields & properties
        // the strings/rects for displaying the tutorial state
        string tutStrA;
        const string tutStrB = "(Press ESC to toggle)";
        Rect tutRectA = new Rect();
        Rect tutRectB = new Rect();

        // y-position of the top tutorial string
        public int tutY = 360;
        #endregion


        #region MonoBehaviour
        protected override void Start()
        {
            base.Start();

            title = "";
            // check if there is an existing pref for showing tuts
            bool showTut = true;
            int tutPref = PlayerPrefs.GetInt(Prefs.ShowTutorials, -1);
            if (tutPref == 0)
                showTut = false;
            SetShowTutorial(showTut);
        }

        protected override void Update()
        {
            base.Update();
            if (!isActive || isHidden) return;

            if (InputWrapper.Pressed(Control.Pause))
                ToggleShowTutorials();
        }

        protected override void OnGUI()
        {
            if (!isActive || isHidden) return;

            // draw the background texture
            GUI.depth = GuiDepth.TextMenuBottom;
            GUI.color = bgColor;
            GUI.DrawTexture(Screenie.RectFull, bgImage, ScaleMode.StretchToFill);
            GUI.color = Color.white;


            // draw the title string
            GUI.depth = GuiDepth.TextMenuTop;
            if (titleRect.width == 0)
                SetUpTitleString();

            GUI.skin = skinTitleBtm;
            GUI.Label(titleRect, title);
            GUI.skin = skinTitleTop;
            GUI.Label(titleRect, title);


            if (tutRectA.width == 0)
                SetUpTutorialStrings();

            GUI.skin = skinItemUnselBtm;
            GUI.Label(tutRectA, tutStrA);
            GUI.skin = skinItemSelBtm;
            GUI.Label(tutRectB, tutStrB);
            GUI.skin = skinItemUnselTop;
            GUI.Label(tutRectA, tutStrA);
            GUI.skin = skinItemSelTop;
            GUI.Label(tutRectB, tutStrB);
        }
        #endregion


        #region string building methods
        void SetUpTutorialStrings()
        {
            GUI.skin = skinItemUnselBtm;
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(tutStrA));
            float x, y, w, h;
            x = (Screen.width / 2) - (size.x / 2);
            y = tutY - ((size.y * STR_HEIGHT_OFFSET) / 2);
            w = size.x;
            h = size.y;
            tutRectA = new Rect(x, y, w, h);

            GUI.skin = skinItemSelBtm;
            size = GUI.skin.label.CalcSize(new GUIContent(tutStrB));
            x = (Screen.width / 2) - (size.x / 2);
            y = tutRectA.y + (size.y);
            w = size.x;
            h = size.y;
            tutRectB = new Rect(x, y, w, h);
        }
        #endregion


        #region menu mgmt
        void ToggleShowTutorials() { SetShowTutorial(!Globals.ShowTutorial); }

        void SetShowTutorial(bool tut)
        {
            SjTutorialMenu.SetShowTutorials(tut);
            tutStrA = tut ? "Show Tutorials: Yes" : "Show Tutorials: No";
            tutRectA.width = 0;
            snd.PlaySound(SjSounds.menuClick01);
        }

        // abstract methods (no implementation needed for this class)
        protected override void BuildItemList() {}
        protected override void OnItemSelected() {}
        #endregion
    }
}