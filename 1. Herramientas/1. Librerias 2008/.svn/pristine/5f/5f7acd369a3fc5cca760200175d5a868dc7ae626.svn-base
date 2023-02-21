using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Desktop.Windows.Forms
{

    #region ListBarScrollButton class
    /// <summary>
    /// A class which manages the behaviour and data associated with
    /// a scrolling button in the ListBar control.  This class can
    /// be overridden to provide (for example) an alternative rendering
    /// of the button.
    /// </summary>
    public class ListBarScrollButton : IMouseObject
    {
        /// <summary>
        /// Enumeration of available scroll button types 
        /// for this control.
        /// </summary>
        public enum ListBarScrollButtonType
        {
            /// <summary>
            /// The scroll button is an up button.
            /// </summary>
            Up,
            /// <summary>
            /// The scroll button is a down button.
            /// </summary>
            Down
        }

        /// <summary>
        /// The bounding rectangle for this button
        /// </summary>
        private Rectangle rectangle = new Rectangle(0, 0, SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight);
        /// <summary>
        /// Whether the mouse is down on the button or not
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// Whether the mouse is over this button or not
        /// </summary>
        private bool mouseOver = false;
        /// <summary>
        /// The point at which the mouse was pressed on this button.
        /// </summary>
        private Point mouseDownPoint = new Point(0, 0);
        /// <summary>
        /// Whether this button is visible or not.
        /// </summary>
        private bool visible = false;
        /// <summary>
        /// The type of scroll button.
        /// </summary>
        private ListBarScrollButtonType buttonType = ListBarScrollButtonType.Up;
        /// <summary>
        /// ToolTip Text to display.
        /// </summary>
        private string toolTipText = "";

        /// <summary>
        /// Gets/sets the tooltip text to display for this button.
        /// </summary>
        [Description("Gets/sets the tooltip text to display for this button.")]
        public string ToolTipText
        {
            get
            {
                return this.toolTipText;
            }
            set
            {
                this.toolTipText = value;
            }
        }

        /// <summary>
        /// Gets/sets whether this object is visible or not.
        /// </summary>
        [Description("Gets/sets whether this object is visible or not.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
                if (!value)
                {
                    this.mouseDown = false;
                    this.mouseOver = false;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified point is within the control.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <returns>True if the point is over the button and the button
        /// is visible, false otherwise.</returns>
        public bool HitTest(Point pt)
        {
            bool hitTest = false;
            if (visible)
            {
                hitTest = this.rectangle.Contains(pt);
            }
            return hitTest;
        }

        /// <summary>
        /// Gets which type of scroll button this is (Up or Down)
        /// </summary>
        [Description("Gets which type of scroll button this is (Up or Down)")]
        public ListBarScrollButtonType ButtonType
        {
            get
            {
                return this.buttonType;
            }
        }

        /// <summary>
        /// Draws the button onto the specified <see cref="System.Drawing.Graphics" /> 
        /// object.
        /// </summary>
        /// <remarks>
        /// Note that this method is called by the owning bar even if the 
        /// the button's <see cref="Visible"/> property is set to <c>False</c>.
        /// In subclasses of this object this enables the button to 		
        /// be shown disabled when it isn't needed, rather than the default
        /// behaviour which is to remove it entirely.
        /// </remarks>
        /// <param name="gfx">The <see cref="System.Drawing.Graphics"/> object 
        /// to draw on.</param>
        /// <param name="defaultBackColor">The default background
        /// <see cref="System.Drawing.Color"/> to use when drawing
        /// the button.</param>
        /// <param name="controlEnabled">Whether the owning control is enabled
        /// or not.</param>
        public virtual void DrawItem(
            Graphics gfx,
            Color defaultBackColor,
            bool controlEnabled
            )
        {
            if (this.visible)
            {
                if (defaultBackColor.Equals(Color.FromKnownColor(KnownColor.Control)))
                {
                    // Use the default mechanism:
                    ButtonState buttonState = ButtonState.Normal;
                    if (controlEnabled)
                    {
                        buttonState = ((mouseDown && mouseOver) ? ButtonState.Pushed : ButtonState.Normal);
                    }
                    else
                    {
                        buttonState = ButtonState.Inactive;
                    }
                    ControlPaint.DrawScrollButton(gfx,
                        this.rectangle,
                        (this.buttonType == ListBarScrollButtonType.Up ? ScrollButton.Up : ScrollButton.Down),
                        buttonState);
                }
                else
                {
                    // Not as easy when using custom border colours:

                    // Fill background:
                    Brush br = new SolidBrush(defaultBackColor);
                    gfx.FillRectangle(br, this.rectangle);
                    br.Dispose();

                    // Draw the glyph:
                    Point centrePoint = new Point(
                        (this.rectangle.Width / 2),
                        (this.rectangle.Height / 2)
                        );
                    centrePoint.Offset(this.rectangle.Left + 1, this.rectangle.Top);
                    if (mouseDown && mouseOver)
                    {
                        centrePoint.Offset(1, 1);
                    }
                    int opposite = 0;
                    if (this.ButtonType == ListBarScrollButtonType.Up)
                    {
                        opposite = -4;
                        centrePoint.Offset(0, 2);
                    }
                    else
                    {
                        opposite = 4;
                        centrePoint.Offset(0, -1);
                    }

                    if (!controlEnabled)
                    {
                        br = new SolidBrush(CustomBorderColor.ColorLightLight(defaultBackColor));
                        centrePoint.Offset(1, 1);
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, br, centrePoint, 4, opposite);
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, br, centrePoint, -4, opposite);
                        br.Dispose();
                        centrePoint.Offset(-1, -1);
                        br = new SolidBrush(CustomBorderColor.ColorDark(defaultBackColor));
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, br, centrePoint, 4, opposite);
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, br, centrePoint, -4, opposite);
                        br.Dispose();
                    }
                    else
                    {
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, SystemBrushes.WindowText, centrePoint, 4, opposite);
                        ListBarUtility.FillRightAngleTriangle(
                            gfx, SystemBrushes.WindowText, centrePoint, -4, opposite);
                    }

                    // Draw the border:
                    CustomBorderColor.DrawBorder(
                        gfx, this.rectangle, defaultBackColor, true,
                        (mouseDown && mouseOver));
                }
            }
        }

        /// <summary>
        /// Gets/sets whether the mouse is down on this object or not.
        /// </summary>
        [Description("Gets/sets whether the mouse is down on this object or not.")]
        public bool MouseDown
        {
            get
            {
                return this.mouseDown;
            }
            set
            {
                this.mouseDown = value;
            }
        }
        /// <summary>
        /// Gets/sets whether the mouse is over this object or not.
        /// </summary>
        [Description("Gets/sets whether the mouse is over this object or not.")]
        public bool MouseOver
        {
            get
            {
                return this.mouseOver;
            }
            set
            {
                this.mouseOver = value;
            }
        }
        /// <summary>
        /// Gets/sets the point at which the mouse was pressed on
        /// this object.
        /// </summary>
        [Description("Gets/sets the point at which the mouse was pressed on this object.")]
        public Point MouseDownPoint
        {
            get
            {
                return this.mouseDownPoint;
            }
            set
            {
                this.mouseDownPoint = value;
            }
        }

        /// <summary>
        /// Gets the bounding rectangle for this button.
        /// </summary>
        [Description("Gets the bounding rectangle for this button.")]
        public Rectangle Rectangle
        {
            get
            {
                return this.rectangle;
            }
        }

        /// <summary>
        /// Sets the bounding rectangle for this button.
        /// </summary>
        /// <param name="rect"></param>
        protected internal virtual void SetRectangle(Rectangle rect)
        {
            this.rectangle = rect;
        }

        /// <summary>
        /// Creates a new instance of this class with the specified
        /// button type (Up or Down)
        /// </summary>
        /// <param name="buttonType">The scroll button type to create.</param>
        public ListBarScrollButton(ListBarScrollButtonType buttonType)
        {
            this.buttonType = buttonType;
        }
    }
    #endregion

}
