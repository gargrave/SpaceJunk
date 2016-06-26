using UnityEngine;

namespace gkh
{
    public abstract class SceneScript : MonoBehaviour
    {
        #region 
        protected static bool firstLoad = true;
        #endregion


        #region MonoBehaviour
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        #endregion


        #region scene mgmt
        public abstract void OnSceneLoaded(int scene);
        public abstract void OnSceneExit();

        // this is just a pass-on method, to "forward" the 
        // event to our local methods
        void OnLevelWasLoaded(int level)
        { OnSceneLoaded(level); }
        #endregion
    }
}