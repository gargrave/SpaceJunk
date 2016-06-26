using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class SjTutorialMenu : AbstractMenu
    {
        #region static members
        const float EXIT_BLINK_INTERVAL = .75f;

        // the index of the last tutorial
        static int tutCounter;
        public static int tut_CatchPieces = tutCounter++;
        public static int tut_ShootPieces = tutCounter++;
        public static int tut_ClearGrid = tutCounter++;
        public static int maxTutIndex = tutCounter;
        public static int lastTutShown;

        static string[] titles = { "Welcome to the team!", "We've got a smart one here!", "One last thing." };
        // a reference to the active instance, so it can be called statically
        static SjTutorialMenu instance;
        // a timer to show a tutorial on a delay
        // if this is greater than zero, we are counting down to showing a tut
        static float tutorialDelayTimer;
        #endregion


        #region fields & properties
        // the minimum amount of time before the player can close the screen
        // used to prevent accidental closing
        public float minShowTime = 1.0f;

        // the current tutorial number we are displaying
        int tutorialIndex;
        // timer to track how long we have been showing this instance
        float currentShowTime;

        // the "return" strings; all tutorials will show these
        string exitString = "";
        Rect exitRect;
        bool readyToExit;
        float exitBlinkTimer;
        bool exitBlinkState = true;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            instance = this;
            hasSelectableItems = false;
        }

        protected override void Start()
        {
            base.Start();
            tutorialIndex = 0;
            Hide();

            skinItemUnselTop = Skins.Tutorial_Top;
            skinItemUnselBtm = Skins.Tutorial_Btm;
        }

        protected override void Update()
        {
            base.Update();
            // if we have a delayed tutorial counter, update that before proceeding
            if (tutorialDelayTimer > 0)
            {
                tutorialDelayTimer -= Time.deltaTime;
                if (tutorialDelayTimer <= 0)
                {
                    tutorialDelayTimer = 0;
                    ShowTutorial();
                }
            }

            if (!isActive || isHidden) return;

            // if the countdown has expired, blink the exit string
            if (readyToExit)
            {
                exitBlinkTimer -= Time.deltaTime;
                if (exitBlinkTimer <= 0)
                {
                    exitBlinkTimer = EXIT_BLINK_INTERVAL;
                    exitBlinkState = !exitBlinkState;
                }
            }
            // count down the pre-delay before showing the exit string
            else
            {
                currentShowTime -= Time.deltaTime;
                if (currentShowTime <= 0)
                {
                    readyToExit = true;
                    currentShowTime = 0;
                    exitRect.width = 0;
                    exitString = "Press any key to continue.";
                    exitBlinkTimer = EXIT_BLINK_INTERVAL;
                    exitBlinkState = true;
                }
            }
           

            if (!controlsAreLocked && Input.anyKeyDown && readyToExit)
            {
                if (!Input.GetMouseButtonDown(0) && 
                    !InputWrapper.Pressed(Control.Shoot) && 
                    !InputWrapper.Pressed(Control.Pause))
                {
                    Hide();
                    Globals.pauser.Activate();
                    Globals.pauser.Unpause();
                }
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (!isActive || isHidden) return;

            if (readyToExit)
            {
                if (exitRect.width == 0)
                    SetUpExitString();

                if (exitBlinkState)
                {
                    GUI.skin = skinItemUnselBtm;
                    GUI.Label(exitRect, exitString);
                    GUI.skin = skinItemUnselTop;
                    GUI.Label(exitRect, exitString);
                }
            }
        }
        #endregion


        #region string building methods
        void SetUpTutorialStrings()
        {
            GUI.skin = skinItemUnselBtm;
            for (int i = 0; i < itemCount; i++)
            {
                Vector3 size = GUI.skin.label.CalcSize(new GUIContent(items[i]));
                float x, y;
                // center the x-position
                x = (Screen.width / 2) - (size.x / 2);
                float itemHalf = ((float)itemCount) * .5f;
                y = ((Screen.height / 2) + (lineHeight * i)) - (itemHalf * lineHeight);
                // then store the new rect based on these figures
                itemUnselRects.Add(new Rect(x, y + itemOffsetY, size.x, size.y));
            }
        }

        void SetUpExitString()
        {
            GUI.skin = skinItemUnselBtm;
            Vector2 exitSize = GUI.skin.label.CalcSize(new GUIContent(exitString));
            float x, y, w, h;
            x = (Screen.width / 2) - (exitSize.x / 2);
            y = Screen.height - (exitSize.y * 1.3f);
            w = exitSize.x;
            h = exitSize.y;
            exitRect = new Rect(x, y, w, h);
        }
        #endregion


        #region menu mgmt
        public override void Show(bool resetSel)
        {
            base.Show(resetSel);
            Globals.pauser.Pause(false);
            Globals.pauser.Deactivate();
            Activate();
            readyToExit = false;

            title = titles[tutorialIndex];
            titleRect.width = 0;
            exitString = "";
            currentShowTime = minShowTime;
            exitBlinkTimer = EXIT_BLINK_INTERVAL;

            items = TutorialData.GetTutStrings(tutorialIndex++);
            itemCount = items.Count;
            itemUnselRects = new List<Rect>();

            lastTutShown = tutorialIndex;
            if (tutorialIndex >= maxTutIndex)
                SetShowTutorials(false);
        }

        // abstract
        protected override void BuildItemList(){}
        protected override void OnItemSelected(){}
        #endregion


        #region tut-showing methods
        public static void ShowTutorial() { ShowTutorial(0); }

        public static void ShowTutorial(float delay)
        {
            if (instance == null)
            {
                Debug.LogError("SjTutorialMenu.instance is null!");
                return;
            }
            if (delay > 0)
                tutorialDelayTimer = Mathf.Clamp(delay, .01f, 1.5f);
            else instance.Show(false);
        }

        public static void SetShowTutorials(bool tut)
        {
            Globals.ShowTutorial = tut;
            int tutPref = tut ? 1 : 0;
            PlayerPrefs.SetInt(Prefs.ShowTutorials, tutPref);
            PlayerPrefs.Save();
        }
        #endregion
    }
}