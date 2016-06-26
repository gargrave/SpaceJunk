using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class ShootQueueHUD : MonoBehaviour
    {
        #region
        public SpriteRenderer spriteRen1;
        public SpriteRenderer spriteRen2;
        public SpriteRenderer spriteRen3;
        public SpriteRenderer spriteRen4;
        public SpriteRenderer spriteRen5;

        public Sprite spriteRed;
        public Sprite spriteGreen;
        public Sprite spriteBlue;
        public Sprite spriteYellow;
        public Sprite spritePurple;
        public Sprite spriteGray;

        List<SpriteRenderer> spriteRens = new List<SpriteRenderer>();
        #endregion


        #region MonoBehaviour
        void Start()
        {
            spriteRens = new List<SpriteRenderer>() 
            { 
                spriteRen1, spriteRen2, spriteRen3, 
                spriteRen4, spriteRen5
            };
            ResetHUD();
        }
        #endregion


        #region HUD mgmt
        public void ResetHUD()
        {
            foreach (SpriteRenderer sr in spriteRens)
                sr.sprite = null;
        }

        public void UpdateHUD(ref ShootQueue queue)
        {
            for (int i = 0; i < spriteRens.Count; i++)
                SetSpriteByColor(i, queue.GetPieceColorAt(i+1));
        }

        void SetSpriteByColor(int i, PieceColor color)
        {
            switch (color)
            {
                case PieceColor.Red:
                    spriteRens[i].sprite = spriteRed;
                    break;
                case PieceColor.Green:
                    spriteRens[i].sprite = spriteGreen;
                    break;
                case PieceColor.Blue:
                    spriteRens[i].sprite = spriteBlue;
                    break;
                case PieceColor.Yellow:
                    spriteRens[i].sprite = spriteYellow;
                    break;
                case PieceColor.Purple:
                    spriteRens[i].sprite = spritePurple;
                    break;
                case PieceColor.Gray:
                    spriteRens[i].sprite = spriteGray;
                    break;
                case PieceColor.Undefined:
                    spriteRens[i].sprite = null;
                    break;
            }
        }
        #endregion
    }
}