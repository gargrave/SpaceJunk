using UnityEngine;

namespace gkh
{
    public class FadeScreen : MonoBehaviour
    {
        #region static members
        public const float MIN_FADE_SPD = .1f;
        public const float MAX_FADE_SPD = 7.5f;
        public const float DEFAULT_FADE_SPD = 1.75f;
        #endregion


        #region
        /* the texture to use for the background */
        public Texture _bgImage;

        public static bool IsActive { get; private set; }
        public static bool IsFinished { get; private set; }

        // whether static members have been initialized
        static bool initialized = false;
        /* the texture to use for the background */
        static Texture bgImage;
        static Color currentFadeColor = Color.black;
        static float currentFadeSpeed = 1.75f;
        /* the amount of alpha to use for the bg image's color */
        [Range(0, 1)]
        static float alpha = 1f;
        // the current fade color
        static Color bgColor = Color.black;
        /* the amount of time this instance will wait before beginning its fade, 
         * (i.e. "hold a solid color for x seconds") */
        static float preFadeDelay = 0f;
        /* the amount of time this instance will wait after its fade 
         * has finished before declaring itself officially finished (i.e. "black time") */
        static float finishDelay = .35f;
        /* the counter to keep track of how long delays have been running */
        static float counter = 0f;
        /* whether this instance's fade speed has been manually set */
        static bool useDefaultFadeSpeed = true;
        /* whether this instance is currently in "black time" */
        static bool finishing = false;
        /* flags for each direction of fading */
        static bool fadingToZero = false, fadingToOne = false;

        // dev tools
        static float devReminderInterval = 1f;
        static float devReminderTimer = 0f;
        #endregion


        #region
        void Awake()
        {
            if (!initialized)
            {
                bgImage = _bgImage;
                Color c = currentFadeColor;
                bgColor = new Color(c.r, c.g, c.b, alpha);
                currentFadeSpeed = DEFAULT_FADE_SPD;
                initialized = true;
                IsActive = false;
            }
        }

        void Update()
        {
            if (!IsActive) return;
            
            /* if we are in "black time," simply increment the counter */
            if (preFadeDelay > 0f) UpdatePreFading();
            else if (finishing) UpdateFinishing();
            /* if fading in/out, update the alpha and color as needed */
            else if (fadingToOne) UpdateFadeToSolid();
            else if (fadingToZero) UpdateFadeToClear();

            Color c = currentFadeColor;
            bgColor = new Color(c.r, c.g, c.b, alpha);


            // adding this periodic console reminder to give warnings
            // when the screen is running
            if (Application.isEditor)
            {
                devReminderTimer -= Time.deltaTime;
                if (devReminderTimer <= 0)
                {
                    devReminderTimer = devReminderInterval;
                    Debug.Log("FadeScreen is active: " + Time.time);
                }
            }
        }

        void OnGUI()
        {
            if (!IsActive) return;

            /* the draw black overlay at the current color/alpha */
            GUI.depth = GuiDepth.FadeScreen;
            GUI.color = bgColor;
            GUI.DrawTexture(Screenie.RectFull, bgImage);
        }
        #endregion


        #region fade mgmt
        static void UpdatePreFading()
        {
            counter += Time.deltaTime;
            if (counter >= preFadeDelay)
                preFadeDelay = 0f;
        }

        static void UpdateFinishing()
        {
            counter += Time.deltaTime;
            if (counter >= finishDelay)
            {
                counter = 0f;
                finishing = false;
                IsFinished = true;
            }
        }

        static void UpdateFadeToSolid()
        {
            alpha += currentFadeSpeed * Time.deltaTime;
            if (alpha >= 1f)
            {
                alpha = 1f;
                finishing = true;
            }
        }

        static void UpdateFadeToClear()
        {
            alpha -= currentFadeSpeed * Time.deltaTime;
            if (alpha <= 0f)
            {
                alpha = 0f;
                finishing = false;
                IsFinished = true;
                IsActive = false;
            }
        }
        #endregion


        #region screen mgmt
        public static void FadeToClear()
        {
            alpha = 1f;
            ResetFadeSettings();
            fadingToZero = true;
            fadingToOne = false;
            IsActive = true;
        }

        public static void FadeToSolid()
        {
            alpha = 0f;
            ResetFadeSettings();
            fadingToOne = true;
            fadingToZero = false;
            IsActive = true;
        }

        public static void FreezeBlackScreen()
        {
            ResetFadeSettings();
            IsActive = true;
            fadingToZero = false;
            fadingToOne = false;
            alpha = 1f;
            bgColor = Color.black;
        }
        #endregion


        #region setters
        static void ResetFadeSettings()
        {
            if (useDefaultFadeSpeed)
                currentFadeSpeed = DEFAULT_FADE_SPD;
            useDefaultFadeSpeed = true;

            Color c = currentFadeColor;
            bgColor = new Color(c.r, c.g, c.b, alpha);
            counter = 0;
            finishing = false;
            IsFinished = false;
        }

        public static void SetFadeSpeed(float speed)
        {
            currentFadeSpeed = Mathf.Clamp(speed, MIN_FADE_SPD, MAX_FADE_SPD);
            useDefaultFadeSpeed = false;
        }

        public static void SetPrefadeDelay(float prefade)
        { preFadeDelay = Mathf.Clamp(prefade, 0f, 5f); }

        public static void SetFinishDelay(float delay)
        { finishDelay = Mathf.Clamp(delay, .1f, 5f); }
        #endregion
    }
}