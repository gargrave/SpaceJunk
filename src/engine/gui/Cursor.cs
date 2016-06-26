using UnityEngine;
using System;

namespace gkh
{
    public class Cursor : MonoBehaviour
    {
        #region fields & properties
        /* the texture to display for the cursor */
        public Texture2D cursorTex;
        /* the display size for the cursor texture (if any) */
        protected Vector2 cursorSize;
        /* the Rect to use for displaying the cursor (i.e. to avoid instantiating a new one every frame) */
        protected Rect cursorRect;
        #endregion


        #region
        protected virtual void Awake()
        {
            if (cursorTex)
            {
                MouseWrapper.HideMouse();
                var t = GetComponent<Transform>();
                cursorSize = new Vector2(
                    cursorTex.width * t.localScale.x,
                    cursorTex.height * t.localScale.y);
                cursorRect = new Rect(0, 0, cursorSize.x, cursorSize.y);
            }
        }

        protected virtual void OnGUI()
        {
            cursorRect.x = MouseWrapper.X - (cursorSize.x / 2);
            cursorRect.y = MouseWrapper.Y - (cursorSize.y / 2);
            GUI.DrawTexture(cursorRect, cursorTex);
        }
        #endregion
    }
}