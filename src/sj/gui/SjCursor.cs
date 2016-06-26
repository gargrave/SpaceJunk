using UnityEngine;

namespace gkh
{
    public class SjCursor : Cursor 
    {
        const float UPDATE_INTERVAL = .1f;


        #region fields & properties
        // size multipliers for the textture
        // these need to STAY PUBLIC so they are visible to the Animator
        public float sx = 1, sy = 1;
        // whether this instance is allowed to change colors
        public bool changeColors = true;

        Animator anim;
        float updateTimer;
        #endregion


        #region MonoBehaviour
        protected override void Awake()
        {
            cursorTex = changeColors ? SjCursorSprites.empty : SjCursorSprites.gray;
            updateTimer = UPDATE_INTERVAL;
            anim = GetComponent<Animator>();
            anim.Play("stop");
            base.Awake();
        }

        void Update()
        {
            cursorRect.width = cursorSize.x * sx;
            cursorRect.height = cursorSize.y * sy;
            cursorRect.x = MouseWrapper.X - (cursorSize.x * sx / 2);
            cursorRect.y = MouseWrapper.Y - (cursorSize.y * sy / 2);

            updateTimer -= Time.deltaTime;
            if (updateTimer <= 0)
            {
                updateTimer = UPDATE_INTERVAL;

                // when there is no active player (e.g. at the title or game over screen),
                // use the white/gray cursor
                if (Globals.player == null || !Globals.player.IsActive || Globals.Paused)
                {
                    cursorTex = SjCursorSprites.gray;
                    return;
                } 
                else
                {
                    // otherwise, use the color of the next piece in the queue
                    var color = Globals.player.GetQueuedPieceColor();
                    switch (color)
                    {
                        case PieceColor.Blue:
                            cursorTex = SjCursorSprites.blue;
                            break;
                        case PieceColor.Gray:
                            cursorTex = SjCursorSprites.gray;
                            break;
                        case PieceColor.Green:
                            cursorTex = SjCursorSprites.green;
                            break;
                        case PieceColor.Purple:
                            cursorTex = SjCursorSprites.purple;
                            break;
                        case PieceColor.Red:
                            cursorTex = SjCursorSprites.red;
                            break;
                        case PieceColor.Yellow:
                            cursorTex = SjCursorSprites.yellow;
                            break;
                        default:
                            cursorTex = SjCursorSprites.empty;
                            break;
                    }
                }
            }
        }

        protected override void OnGUI()
        {
            GUI.DrawTexture(cursorRect, cursorTex);
        }
        #endregion


        public void PlayShootAnim() { anim.Play("shoot"); }
        void OnShootAnimFinalFrame() { anim.Play("stop"); }
    }
}