using UnityEngine;

namespace gkh
{
    public class ParticlePrefabs : MonoBehaviour 
    {
        #region static accessors
        // systems for "drifting" pieces
        public static GameObject Piece_Drift_Blue;
        public static GameObject Piece_Drift_Gray;
        public static GameObject Piece_Drift_Green;
        public static GameObject Piece_Drift_Purple;
        public static GameObject Piece_Drift_Red;
        public static GameObject Piece_Drift_Wild;
        public static GameObject Piece_Drift_Yellow;
        // systems for "in grid" pieces
        public static GameObject Piece_InGrid_Blue;
        public static GameObject Piece_InGrid_Gray;
        public static GameObject Piece_InGrid_Green;
        public static GameObject Piece_InGrid_Purple;
        public static GameObject Piece_InGrid_Red;
        public static GameObject Piece_InGrid_Wild;
        public static GameObject Piece_InGrid_Yellow;
        // systems for "shooting" pieces
        public static GameObject Piece_Shoot_Blue;
        public static GameObject Piece_Shoot_Gray;
        public static GameObject Piece_Shoot_Green;
        public static GameObject Piece_Shoot_Purple;
        public static GameObject Piece_Shoot_Red;
        public static GameObject Piece_Shoot_Wild;
        public static GameObject Piece_Shoot_Yellow;

        public static GameObject Piece_Break_Blue;
        public static GameObject Piece_Break_Gray;
        public static GameObject Piece_Break_Green;
        public static GameObject Piece_Break_Purple;
        public static GameObject Piece_Break_Red;
        public static GameObject Piece_Break_Wild;
        public static GameObject Piece_Break_Yellow;

        public static GameObject Asteroid_Break;
        #endregion


        #region prefab instances
        // "drifting"
        public GameObject _Piece_Drift_Blue;
        public GameObject _Piece_Drift_Gray;
        public GameObject _Piece_Drift_Green;
        public GameObject _Piece_Drift_Purple;
        public GameObject _Piece_Drift_Red;
        public GameObject _Piece_Drift_Wild;
        public GameObject _Piece_Drift_Yellow;
        // "in grid"
        public GameObject _Piece_InGrid_Blue;
        public GameObject _Piece_InGrid_Gray;
        public GameObject _Piece_InGrid_Green;
        public GameObject _Piece_InGrid_Purple;
        public GameObject _Piece_InGrid_Red;
        public GameObject _Piece_InGrid_Wild;
        public GameObject _Piece_InGrid_Yellow;
        // "shooting"
        public GameObject _Piece_Shoot_Blue;
        public GameObject _Piece_Shoot_Gray;
        public GameObject _Piece_Shoot_Green;
        public GameObject _Piece_Shoot_Purple;
        public GameObject _Piece_Shoot_Red;
        public GameObject _Piece_Shoot_Wild;
        public GameObject _Piece_Shoot_Yellow;

        public GameObject _Piece_Break_Blue;
        public GameObject _Piece_Break_Gray;
        public GameObject _Piece_Break_Green;
        public GameObject _Piece_Break_Purple;
        public GameObject _Piece_Break_Red;
        public GameObject _Piece_Break_Wild;
        public GameObject _Piece_Break_Yellow;

        public GameObject _Asteroid_Break;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            // "drifting"
            Piece_Drift_Blue = _Piece_Drift_Blue;
            Piece_Drift_Gray = _Piece_Drift_Gray;
            Piece_Drift_Green = _Piece_Drift_Green;
            Piece_Drift_Purple = _Piece_Drift_Purple;
            Piece_Drift_Red = _Piece_Drift_Red;
            Piece_Drift_Wild = _Piece_Drift_Wild;
            Piece_Drift_Yellow = _Piece_Drift_Yellow;
            // "in grid"
            Piece_InGrid_Blue = _Piece_InGrid_Blue;
            Piece_InGrid_Gray = _Piece_InGrid_Gray;
            Piece_InGrid_Green = _Piece_InGrid_Green;
            Piece_InGrid_Purple = _Piece_InGrid_Purple;
            Piece_InGrid_Red = _Piece_InGrid_Red;
            Piece_InGrid_Wild = _Piece_InGrid_Wild;
            Piece_InGrid_Yellow = _Piece_InGrid_Yellow;
            // "shooting"
            Piece_Shoot_Blue = _Piece_Shoot_Blue;
            Piece_Shoot_Gray = _Piece_Shoot_Gray;
            Piece_Shoot_Green = _Piece_Shoot_Green;
            Piece_Shoot_Purple = _Piece_Shoot_Purple;
            Piece_Shoot_Red = _Piece_Shoot_Red;
            Piece_Shoot_Wild = _Piece_Shoot_Wild;
            Piece_Shoot_Yellow = _Piece_Shoot_Yellow;

            Piece_Break_Blue = _Piece_Break_Blue;
            Piece_Break_Gray = _Piece_Break_Gray;
            Piece_Break_Green = _Piece_Break_Green;
            Piece_Break_Purple = _Piece_Break_Purple;
            Piece_Break_Red = _Piece_Break_Red;
            Piece_Break_Wild = _Piece_Break_Wild;
            Piece_Break_Yellow = _Piece_Break_Yellow;

            Asteroid_Break = _Asteroid_Break;
        }
        #endregion
    }
}