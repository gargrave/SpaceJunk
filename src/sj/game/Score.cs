using UnityEngine;
using System;

namespace gkh
{
    public class Score : MonoBehaviour
    {
        const string STR_LEVEL_CLEAR = "All junk cleared!";


        #region fields & properties
        public int pointsForGridPiece = 10;

        // the base amount of points rewarded when the player catches a piece
        public int pointsPerCatch = 10;
        // the amount of bonus points received for each additional piece
        // already in the player's queue when he catches the pieces
        public int pointsPerCatchBonus = 3;

        // the points awarded for each piece based on the length of the clear
        // i.e. 3 pieces earns [0] per piece, 4 pieces earns [1] per piece, etc.
        public int[] pointsPerPiece = { 30, 40, 45 };
        // the multiplier applied for each consecutive chain cleared
        // i.e. 1 chain gets [0], 2 chains gets [1], etc.
        public float[] chainMults = { 1f, 1.2f, 1.35f, 1.55f, 1.8f, 2.2f };
        
        // returns the current score
        public int Current { get { return score; } }
        // the last amount of points added to the total
        public int LastPointTotal { get; private set; }

        // the current score
        int score;
        #endregion


        #region score mgmt
        public void AddPointsForClear(int count, int chain)
        {
            var pointsPer = pointsPerPiece[count - 3];
            // start with points-per-piece, based on the number cleared
            var final = (float)(pointsPer * count);
            // add the mult for consecutive chains
            final *= chainMults[Mathf.Clamp(chain - 1, 0, chainMults.Length - 1)];
            LastPointTotal = (int)final;
            score += LastPointTotal;
        }

        public void AddPointsForCatch(int queueSize)
        {
            var final = pointsPerCatch;
            final += (queueSize - 1) * pointsPerCatchBonus;
            LastPointTotal = (int)final;
            score += LastPointTotal;

            // show a text drift on the player for the points
            var tdStr = "+" + LastPointTotal;
            Globals.tdFactory.CreateTD(
                Sj.TD_POINTS, tdStr, Globals.player.trans.position);
        }

        public void AddPointsForGrid(ref Piece piece)
        {
            var final = pointsForGridPiece;
            LastPointTotal = (int)final;
            score += LastPointTotal;

            // show a text drift on the piece for the points
            var tdStr = "+" + LastPointTotal;
            Globals.tdFactory.CreateTD(
                Sj.TD_POINTS, tdStr, piece.trans.position + (Vector3.up * Globals.pieceVars.size));
        }

        public void ResetScore() { score = 0; }
        #endregion


        #region
        public void TA_AddPointsForClearedGrid(ref PieceGrid grid)
        {
            var points = grid.InitialSize * 20;
            LastPointTotal = (int)points;
            score += LastPointTotal;

            var tdStr = string.Format("{0} +{1}", STR_LEVEL_CLEAR, LastPointTotal);
            Globals.tdFactory.CreateTD(Sj.TD_POINTS, tdStr, new Vector2(0, .5f));
        }
        #endregion
    }
}