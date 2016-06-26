using UnityEngine;
using System.Collections;

namespace gkh
{
    public class WarningMessage : MonoBehaviour 
    {
        #region
        bool showing = false;
        // whether red or white text is showing
        bool blinkStateA = true;

        // the total length of time the warning will show
        float showDuration = 4f;
        float currentShowTime = 0;

        // the amount of time between red/white text alternations
        float blinkInterval = .3f;
        float blinkTimer = 0f;

        string text = "Warning!";
        Rect textRect = new Rect();

        SoundPlayer snd;
        #endregion


        #region
        void Awake()
        {
            snd = GetComponent<SoundPlayer>();
        }

        void Update()
        {
            if (Globals.Paused || !showing) return;

            // check if it is time to shop showing
            currentShowTime -= Time.deltaTime;
            if (currentShowTime <= 0)
            {
                showing = false;
                currentShowTime = 0;
                text = "";
            }

            // update the blink counter and/or status
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0)
            {
                blinkTimer = blinkInterval;
                blinkStateA = !blinkStateA;
            }
        }

        void OnGUI()
        {
            if (!showing) return;

            // set the skins based on the current blink state
            GUISkin skinTop = blinkStateA ? Skins.Warning_Top : Skins.Warning_TopRed;
            GUISkin skinBtm = blinkStateA ? Skins.Warning_Btm : Skins.Warning_BtmRed;

            GUI.skin = skinBtm;
            Vector3 artStrSize = GUI.skin.GetStyle("label").CalcSize(new GUIContent(text));
            textRect = new Rect(Screen.width/2 - artStrSize.x/2, 450, artStrSize.x, artStrSize.y);


            GUI.Label(textRect, text);
            GUI.skin = skinTop;
            GUI.Label(textRect, text);
        }
        #endregion


        #region
        public void Show(string text) 
        { 
            snd.PlaySound(SjSounds.asteroidWarning);
            this.text = text;
            currentShowTime = showDuration;
            blinkTimer = blinkInterval;
            blinkStateA = true;
            showing = true; 
        }
        #endregion
    }
}