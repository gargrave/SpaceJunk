using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class TitlePieceFactory : MonoBehaviour
    {
        #region static members
        static PieceVars pieceVars;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            pieceVars = Globals.pieceVars;
        }
        #endregion


        #region piece creation
        //creates a randomized "drifting" piece
        public static Piece CreateDriftingPiece()
        {
            // the new piece's x/y positions and velocity
            float x, y, velX, velY;
            // choose which side to spawn from
            bool spawnFromTop = Random.Range(0f, 1f) < .4f;
            float driftRandA = Random.Range(.675f, 1f);
            float driftRandB = Random.Range(.5f, .1f);

            if (spawnFromTop)
            {
                // pick a random x-position
                x = Random.Range(
                    Screenie.ScreenLeft + pieceVars.size/2, 
                    Screenie.ScreenRight - pieceVars.size/2);
                // drift towards the center of the screen
                velX = driftRandB;
                if (x > Screenie.ScreenMidX) 
                    velX *= -1;
                
                // place it just above screen-top and drift down
                y = Screenie.ScreenTop + pieceVars.size;
                velY = -driftRandA;
            }
            // spawn from one of the sides
            else
            {
                bool spawnFromLeft = Random.Range(0f, 1f) < .5f;
                // place the piece off-screen to the left
                if (spawnFromLeft)
                {
                    // place it off screen left and drift right
                    x = Screenie.ScreenLeft - pieceVars.size;
                    velX = driftRandA;
                }
                // place the piece off-screen to the right
                else
                {
                    // place it off screen right and drift left
                    x = Screenie.ScreenRight + pieceVars.size;
                    velX = -driftRandA;
                }
                // ranomize the y-position
                y = Random.Range(
                    Screenie.ScreenBottom - pieceVars.size/2, 
                    Screenie.ScreenTop + pieceVars.size/2);
                // and drift vertically towards the center
                velY = driftRandB;
                if (y > Screenie.ScreenMidY)
                    velY *= -1;
            }
            Piece piece = ((GameObject)GameObject.Instantiate(pieceVars.piecePrefab)).GetComponent<Piece>();
            SetRandomColor(ref piece, false);

            piece.SetPosition(x, y);
            piece.SetState(PieceState.Drifting);
            piece.velX = velX;
            piece.velY = velY;
            return piece;
        }
        #endregion


        #region piece coloring
        // assigns a randomized color to the supplied piece
        public static void SetRandomColor(ref Piece piece, bool safe)
        {
            PieceColor c = PieceColor.Undefined;
            int max = 100;
            for (int i = 0; i < max; i++)
            {
                int r = Random.Range(0, 6);
                switch (r)
                {
                    case 0: c = PieceColor.Red; break;
                    case 1: c = PieceColor.Green; break;
                    case 2: c = PieceColor.Blue; break;
                    case 3: c = PieceColor.Yellow; break;
                    case 4: c = PieceColor.Purple; break;
                    case 5: c = PieceColor.Gray; break;
                }
            }
            SetPieceColor(ref piece, c);
        }

        public static void SetPieceColor(ref Piece piece, PieceColor color)
        {
            PieceVars v = Piece.pieceVars;
            piece.color = color;
            piece.isAnimated = false;
            piece.SetAnimEnabled(false);

            switch(color)
            {
            case PieceColor.Red:
                piece.spriteRen.sprite = v.spriteRed;
                break;
            case PieceColor.Green:
                piece.spriteRen.sprite = v.spriteGreen;
                break;
            case PieceColor.Blue:
                piece.spriteRen.sprite = v.spriteBlue;
                break;
            case PieceColor.Yellow:
                piece.spriteRen.sprite = v.spriteYellow;
                break;
            case PieceColor.Purple:
                piece.spriteRen.sprite = v.spritePurple;
                break;
            case PieceColor.Gray:
                piece.spriteRen.sprite = v.spriteGray;
                break;
            case PieceColor.Undefined:
                piece.spriteRen.sprite = null;
                break;
            }
        }
        #endregion
    }
}