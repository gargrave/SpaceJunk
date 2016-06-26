using UnityEngine;

namespace gkh
{
    public class EffectMgr : MonoBehaviour
    {
        #region
        public GameObject prefab_ScreenShake;
        public GameObject prefabScreenShakeAsteroid;
        #endregion

        #region
        public void DoScreenShake()
        {
            GameObject.Instantiate(prefab_ScreenShake);
        }

        public void DoAsteroidScreenShake()
        {
            GameObject.Instantiate(prefabScreenShakeAsteroid);
        }
        #endregion
    }
}