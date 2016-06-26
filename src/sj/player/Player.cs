using UnityEngine;
using System;

namespace gkh
{
    public class Player : Entity 
    {
        public Transform bg1;

        #region fields & properties
        // the speed at which the player rotates toward the mouse
        public float rotateSpeed = .3f;
        // the amount of speed reduction each queue piece will cause
        [Range(.01f, .5f)]
        public float slowdownPerPiece = .08f;
        // the mult to be used to reduce speed during asteroid damage
        public float asteroidDmgMult = .45f;
        // the length of time the player will remain "damaged" after an asteroid collision
        public float asteroidDmgDuration = 5f;

        public int PowerLevel { get; private set; }

        ShootQueue shootQueue;
        Piece shootQueuePiece;
        ShootQueueHUD shootQueueHUD;

        // a timer to track when asteroid damage expires
        float asteroidDamageTimer;
        float minSpeedMult = 1;
        // whether this is the first piece the player has caught
        // will be used to trigger the tutorial
        bool isFirstPieceCaught = true;
        #endregion


        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            act.Activate();

            isFirstPieceCaught = true;
            minSpeedMult = (1 - (6 * slowdownPerPiece)) * asteroidDmgMult;
            PowerLevel = 100;
        }

        protected override void Start()
        {
            base.Start();
            shootQueue = GetComponent<ShootQueue>();
            shootQueuePiece = GetComponentInChildren<Piece>();
            shootQueueHUD = GetComponentInChildren<ShootQueueHUD>();
        }

        protected override void Update() 
        {
            base.Update();
            if (Globals.Paused || IsFrozen) return;

            CheckMovementControls();
            RotateToMouse();
            CheckShootingControls();

            // update the current speed mult
            speedMult = 1 - (shootQueue.Count * slowdownPerPiece);
            speedMult = Mathf.Clamp(speedMult, .01f, 10f);
            // update the speed mult for asteroid damage
            if (asteroidDamageTimer > 0)
            {
                speedMult *= asteroidDmgMult;
                UpdateAsteroidDamage();
            }

            if (speedMult < minSpeedMult)
                speedMult = minSpeedMult;

            int poweri = (int)(speedMult * 100);
            PowerLevel = poweri;


            moveX = velX * speed * speedMult * Time.deltaTime;
            moveY = velY * speed * speedMult * Time.deltaTime;
            Move();

            var tempBgSpeed = .02f;
            bg1.Translate(moveX * tempBgSpeed, moveY * tempBgSpeed, 0);
        }

        public override void Deactivate() 
        { 
            base.Deactivate(); 
            PieceFactory.SetPieceColor(ref shootQueuePiece, PieceColor.Undefined);
        }
        #endregion


        #region control checks
        void CheckMovementControls()
        {
            if (InputWrapper.Held(Control.MoveLeft))
                velX = -1;
            else if (InputWrapper.Held(Control.MoveRight))
                velX = 1;
            else velX = 0;

            if (InputWrapper.Held(Control.MoveUp))
                velY = 1;
            else if (InputWrapper.Held(Control.MoveDown))
                velY = -1;
            else velY = 0;
        }

        void RotateToMouse()
        {
            var screenPos = Screenie.ScreenPos(trans.position);
            var rad = Mathf.Atan2(
                Input.mousePosition.y - screenPos.y,
                Input.mousePosition.x - screenPos.x);
            var targetRot = Quaternion.Euler(
                new Vector3(0, 0, (rad * Mathf.Rad2Deg) - 90));
            trans.rotation = Quaternion.Slerp(
                trans.rotation, targetRot, rotateSpeed);
        }

        void CheckShootingControls()
        {
            if (Input.GetMouseButtonDown(0) || InputWrapper.Pressed(Control.Shoot))
            {
                var color = shootQueue.GetNextPiece();
                if (color != PieceColor.Undefined)
                {
                    PieceFactory.CreateShootingPiece(
                        color, shootQueuePiece.trans.position);
                    Globals.cursor.PlayShootAnim();
                    snd.PlaySound(SjSounds.playerShoot01);
                    UpdateNextPiece();
                    shootQueueHUD.UpdateHUD(ref shootQueue);
                }
            }
        }
        #endregion


        #region piece mgmt
        void UpdateNextPiece()
        {
            if (shootQueuePiece == null)
            {
                shootQueuePiece = GetComponentInChildren<Piece>();
                if (shootQueuePiece == null)
                {
                    Debug.LogError("Player.shootQueuePiece is null.");
                    return;
                }
            }
            PieceFactory.SetPieceColor(ref shootQueuePiece, shootQueue.GetNextPieceColor());
        }

        void CatchPiece(Piece p)
        {
            // add the piece to the queue, assuming it is not full
            if (shootQueue.AddPieceToQueue(p.color))
            {
                UpdateNextPiece();
                shootQueueHUD.UpdateHUD(ref shootQueue);
                snd.PlaySound(SjSounds.pieceClick01);
                p.QueueToDestroy();
                Globals.cursor.PlayShootAnim();
                Globals.score.AddPointsForCatch(shootQueue.Count);
            }

            if (isFirstPieceCaught && Globals.ShowTutorial)
                SjTutorialMenu.ShowTutorial();
            isFirstPieceCaught = false;
        }

        public PieceColor GetQueuedPieceColor()
        { return shootQueue.GetNextPieceColor(); }
        #endregion


        #region asteroid damage
        void BeginAsteroidDamage(ref Asteroid a)
        {
            SjMusic.Snap2.TransitionTo(.01f);
            asteroidDamageTimer = asteroidDmgDuration;
            a.OnPlayerCollision();
        }

        void UpdateAsteroidDamage()
        {
            asteroidDamageTimer -= Time.deltaTime;
            if (asteroidDamageTimer <= 0)
                EndAsteroidDamage();
        }

        void EndAsteroidDamage()
        {
            SjMusic.Snap1.TransitionTo(4.0f);
            Debug.Log("<b>Asteroid damage</b> is over!");
        }
        #endregion


        #region collisions
        void OnTriggerEnter2D(Collider2D c)
        {
            if (c.tag == Tags.Piece)
                OnPieceColl(c);
            // asteroid collisions
            else if (c.tag == Tags.Asteroid)
                OnAsteroidColl(c);
        }

        void OnPieceColl(Collider2D c)
        {
            // make sure we get a valid Piece reference
            var p = c.GetComponent<Piece>();
            if (p == null || p.isUncatchable) return;

            // if we hit a drifting piece, add it to the shooting queue
            if (p.state == PieceState.Drifting)
                CatchPiece(p);
        }

        void OnAsteroidColl(Collider2D c)
        {
            // make sure we get a valid Asteroid reference
            var a = c.GetComponent<Asteroid>();
            if (a != null)
            {
                snd.PlaySound(SjSounds.playerAsteroidColl);
                Globals.effectMgr.DoAsteroidScreenShake();
                BeginAsteroidDamage(ref a);
            }
        }
        #endregion
    }
}