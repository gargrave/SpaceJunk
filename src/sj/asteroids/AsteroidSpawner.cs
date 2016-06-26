using UnityEngine;
using System.Collections;

namespace gkh
{
    public class AsteroidSpawner : MonoBehaviour 
    {
        #region
        public GameObject prefabAsteroid;

        public bool isForTitle = false;

        // the spawn rate for a new game
        public float initSpawnRate = 16f;
        // the minimum allowable spawn rate
        public float minSpawnRate = 8f;
        // the amount by which the spawn rate will decrease on level-up
        public float spawnRateInc = .35f;

        float currentSpawnRate = 0f;
        float spawnTimer = 0f;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            Globals.asteroidSpawner = this;

            currentSpawnRate = initSpawnRate;

            if (!isForTitle)
                Deactivate();
            else Activate();
        }

        void Update()
        {
            if (Globals.Paused) return;

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
                SpawnAsteroid();
        }
        #endregion


        #region
        public void Activate()
        {
            if (!enabled)
            {
                Debug.Log("<b>AsteroidSpawner</b> activated!");
                spawnTimer = currentSpawnRate * .2f;
                SjDebug.asteroidsEnabled = true;
                enabled = true;
            }
        }

        public void Deactivate()
        {
            SjDebug.asteroidsEnabled = false;
            enabled = false;
        }

        public void SpawnAsteroid()
        {
            bool spawnFromLeft = Random.Range(0, 99) > 50;
            int offsetX = 1;
            // place off to the appropriate side of the screen
            float x = spawnFromLeft ? 
                Screenie.ScreenLeft - offsetX : 
                Screenie.ScreenRight + offsetX;
            // set y randomly between screen top/btm
            float y = Random.Range(Screenie.ScreenBottom, Screenie.ScreenTop);

            Asteroid a = ((GameObject)GameObject.Instantiate(prefabAsteroid)).GetComponent<Asteroid>();
            Vector3 pos = new Vector3(x, y);
            a.SetPosition(x, y);

            if (isForTitle) 
            {
                Vector2 target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                a.SetVelocity(Utils.Towards(target, pos));
            }
            // drift towards the player
            else a.SetVelocity(Utils.Towards(Globals.player.Pos, pos));

            spawnTimer = currentSpawnRate;
        }

        public void ReduceSpawnRate()
        {
            currentSpawnRate = Mathf.Clamp(
                currentSpawnRate - spawnRateInc, 
                minSpawnRate, 
                initSpawnRate);
            Debug.Log("Updated AsteroidSpawner.<b>currentSpawnRate</b>: " + currentSpawnRate);
        }
        #endregion
    }
}