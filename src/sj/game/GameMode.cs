using UnityEngine;
using System;

namespace gkh
{
    public class GameMode : MonoBehaviour
    {
        #region static members
        public static int CurrentLevel { get { return level; } }

        static int level = 1;
        // the amount by which the grid grew at least level up
        static int lastGrow = 2;
        // the amount of extra time granted based on the size of the grid
        const float TIME_ADD_PER_PIECE = 3.15f;
        // the level at which asteroids will begin spawning
        const int ASTEROID_SPAWN_LVL = 5;

        const string ASTEROID_WARNING = "WARNING! Asteroids Incoming!";
        #endregion


        #region MonoBehaviour
        public static void ResetTimeAttack()
        {
            level = 1;
            lastGrow = 2;
            Piece.DestroyAllDroppingPieces();
            Globals.asteroidSpawner.Deactivate();
        }

        public static void IncrementTimeAttack()
        {
            level++;
            // check if we have reached the level where asteroids begin
            if (level == ASTEROID_SPAWN_LVL)
            {
                Globals.warn.Show(ASTEROID_WARNING);
                Globals.asteroidSpawner.Activate();
            }

            // build a new, larger grid
            var g = PieceFactory.grid;
            var prevSize = g.InitialSize;
            g.BuildGrid(prevSize + lastGrow++);

            // speed up the spawn rates
            Globals.pieceSpawner.SpeedUpDropRate();
            Globals.asteroidSpawner.ReduceSpawnRate();

            // add more time based on the size of the previous grid
            // FIXME reduce the amount of time added after a certain level
            var addTime = (int)(prevSize * TIME_ADD_PER_PIECE);
            Globals.gameTimer.AddSeconds(addTime);

            // add points for clearing the grid
            Globals.score.TA_AddPointsForClearedGrid(ref g);
        }
        #endregion
    }
}