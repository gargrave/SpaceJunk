using UnityEngine;

namespace gkh
{
    public class DevGUI : MonoBehaviour
    {
        #region fields and properties
        /* skins for title diplay */
        public GUISkin skinTextTop;
        public GUISkin skinTextBottom;

        public string nameOfGame = "Name of Game";
        public string version = "YYYY-MMDD";
        public string company = "Company";

        Rect textRect;
        string text;
        #endregion


        #region MonoBehaviour
        void OnGUI()
        {
            if (!Settings.ShowWatermark) return;

            GUI.depth = GuiDepth.AbsoluteTop;
            // set up the text rect on our first time through
            if (string.IsNullOrEmpty(text))
                BuildString();

            GUI.skin = skinTextBottom;
            GUI.Label(textRect, text);
            GUI.skin = skinTextTop;
            GUI.Label(textRect, text);
        }

        void BuildString()
        {
            text = string.Format("{0} dev build {1} | (c) 2015 {2}", 
                nameOfGame, version, company);

            GUI.skin = skinTextBottom;
            var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
            textRect = new Rect(
                Screen.width - textSize.x - 6,
                Screen.height - textSize.y + 2,
                textSize.x, textSize.y);
        }
        #endregion
    }
}
