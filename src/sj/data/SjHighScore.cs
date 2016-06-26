using UnityEngine;

namespace gkh
{
    public class SjHighScore : MonoBehaviour
    {
        #region static members
        public static int highScore = 0;
        static bool initialized = false;
        #endregion


        #region
        // a dev switch to force the clearing of high score on launch
        // only applies when running the game from the editor
        public bool dev_ClearSavedScore = false;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            if (!initialized)
            {
                // dev switch for overwriting high score on launch
                if (Application.isEditor && dev_ClearSavedScore)
                    PlayerPrefs.DeleteKey(Prefs.HighScore);
                else
                {
                    // check for a previously-stored high score
                    int highScorePref = PlayerPrefs.GetInt(Prefs.HighScore, 0);
                    if (highScorePref > 0)
                        highScore = highScorePref;
                }
                initialized = true;
            }
        }
        #endregion


        #region score mgmt
        public static int CheckForNewHighScore(int score)
        {
            // if the submitted score is higher, updated to the
            // high score to that value
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(Prefs.HighScore, highScore);
                PlayerPrefs.Save();
            }
            return highScore;
        }
        #endregion
    }
}