using UnityEngine;

namespace gkh
{
    public class SceneMaster : MonoBehaviour
    {
        #region
        public SceneScript CurrentScene { get; set; }
        #endregion


        #region
        public void ChangeScene(int newScene)
        {
            if (CurrentScene != null)
                CurrentScene.OnSceneExit();
            Application.LoadLevel(newScene);
        }
        #endregion
    }
}