using UnityEngine;

namespace gkh
{
    public class MouseWrapper : MonoBehaviour
    {
        #region fields and properties
        /* whether the mouse has moved since the last frame */
        public static bool MouseHasMoved { get { return mousePos != previousMousePos; } }
        /* the current adjusted screen position of the mouse */
        public static Vector2 MouseXY { get { return new Vector2(mousePos.x, mousePos.y); } }
        public static float X { get { return mousePos.x; } }
        public static float Y { get { return mousePos.y; } }
        public static float AdjY { get { return Screen.height - mousePos.y; } }

        /* mouse position this frame */
        public static Vector2 mousePos = new Vector2();
        /* mouse position previous frame */
        public static Vector2 previousMousePos = new Vector2();
        #endregion


        #region Monobehaviour
        void Update()
        {
            /* update previous and current mouse positions */
            previousMousePos.x = mousePos.x;
            previousMousePos.y = mousePos.y;
            mousePos.x = Input.mousePosition.x;
            mousePos.y = Screen.height - Input.mousePosition.y;
        }
        #endregion


        #region mouse mgmt
        public static void ShowMouse() { UnityEngine.Cursor.visible = true; }
        public static void HideMouse() { UnityEngine.Cursor.visible = false; }
        #endregion
    }
}