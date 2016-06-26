using UnityEngine;

namespace gkh
{
    /* a simple class, designed to be globally accesible, which
     * simply stores the values for all of the game's collision
     * layers; this is just to save the effort of finding the 
     * layers from various other scripts */
    public class Layers : MonoBehaviour
    {
        #region
        /* accessor properties */
        public LayerMask Pieces { get; private set; }
        public LayerMask Player { get; private set; }
        #endregion


        #region
        void Awake()
        {
            Pieces = 1 << LayerMask.NameToLayer("Pieces");
            Player = 1 << LayerMask.NameToLayer("Player");
        }
        #endregion
    }
}