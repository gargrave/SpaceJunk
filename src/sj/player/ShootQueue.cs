using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class ShootQueue : MonoBehaviour 
    {
        #region fields & properties
        // the max number of pieces the queue can hold
        public int queueSize = 6;

        // returns the current number of pieces in queue
        public int Count { get { return queue.Count; } }

        // the current set of queued pieces
        Stack<PieceColor> queue = new Stack<PieceColor>();
        #endregion


        #region queue mgmt
        public bool AddPieceToQueue(PieceColor pieceType)
        {
            if (queue.Count < queueSize)
            {
                queue.Push(pieceType);
                return true;
            }
            return false;
        }

        public PieceColor GetNextPiece()
        {
            if (queue.Count > 0)
                return queue.Pop();
            return PieceColor.Undefined;
        }

        public PieceColor GetNextPieceColor()
        {
            if (queue.Count > 0)
                return queue.Peek();
            return PieceColor.Undefined;
        }

        public PieceColor GetPieceColorAt(int index)
        {
            int i = 0;
            foreach (PieceColor c in queue)
            {
                if (i == index)
                    return c;
                i++;
            }
            return PieceColor.Undefined;
        }
        #endregion
    }
}