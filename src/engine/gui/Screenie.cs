using UnityEngine;

namespace gkh
{
    public class Screenie : MonoBehaviour
    {
        #region constants
        private const float RECT_UPDATE_INTERVAL = .35f;
        #endregion


        #region fields & properties
        // screen positions, for easier access
        public static float ScreenRight { get { return camTrans.position.x + (pxWidth * .005f); } }
        public static float ScreenLeft { get { return camTrans.position.x - (pxWidth * .005f); } }
        public static float ScreenMidX { get { return camTrans.position.x; } }
        public static float ScreenTop { get { return camTrans.position.y + (pxHeight * .005f); } }
        public static float ScreenBottom { get { return camTrans.position.y - (pxHeight * .005f); } }
        public static float ScreenMidY { get { return camTrans.position.y; } }

        public static Rect RectFull { get { return rect; } }

        static Camera cam;
        // the main cam's Transform
        static Transform camTrans;
        // pixel w/h of the current cam
        static float pxWidth, pxHeight;
        static Rect rect = new Rect();
        static float counter = 0f;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            cam = Camera.main;
            camTrans = Camera.main.transform;
        }

        void Start() 
        {
            pxWidth = Camera.main.pixelWidth;
            pxHeight = Camera.main.pixelHeight;
        }

        void Update()
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                counter = RECT_UPDATE_INTERVAL;
                rect.width = Screen.width;
                rect.height = Screen.height;
            }
        }
        #endregion


        #region
        public static Vector2 ScreenPos(Vector3 pos)
        { return cam.WorldToScreenPoint(pos); }
        #endregion
    }
}