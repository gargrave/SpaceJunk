using UnityEngine;

namespace gkh
{
    public class SjTextDriftFactory : TextDriftFactory
    {
        #region constants
        const float OFFSET_SIZE = .165f;
        #endregion


        #region 
        float lastTimeTd = 0f;
        float lastPointTd = 0f;
        float offsetY = 0;
        #endregion


        #region
        protected override TextDrift GetNewTD(string type, string text, Vector2 pos)
        {
            GameObject go = (GameObject)Instantiate(prefabTextDrift);
            TextDrift td = go.GetComponent<TextDrift>();
            if (td == null)
                return null;

            offsetY = 0;
            td.SetText(text);

            // TDs for basic points additions
            if (type == Sj.TD_POINTS)
            {
                if (Time.time - lastPointTd < .35f)
                    offsetY = -OFFSET_SIZE;

                td.SetPosition(pos.x, pos.y + offsetY);
                lastPointTd = Time.time;
            }

            // TDs for adding time to the timer
            if (type == Sj.TD_TIMER_ADD)
            {
                if (Time.time - lastTimeTd < .35f)
                    offsetY = OFFSET_SIZE;

                Vector3 v3 = Camera.main.ScreenToWorldPoint(
                    new Vector3(0, Screen.height - Globals.hud.TimerRectY - 45, 0));
                
                td.SetPosition(0, v3.y + offsetY);
                td.speed *= -1;
                lastTimeTd = Time.time;
            }
            return td;
        }
        #endregion
    }
}