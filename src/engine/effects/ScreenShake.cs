using UnityEngine;

namespace gkh
{
    public class ScreenShake : MonoBehaviour 
    {
        #region fields & properties
        // the strength of the shaking
        public float intens = .065f;
        // the rate at which intens will decrease
        public float slowDown = .97f;
        // the time (in seconds) which the shake will last
        public float duration = .335f;
        private float lifeTimer = 0;
        // the original position of the camera
        private Vector2 camOrigin;
        #endregion


        #region MonoBehaviour
        void Start() 
        {
            lifeTimer = duration;
            camOrigin = new Vector2(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y);
        }
        
        void Update() 
        {
            if (Globals.Paused) return;

            lifeTimer -= Time.deltaTime;
            if (lifeTimer > 0)
            {
                intens *= slowDown;
                intens *= slowDown;
                Camera.main.transform.position = new Vector3(
                    camOrigin.x + Random.Range(-intens, intens),
                    camOrigin.y + Random.Range(-intens, intens), -10);
            }
            else
            {
                // reset the camera's position before killing this object
                Camera.main.transform.position = new Vector3(
                    camOrigin.x, camOrigin.y, -10);
                Destroy(gameObject);
            }
        }
        #endregion
    }
}