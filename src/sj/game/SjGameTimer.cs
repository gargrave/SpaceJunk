using UnityEngine;

namespace gkh
{
    public class SjGameTimer : GameTimer
    {
        #region timer mgmt
        public override void AddSeconds(int sec)
        {
            base.AddSeconds(sec);
            // early-out to ignore null values
            if (sec <= 0) return;

            string timeStr = string.Format(
                "+{0}:{1}", 
                Utils.PadString((sec / 60).ToString(), 2, "0"), 
                Utils.PadString((sec % 60).ToString(), 2, "0"));

            Globals.tdFactory.CreateTD(
                Sj.TD_TIMER_ADD, timeStr, Vector2.zero);
        }
        #endregion
    }
}