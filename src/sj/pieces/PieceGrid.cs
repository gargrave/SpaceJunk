using UnityEngine;
using System.Collections.Generic;
using System;

namespace gkh
{
    public class PieceGrid : MonoBehaviour 
    {
        #region static members
        private static PieceVars pieceVars;
        #endregion


        #region fields & properties
        // the number of pieces in the initial grid
        public int startCount = 3;
        // the extra seconds awarded for multiple chains
        public int bonusSecPerChain = 3;

        public int MidPoint { get; private set; }
        public int CurrentSize { get { return pieces.Count; } }
        public int InitialSize { get; private set; }
        public float GridX { get; private set; }
        public float GridY { get; private set; }
        public float LastPieceX { get { return pieces[CurrentSize-1].Pos.x; } }

        List<Piece> pieces = new List<Piece>();
        int currentChain = 0;
        int consecPiecesCleared = 0;
        SoundPlayer snd;

        // whether this is the first piece the player has caught
        // will be used to trigger the tutorial
        bool isFirstGridCleared = true;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            if (pieceVars == null)
                pieceVars = Globals.pieceVars;

            isFirstGridCleared = true;
            GridX = pieceVars.size;
            GridY = pieceVars.size;
            snd = GetComponent<SoundPlayer>();
        }

        void Start() 
        {
            BuildGrid(startCount);
        }

        void Update()
        {
            ScanForMatches();
        }
        #endregion


        #region grid mgmt
        public void BuildGrid(int size)
        {
            var prevColor1 = PieceColor.Undefined;
            var prevColor2 = PieceColor.Undefined;

            pieces.Clear();
            InitialSize = size;
            for (int i = 0; i < InitialSize; i++)
            {
                Piece piece = PieceFactory.CreateInGridPiece();
                // make sure we don't create a grid with any matches
                while (piece.color == prevColor1 &&
                       piece.color == prevColor2)
                    PieceFactory.SetRandomColor(ref piece, false);
                
                piece.gridPos = i;
                piece.parentGrid = this;
                pieces.Add(piece);
                
                prevColor2 = prevColor1;
                prevColor1 = piece.color;
            }
            MidPoint = (int)(pieces.Count * .4f);
            foreach (Piece p in pieces)
            {
                p.SetPosition(
                    Screenie.ScreenLeft + GridX,
                    Screenie.ScreenBottom + GridY);
            }
            PieceFactory.grid = this;
        }

        public void InsertPiece(Piece piece, int i)
        {
            var prevState = piece.state;
            // insert the piece and update the grid
            pieces.Insert(i, piece);
            piece.parentGrid = this;
            piece.gridPos = i;
            piece.SetState(PieceState.InGrid);
            UpdateAllGridPos();

            // if no matches were made from this piece
            if (!ScanForMatches())
            {
                snd.PlaySound(SjSounds.pieceClick02, .7f);
                ShiftAllPieces();
                // if this piece was shot by the player, add points
                if (prevState == PieceState.Shooting)
                    Globals.score.AddPointsForGrid(ref piece);
            }
        }

        public bool ScanForMatches()
        {
            // don't let any matches be counted if the game is over
            if (!Globals.player.IsActive) return false;

            PieceColor prevColor1 = PieceColor.Undefined;
            PieceColor prevColor2 = PieceColor.Undefined;

            // the number of pieces (if any) that need to be cleared
            int clearCount = 0;
            // the starting index of pieces (if any) that needs to be cleared
            int clearFrom = 0;
            bool piecesAreShifting = false;

            for (int i = 0; i < CurrentSize; i++)
            {
                // skip any shifting pieces
                if (pieces[i].state == PieceState.Shifting)
                {
                    piecesAreShifting = true;
                    continue;
                }
                // skip any pieces that are not locked in position
                if (pieces[i].state != PieceState.InGrid)
                    continue;

                PieceColor color = pieces[i].color;
                if (prevColor1 != PieceColor.Undefined &&
                    prevColor2 != PieceColor.Undefined)
                {
                    bool matchFound = false;
                    // if the piece we are shooting is wild
                    if (color == PieceColor.Wild)
                    {
                        if (prevColor1 == prevColor2 ||
                            prevColor1 == PieceColor.Wild ||
                            prevColor2 == PieceColor.Wild)
                            matchFound = true;
                    }
                    else
                    {
                        if ((prevColor1 == PieceColor.Wild ||
                            prevColor1 == color) &&
                            (prevColor2 == PieceColor.Wild ||
                            prevColor2 == color))
                            matchFound = true;
                    }

                    // check if we have a match
                    if (matchFound)
                    {
                        int extra = 0;
                        // check for a 4th match
                        if (i < CurrentSize - 1 && pieces[i + 1].color == color)
                        {
                            extra++;
                            // now check for a 5th match
                            if (i < CurrentSize - 2 && pieces[i + 2].color == color)
                                extra++;
                        }

                        clearCount = 3 + extra;
                        clearFrom = i - 2;
                        break;
                    }
                }
                prevColor2 = prevColor1;
                prevColor1 = color;
            }
            if (clearCount > 0)
            {
                currentChain++;
                consecPiecesCleared += clearCount;
                ClearMatches(clearCount, clearFrom);
                // create wild pieces when enough pieces are cleared
                if (consecPiecesCleared >= pieceVars.wildStringCount)
                    PieceFactory.CreateWildPiece();
                // add time for consecutive chains
                Globals.gameTimer.AddSeconds((currentChain - 1) * bonusSecPerChain);
            }
            else if (!piecesAreShifting)
            {
                currentChain = 0;
                consecPiecesCleared = 0;
            }
            return clearCount > 0;
        }

        public void ClearMatches(int clearCount, int clearFrom)
        {
            snd.PlaySound(SjSounds.pieceClear01);
            Globals.effectMgr.DoScreenShake();
            Globals.score.AddPointsForClear(clearCount, currentChain);

            // set up the TextDrift showing the points
            string tdStr = "+" + Globals.score.LastPointTotal;
            Vector2 tdPos = Vector2.zero;
            for (int i = 0; i < clearCount; i++)
            {
                Piece p = pieces[clearFrom + i];
                // get the position of the piece in the middle of the chain
                if (i == (int)Mathf.Ceil(clearCount / 2))
                {
                    tdPos.x = p.x;
                    tdPos.y = p.y + pieceVars.size;
                }
                ShowPieceBreakPS(p);
                p.QueueToDestroy();
            }
            Globals.tdFactory.CreateTD(Sj.TD_POINTS, tdStr, tdPos);

            pieces.RemoveRange(clearFrom, clearCount);
            UpdateAllGridPos();
            ShiftAllPieces();
        }

        public void UpdateAllGridPos()
        {
            // increment the game if we have cleared it
            if (CurrentSize == 0)
                OnGridCleared();
            else
            {
                for (int i = 0; i < CurrentSize; i++)
                    pieces[i].gridPos = i;
            }
        }

        public void ShiftAllPieces()
        {
            MidPoint = (int)(pieces.Count * .4f);
            for (int i = 0; i < CurrentSize; i++)
                pieces[i].SetState(PieceState.Shifting);
        }

        public bool ContainsColor(PieceColor color)
        {
            foreach (Piece p in pieces)
                if (p.color == color)
                    return true;
            return false;
        }
        #endregion


        void ShowPieceBreakPS(Piece p)
        {
            var ps = SjParticleFactory.instance.GetPieceBreakPS(p.color);
            ps.GetComponent<Transform>().position = p.Pos;
        }

        void OnGridCleared()
        {
            // we need to destory any dropping pieces before building a new grid,
            // as it can cause some buggy behavior if they collide while the grid is expanding
            Piece.DestroyAllDroppingPieces();
            GameMode.IncrementTimeAttack();

            // show the tutorial for the first grid cleared
            if (Globals.ShowTutorial && isFirstGridCleared)
                SjTutorialMenu.ShowTutorial();
            isFirstGridCleared = false;

            snd.PlaySound(SjSounds.gridClear, .5f);
        }
    }
}