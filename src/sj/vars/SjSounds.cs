using UnityEngine;

namespace gkh
{
    public class SjSounds : MonoBehaviour
    {
        #region
        public AudioClip _menuClick01;
        public AudioClip _menuPause01;
        public AudioClip _menuUnpause01;

        public AudioClip _countdownEnd;
        public AudioClip _chargeUp;
        public AudioClip _timerTick01;
        public AudioClip _timerTick02;

        public AudioClip _asteroidWarning;

        public AudioClip _pieceClear01;
        public AudioClip _pieceClick01;
        public AudioClip _pieceClick02;

        public AudioClip _playerShoot01;
        public AudioClip _playerAsteroidColl;

        public AudioClip _gridClear;
        public AudioClip _endGameLose01;
        public AudioClip _endGameWin01;
        #endregion


        #region
        static bool initialized = false;

        public static AudioClip menuClick01 { get; private set; }
        public static AudioClip menuPause01 { get; private set; }
        public static AudioClip menuUnpause01 { get; private set; }

        public static AudioClip countdownEnd { get; private set; }
        public static AudioClip chargeUp { get; private set; }
        public static AudioClip timerTick01 { get; private set; }
        public static AudioClip timerTick02 { get; private set; }

        public static AudioClip asteroidWarning { get; private set; }

        public static AudioClip pieceClear01 { get; private set; }
        public static AudioClip pieceClick01 { get; private set; }
        public static AudioClip pieceClick02 { get; private set; }

        public static AudioClip playerShoot01 { get; private set; }
        public static AudioClip playerAsteroidColl { get; private set; }

        public static AudioClip gridClear { get; private set; }
        public static AudioClip endGameLose01 { get; private set; }
        public static AudioClip endGameWin01 { get; private set; }
        #endregion


        #region
        void Awake()
        {
            if (!initialized)
            {
                menuClick01 = _menuClick01;
                menuPause01 = _menuPause01;
                menuUnpause01 = _menuUnpause01;

                countdownEnd = _countdownEnd;
                chargeUp = _chargeUp;
                timerTick01 = _timerTick01;
                timerTick02 = _timerTick02;

                asteroidWarning = _asteroidWarning;

                pieceClear01 = _pieceClear01;
                pieceClick01 = _pieceClick01;
                pieceClick02 = _pieceClick02;

                playerShoot01 = _playerShoot01;
                playerAsteroidColl = _playerAsteroidColl;

                gridClear = _gridClear;
                endGameLose01 = _endGameLose01;
                endGameWin01 = _endGameWin01;
            }
        }
        #endregion
    }
}