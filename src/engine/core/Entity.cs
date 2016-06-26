using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class Entity : MonoBehaviour
    {
        #region fields & properties
        // component references
        [HideInInspector]
        public Activator act;
        [HideInInspector]
        public Transform trans;
        public Collider2D coll { get; protected set; }
        public Animator anim { get; protected set; }
        public SpriteRenderer spriteRen { get; protected set; }

        // do we need to activate the Animator?
        public bool isAnimated;

        // the max movement speed for this instance
        public float speed = 3.5f; 
        // the current mult for movement speed
        [HideInInspector]
        public float speedMult = 1f;
        // the current velocity (i.e. bearing)
        [HideInInspector]
        public float velX, velY;
        [HideInInspector]
        public float x, y;

        // whether this instance's position should be clamped to screen
        public bool clampToScreen;

        public float X { get { return pos.x; } }
        public float MoveX { get { return moveX; } }
        public float Y { get { return pos.y; } }
        public float MoveY { get { return moveY; } }
        public Vector2 Pos { get { return new Vector2(pos.x, pos.y); } }

        public bool IsActive { get { return act.IsActive; } }
        public bool IsFrozen { get; protected set; }
        public bool InDestroyQueue { get; protected set; }

        // the actual amount this instance will move this frame
        protected float moveX, moveY;
        protected Vector2 pos = Vector2.zero;
        protected List<ParticleSysParent> psys = new List<ParticleSysParent>();
        protected SoundPlayer snd;
        protected AudioSource audioSource;
        #endregion


        #region MonoBehaviour
        protected virtual void Awake()
        {
            act = GetComponent<Activator>();
            if (act == null)
                act = (Activator)gameObject.AddComponent<Activator>();

            coll = GetComponent<Collider2D>();

            trans = GetComponent<Transform>();
            anim = GetComponent<Animator>();
            spriteRen = GetComponentInChildren<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            snd = GetComponent<SoundPlayer>();
        }

        protected virtual void Start(){}

        protected virtual void Update()
        {
            if (InDestroyQueue)
            {
                bool ready = true;
                // if we still have active particles, we are not ready to die!
                foreach (ParticleSysParent p in psys)
                {
                    if (p.IsAlive())
                    {
                        ready = false;
                        break;
                    }
                }
                if (ready)
                    Destroy(gameObject);
                return;
            }

            if (Globals.Paused || IsFrozen)
            {
                // be sure to pause the Animator for animated objects
                if (isAnimated && anim.enabled)
                    SetAnimEnabled(false);
                return;
            }

            // be sure to unpause the Animator for animated objects
            if (isAnimated && !anim.enabled)
                SetAnimEnabled(true);
        }
        #endregion


        #region movement methods
        protected virtual void Move()
        {
            x += moveX;
            y += moveY;

            if (clampToScreen)
            {
                x = Mathf.Clamp(x, Screenie.ScreenLeft, Screenie.ScreenRight);
                y = Mathf.Clamp(y, Screenie.ScreenBottom, Screenie.ScreenTop);
            }

            SetPosition(x, y);
        }

        public virtual void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
            pos.x = x;
            pos.y = y;
            trans.position = pos;
        }

        public virtual void SetVelocity(float vx, float vy)
        { 
            velX = vx;
            velY = vy;
        }

        public virtual void SetVelocity(Vector2 vel) { SetVelocity(vel.x, vel.y); }
        #endregion


        #region
        // toggle Activator state
        public virtual void Activate() { act.Activate(); }
        public virtual void Deactivate() { act.Deactivate(); }
        // toggle freeze state
        public virtual void Freeze() { IsFrozen = true; }
        public virtual void Unfreeze() { IsFrozen = false; }

        // toggle Animator state
        public virtual void SetAnimEnabled(bool e) { anim.enabled = e; }

        public virtual void QueueToDestroy() 
        { 
            IsFrozen = true;
            InDestroyQueue = true; 
            spriteRen.enabled = false;
            coll.enabled = false;

            foreach (ParticleSysParent p in psys)
                p.Stop();
        }
        #endregion
    }
}