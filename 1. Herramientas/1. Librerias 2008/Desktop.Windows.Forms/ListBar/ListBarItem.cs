using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;


namespace Desktop.Windows.Forms
{
    /// <summary>
    /// A class containing the information describing an Item in the ListBar
    /// control.
    /// </summary>
    [SerializableAttribute()]
    public class ListBarItem : IComparable, IMouseObject, ISerializable
    {
        private ListBar owner = null;
        private bool selected = false;
        private Font font = null;
        private string toolTipText = "";
        private string caption = "";
        private Color foreColor = Color.FromKnownColor(KnownColor.WindowText);
        private Color backColor = Color.FromKnownColor(KnownColor.Control);
        private object tag = "";
        private string key = "";
        private int iconIndex;
        /// <summary>
        /// Bounding rectangle for this item, relative to its owning
        /// group.  The members of this are typically adjusted by the 
        /// owning control through the <see cref="SetLocationAndWidth"/>
        /// and the <see cref="SetSize"/> methods.
        /// </summary>
        protected Rectangle rectangle = new Rectangle(0, 0, 0, 72);
        /// <summary>
        /// The rectangle containing the icon for this item.  Set this 
        /// when overriding the standard drawing mode for an item;
        /// the owning ListBar control uses it for hit-testing.
        /// </summary>
        protected Rectangle iconRectangle;
        /// <summary>
        /// The rectangle containing the text for this item.  Set this
        /// when overriding the standard drawing mode for an item; 
        /// the owning ListBar control uses it for hit-testing.
        /// </summary>
        protected Rectangle textRectangle;
        private bool enabled = true;
        private bool mouseOver = false;
        private bool mouseDown = false;
        private Point mouseDownPoint = new Point(0, 0);

        /// <summary>
        /// Returns a string representation of this <see cref="ListBarItem"/>.
        /// </summary>
        /// <returns>A string containing the class name, caption, icon index,
        /// enabled state and rectangle for this item.</returns>
        [Description("Returns a string representation of this ListBarItem")]
        public override string ToString()
        {
            return String.Format("{0} Caption={1} IconIndex={2} Enabled={3} Location={4} Height={5}",
                this.GetType().FullName, this.caption, this.iconIndex, this.enabled, this.Location, this.Height);
        }

        /// <summary>
        /// Gets/sets the point at which the mouse was pressed
        /// on this object.
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
        /// Gets/sets whether the mouse is over this item.
        /// </summary>
        [Description("Gets/sets whether the mouse is over this item.")]
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
        /// Gets/sets whether the mouse is down on this item.
        /// </summary>
        [Description("Gets/sets whether the mouse is down on this item.")]
        public bool MouseDown
        {
            get
            {
                return this.mouseDown;
            }
            set
            {
                this.mouseDown = (value & this.enabled);
            }
        }

        /// <summary>
        /// Draws this item into the specified graphics object.
        /// </summary>
        /// <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        /// <param name="ils">The <see cref="System.Windows.Forms.ImageList"/>to source icons from.</param>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use to render
        /// the item.</param>
        /// <param name="style">The style (Outlook version) to draw using.</param>
        /// <param name="view">The view (large or small icons) to draw using.</param>
        /// <param name="scrollOffset">The offset of the first item from the 
        /// (0,0) point in the graphics object.</param>
        /// <param name="controlEnabled">Whether the control is enabled or not.</param>
        /// <param name="skipDrawText">Whether to skip drawing text or not
        /// (the item is being edited)</param> 
        [Description("Draws this item into the specified graphics object")]
        public virtual void DrawButton(
            Graphics gfx,
            ImageList ils,
            Font defaultFont,
            ListBarDrawStyle style,
            ListBarGroupView view,
            int scrollOffset,
            bool controlEnabled,
            bool skipDrawText
            )
        {

            bool rightToLeft = false;
            Color backColor = Color.FromKnownColor(KnownColor.Control);
            if (this.owner != null)
            {
                //backColor = this.owner.BackColor;
                if (this.owner.RightToLeft == RightToLeft.Yes)
                {
                    rightToLeft = true;
                }
            }

            // Work out the icon & text rectangles:			
            textRectangle = new Rectangle(this.rectangle.Location, this.rectangle.Size);
            textRectangle.Offset(0, scrollOffset);
            if (view == ListBarGroupView.SmallIcons)
            {
                textRectangle.Y += 1;
                textRectangle.Height -= 1;
            }
            iconRectangle = new Rectangle(textRectangle.Location, textRectangle.Size);

            if (view == ListBarGroupView.SmallIcons)
            {
                if (ils != null)
                {
                    if (rightToLeft)
                    {
                        iconRectangle.X = iconRectangle.Right - ils.ImageSize.Width - 4;
                        iconRectangle.Width = ils.ImageSize.Width;
                    }
                    else
                    {
                        iconRectangle.X += 4;
                        iconRectangle.Width = ils.ImageSize.Width;
                        textRectangle.X += ils.ImageSize.Width + 8;
                    }
                    textRectangle.Width -= (iconRectangle.Width + 8);
                    iconRectangle.Height = ils.ImageSize.Height;
                    iconRectangle.Y += (this.rectangle.Height - iconRectangle.Height) / 2;
                }
                else
                {
                    textRectangle.Inflate(-2, -2);
                }
            }
            else
            {
                if (ils != null)
                {
                    iconRectangle.Y += 7;
                    iconRectangle.Height = ils.ImageSize.Height;
                    iconRectangle.Width = ils.ImageSize.Width;
                    iconRectangle.X = iconRectangle.Left + (this.rectangle.Width - iconRectangle.Width) / 2;

                    textRectangle.Y += ils.ImageSize.Height + 11;
                    textRectangle.Height -= (ils.ImageSize.Height + 11);
                }
                else
                {
                    textRectangle.Inflate(-2, -2);
                }
            }

            // If we're drawing using XP style and the button is
            // hot or down then we draw the background:
            //Rectangle rcHighlight = new Rectangle(iconRectangle.Location, iconRectangle.Size);					
            Rectangle rcHighlight = new Rectangle(this.rectangle.Location, this.rectangle.Size);
            rcHighlight.Offset(0, scrollOffset);
            //rcHighlight.Inflate(2,2);
            if (style == ListBarDrawStyle.ListBarDrawStyleOfficeXP)
            {
                if ((this.enabled && controlEnabled) && (this.MouseOver || this.mouseDown))
                {
                    Color highlightColor;
                    if (this.mouseDown && this.mouseOver)
                    {
                        highlightColor = ListBarUtility.BlendColor(
                            Color.FromKnownColor(KnownColor.Highlight),
                            Color.FromKnownColor(KnownColor.Window),
                            224);
                    }
                    else
                    {
                        highlightColor = ListBarUtility.BlendColor(
                            Color.FromKnownColor(KnownColor.Highlight),
                            Color.FromKnownColor(KnownColor.Window),
                            128);
                    }                    

                    SolidBrush highlight = new SolidBrush(Color.FromArgb(128, highlightColor));
                    gfx.FillRectangle(highlight, rcHighlight);
                    highlight.Dispose();
                    gfx.DrawRectangle(SystemPens.Highlight, rcHighlight);
                }
            }


            // Draw the icon if necessary:
            if (ils != null)
            {
                if (this.iconIndex >= 0 && this.iconIndex <= ils.Images.Count)
                {

                    int iconX = iconRectangle.X;
                    int iconY = iconRectangle.Y;


                    if (this.mouseDown && this.mouseOver)
                    {
                        iconX++;
                        iconY++;
                    }
                    if (this.enabled && controlEnabled)
                    {
                        ils.Draw(gfx, iconRectangle.X + 1, iconRectangle.Y + 1, this.iconIndex);
                    }
                    else
                    {
                        ControlPaint.DrawImageDisabled(gfx, ils.Images[this.iconIndex], iconX, iconY, Color.FromArgb(0, 0, 0, 0));
                    }

                }
                else
                {
                    // We don't want an exception in a paint event
                    Trace.WriteLine(
                        String.Format("Icon {0} doesn't exist in ImageList {1}",
                        this.iconIndex,
                        ils));
                }
            }
            // We do this to make the hit testing more usable:
            iconRectangle.Inflate(4, 4);

            if (skipDrawText)
            {
                return;
            }

            if ((view == ListBarGroupView.SmallIconsOnly) || (view == ListBarGroupView.LargeIconsOnly))
            {
                textRectangle = new Rectangle(0, 0, 0, 0);
            }
            else
            {

                // Draw the text:
                // Get the font to draw with:
                Font drawFont = this.font;
                if (drawFont == null)
                {
                    if (this.owner != null)
                    {
                        drawFont = this.owner.Font;
                    }
                }
                if (drawFont == null)
                {
                    drawFont = System.Windows.Forms.SystemInformation.MenuFont;
                }
                // Set up format:
                StringFormat format = new StringFormat(StringFormatFlags.LineLimit);
                format.Trimming = StringTrimming.EllipsisCharacter;
                if (view == ListBarGroupView.SmallIcons)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.FormatFlags = format.FormatFlags | StringFormatFlags.NoWrap;
                }
                else
                {
                    format.Alignment = StringAlignment.Center;
                }
                format.LineAlignment = StringAlignment.Near;
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                // Bounding rectangle:
                RectangleF rectF = new RectangleF(textRectangle.X, textRectangle.Y,
                    textRectangle.Width, textRectangle.Height);
                if (view == ListBarGroupView.SmallIcons)
                {
                    SizeF textSize = gfx.MeasureString(this.caption, drawFont, textRectangle.Width, format);
                    rectF.Y += (rectF.Height - textSize.Height) / 2;
                    textRectangle.Y += (int)((rectF.Height - textSize.Height) / 2);
                    textRectangle.Height = (int)textSize.Height;
                }
                // Color:
                SolidBrush br = new SolidBrush(this.foreColor);
                
                // Finally...
                if (this.enabled && controlEnabled)
                {
                    if (this.selected)
                    {
                        Rectangle rcSelected = new Rectangle(this.rectangle.Location, this.rectangle.Size);
                        rcSelected.Offset(0, scrollOffset);

                        Brush lightBrush = new SolidBrush(CustomBorderColor.ColorLightLight(Color.Black));
                        Brush darkBrush = new SolidBrush(CustomBorderColor.ColorDark(Color.DarkGray));
                        //rectF.Offset(1F, 1F);                        
                        //rectF.Offset(-1F, -1F);
                        //gfx.DrawString(this.caption, drawFont, darkBrush, rectF, format);
                        gfx.FillRectangle(darkBrush, rcSelected);
                        gfx.DrawString(this.caption, drawFont, lightBrush, rectF, format);
                        darkBrush.Dispose();
                        lightBrush.Dispose();
                    }
                    else
                    {
                        gfx.DrawString(this.caption, drawFont, br, rectF, format);
                    }
                }
                else
                {
                    Brush lightBrush = new SolidBrush(CustomBorderColor.ColorLightLight(backColor));
                    Brush darkBrush = new SolidBrush(CustomBorderColor.ColorDark(backColor));
                    rectF.Offset(1F, 1F);
                    gfx.DrawString(this.caption, drawFont, lightBrush, rectF, format);
                    rectF.Offset(-1F, -1F);
                    gfx.DrawString(this.caption, drawFont, darkBrush, rectF, format);
                    darkBrush.Dispose();
                    lightBrush.Dispose();
                    /*	
                    ControlPaint.DrawStringDisabled(gfx, 
                        this.caption, drawFont, 
                        Color.FromKnownColor(KnownColor.Control), 
                        rectF, format);
                    */
                }
                br.Dispose();
                format.Dispose();
            }            

            // The border around the item if required:
            if (this.owner.DrawStyle == ListBarDrawStyle.ListBarDrawStyleNormal)
            {
                if (this.enabled && controlEnabled)
                {
                    Pen penTopLeft = null;
                    Pen penBottomRight = null;
                    if ((this.mouseDown) && (this.mouseDown))
                    {
                        // inset 3d border:
                        penTopLeft = SystemPens.ControlDarkDark;
                        penBottomRight = SystemPens.ControlLight;
                    }
                    else if ((this.mouseOver) || (this.mouseDown))
                    {
                        // raised 3d border:
                        penTopLeft = SystemPens.ControlLight;
                        penBottomRight = SystemPens.ControlDarkDark;
                    }
                    if (penTopLeft != null)
                    {
                        gfx.DrawLine(penTopLeft, rcHighlight.Left, rcHighlight.Bottom - 2,
                            rcHighlight.Left, rcHighlight.Top);
                        gfx.DrawLine(penTopLeft, rcHighlight.Left, rcHighlight.Top,
                            rcHighlight.Right - 2, rcHighlight.Top);
                        gfx.DrawLine(penBottomRight, rcHighlight.Right - 1, rcHighlight.Top,
                            rcHighlight.Right - 1, rcHighlight.Bottom - 1);
                        gfx.DrawLine(penBottomRight, rcHighlight.Right - 1, rcHighlight.Bottom - 1,
                            rcHighlight.Left, rcHighlight.Bottom - 1);
                    }                    
                }
            }

        }

        /// <summary>
        /// Gets/sets whether this item is enabled.
        /// </summary>
        [Description("Gets/sets whether this item is enabled.")]
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets/sets the foreground colour for this item.
        /// </summary>
        [Description("Gets/sets the foreground colour for this item.")]
        public Color ForeColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                this.foreColor = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets/sets the foreground colour for this item.
        /// </summary>
        [Description("Gets/sets the background colour for this item.")]
        public Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets/sets the font used for this object.  The default
        /// font is null which means the item renders using the
        /// font of the parent control.
        /// </summary>
        [Description("Gets/sets the font for this item.")]
        public Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets the location for this item in the control.
        /// </summary>
        /// <remarks>
        /// The location is relative to the group the 
        /// item belongs to.  Therefore to find the position
        /// relative to the control you need to add the 
        /// bottom position of the button rectangle for the group
        /// and the scroll offset of the item. 
        /// </remarks>
        [Description("Gets the location of this item in the control.")]
        public virtual Point Location
        {
            get
            {
                return this.rectangle.Location;
            }
        }

        /// <summary>
        /// Gets the height of this item.
        /// </summary>
        [Description("Gets the height of this item in the control.")]
        public virtual int Height
        {
            get
            {
                return this.rectangle.Height;
            }
        }

        /// <summary>
        /// Gets the width of this item.
        /// </summary>
        [Description("Gets the width of this item in the control.")]
        public virtual int Width
        {
            get
            {
                return this.rectangle.Width;
            }
        }

        /// <summary>
        /// Returns the rectangle in which the icon is drawn for
        /// this item, relative to the control.
        /// </summary>
        [Description("Returns the rectangle in which the icon is drawn for this item, relative to the control.")]
        public virtual Rectangle IconRectangle
        {
            get
            {
                return this.iconRectangle;
            }
        }

        /// <summary>
        /// Returns the rectangle in which the text is drawn for
        /// this item, relative to the control.
        /// </summary>
        [Description("Returns the rectangle in which the text is drawn for this item, relative to the control.")]
        public virtual Rectangle TextRectangle
        {
            get
            {
                return this.textRectangle;
            }
        }

        /// <summary>
        /// Sets the location and width of this item.  This method
        /// is called by internally by the <see cref="ListBar"/> or
        /// the <see cref="ListBarGroup"/> which owns this item.
        /// </summary>
        /// <remarks>
        /// This member is not intended to be called from client code.
        /// If you do use it, it is likely that a subsequent operation
        /// on the control or group will replace the values.  If you
        /// need more control over placement, override this class
        /// and build the logic into the override for this method
        /// instead.
        /// </remarks>
        /// <param name="location">The new location for the item.</param>
        /// <param name="width">The new width of the item.</param>
        [Description("Sets the location and width of this item in the control. Called internally by the owning ListBar or group")]
        public virtual void SetLocationAndWidth(Point location, int width)
        {
            this.rectangle.Location = location;
            this.rectangle.Width = width;
        }

        /// <summary>
        /// Called to set the height of the item by the owning control.
        /// </summary>
        /// <param name="view">The <see cref="ListBarGroupView"/> in which this
        /// item is being shown.</param>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/>
        /// to use when this item does not have a specific font set.</param>
        /// <param name="imageSize">The size of the images in the ImageList
        /// used to render this view.</param>		
        [Description("Called to set the height of an item by the owning control.")]
        public virtual void SetSize(
            ListBarGroupView view,
            Font defaultFont,
            Size imageSize
            )
        {
            // Select the font we're going to use
            Font drawFont = defaultFont;
            if (this.Font != null)
            {
                drawFont = this.Font;
            }

            // Get the string to measure to determine
            // the item's height
            string measureString = "Xg";
            if (view == ListBarGroupView.LargeIcons)
            {
                // by default we allow for two lines:
                measureString += "\r\nXg";
            }

            // Measure the height of an item 
            Bitmap measureBitmap = new Bitmap(30, 30);
            Graphics graphics = Graphics.FromImage(measureBitmap);
            SizeF textSize = graphics.MeasureString(measureString, drawFont);
            graphics.Dispose();
            measureBitmap.Dispose();

            // Set the height using the text size & the image size
            int height = imageSize.Height;
            if (view == ListBarGroupView.LargeIcons)
            {
                height += (int)textSize.Height;
                height += 12;
            }
            else
            {
                if (textSize.Height > height)
                {
                    height = (int)textSize.Height;
                }
                height += 8;
            }
            this.rectangle.Height = height;
        }

        /// <summary>
        /// Compares this object with another object of the same type.
        /// The implementation compares the captions of the items to
        /// allow items to be sorted alphabetically.
        /// </summary>
        /// <param name="obj">Another ListBarItem object</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the comparands.  
        /// The return value has these meanings: 
        /// &lt; 0: This instance is less than obj.  
        /// 0: This instance is equal to obj.  
        /// &gt; 0: This instance is greater than obj. </returns>
        [Description("Compares this object with another object of the same type.")]
        public virtual System.Int32 CompareTo(System.Object obj)
        {
            return caption.CompareTo(((ListBarItem)obj).Caption);
        }

        /// <summary>
        /// Gets/sets whether this item is "selected" or not.
        /// Only one item in the ListBar control can be selected
        /// at a time.
        /// </summary>
        [Description("Gets/sets whether this item is selected or not.")]
        public virtual bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    NotifyOwner(false);
                }
            }
        }

        /// <summary>
        /// Ensures that this item can be seen in the owner
        /// control.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item is not
        /// part of a ListBarGroup.</exception>
        [Description("Ensures that this item can be seen in the owning control.")]
        public virtual void EnsureVisible()
        {
            if (this.owner != null)
            {
                owner.EnsureItemVisible(this);
            }
            else
            {
                throw new InvalidOperationException("Owner of this ListBarItem has not been set.");
            }
        }

        /// <summary>
        /// Starts editing this item.  The <c>BeforeLabelEdit</c> event will
        /// be fired prior to the text box being made visible.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item is not
        /// part of a ListBarGroup or not part of the selected group
        /// in the control.</exception>
        [Description("Starts editing this item.  The BeforeLabelEdit event will be fired prior to editing commencing.")]
        public virtual void StartEdit()
        {
            if (this.owner != null)
            {
                owner.StartItemEdit(this);
            }
            else
            {
                throw new InvalidOperationException("Owner of this ListBarItem has not been set.");
            }
        }

        /// <summary>
        /// Gets/sets a user-defined string value which can be used
        /// to look up the item in the collection which owns it.
        /// </summary>
        [Description("Gets/sets a user-defined string value which can be used to look up the item in the collection which owns it.")]
        public virtual string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// Gets/sets the tooltip text that will be displayed when
        /// the user hovers over this item.
        /// </summary>
        [Description("Gets/sets the tooltip text that will be displayed when the user hovers over this item.")]
        public virtual string ToolTipText
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
        /// Gets/sets the caption displayed for this item.
        /// </summary>
        [Description("Gets/sets the caption displayed for this item.")]
        public virtual string Caption
        {
            get
            {
                return this.caption;
            }
            set
            {
                this.caption = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets/sets the 0-based index of an icon in an <see cref="System.Windows.Forms.ImageList"/>
        /// displayed with this item.
        /// </summary>
        [Description("Gets/sets the 0-based index of an icon in an ImageList displayed with this item.")]
        public virtual int IconIndex
        {
            get
            {
                return this.iconIndex;
            }
            set
            {
                this.iconIndex = value;
                NotifyOwner(false);
            }
        }

        /// <summary>
        /// Gets/sets an object which can be used to associate
        /// user-defined data with this item.
        /// </summary>
        [Description("Gets/sets an object which can be used to associate user-defined data with this item.")]
        public virtual object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        /// <summary>
        /// Notifies the owning control of a change in this item.
        /// </summary>
        /// <param name="addRemove">Set to true if the change
        /// that has been made requires the size of the display
        /// to be recalculated.</param>
        protected virtual void NotifyOwner(bool addRemove)
        {
            if (owner != null)
            {
                owner.ItemChanged(this, addRemove);
            }
        }

        /// <summary>
        /// Gets the owning ListBar control for this item.
        /// </summary>
        protected internal ListBar Owner
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        /// Sets the owning ListBar control for this item.
        /// </summary>
        /// <param name="owner">The owning ListBar control for this item.</param>
        protected internal void SetOwner(ListBar owner)
        {
            this.owner = owner;
            NotifyOwner(true);
        }

        /// <summary>
        /// Populates a System.Runtime.Serialization.SerializationInfo object with the 
        /// data needed to serialize this object.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo 
        /// to populate with data.</param>
        /// <param name="context">The destination (see 
        /// System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        public virtual void GetObjectData(
            SerializationInfo info,
            StreamingContext context)
        {
            info.AddValue("Font", this.font);
            info.AddValue("ToolTipText", this.toolTipText);
            info.AddValue("Caption", this.caption);
            info.AddValue("ForeColor", this.foreColor);
            info.AddValue("BackColor", this.backColor);
            info.AddValue("Tag", this.tag);
            info.AddValue("Key", this.key);
            info.AddValue("IconIndex", this.iconIndex);
            info.AddValue("Rectangle", this.rectangle);
            info.AddValue("Selected", this.selected);
        }

        /// <summary>
        /// Constructs this object from a serialized representation.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo 
        /// containing the serialized data to build this object from.</param>
        /// <param name="context">The destination (see 
        /// System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        public ListBarItem(
            SerializationInfo info,
            StreamingContext context)
        {
            this.font = (Font)info.GetValue("Font", typeof(Font));
            this.toolTipText = info.GetString("ToolTipText");
            this.caption = info.GetString("Caption");
            this.foreColor = (Color)info.GetValue("ForeColor", typeof(Color));
            this.backColor = (Color)info.GetValue("BackColor", typeof(Color));
            this.tag = info.GetString("Tag");
            this.key = info.GetString("Key");
            this.iconIndex = info.GetInt32("IconIndex");
            this.rectangle = (Rectangle)info.GetValue("Rectangle", typeof(Rectangle));
        }

        /// <summary>
        /// Constructs a new, empty instance of a ListBarItem.
        /// </summary>
        public ListBarItem()
        {
        }
        /// <summary>
        ///  Constructs a new instance of a ListBarItem, specifying
        ///  the caption to display.
        /// </summary>
        /// <param name="caption">The caption for this item.</param>
        public ListBarItem(string caption)
            : this()
        {
            this.caption = caption;
        }
        /// <summary>
        ///  Constructs a new instance of a ListBarItem, specifying
        ///  the caption and the index of the icon to display.
        /// </summary>
        /// <param name="caption">The caption for this item.</param>
        /// <param name="iconIndex">The 0-based index of the icon
        /// to display</param>
        public ListBarItem(
                string caption,
                int iconIndex
            )
            : this(caption)
        {
            this.iconIndex = iconIndex;
        }
        /// <summary>
        ///  Constructs a new instance of a ListBarItem, specifying
        ///  the caption, the index of the icon and the 
        ///  tooltip text.
        /// </summary>
        /// <param name="caption">The caption for this item.</param>
        /// <param name="iconIndex">The 0-based index of the icon
        /// to display</param>
        /// <param name="toolTipText">The tooltip text to show
        /// when the mouse hovers over this item.</param>
        public ListBarItem(
            string caption,
            int iconIndex,
            string toolTipText
            )
            : this(caption, iconIndex)
        {
            this.toolTipText = toolTipText;
        }
        /// <summary>
        ///  Constructs a new instance of a ListBarItem, specifying
        ///  the caption, the index of the icon, the 
        ///  tooltip text and the tag.
        /// </summary>
        /// <param name="caption">The caption for this item.</param>
        /// <param name="iconIndex">The 0-based index of the icon
        /// to display</param>
        /// <param name="toolTipText">The tooltip text to show
        /// when the mouse hovers over this item.</param>
        /// <param name="tag">An object which can be used to 
        /// associate user-defined data with the item.</param>
        public ListBarItem(
            string caption,
            int iconIndex,
            string toolTipText,
            object tag
            )
            : this(caption, iconIndex, toolTipText)
        {
            this.tag = tag;
        }
        /// <summary>
        ///  Constructs a new instance of a ListBarItem, specifying
        ///  the caption, the index of the icon, the 
        ///  tooltip text, the tag and the key.
        /// </summary>
        /// <param name="caption">The caption for this item.</param>
        /// <param name="iconIndex">The 0-based index of the icon
        /// to display</param>
        /// <param name="toolTipText">The tooltip text to show
        /// when the mouse hovers over this item.</param>
        /// <param name="tag">An object which can be used to 
        /// associate user-defined data with the item.</param>
        /// <param name="key">A user-defined string which is 
        /// associated with the item.</param>
        public ListBarItem(
            string caption,
            int iconIndex,
            string toolTipText,
            object tag,
            string key
            )
            : this(caption, iconIndex, toolTipText, tag)
        {
            this.key = key;
        }

    }

}
