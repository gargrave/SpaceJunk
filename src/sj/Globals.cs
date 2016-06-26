using UnityEngine;

namespace gkh
{
    public class Globals : MonoBehaviour
    {
        #region static members
        /* whether static members have been initialized */
        public bool Initialized { get; private set; }
        // whether the game is paused
        public static bool Paused { get; set; }
        // whether the tutorial will be shown at game-start
        public static bool ShowTutorial { get; set; }
        public static GamepadState InitialGamepadState { get; private set; }

        public static GameState gameState { get; private set; }
        public static SceneMaster sceneMaster { get; private set; }
        public static Pauser pauser { get; private set; }
        public static Layers layers { get; private set; }
        public static MusicManager music { get; private set; }
        public static SoundManager sound { get; private set; }
        
        // Space Junk specific objects
        public static Player player { get; private set; }
        public static Score score { get; private set; }
        public static HUD hud { get; private set; }
        public static SjCursor cursor { get; private set; }
        public static WarningMessage warn { get; private set; }
        public static GameTimer gameTimer { get; private set; }
        public static PieceVars pieceVars { get; private set; }
        public static PieceSpawner pieceSpawner { get; set; }
        public static AsteroidSpawner asteroidSpawner { get; set; }
        public static EffectMgr effectMgr { get; private set; }
        public static TextDriftFactory tdFactory { get; private set; }
        #endregion
        
        
        #region dev settings
        public GamepadState initialGamepadState = GamepadState.Disabled;
        #endregion
        
        
        #region initialization
        void Awake()
        {
            if (!Initialized)
            {
                Initialized = true;
                InitialGamepadState = initialGamepadState;

                gameState = GetComponentInChildren<GameState>();
                sceneMaster = GetComponentInChildren<SceneMaster>();
                pauser = GetComponentInChildren<Pauser>();
                layers = GetComponentInChildren<Layers>();
                music = GetComponentInChildren<MusicManager>();
                sound = GetComponentInChildren<SoundManager>();
                
                player = GetComponentInChildren<Player>();
                score = GetComponentInChildren<Score>();
                hud = GetComponentInChildren<HUD>();
                cursor = GetComponentInChildren<SjCursor>();
                warn = GetComponentInChildren<WarningMessage>();
                gameTimer = GetComponentInChildren<GameTimer>();
                pieceVars = GetComponentInChildren<PieceVars>();
                effectMgr = GetComponentInChildren<EffectMgr>();
                tdFactory = GetComponentInChildren<TextDriftFactory>();
            }
        }
        #endregion
    }
}