using UnityEngine;
using System.Collections;

namespace gkh
{
    public class Asteroid : Entity 
    {
        #region
        const float ROT_SPD_MIN = 15.0f;
        const float ROT_SPD_MAX = 22.5f;
        #endregion


        #region
        float rotateSpeed = 15f;
        #endregion


        #region
        protected override void Awake()
        {
            base.Awake();
            rotateSpeed = Random.Range(ROT_SPD_MIN, ROT_SPD_MAX);
            psys.Add(GetComponentInChildren<ParticleSysParent>());
        }

        protected override void Update() 
        {
            base.Update();
            if (Globals.Paused || InDestroyQueue) return;

            spriteRen.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            moveX = velX * speed * Time.deltaTime;
            moveY = velY * speed * Time.deltaTime;
            Move();

            if (Mathf.Abs(x) > 10 || Mathf.Abs(y) > 10)
            {
                Debug.Log("Asteroid out of bounds! Destroyed!");
                QueueToDestroy();
            }
        }
        #endregion


        #region
        public void OnPlayerCollision()
        {
            QueueToDestroy();
            var ps = SjParticleFactory.instance.GetAsteroidBreakPS();
            if (ps == null)
                return;
            
            var psp = ps.GetComponent<ParticleSysParent>();
            if (psp == null)
                return;
            ps.transform.position = Pos;
            psys.Add(psp);
        }
        #endregion
    }
}