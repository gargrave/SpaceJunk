using UnityEngine;

namespace gkh
{
    public class ParticleSysParent : MonoBehaviour 
    {
        #region fields & properties
        public string sortingLayer = SortingLayers.ParticlesBottom;
        public bool destroyWhenFinished;

        public bool IsStillActive { get; private set; }

        ParticleSystem[] ps;
        bool paused, stopped;
        #endregion


        #region MonoBehaviour
        void Awake() 
        {
            ps = GetComponentsInChildren<ParticleSystem>();
            // make sure to set a reasonable default sorting layer,
            // because Unity tends to default to something that makes them invisible
            foreach (ParticleSystem p in ps)
                p.GetComponent<Renderer>().sortingLayerName = sortingLayer;
        }

        void Update()
        {
            // pause all systems when necssary
            if (Globals.Paused && !paused)
            {
                paused = true;
                foreach (ParticleSystem p in ps)
                    p.Pause();
            }
            // unpause all systems when necessary
            else if (!Globals.Paused && paused)
            {
                paused = false;
                foreach (ParticleSystem p in ps)
                {
                    p.Play(); 
                    // if this parent has already been stopped, go ahead and "re-stop" the system;
                    // we are basically just letting it die before destroying the gameobject
                    if (stopped)
                        p.Stop();
                }
            }

            if (destroyWhenFinished && !IsAlive())
                Destroy(gameObject);
        }
        #endregion


        public void Stop()
        {
            foreach (ParticleSystem p in ps)
                p.Stop();
            stopped = true;
        }

        public bool IsAlive()
        {
            foreach (ParticleSystem p in ps)
                if (p.IsAlive())
                    return true;
            return false;
        }
    }
}