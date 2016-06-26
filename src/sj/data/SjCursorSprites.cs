using UnityEngine;

namespace gkh
{
    public class SjCursorSprites : MonoBehaviour 
    {
        public static Texture2D empty;
        public static Texture2D blue;
        public static Texture2D gray;
        public static Texture2D green;
        public static Texture2D purple;
        public static Texture2D red;
        public static Texture2D yellow;


        public Texture2D _empty;
        public Texture2D _blue;
        public Texture2D _gray;
        public Texture2D _green;
        public Texture2D _purple;
        public Texture2D _red;
        public Texture2D _yellow;


        void Awake()
        {
            empty = _empty;
            blue = _blue;
            gray = _gray;
            green = _green;
            purple = _purple;
            red = _red;
            yellow = _yellow;
        }
    }
}