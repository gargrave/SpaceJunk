using UnityEngine;

namespace gkh
{
    public class PlayerShootPiece : Piece
    {
        protected override void Start()
        {
            base.Start();
            trans.parent = Globals.player.trans;
        }
    }
}