using UnityEngine;

namespace gkh
{
    public class Skins : MonoBehaviour
    {
        #region
        // basic skins
        public GUISkin _text01_Top;
        public GUISkin _text01_Btm;
        public static GUISkin Text01_Top { get; set; }
        public static GUISkin Text01_Btm { get; set; }

        public GUISkin _textSel01_Top;
        public GUISkin _textSel01_Btm;
        public static GUISkin TextSel01_Top { get; set; }
        public static GUISkin TextSel01_Btm { get; set; }


        // title menu skins
        public GUISkin _titleMenu01_Top;
        public GUISkin _titleMenu01_Btm;
        public static GUISkin TitleMenu01_Top { get; set; }
        public static GUISkin TitleMenu01_Btm { get; set; }

        public GUISkin _titleMenuSel01_Top;
        public GUISkin _titleMenuSel01_Btm;
        public static GUISkin TitleMenuSel01_Top { get; set; }
        public static GUISkin TitleMenuSel01_Btm { get; set; }


        // timer skins
        public GUISkin _timer01_Top;
        public GUISkin _timer01_TopRed;
        public GUISkin _timer01_Btm;
        public GUISkin _timer01_BtmRed;
        public static GUISkin Timer01_Top { get; set; }
        public static GUISkin Timer01_TopRed { get; set; }
        public static GUISkin Timer01_Btm { get; set; }
        public static GUISkin Timer01_BtmRed { get; set; }


        // warning message skins
        public GUISkin _warning_Top;
        public GUISkin _warning_TopRed;
        public GUISkin _warning_Btm;
        public GUISkin _warning_BtmRed;
        public static GUISkin Warning_Top { get; set; }
        public static GUISkin Warning_TopRed { get; set; }
        public static GUISkin Warning_Btm { get; set; }
        public static GUISkin Warning_BtmRed { get; set; }


        // tutorial text skins
        public GUISkin _tutorial_Top;
        public GUISkin _tutorial_Btm;
        public static GUISkin Tutorial_Top { get; set; }
        public static GUISkin Tutorial_Btm { get; set; }
        #endregion


        #region
        void Awake()
        {
            Text01_Top = _text01_Top;
            Text01_Btm = _text01_Btm;

            TextSel01_Top = _textSel01_Top;
            TextSel01_Btm = _textSel01_Btm;


            TitleMenu01_Top = _titleMenu01_Top;
            TitleMenu01_Btm = _titleMenu01_Btm;

            TitleMenuSel01_Top = _titleMenuSel01_Top;
            TitleMenuSel01_Btm = _titleMenuSel01_Btm;


            Timer01_Top = _timer01_Top;
            Timer01_TopRed = _timer01_TopRed;
            Timer01_Btm = _timer01_Btm;
            Timer01_BtmRed = _timer01_BtmRed;


            Warning_Top = _warning_Top;
            Warning_TopRed = _warning_TopRed;
            Warning_Btm = _warning_Btm;
            Warning_BtmRed = _warning_BtmRed;


            Tutorial_Top = _tutorial_Top;
            Tutorial_Btm = _tutorial_Btm;
        }
        #endregion
    }
}