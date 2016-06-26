using UnityEngine;

namespace gkh
{
    public class Activator : MonoBehaviour
    {
        #region field & properties
        MonoBehaviour[] scripts;
        Collider2D[] colliders;
        Renderer[] renderers;
        Animator[] anims;

        bool initialized = false;
        bool activating = false;
        bool deactivating = false;

        public bool IsActive { get; protected set; }
        #endregion


        #region initialization
        public void Init()
        {
            if (initialized)
                return;

            scripts = GetComponentsInChildren<MonoBehaviour>();
            colliders = GetComponentsInChildren<Collider2D>();
            renderers = GetComponentsInChildren<Renderer>();
            anims = GetComponentsInChildren<Animator>();
            initialized = true;
        }
        #endregion


        #region state-management
        public void Activate()
        {
            if (activating) return;
            if (!initialized) Init();

            activating = true;

            if (!initialized) Init();

            foreach (MonoBehaviour s in scripts) s.enabled = true;
            foreach (Collider2D c in colliders) c.enabled = true;
            foreach (Renderer r in renderers) r.enabled = true;
            foreach (Animator a in anims) a.enabled = true;

            activating = false;
            IsActive = true;
        }

        public void Deactivate()
        {
            if (deactivating) return;
            if (!initialized) Init();

            deactivating = true;

            if (!initialized) Init();

            foreach (MonoBehaviour s in scripts) s.enabled = false;
            foreach (Collider2D c in colliders) c.enabled = false;
            foreach (Renderer r in renderers) r.enabled = false;
            foreach (Animator a in anims) a.enabled = false;

            deactivating = false;
            IsActive = false;
        }
        #endregion
    }
}