using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public enum PieceColor
    { Red, Green, Blue, Yellow, Purple, Gray, Wild, Undefined }

    public enum PieceState
    { Idle, Drifting, Shooting, Dropping, InGrid, Shifting }

    public enum Alignment { Horizontal, Vertical }


    public class Piece : Entity
    {
        #region static members
        public static PieceVars pieceVars;
        public static Transform pieceParent;

        static SjParticleFactory pFactory;
        static List<Piece> droppingPieces = new List<Piece>();
        #endregion


        #region fields & properties
        // the current state of hte piece
        public PieceState state = PieceState.Idle;
        // the grid (if any) that this piece is currently in
        public PieceGrid parentGrid { get; set; }
        // the current position (if any) this piece holds in the grid
        public int gridPos { get; set; }
        // the color of this piece
        public PieceColor color { get; set; }
        // the alignment of the piece (i.e. "where is it on the grid?)
        // used for piece-to-piece hit-detection
        public Alignment align { get; set; }
        // whether this piece is locked from being caught
        public bool isUncatchable = false;

        // the SpriteRen's transform; used for rotating the sprite without the gameobject
        Transform spriteTrans;
        // the speed at which this piece rotates; changes with piece-state
        float rotateSpeed;
        // the x/y position this piece should be at based on its grid position
        float gridTargetX, gridTargetY;

        ParticleSysParent pspShoot, pspInGrid;
        #endregion


        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            SetAnimEnabled(false);
            
            // make sure we have initialized static members
            if (pieceVars == null)
            {
                pieceVars = Globals.pieceVars;
                pieceParent = new GameObject("__PieceParent__").transform;
                pFactory = SjParticleFactory.instance;
            }
            spriteTrans = spriteRen.GetComponent<Transform>();
            color = PieceColor.Blue;
            align = Alignment.Horizontal;
            gridPos = -1;
            trans.parent = pieceParent;
        }

        protected override void Update() 
        {
            base.Update();
            if (Globals.Paused || IsFrozen) return;

            // "drifting" or "shooting" state
            if (state == PieceState.Drifting ||
                state == PieceState.Shooting || 
                state == PieceState.Dropping)
                UpdateDriftingShootingState();

            // "shifting" state
            else if (state == PieceState.Shifting)
                UpdateShiftingState();

            // destroy the piece if it is out of bounds
            if (Mathf.Abs(x) > pieceVars.maxXY ||
                Mathf.Abs(y) > pieceVars.maxXY)
                QueueToDestroy();
        }

        void UpdateDriftingShootingState()
        {
            spriteTrans.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            moveX = velX * speed * speedMult * Time.deltaTime;
            moveY = velY * speed * speedMult * Time.deltaTime;
            Move();
        }

        void UpdateShiftingState()
        {
            SetGridTargetPosition();

            float diffX = x - gridTargetX;
            float diffY = y - gridTargetY;
            float min = .01f;
            bool atX = Mathf.Abs(diffX) < min;
            bool atY = Mathf.Abs(diffY) < min;

            float newX = x;
            float newY = y;

            // move x-pos as necessary
            if (atX)  
            {
                newX = gridTargetX;
                moveX = 0;
            }
            else moveX = -diffX * pieceVars.shiftSpeed * Time.deltaTime;
            // move y-pos as necessary
            if (atY)  
            {
                newY = gridTargetY;
                moveY = 0;
            }
            else moveY = -diffY * pieceVars.shiftSpeed * Time.deltaTime;
            SetPosition(newX, newY);

            // if we have reached both target positions, change the piece's state
            if (atX && atY)
                SetState(PieceState.InGrid);
            else Move();
        }
        #endregion


        #region piece mgmt
        public void SetState(PieceState state)
        {
            this.state = state;
            GameObject ps = null;

            switch (state)
            {
                case PieceState.Drifting:
                    speed = pieceVars.driftSpeed;
                    speedMult = 1 + (pieceVars.speedMultPerLevel * (GameMode.CurrentLevel - 1));

                    if (color == PieceColor.Wild)
                        speed *= .5f;
                    rotateSpeed = pieceVars.driftRotateSpeed;
                    spriteRen.sortingLayerName = SortingLayers.PiecesDrift;
                    // add a particle system!
                    ps = pFactory.GetDriftPS(color);
                    break;

                case PieceState.Shooting:
                    speed = pieceVars.shootSpeed;
                    rotateSpeed = pieceVars.shootRotateSpeed;
                    spriteRen.sortingLayerName = SortingLayers.PiecesDrift;
                    // add a particle system!
                    ps = pFactory.GetShootingPS(color);
                    pspShoot = ps.GetComponent<ParticleSysParent>();
                    break;

                case PieceState.Dropping:
                    speed = pieceVars.shootSpeed * .65f;
                    rotateSpeed = 0;
                    velX = -1;
                    velY = 0;

                    // if the grid is reaching off-screen, drop the piece at the end of the grid
                    // otherwise, just drop it right off the right side of the screen
                    PieceGrid pg = PieceFactory.grid;
                    float lastX = pg.LastPieceX;
                    bool offScreen = lastX > Screenie.ScreenRight;
                    x = offScreen ? 
                    lastX + (pieceVars.size * 2) : 
                    Screenie.ScreenRight + pieceVars.size;

                    y = Screenie.ScreenBottom + pieceVars.size;
                    SetPosition(x, y);
                    spriteRen.sortingLayerName = SortingLayers.PiecesDrift;
                    AddDroppingPiece(this);
                    // add a particle system!
                    ps = pFactory.GetShootingPS(color);
                    pspShoot = ps.GetComponent<ParticleSysParent>();
                    break;

                case PieceState.Shifting:
                    speed = pieceVars.shiftSpeed;
                    break;

                case PieceState.InGrid:
                    velX = 0;
                    velY = 0;
                    speed = 0;
                    rotateSpeed = 0;
                    spriteRen.sortingLayerName = SortingLayers.PiecesGrid;
                    spriteTrans.rotation = Quaternion.identity;
                    SetGridTargetPosition();
                    SetPosition(gridTargetX, gridTargetY);

                    if (pspShoot != null)
                        pspShoot.Stop();
                    if (pspInGrid == null)
                    {
                        ps = pFactory.GetInGridPS(color);
                        pspInGrid = ps.GetComponent<ParticleSysParent>();
                    }
                    break;
            }

            // if we instantiated a particle system, initialize it now
            if (ps != null)
            {
                psys.Add(ps.GetComponent<ParticleSysParent>());
                ps.transform.parent = trans;
                ps.transform.position = trans.position;
            }
        }

        public void SetGridTargetPosition()
        {
            float spacer = pieceVars.size + .03f;
            float sl = Screenie.ScreenLeft;
            float sb = Screenie.ScreenBottom;

            // pieces below half are lined up to the left
            if (gridPos < parentGrid.MidPoint)
            {
                float offsetY = parentGrid.MidPoint - gridPos;// -1;
                gridTargetX = sl + parentGrid.GridX;
                gridTargetY = sb + parentGrid.GridY + (offsetY * spacer);
                align = Alignment.Vertical;
            }
            // pieces above half are lined up along the bottom
            else
            {
                float offsetX = gridPos - parentGrid.MidPoint;// +1;
                gridTargetX = sl + parentGrid.GridX + (offsetX * spacer);
                gridTargetY = sb + parentGrid.GridY;
                align = Alignment.Horizontal;
            }
        }
        #endregion


        #region collisions
        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.tag == Tags.Piece)
            {
                // make sure we have a Piece script before proceeding
                Piece p = c.GetComponent<Piece>();
                if (p == null) 
                    return;

                // this piece must be "shooting" or "dropping"
                if ((state == PieceState.Shooting ||
                    state == PieceState.Dropping) &&
                    // the other piece must be in grid or "shifting"
                    (p.state == PieceState.Shifting ||
                    p.state == PieceState.InGrid))
                {
                    // remove a dropping piece from the list
                    if (state == PieceState.Dropping)
                        RemoveDroppingPiece(this);

                    int targetGridPos;
                    // compare x-positions for horizontally-aligned pieces
                    // insert to +0/+1 grid position based on x-diff
                    if (p.align == Alignment.Horizontal)
                    {
                        if (x < p.x) targetGridPos = p.gridPos;
                        else targetGridPos = p.gridPos + 1;
                    }
                    // compare y-positions for vertically-aligned pieces
                    // insert to +0/+1 grid position based on y-diff
                    else
                    {
                        if (y > p.y) targetGridPos = p.gridPos;
                        else targetGridPos = p.gridPos + 1;
                    }
                    p.parentGrid.InsertPiece(this, targetGridPos);
                }
            }
        }
        #endregion


        #region static methods
        public static void DestroyAllDroppingPieces()
        {
            foreach (Piece p in droppingPieces)
                p.QueueToDestroy();
            droppingPieces.Clear();
        }

        static void AddDroppingPiece(Piece p)
        {
            if (!droppingPieces.Contains(p))
                droppingPieces.Add(p);
        }

        static void RemoveDroppingPiece(Piece p)
        {
            if (droppingPieces.Contains(p))
                droppingPieces.Remove(p);
        }
        #endregion
    }
}