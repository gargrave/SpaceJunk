using UnityEngine;

namespace gkh
{
    public class PieceSpawner : MonoBehaviour 
    {
        #region fields & properties
        public bool isForTitleScreen = false;

        public float driftRateIncrease = .075f;
        public float dropRateIncrease = .75f;

        public float minDriftRate = 1.4f;
        public float minDropRate = 1.75f;

        public float driftSpawnRate = 2.1f;
        public float dropSpawnRate = 12.0f;

        float currentDropRate;
        float currentDriftRate;

        float driftTimer = 0;
        float dropTimer = 0;

        // whether this is the first drifting piece being spawned
        // will be used to trigger the tutorial
        bool isFirstDriftingPiece = true;
        Piece tutorialPiece;
        #endregion


        #region MonoBehaviour
        void Awake()
        {
            Globals.pieceSpawner = this;
        }

        void Start() 
        {
            currentDriftRate = driftSpawnRate;
            currentDropRate = dropSpawnRate;

            driftTimer = currentDriftRate;
            dropTimer = dropSpawnRate;
        }

        void Update() 
        {
            if (Globals.Paused) return;

            UpdateDriftSpawn();
            UpdateDropSpawn();
            UpdateTutorialPiece();
        }
        #endregion


        #region update methods
        // checks if it is time to spawn a "drift" piece
        void UpdateDriftSpawn()
        {
            driftTimer -= Time.deltaTime;
            if (driftTimer <= 0)
            {
                Piece p;
                if (isForTitleScreen)
                    p = TitlePieceFactory.CreateDriftingPiece();
                else p = PieceFactory.CreateDriftingPiece();
                // show the tutorial when the first piece appears onscreen
                if (!isForTitleScreen && isFirstDriftingPiece && Globals.ShowTutorial)
                {
                    // make the first piece uncatchable until the tutorial has shown
                    if (p != null) 
                    {
                        p.isUncatchable = true;
                        tutorialPiece = p;
                    }
                    SjTutorialMenu.ShowTutorial(.6f);
                }
                isFirstDriftingPiece = false;
                driftTimer = currentDriftRate;
            }
        }

        // checks if it is time to spawn a "drop" piece
        void UpdateDropSpawn()
        {
            if (!isForTitleScreen && Globals.player.IsActive)
            {
                dropTimer -= Time.deltaTime;
                if (dropTimer <= 0)
                {
                    PieceFactory.CreateDroppingPiece();
                    dropTimer = currentDropRate;
                }
            }
        }

        // if we have the tutorial piece (i.e. the first "non-catchable" piece) active,
        // watch for the delayed tutorial to show and update this piece accordingly
        void UpdateTutorialPiece()
        {
            if (tutorialPiece != null)
            {
                if (SjTutorialMenu.lastTutShown > SjTutorialMenu.tut_CatchPieces)
                {
                    tutorialPiece.isUncatchable = false;
                    tutorialPiece = null;
                }
            }
        }
        #endregion


        #region spawn mgmt
        public void SpeedUpDropRate()
        {
            currentDropRate = Mathf.Clamp(
                currentDropRate - dropRateIncrease, minDropRate, 9999);
            driftSpawnRate = Mathf.Clamp(
                driftSpawnRate - driftRateIncrease, minDriftRate, 9999);
            Debug.Log("Updated PieceSpawner.<b>currentDropRate</b>: " + currentDropRate);
        }
        #endregion
    }
}