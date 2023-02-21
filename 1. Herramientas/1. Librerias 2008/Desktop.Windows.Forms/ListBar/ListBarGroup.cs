using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Desktop.Windows.Forms
{
    #region ListBarGroup class
    /// <summary>
    /// A <c>ListBarGroup</c> is a bar within a <see cref="ListBar"/> control.
    /// A bar can either contain items or it can contain a Windows
    /// Forms control.
    /// </summary>
    [SerializableAttribute()]
    public class ListBarGroup : IMouseObject, ISerializable
    {
        /// <summary>
        /// The owning control.
        /// </summary>
        private ListBar owner = null;
        /// <summary>
        /// The caption of the group.
        /// </summary>
        private string caption = "";
        /// <summary>
        /// Whether the item is selected or not.
        /// </summary>
        private bool selected = false;
        /// <summary>
        /// The tooltip text for this group.
        /// </summary>
        private string toolTipText = "";
        /// <summary>
        /// The string key to associate with this item.
        /// </summary>
        private string key = "";
        /// <summary>
        /// User-defined data to associate with this item.
        /// </summary>
        private object tag = null;
        /// <summary>
        /// Temporary array to hold the subitems to add to
        /// this group once it's owner has been assigned.
        /// </summary>
        private ListBarItem[] subItems;
        /// <summary>
        /// The collection of items associated with this 
        /// group.
        /// </summary>
        private ListBarItemCollection items = null;
        /// <summary>
        /// A child control to display in this bar instead
        /// of the child items.
        /// </summary>
        private Control childControl = null;
        /// <summary>
        /// Font to render this group with.
        /// </summary>
        private Font font = null;
        /// <summary>
        /// ForeColor to render this group with.
        /// </summary>
        private Color foreColor = Color.FromKnownColor(KnownColor.WindowText);
        /// <summary>
        /// BackColor to render this group with.
        /// </summary>
        private Color backColor = Color.FromKnownColor(KnownColor.Control);
        /// <summary>
        /// Bounding rectangle for this group's button.  The height
        /// is managed by this object but the other members are typically
        /// adjusted by the owning control through the <see cref="SetLocationAndWidth"/>
        /// and the <see cref="SetButtonHeight"/> methods.
        /// </summary>
        protected Rectangle rectangle = new Rectangle(0, 0, 0, 24);
        /// <summary>
        /// The view (LargeIcons or SmallIcons) to use when drawing the items 
        /// in the bar.
        /// </summary>
        private ListBarGroupView iconSize = ListBarGroupView.LargeIcons;
        /// <summary>
        /// The scroll 
        /// </summary>
        private int scrollOffset = 0;
        /// <summary>
        /// Whether the mouse is over the button or not.
        /// </summary>
        private bool mouseOver = false;
        /// <summary>
        /// Whether the mouse is down on the button or not.
        /// </summary>
        private bool mouseDown = false;
        /// <summary>
        /// The point at which the mouse was clicked on the group
        /// button.
        /// </summary>
        private Point mouseDownPoint = new Point(0, 0);
        /// <summary>
        /// Whether the group is visible or not.
        /// </summary>
        private bool visible = true;

        /// <summary>
        /// Returns a string representation of this <see cref="ListBarGroup"/>.
        /// </summary>
        /// <returns>A string containing the class name, caption, rectangle
        /// and item count for this group.</returns>
        [Description("Returns a string representation of this ListBarGroup.")]
        public override string ToString()
        {
            return String.Format("{0} Caption={1} Location={2} Height={3} ItemCount={4}",
                this.GetType().FullName, this.caption, this.ButtonLocation, this.ButtonHeight, this.items.Count);
        }

        /// <summary>
        /// Returns the selected <see cref="ListBarItem"/> in this Group, if any, otherwise null.
        /// </summary>
        [Description("Returns the selected ListBarItem in this Group, if any, otherwise null.")]
        public ListBarItem SelectedItem
        {
            get
            {
                ListBarItem ret = null;
                foreach (ListBarItem item in this.items)
                {
                    if (item.Selected)
                    {
                        ret = item;
                        break;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets/sets a <see cref="System.Windows.Forms.Control"/>
        /// which can be displayed within this group.
        /// </summary>
        /// <remarks>
        /// Do not set the child control until this group has
        /// been added to the control.
        /// </remarks>
        [Description("Gets/sets a Control which is displayed in this group rather than items.")]
        public Control ChildControl
        {
            get
            {
                return this.childControl;
            }
            set
            {
                value.Visible = false;
                this.childControl = value;
                this.childControl.Parent = this.owner;
                NotifyOwner(true);
            }
        }

        /// <summary>
        /// Gets/sets the point at which the mouse was clicked on the group
        /// button.
        /// </summary>
        [Description("Gets/sets the point at which the mouse was clicked on the group button.")]
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
        /// Gets/sets whether the mouse is over the group button.
        /// </summary>
        [Description("Gets/sets whether the mouse is over the group button.")]
        public bool MouseOver
        {
            get
            {
                return this.mouseOver;
            }
            set
            {
                this.mouseOver = (value & this.visible);
            }
        }

        /// <summary>
        /// Gets/sets whether the mouse is down over the group button.
        /// </summary>
        [Description("Gets/sets whether the mouse is down over the group button.")]
        public bool MouseDown
        {
            get
            {
                return this.mouseDown;
            }
            set
            {
                this.mouseDown = (value & this.visible);
            }
        }

        /// <summary>
        /// Called to create a new item collection for this ListBarGroup.
        /// </summary>
        /// <returns>The ListBarItemCollection that will be used for this
        /// ListBarGroup</returns>
        protected virtual ListBarItemCollection CreateListBarItemCollection()
        {
            return new ListBarItemCollection(owner);
        }

        /// <summary>
        /// Called to create a new item collection for this ListBarGroup
        /// when the data is being deserialized
        /// </summary>
        /// <returns>The ListBarItemCollection that will be used for this
        /// ListBarGroup</returns>
        protected virtual ListBarItemCollection CreateListBarItemCollection(
            SerializationInfo info,
            StreamingContext context)
        {
            return new ListBarItemCollection(info, context);
        }

        /// <summary>
        /// Internal member holding the negative scrolled 
        /// offset of this bar from the top of the client area
        /// </summary>
        protected internal int ScrollOffset
        {
            get
            {
                return this.scrollOffset;
            }
            set
            {
                this.scrollOffset = value;
            }
        }

        /// <summary>
        /// Gets/sets the which view to show the items within this bar.
        /// </summary>
        [Description("Gets/sets the which view to show the items within this bar.")]
        public ListBarGroupView View
        {
            get
            {
                return this.iconSize;
            }
            set
            {
                if (this.iconSize != value)
                {
                    this.iconSize = value;
                    SetLocationAndWidth(this.rectangle.Location, this.rectangle.Width);
                    NotifyOwner(true);
                }
            }
        }

        private void SetItemSize()
        {
            if (this.items != null)
            {
                foreach (ListBarItem item in this.items)
                {
                    this.owner.ItemChanged(item, false);
                }
                NotifyOwner(true);
            }
        }
        /// <summary>
        /// Called to set the height of this group's button by the owning control.
        /// </summary>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/>
        /// to use when this item does not have a specific font set.</param>
        [Description("Called to set the height of this group's button by the owning control.")]
        public virtual void SetButtonHeight(
            Font defaultFont
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
            // Measure the height of an item 
            Bitmap measureBitmap = new Bitmap(30, 30);
            Graphics graphics = Graphics.FromImage(measureBitmap);
            SizeF textSize = graphics.MeasureString(measureString, drawFont);
            graphics.Dispose();
            measureBitmap.Dispose();

            this.rectangle.Height = (int)textSize.Height + 8;
        }


        /// <summary>
        /// Returns the location of the button
        /// which activates this group relative
        /// to the owning control.
        /// </summary>
        [Description("Returns the location of the button which activates this group relative to the owning control.")]
        public virtual Point ButtonLocation
        {
            get
            {
                return this.rectangle.Location;
            }
        }

        /// <summary>
        /// Returns the width of the button
        /// which activates this group.
        /// </summary>
        [Description("Returns the width of the button which activates this group.")]
        public virtual int ButtonWidth
        {
            get
            {
                return this.rectangle.Width;
            }
        }

        /// <summary>
        /// Returns the height of the button
        /// which activates this group.
        /// </summary>
        [Description("Returns the height of the button which activates this group.")]
        public virtual int ButtonHeight
        {
            get
            {
                return this.rectangle.Height;
            }
        }

        /// <summary>
        /// Sets the location and width of the button which
        /// activates this <see cref="ListBarGroup"/>.  This method
        /// is called by internally by the <see cref="ListBar"/> 
        /// which owns this item.
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
        [Description("Sets the location and width of the button which activates this group.  This method is called internally by the owning ListBar control.")]
        public virtual void SetLocationAndWidth(Point location, int width)
        {
            this.rectangle.Location = location;
            this.rectangle.Width = width;
            if (this.items != null)
            {
                Point itemLocation = new Point(location.X, 0);

                Font defaultFont = this.Font;
                if (defaultFont == null)
                {
                    if (this.owner == null)
                    {
                        defaultFont = System.Windows.Forms.SystemInformation.MenuFont;
                    }
                    else
                    {
                        defaultFont = this.owner.Font;
                    }
                }

                Size imageSize = new Size(32, 32);
                if ((this.iconSize == ListBarGroupView.LargeIcons) || (this.iconSize == ListBarGroupView.LargeIconsOnly))
                {
                    if (this.owner != null)
                    {
                        if (this.owner.LargeImageList != null)
                        {
                            imageSize = this.owner.LargeImageList.ImageSize;
                        }
                    }
                }
                else
                {
                    imageSize.Width = 16;
                    imageSize.Height = 16;
                    if (this.owner != null)
                    {
                        if (this.owner.SmallImageList != null)
                        {
                            imageSize = this.owner.SmallImageList.ImageSize;
                        }
                    }
                }

                if ((this.View == ListBarGroupView.SmallIconsOnly) || (this.View == ListBarGroupView.LargeIconsOnly))
                {
                    int itemWidth = imageSize.Width + 16;
                    for (int i = 0; i < this.items.Count; i++)
                    {
                        ListBarItem item = this.items[i];
                        item.SetSize(this.View, defaultFont, imageSize);
                        item.SetLocationAndWidth(itemLocation, itemWidth);
                        itemLocation.X += item.Width;
                        if (i < this.items.Count - 1)
                        {
                            if ((item.Location.X + this.items[i + 1].Width) > width)
                            {
                                itemLocation.X = location.X;
                                itemLocation.Y += item.Height;
                            }
                        }
                    }
                }
                else
                {
                    if (this.Owner != null)
                    {
                        width = this.Owner.Width;
                    }

                    foreach (ListBarItem item in this.items)
                    {
                        item.SetSize(this.iconSize, defaultFont, imageSize);
                        item.SetLocationAndWidth(itemLocation, width);
                        itemLocation.Y += item.Height;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the items within this <see cref="ListBarGroup"/> onto the control.
        /// </summary>
        /// <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        /// <param name="bounds">The bounding <see cref="System.Drawing.Rectangle"/> within which
        /// to draw the items.</param>
        /// <param name="ils">The <see cref="System.Windows.Forms.ImageList"/> object to use to draw
        /// the bar.</param>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use.</param>
        /// <param name="style">The style to draw the ListBar in.</param>
        /// <param name="enabled">Whether the ListBar control is enabled or not.</param>
        [Description("Draws the items within this group bar onto the ListBar control.  Called internally by the owning ListBar control.")]
        public virtual void DrawBar(
            Graphics gfx,
            Rectangle bounds,
            ImageList ils,
            Font defaultFont,
            ListBarDrawStyle style,
            bool enabled
            )
        {
            if (this.childControl != null)
            {
                this.childControl.Location = bounds.Location;
                this.childControl.Size = bounds.Size;
            }
            else
            {
                this.Items.Draw(
                    gfx, bounds, ils, defaultFont,
                    style, this.View, enabled,
                    this.scrollOffset + this.rectangle.Bottom);
            }
        }

        /// <summary>
        /// Draws the button for this group onto the control.
        /// </summary>
        /// <param name="gfx">The <see cref="System.Drawing.Graphics"/> object to draw onto.</param>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to 
        /// draw with.</param>
        /// <param name="enabled">Whether this control is enabled or not.</param>
        [Description("Draws the button for this group onto the control.")]
        public virtual void DrawButton(
            Graphics gfx,
            Font defaultFont,
            bool enabled
            )
        {
            if (this.visible)
            {
                // Get the font to draw with:
                Font drawFont = this.font;
                if (drawFont == null)
                {
                    drawFont = defaultFont;
                }

                // Fill the item:
                Brush br = new SolidBrush(this.backColor);
                gfx.FillRectangle(br, this.rectangle);
                br.Dispose();

                // Draw the text:
                StringFormat format = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap);
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                RectangleF rectF = new RectangleF(rectangle.X, rectangle.Y,
                    rectangle.Width, rectangle.Height);
                if (enabled)
                {
                    br = new SolidBrush(this.foreColor);
                    gfx.DrawString(this.caption, drawFont, br, rectF, format);
                    br.Dispose();
                }
                else
                {
                    rectF.Offset(1F, 1F);
                    br = new SolidBrush(CustomBorderColor.ColorLightLight(this.backColor));
                    gfx.DrawString(this.caption, drawFont, br, rectF, format);
                    br.Dispose();
                    rectF.Offset(-1F, -1F);
                    br = new SolidBrush(CustomBorderColor.ColorDark(this.backColor));
                    gfx.DrawString(this.caption, drawFont, br, rectF, format);
                    br.Dispose();
                }
                format.Dispose();

                // Draw the border:
                Pen darkDarkPen = new Pen(CustomBorderColor.ColorDarkDark(this.BackColor));
                Pen darkPen = new Pen(CustomBorderColor.ColorDark(this.BackColor));
                Pen lightPen = new Pen(CustomBorderColor.ColorLight(this.BackColor));
                Pen lightLightPen = new Pen(CustomBorderColor.ColorLightLight(this.BackColor));

                if (this.mouseDown && this.mouseOver)
                {
                    gfx.DrawLine(darkDarkPen,
                        rectangle.Left, rectangle.Bottom - 2,
                        rectangle.Left, rectangle.Top);
                    gfx.DrawLine(darkDarkPen,
                        rectangle.Left, rectangle.Top,
                        rectangle.Right - 2, rectangle.Top);
                    gfx.DrawLine(darkPen,
                        rectangle.Left + 1, rectangle.Bottom - 3,
                        rectangle.Left + 1, rectangle.Top + 1);
                    gfx.DrawLine(darkPen,
                        rectangle.Left + 1, rectangle.Top + 1,
                        rectangle.Right - 3, rectangle.Top + 1);

                    gfx.DrawLine(lightLightPen,
                        rectangle.Right - 1, rectangle.Top,
                        rectangle.Right - 1, rectangle.Bottom - 1);
                    gfx.DrawLine(lightLightPen,
                        rectangle.Right - 1, rectangle.Bottom - 1,
                        rectangle.Left, rectangle.Bottom - 1);
                    gfx.DrawLine(lightPen,
                        rectangle.Right - 2, rectangle.Top + 1,
                        rectangle.Right - 2, rectangle.Bottom - 2);
                    gfx.DrawLine(lightPen,
                        rectangle.Right - 2, rectangle.Bottom - 2,
                        rectangle.Left + 1, rectangle.Bottom - 2);
                }
                else if (this.MouseOver || this.mouseDown)
                {
                    gfx.DrawLine(lightLightPen,
                        rectangle.Left, rectangle.Bottom - 2,
                        rectangle.Left, rectangle.Top);
                    gfx.DrawLine(lightLightPen,
                        rectangle.Left, rectangle.Top,
                        rectangle.Right - 2, rectangle.Top);
                    gfx.DrawLine(lightPen,
                        rectangle.Left + 1, rectangle.Bottom - 3,
                        rectangle.Left + 1, rectangle.Top + 1);
                    gfx.DrawLine(lightPen,
                        rectangle.Left + 1, rectangle.Top + 1,
                        rectangle.Right - 3, rectangle.Top + 1);

                    gfx.DrawLine(darkDarkPen,
                        rectangle.Right - 1, rectangle.Top,
                        rectangle.Right - 1, rectangle.Bottom - 1);
                    gfx.DrawLine(darkDarkPen,
                        rectangle.Right - 1, rectangle.Bottom - 1,
                        rectangle.Left, rectangle.Bottom - 1);
                    gfx.DrawLine(darkPen,
                        rectangle.Right - 2, rectangle.Top + 1,
                        rectangle.Right - 2, rectangle.Bottom - 2);
                    gfx.DrawLine(darkPen,
                        rectangle.Right - 2, rectangle.Bottom - 2,
                        rectangle.Left + 1, rectangle.Bottom - 2);
                }
                else
                {
                    gfx.DrawLine(lightLightPen,
                        rectangle.Left, rectangle.Bottom - 2,
                        rectangle.Left, rectangle.Top + 1);
                    gfx.DrawLine(lightLightPen,
                        rectangle.Left, rectangle.Top + 1,
                        rectangle.Right - 2, rectangle.Top + 1);
                    gfx.DrawLine(darkPen,
                        rectangle.Right - 1, rectangle.Top + 1,
                        rectangle.Right - 1, rectangle.Bottom - 1);
                    gfx.DrawLine(darkPen,
                        rectangle.Right - 1, rectangle.Bottom - 1,
                        rectangle.Left, rectangle.Bottom - 1);
                }

                lightLightPen.Dispose();
                lightPen.Dispose();
                darkPen.Dispose();
                darkDarkPen.Dispose();

            }
        }

        /// <summary>
        /// Returns the collection of items belonging to this <see cref="ListBarGroup" />.
        /// group.
        /// </summary>
        [Description("Returns the collection of items belonging to this ListBarGroup")]
        public virtual ListBarItemCollection Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets/sets whether this group is visible in the control 
        /// or not.
        /// </summary>
        [Description("Gets/sets whether this group is visible in the control or not.")]
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
                NotifyOwner(true);
            }
        }


        /// <summary>
        /// Gets/sets the <see cref="System.Drawing.Font"/> to draw the caption for this group.
        /// </summary>
        [Description("Returns the Font used to draw the caption for this group.")]
        public virtual Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
                NotifyOwner(true);
            }
        }

        /// <summary>
        /// Gets/sets the foreground colour to use when drawing
        /// the button for this group.
        /// </summary>
        [Description("Gets/sets the foreground colour to use when drawing the button for this group.")]
        public virtual Color ForeColor
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
        /// Gets/sets the background colour to use when drawing the button for this group.
        /// </summary>
        [Description("Gets/sets the background colour to use when drawing the button for this group.")]
        public virtual Color BackColor
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
        /// Gets/sets the caption displayed for this group.
        /// </summary>
        [Description("Gets/sets the caption displayed for this group.")]
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
        /// Gets/sets the string key associated with this group.
        /// </summary>
        [Description("Gets/sets a string key associated with this group.")]
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
        /// Gets/sets the tooltip that will be displayed when the user
        /// hovers over this group's button.
        /// </summary>
        [Description("Gets/sets the tooltip text that will be displayed when the user hovers over this group's button.")]
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
        /// Gets/sets whether this group is selected or not.
        /// </summary>
        [Description("Gets/sets whether this group is selected or not.")]
        public virtual bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (value != this.selected)
                {
                    this.selected = value;
                    if (this.childControl != null)
                    {
                        this.childControl.Visible = value;
                    }
                    NotifyOwner(false);
                }
            }
        }
        /// <summary>
        /// Gets/sets a user-defined object associated with this group.
        /// </summary>
        [Description("Gets/sets a user-defined object associated with this group.")]
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
        /// Starts editing this item.  The <c>BeforeLabelEdit</c> event will
        /// be fired prior to the text box being made visible.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item is not
        /// part of a ListBar control.</exception>
        [Description("Starts editing this item and fires the BeforeLabelEdit event.")]
        public virtual void StartEdit()
        {
            if (this.owner != null)
            {
                owner.StartGroupEdit(this);
            }
            else
            {
                throw new InvalidOperationException("Owner of this ListBarGroup has not been set.");
            }
        }
        /// <summary>
        /// Notifies the owning ListBar control of any changes to a group.
        /// </summary>
        /// <param name="addRemove">Whether the control should resize
        /// all groups associated with the ListBar.</param>
        protected virtual void NotifyOwner(bool addRemove)
        {
            if (owner != null)
            {
                owner.GroupChanged(this, addRemove);
            }
        }

        /// <summary>
        /// Sets the owning control for this Group.  Called automatically
        /// whenever a group is added to the group collection associated with
        /// a ListBar control.
        /// </summary>
        /// <param name="owner">The ListBar control which owns this group.</param>
        protected internal void SetOwner(ListBar owner)
        {
            this.owner = owner;
            if (this.items == null)
            {
                this.items = CreateListBarItemCollection();
            }
            if (this.subItems != null)
            {
                this.items.AddRange(subItems);
                this.subItems = null;
            }
            // Set the size of any items which belong
            // to this bar:
            SetItemSize();

            NotifyOwner(true);
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
            info.AddValue("Rectangle", this.rectangle);
            info.AddValue("View", ((int)this.iconSize));
            info.AddValue("Selected", this.selected);

            info.AddValue("Items", this.items);
        }

        /// <summary>
        /// Constructs this object from a serialized representation.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo 
        /// containing the serialized data to build this object from.</param>
        /// <param name="context">The destination (see 
        /// System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        public ListBarGroup(
            SerializationInfo info,
            StreamingContext context)
        {
            this.font = (Font)info.GetValue("Font", typeof(Font));
            this.toolTipText = info.GetString("ToolTipText");
            this.caption = info.GetString("Caption");
            this.foreColor = (Color)info.GetValue("ForeColor", typeof(Color));
            this.backColor = (Color)info.GetValue("BackColor", typeof(Color));
            this.tag = info.GetValue("Tag", typeof(object));
            this.key = info.GetString("Key");
            this.rectangle = (Rectangle)info.GetValue("Rectangle", typeof(Rectangle));
            this.View = (ListBarGroupView)info.GetInt32("View");
            this.selected = info.GetBoolean("Selected");

            this.items = CreateListBarItemCollection(info, context);

        }


        /// <summary>
        /// Constructs a new, blank instance of a ListBarGroup.
        /// </summary>
        public ListBarGroup()
        {
            // intentionally empty
        }

        /// <summary>
        /// Constructs a new instance of a ListBarGroup with the specified
        /// caption.
        /// </summary>
        /// <param name="caption">Caption for the group's control button.</param>
        public ListBarGroup(
            string caption
            )
            : this()
        {
            this.caption = caption;
        }
        /// <summary>
        /// Constructs a new instance of a ListBarGroup with the specified
        /// caption and items.
        /// </summary>
        /// <param name="caption">Caption for the group's control button.</param>
        /// <param name="subItems">The array of items to add to the group's
        /// collection of items.</param>
        public ListBarGroup(
            string caption,
            ListBarItem[] subItems
            )
            : this(caption)
        {
            this.subItems = subItems;
        }
        /// <summary>
        /// Constructs a new instance of a ListBarGroup with the specified
        /// caption and tooltip text.
        /// </summary>
        /// <param name="caption">Caption for the group's control button.</param>
        /// <param name="toolTipText">ToolTip text to show when hovering over
        /// the group.</param>
        public ListBarGroup(
            string caption,
            string toolTipText
            )
            : this(caption)
        {
            this.toolTipText = toolTipText;
        }
        /// <summary>
        /// Constructs a new instance of a ListBarGroup with the specified
        /// caption, tooltip text and user-defined data.
        /// </summary>
        /// <param name="caption">Caption for the group's control button.</param>
        /// <param name="toolTipText">ToolTip text to show when hovering over
        /// the group.</param>
        /// <param name="tag">User-defined object data which is associated with
        /// the group.</param>
        public ListBarGroup(
            string caption,
            string toolTipText,
            object tag
            )
            : this(caption, toolTipText)
        {
            this.tag = tag;
        }

    }
    #endregion

}
