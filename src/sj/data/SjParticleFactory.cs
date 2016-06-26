using UnityEngine;

namespace gkh
{
    public class SjParticleFactory : MonoBehaviour 
    {
        #region static members
        public static SjParticleFactory instance;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            instance = this;
        }
        #endregion


        #region particle methods
        public GameObject GetDriftPS(PieceColor color)
        {
            switch (color)
            {
                case PieceColor.Blue:   return Create(ParticlePrefabs.Piece_Drift_Blue);
                case PieceColor.Gray:   return Create(ParticlePrefabs.Piece_Drift_Gray);
                case PieceColor.Green:  return Create(ParticlePrefabs.Piece_Drift_Green);
                case PieceColor.Purple: return Create(ParticlePrefabs.Piece_Drift_Purple);
                case PieceColor.Red:    return Create(ParticlePrefabs.Piece_Drift_Red);
                case PieceColor.Wild:   return Create(ParticlePrefabs.Piece_Drift_Wild);
                case PieceColor.Yellow: return Create(ParticlePrefabs.Piece_Drift_Yellow);
            }
            return null;
        }

        public GameObject GetInGridPS(PieceColor color)
        {
            switch (color)
            {
                case PieceColor.Blue:   return Create(ParticlePrefabs.Piece_InGrid_Blue);
                case PieceColor.Gray:   return Create(ParticlePrefabs.Piece_InGrid_Gray);
                case PieceColor.Green:  return Create(ParticlePrefabs.Piece_InGrid_Green);
                case PieceColor.Purple: return Create(ParticlePrefabs.Piece_InGrid_Purple);
                case PieceColor.Red:    return Create(ParticlePrefabs.Piece_InGrid_Red);
                case PieceColor.Wild:   return Create(ParticlePrefabs.Piece_InGrid_Wild);
                case PieceColor.Yellow: return Create(ParticlePrefabs.Piece_InGrid_Yellow);
            }
            return null;
        }

        public GameObject GetShootingPS(PieceColor color)
        {
            switch (color)
            {
                case PieceColor.Blue:   return Create(ParticlePrefabs.Piece_Shoot_Blue);
                case PieceColor.Gray:   return Create(ParticlePrefabs.Piece_Shoot_Gray);
                case PieceColor.Green:  return Create(ParticlePrefabs.Piece_Shoot_Green);
                case PieceColor.Purple: return Create(ParticlePrefabs.Piece_Shoot_Purple);
                case PieceColor.Red:    return Create(ParticlePrefabs.Piece_Shoot_Red);
                case PieceColor.Wild:   return Create(ParticlePrefabs.Piece_Shoot_Wild);
                case PieceColor.Yellow: return Create(ParticlePrefabs.Piece_Shoot_Yellow);
            }
            return null;
        }

        public GameObject GetPieceBreakPS(PieceColor color)
        { 
            switch (color)
            {
                case PieceColor.Blue:   return Create(ParticlePrefabs.Piece_Break_Blue);
                case PieceColor.Gray:   return Create(ParticlePrefabs.Piece_Break_Gray);
                case PieceColor.Green:  return Create(ParticlePrefabs.Piece_Break_Green);
                case PieceColor.Purple: return Create(ParticlePrefabs.Piece_Break_Purple);
                case PieceColor.Red:    return Create(ParticlePrefabs.Piece_Break_Red);
                case PieceColor.Wild:   return Create(ParticlePrefabs.Piece_Break_Wild);
                case PieceColor.Yellow: return Create(ParticlePrefabs.Piece_Break_Yellow);
            }
            return null;
        }

        public GameObject GetAsteroidBreakPS()
        { return Create(ParticlePrefabs.Asteroid_Break); }

        GameObject Create(GameObject prefab)
        { return (GameObject)GameObject.Instantiate(prefab); }
        #endregion
    }
}