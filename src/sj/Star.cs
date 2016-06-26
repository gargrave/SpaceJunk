using UnityEngine;

namespace gkh
{
    public enum StarSize { Sml, Med, Lrg }

    public class Star : MonoBehaviour
    {
        float speedMult = 1f;

        #region MonoBehaviour
        public void SetSize(StarSize size)
        {
            SpriteRenderer spriteRen;
            if (size == StarSize.Sml)
            {
                spriteRen = GetComponent<SpriteRenderer>();
                spriteRen.color = new Color(97f/255f, 122f/255f, 122f/255f, 1f);
                speedMult = Random.Range(.5f, .65f);
            }
            else if (size == StarSize.Med)
            {
                spriteRen = GetComponent<SpriteRenderer>();
                spriteRen.color = new Color(97f/255f, 122f/255f, 122f/255f, 1f);
                speedMult = Random.Range(.9f, 1.1f);
            }
        }

        public void Move()
        {
            var velX = Globals.player.MoveX * .05f * speedMult;
            var velY = Globals.player.MoveY * .05f * speedMult;
            var newX = transform.position.x + velX;
            var newY = transform.position.y + velY;

            if (velX < 0 && newX < Screenie.ScreenLeft)
                newX = Screenie.ScreenRight;
            else if (velX > 0 && newX > Screenie.ScreenRight)
                newX = Screenie.ScreenLeft;

            if (velY < 0 && newY < Screenie.ScreenBottom)
                newY = Screenie.ScreenTop;
            else if (velY > 0 && newY > Screenie.ScreenTop)
                newY = Screenie.ScreenBottom;

            transform.position = new Vector3(newX, newY, 0);
        }
        #endregion
    }
}