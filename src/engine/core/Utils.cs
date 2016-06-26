using UnityEngine;

namespace gkh
{
    public class Utils : MonoBehaviour
    {
        #region
        public static string PadString(string str, int length, string pad)
        {
            string newString = str;
            int iterations = 0;

            if (length < 100)
            {
                while (newString.Length < length && ++iterations < 1000)
                    newString = newString.Insert(0, pad);
            }
            else Debug.LogError("Utils.PadString() received an unsafe length value, " +
                "so the operation is being ignored.");

            return newString;
        }

        public static int Wrap(int value, int min, int max)
        {
            int i = value;
            if (i < min)
                i = max;
            else if (i > max)
                i = min;
            return i;
        }


        static Vector2 _dist = Vector2.zero;
        static Vector2 _angle = Vector2.zero;
        public static Vector2 Towards(Vector2 vecA, Vector2 vecB)
        {
            _dist = vecA - vecB;
            float rad = Mathf.Atan2(_dist.y, _dist.x);
            _angle.x = Mathf.Cos(rad);
            _angle.y = Mathf.Sin(rad);
            _angle.Normalize();
            return new Vector2(_angle.x, _angle.y);
        }
        #endregion
    }
}