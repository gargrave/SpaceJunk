using UnityEngine;

namespace gkh
{
    public class PieceVars : MonoBehaviour 
    {
        // piece sprites
        public Sprite spriteRed;
        public Sprite spriteBlue;
        public Sprite spriteGreen;
        public Sprite spriteYellow;
        public Sprite spritePurple;
        public Sprite spriteGray;
        // piece object for instantiating
        public GameObject piecePrefab;
        // the x/y position at which a piece will be destroyed (i.e. "out of bounds")
        public int maxXY = 20;
        // size in px/100
        public float size = .32f;
        // "drifting" state vars
        public float driftSpeed = 1f;
        public float driftRotateSpeed = 110f;
        // "shooting" state vars
        public float shootSpeed = 10f;
        public float shootRotateSpeed = 750f;
        // the speed at which pieces shift in the grid
        public float shiftSpeed = 2.5f;
        // the chance that a piece being made will be required
        // to be the same color as a piece in the grid
        public float safeColorChance = .75f;
        // the chances of a wild piece randomly spawning
        public float wildSpawnChance = .03f;
        // the number of consec pieces cleared for a wild piece
        public float wildStringCount = 7;
        // the amount of speed boost a "drifting" piece will receive per level
        public float speedMultPerLevel = .08f;
    }
}