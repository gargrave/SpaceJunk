using UnityEngine;

namespace gkh
{
    public class GameTimer : MonoBehaviour
    {
        #region fields & properties
        public int Min { get { return minutes; } }
        public int Sec { get { return seconds; } }
        public bool running { get; protected set; }
        public bool Expired { get; protected set; }

        protected int minutes = 3;
        protected int seconds = 0;
        protected float accum;
        #endregion


        #region MonoBehaviour
        protected virtual void Start()
        {
            Expired = false;
        }

        protected virtual void Update()
        {
            if (Globals.Paused || !running) 
                return;

            accum += Time.deltaTime;
            while (accum >= 1)
            {
                accum--;
                if (--seconds < 0)
                {
                    // if we still have time, reset the seconds and decrement the minutes
                    if (minutes > 0)
                    {
                        minutes--;
                        seconds = 59;
                    }
                    // end the game with loss conditions
                    else
                    {
                        running = false;
                        minutes = 0;
                        seconds = 0;
                        Expired = true;
                    }
                }
            }
        }
        #endregion


        #region timer mgmt
        public virtual void SetTimer(int min, int sec)
        {
            minutes = min;
            seconds = sec;
            Expired = false;
        }

        public virtual void ResetTimerState() { Expired = false; }

        public virtual void StartTimer() 
        { 
            SjDebug.timerDisabled = false;
            running = true; 
        }

        public virtual void StopTimer() 
        { 
            SjDebug.timerDisabled = true;
            running = false; 
        }

        public virtual void AddSeconds(int sec)
        {
            // early-out to ignore null values
            if (sec <= 0 || SjDebug.timerDisabled) 
                return;

            seconds += sec;
            while (seconds >= 60)
            {
                seconds -= 60;
                minutes++;
            }
        }
        #endregion
    }
}