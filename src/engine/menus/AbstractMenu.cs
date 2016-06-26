using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public abstract class AbstractMenu : MonoBehaviour
    {
        #region constants
        /* the string size calc seem a little off, so 
         * we will use this mult to adjust it a bit*/
        public const float STR_HEIGHT_OFFSET = .825f;
        // a dummy string to use for menu "spacers"
        public const string SPACER = "_SPACE_";
        #endregion


        #region
        public bool isActive { get; protected set; }
        public bool isHidden { get; protected set; }
        public bool isFrozen { get; protected set; }

        // skins for displaying the title
        public GUISkin skinTitleTop;
        public GUISkin skinTitleBtm;
        // skins for displaying basic (i.e. non-title, non-selected) items
        public GUISkin skinItemUnselTop;
        public GUISkin skinItemUnselBtm;
        // skins for displaying selected items
        public GUISkin skinItemSelTop;
        public GUISkin skinItemSelBtm;

        // the y-position (from top) to display the title
        public int titleY = 150;
        // the amount of offset from y-center the items will placed
        public int itemOffsetY = 0;
        // the y-space between each text line
        public float lineHeight = 40;

        // the texture to use for the background
        public Texture bgImage;
        // the color to use for displaying the bg image
        protected Color bgColor = Color.white;
        // the amount of alpha to use for the bg image's color
        [Range(0, 1)]
        public float bgAlpha = .65f;

        // this is a bit of a hack to allow instances to have a frame where nothing
        // is drawn in OnGUI(), so that strings can be updated and measured
        // once before they are drawn
        protected bool skipFirstFrame = false;
        // whether we are currently ignoring key-presses
        protected bool controlsAreLocked = false;

        protected SoundPlayer snd;
        protected AudioClip sndClick01;
        #endregion


        #region selectable text item settings
        // the string to use for a title (if any)
        protected string title = string.Empty;
        // the text-area rect for the title string
        protected Rect titleRect;
        // the list of selectable strings
        protected List<string> items = new List<string>();
        // text-area rects for selected and unselected items
        protected List<Rect> itemUnselRects = new List<Rect>();
        protected List<Rect> itemSelRects = new List<Rect>();
        // the index of the currently-selected item
        protected int sel = 0;
        // the number of selectable items
        protected int itemCount = 1;
        // whether this instance has selectable items
        // if a menu doesn't need any, we can disregard any selection action
        protected bool hasSelectableItems = true;
        #endregion


        #region
        protected virtual void Start()
        {
            bgColor = new Color(1f, 1f, 1f, bgAlpha);

            BuildItemList();
            itemCount = items.Count;
            snd = GetComponent<SoundPlayer>();
        }

        protected virtual void Update()
        {
            if (!isActive || isHidden) return;

            if (isFrozen)
            {
                if (FadeScreen.IsFinished && Globals.music.NoMusicPlaying)
                    OnFadeFinished();
                return;
            }

            UpdateSelection();
            CheckForSelected();
            CheckForLeftRight();

            // check if all keys/buttons are released, and if so, unlocked controls
            if (controlsAreLocked && !Input.anyKeyDown && !Input.GetMouseButton(0))
                controlsAreLocked = false;
        }

        protected virtual void OnGUI()
        {
            if (!isActive || isHidden) return;

            // draw the background texture
            GUI.depth = GuiDepth.TextMenuBottom;
            GUI.color = bgColor;
            GUI.DrawTexture(Screenie.RectFull, bgImage, ScaleMode.StretchToFill);
            GUI.color = Color.white;


            // make sure the title string is set up before proceeding            
            if (titleRect.width == 0)
                SetUpTitleString();

            GUI.depth = GuiDepth.TextMenuTop;
            // draw the title strings
            if (!skipFirstFrame)
            {
                GUI.skin = skinTitleBtm;
                GUI.Label(titleRect, title);
                GUI.skin = skinTitleTop;
                GUI.Label(titleRect, title);
            }


            // no need to continue if there are no items
            if (itemCount == 0) return;

            // if the items' rects have not been created yet, let's go ahead and do so
            if (itemUnselRects.Count == 0)
                SetUpItemStrings();
            // draw all of the non-selected strings
            if (itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    // skip the selected item; we will draw it below
                    if (i == sel && hasSelectableItems) continue;
                    // skip any "spacer" items
                    if (items[i] == SPACER) continue;

                    if (!skipFirstFrame)
                    {
                        GUI.skin = skinItemUnselBtm;
                        GUI.Label(itemUnselRects[i], items[i]);
                        GUI.skin = skinItemUnselTop;
                        GUI.Label(itemUnselRects[i], items[i]);
                    }
                }
            }

            // finally, draw the currently-selected string
            if (!skipFirstFrame && hasSelectableItems)
            {
                GUI.skin = skinItemSelBtm;
                GUI.Label(itemSelRects[sel], items[sel]);
                GUI.skin = skinItemSelTop;
                GUI.Label(itemSelRects[sel], items[sel]);
            }
        }
        #endregion


        #region string building methods
        protected virtual void SetUpTitleString()
        {
            GUI.skin = skinTitleBtm;
            Vector2 titleSize = GUI.skin.label.CalcSize(new GUIContent(title));
            float x, y, w, h;
            x = (Screen.width / 2) - (titleSize.x / 2);
            y = titleY - ((titleSize.y * STR_HEIGHT_OFFSET) / 2);
            w = titleSize.x;
            h = titleSize.y;
            titleRect = new Rect(x, y, w, h);
        }

        protected virtual void SetUpItemStrings()
        {
            // build the unselectable items
            GUI.skin = skinItemUnselBtm;
            for (int i = 0; i < itemCount; i++)
            {
                var size = GUI.skin.label.CalcSize(new GUIContent(items[i]));
                float x, y;
                // center the x-position
                x = (Screen.width / 2) - (size.x / 2);
                var itemHalf = ((float)itemCount) * .5f;
                y = ((Screen.height / 2) + (lineHeight * i)) - (itemHalf * lineHeight);
                // then store the new rect based on these figures
                itemUnselRects.Add(new Rect(x, y + itemOffsetY, size.x, size.y));
            }

            if (hasSelectableItems)
            {
                // build the selectable items
                GUI.skin = skinItemSelTop;
                for (int i = 0; i < itemCount; i++)
                {
                    var size = GUI.skin.label.CalcSize(new GUIContent(items [i]));
                    float x, y;
                    // center the x-position
                    x = (Screen.width / 2) - (size.x / 2);
                    var itemHalf = ((float)itemCount) * .5f;
                    var heightOffset = (size.y - itemUnselRects [i].height) * .5f;
                    y = ((Screen.height / 2) + (lineHeight * i)) - (itemHalf * lineHeight) - heightOffset;
                    // then store the new rect based on these figures
                    itemSelRects.Add(new Rect(x, y + itemOffsetY, size.x, size.y));
                }
            }
        }
        #endregion


        #region menu mgmt
        public virtual void Activate() { isActive = true; }
        public virtual void Deactivate() { isActive = false; }
        public virtual void Hide() { isHidden = true; }
        public virtual void Show() { Show(false); }

        public virtual void Show(bool resetSel)
        {
            if (resetSel) sel = 0;
            controlsAreLocked = true;
            isHidden = false;
            isFrozen = false;
        }

        public virtual void SetTitle(string title) 
        { 
            this.title = title;
            titleRect = new Rect();
        }

        protected abstract void BuildItemList();
        protected abstract void OnItemSelected();
        // called when the left key is pressed
        // not abstract because not all menus will need this
        protected virtual void OnLeftPressed() {}
        // called when the right key is pressed
        // not abstract because not all menus will need this
        protected virtual void OnRightPressed() {}
        #endregion


        #region control methods
        protected virtual void UpdateSelection()
        {
            if (itemCount == 0 || controlsAreLocked || !hasSelectableItems) return;

            int prevSel = sel;
            // check if the mouse has moved over a text-rect
            if (MouseWrapper.MouseHasMoved)
            {
                for (int i = 0; i < itemUnselRects.Count; i++)
                {
                    // make sure we ignore spacer items
                    if (items[i] == SPACER) 
                        continue;
                    if (itemUnselRects[i].Contains(MouseWrapper.MouseXY))
                    {
                        sel = i;
                        break;
                    }
                }
            }

            if (InputWrapper.Pressed(Control.MoveDown))
            {
                // incrase selection while skipping any "spacers"
                do { sel = Utils.Wrap(sel + 1, 0, itemCount - 1); }
                while (items[sel] == SPACER);
            }
            else if (InputWrapper.Pressed(Control.MoveUp))
            {
                // decrase selection while skipping any "spacers"
                do { sel = Utils.Wrap(sel - 1, 0, itemCount - 1); }
                while (items[sel] == SPACER);
            }
            // play the sound if selection has changed
            if (prevSel != sel)
                snd.PlaySound(sndClick01);
        }

        protected virtual void CheckForSelected()
        {
            if (controlsAreLocked || items.Count == 0 || !hasSelectableItems)
                return;

            if (InputWrapper.Pressed(Control.MenuSelect))
                OnItemSelected();

            if (Input.GetMouseButtonDown(0) &&
                itemSelRects[sel].Contains(MouseWrapper.MouseXY))
                OnItemSelected();
        }

        protected virtual void CheckForLeftRight()
        {
            if (!controlsAreLocked)
            {
                if (InputWrapper.Pressed(Control.MoveLeft))
                    OnLeftPressed();
                else if (InputWrapper.Pressed(Control.MoveRight))
                    OnRightPressed();
            }
        }
        #endregion


        #region fade mgmt
        protected virtual void FadeOut()
        {
            isFrozen = true;
            FadeScreen.FadeToSolid();
        }

        protected virtual void OnFadeFinished()
        { Debug.LogWarning("OnFadeFinished() not implemented!"); }
        #endregion
    }
}