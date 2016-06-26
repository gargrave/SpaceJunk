using UnityEngine;

namespace gkh
{
    public class TextDrift : Entity
    {
        #region fields & properties
        string text = "unset";
        Rect rect;

        // the measured size of the string
        Vector2 tdSize;
        // the on-screen positions (i.e. because this will be drawn on the GUI)
        Vector3 screenVec;
        // the length this instance will show
        float lifeSpan = 3.5f;
        // the counter to keep track of its lifespan
        float lifeTimer;

        // the draw color for this instance 
        // mostly just used for hte alpha value, since a GUISkin will set the actual color
        Color color = Color.white;
        // the current alpha
        float alpha = 1f;
        // the speed at which alpha will fade
        float alphaFade = .4f;
        #endregion


        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            speed = .25f;
            lifeTimer = lifeSpan;
        }

        protected override void Update()
        {
            base.Update();
            if (Globals.Paused) return;

            y += Time.deltaTime * speed;

            // adjust the display color to the updated alpha
            alpha = Mathf.Clamp(alpha - alphaFade * Time.deltaTime, 0, 1);
            color = new Color(1f, 1f, 1f, alpha);

            // check if the life timer has expired
            lifeTimer -= Time.deltaTime;
            if (alpha <= 0 || lifeTimer <= 0)
                Destroy(gameObject);
        }

        void OnGUI()
        {
            GUI.skin = Skins.Text01_Btm;
            // set up the new rect (since the text is moving)
            tdSize = GUI.skin.label.CalcSize(new GUIContent(text));
            screenVec = Camera.main.WorldToScreenPoint(new Vector3(x, y, 0));
            rect.x = screenVec.x - (tdSize.x / 2);
            rect.y = Screen.height - screenVec.y;
            rect.width = tdSize.x;
            rect.height = tdSize.y;

            GUI.depth = GuiDepth.TextDrift;
            GUI.color = color;
            GUI.Label(rect, text);
            GUI.skin = Skins.Text01_Top;
            GUI.Label(rect, text);
        }
        #endregion

        #region
        public void SetText(string text) { this.text = text; }

        public void SetAlphaFade(float fade)
        {
            alphaFade = Mathf.Clamp(fade, .125f, 2f);
        }
        #endregion
    }
}