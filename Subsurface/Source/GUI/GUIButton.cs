﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Subsurface
{
    public class GUIButton : GUIComponent
    {
        protected GUITextBlock textBlock;
        protected GUIFrame frame;

        public delegate bool OnClickedHandler(GUIButton button, object obj);
        public OnClickedHandler OnClicked;

        public delegate bool OnPressedHandler();
        public OnPressedHandler OnPressed;

        public bool CanBeSelected = true;

        public bool Enabled { get; set; }

        public override Color Color
        {
            get { return base.Color; }
            set
            {
                base.Color = value;
                frame.Color = value;
            }
        }

        public override Color HoverColor
        {
            get { return base.HoverColor; }
            set
            {
                base.HoverColor = value;
                frame.HoverColor = value;
            }
        }

        public override Color OutlineColor
        {
            get { return base.OutlineColor; }
            set
            {
                base.OutlineColor = value;
                if (frame != null) frame.OutlineColor = value;
            }
        }

        public Color TextColor
        {
            get { return textBlock.TextColor; }
            set { textBlock.TextColor = value; }
        }
        
        public string Text
        {
            get { return textBlock.Text; }
            set { textBlock.Text = value; }
        }

        public bool Selected { get; set; }

        public GUIButton(Rectangle rect, string text, GUIStyle style, GUIComponent parent = null)
            : this(rect, text, null, Alignment.Left, style, parent)
        {
        }

        public GUIButton(Rectangle rect, string text, Alignment alignment, GUIStyle style, GUIComponent parent = null)
            : this(rect, text, null, alignment, style, parent)
        {
        }

        public GUIButton(Rectangle rect, string text, Color? color, GUIStyle style, GUIComponent parent = null)
            : this(rect, text, color, (Alignment.Left | Alignment.Top), style, parent)
        {
        }

        public GUIButton(Rectangle rect, string text, Color? color, Alignment alignment, GUIStyle style, GUIComponent parent = null)
            :this(rect, text, color, alignment, Alignment.Center, style, parent)
        {

        }

        public GUIButton(Rectangle rect, string text, Color? color, Alignment alignment, Alignment textAlignment, GUIStyle style, GUIComponent parent = null)
            :base (style)
        {
            this.rect = rect;
            if (color!=null) this.color = (Color)color;
            this.alignment = alignment;

            Enabled = true;

            if (parent != null) parent.AddChild(this);

            frame = new GUIFrame(Rectangle.Empty, style, this);
            if (style != null) style.Apply(frame, this);

            textBlock = new GUITextBlock(Rectangle.Empty, text,
                Color.Transparent, (this.style == null) ? Color.Black : this.style.textColor,
                textAlignment, style, this);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            if (rect.Contains(PlayerInput.MousePosition) && Enabled && (MouseOn == null || MouseOn == this || IsParentOf(MouseOn)))
            {
                state = ComponentState.Hover;
                if (PlayerInput.GetMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (OnPressed != null)
                    {
                        if (OnPressed()) state = ComponentState.Selected;
                    }
                }
                else if (PlayerInput.LeftButtonClicked())
                {
                    if (OnClicked != null)
                    {
                        if (OnClicked(this, UserData) && CanBeSelected) state = ComponentState.Selected;
                    }
                }
            }
            else
            {
                state = Selected ? ComponentState.Selected : ComponentState.None;
            }

            frame.State = state;

            //Color currColor = color;
            //if (state == ComponentState.Hover) currColor = hoverColor;
            //if (state == ComponentState.Selected) currColor = selectedColor;

            //GUI.DrawRectangle(spriteBatch, rect, currColor * alpha, true);

            ////spriteBatch.DrawString(HUD.font, text, new Vector2(rect.X+rect.Width/2, rect.Y+rect.Height/2), Color.Black, 0.0f, new Vector2(0.5f,0.5f), 1.0f, SpriteEffects.None, 0.0f);

            //GUI.DrawRectangle(spriteBatch, rect, Color.Black * alpha, false);

            DrawChildren(spriteBatch);

            if (!Enabled) GUI.DrawRectangle(spriteBatch, rect, Color.Gray*0.5f, true);
        }
    }
}
