using UnityEngine;

namespace gkh
{
    public abstract class TextDriftFactory : MonoBehaviour
    {
        #region
        public GameObject prefabTextDrift;
        #endregion


        #region
        public virtual void CreateTD(string type, string text, Vector2 pos)
        {
            if (prefabTextDrift == null) 
                return;

            TextDrift td = GetNewTD(type, text, pos);
            if (td == null)
                return;
        }

        protected abstract TextDrift GetNewTD(string type, string text, Vector2 pos);
        #endregion
    }
}