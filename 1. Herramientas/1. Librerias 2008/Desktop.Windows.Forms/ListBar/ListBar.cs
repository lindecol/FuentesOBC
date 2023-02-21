using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Diagnostics;



namespace Desktop.Windows.Forms
{
	#region ListBar Control class
	/// <summary>
	/// An implementation of a Microsoft Outlook Style ListBar control.
	/// The control provides all the features needed to implement a replica
	/// of the Outlook style control and is also designed to allow the same
	/// functionality to be used in overriden controls in which the
	/// individual sizing and appearance of each of the UI components can be
	/// customised.
	/// 
	/// The <c>ListBar</c> control is modelled as an extension to
	/// the <c>System.Windows.Forms.UserControl</c> class.  Bars
	/// are configured using <see cref="ListBarGroup" /> objects which are
	/// collected in the <see cref="ListBarGroupCollection" /> object
	/// accessible through the control's <see cref="Groups" /> accessor.
	/// Each <see cref="ListBarGroup" /> in turn contains a 
	/// <see cref="ListBarItemCollection" /> of <see cref="ListBarItem" /> objects 
	/// which represent the buttons within a group.
	/// </summary>	
	///
	/// <remarks>
	/// Copyright &#169; 2003 Steve McMahon for vbAccelerator.com.
	/// vbAccelerator is a Trade Mark of vbAccelerator Ltd.  All Rights
	/// Reserved.  Please visit http://vbaccelerator.com/ for more
	/// on this and other VB and .NET Framework code.  Comments to
	/// mailto:steve@vbaccelerator.com.
	/// </remarks>
	/// 
	public class ListBar : System.Windows.Forms.UserControl
	{
	
		#region Member Variables
		/// <summary>
		/// Reference to the collection of groups contained within the ListBar control.
		/// </summary>
		private ListBarGroupCollection groups = null;
		/// <summary>
		/// Reference to an external ToolTip object.
		/// </summary>
		private ToolTip toolTip = null;
		/// <summary>
		/// Reference to an external Image List for drawing the large icon view.
		/// </summary>
		private ImageList largeImageList = null;
		/// <summary>
		/// Reference to an external Image List for drawing the small icon view.
		/// </summary>
		private ImageList smallImageList = null;
		/// <summary>
		/// A timer for controlling scrolling when the scroll buttons are held
		/// down.
		/// </summary>
		private Timer buttonPressed = new Timer();
		/// <summary>
		/// Contains a reference to the active scroll button when one is pressed
		/// and the mouse is over it.
		/// </summary>
		private ListBarScrollButton activeButton = null;
		/// <summary>
		/// The last time a scroll occurred during a drag-drop operation.  Used
		/// to control the speed of scrolling during drag-drop.
		/// </summary>
		private DateTime lastScrollTime = DateTime.Now;
		/// <summary>
		/// Drawing style fo the control.
		/// </summary>
		private ListBarDrawStyle drawStyle = ListBarDrawStyle.ListBarDrawStyleOfficeXP;
		/// <summary>
		/// Last width the control was drawn at.  Used to control resizing.
		/// </summary>
		private int lastWidth = 0;
		/// <summary>
		/// Last height the control was drawn at.  Used to control resizing.
		/// </summary>
		private int lastHeight = 0;
		/// <summary>
		/// Flag to control whether redrawing occurs or not
		/// during updating:
		/// </summary>
		private bool redraw = true;
		/// <summary>
		/// Up scroll button reference.
		/// </summary>
		protected ListBarScrollButton btnUp;
		/// <summary>
		/// Down scroll buttons reference.
		/// </summary>
		protected ListBarScrollButton btnDown;
		/// <summary>
		/// The rectangle containing the "ListView" portion of the control.
		/// </summary>
		private Rectangle rcListView;
		/// <summary>
		/// The object that the mouse is currently over, if any.
		/// </summary>
		private IMouseObject mouseTrack = null;
		/// <summary>
		/// The object that the mouse is currently down on, if any.
		/// </summary>
		private IMouseObject mouseDown = null;
		/// <summary>
		/// Whether items are selected on MouseDown or
		/// MouseUp.
		/// </summary>
		private bool selectOnMouseDown = false;
		/// <summary>
		/// Whether items can be dragged or not
		/// </summary>
		private bool allowDragItems = true;
		/// <summary>
		/// Whether groups can be dragged or not
		/// </summary>
		private bool allowDragGroups = true;
		/// <summary>
		/// During drag-drop, the insert point, if any.
		/// </summary>
		private ListBarDragDropInsertPoint dragInsertPoint = null;
		/// <summary>
		/// The object that was last hovered over during
		/// drag-drop, if any:
		/// </summary>
		private IMouseObject dragHoverOver = null;
		/// <summary>
		/// The time at which hovering started over the object
		/// which is currently being hovered over:
		/// </summary>
		private DateTime dragHoverOverStartTime = DateTime.Now;
		/// <summary>
		/// The ListBarItem currently being edited, if any
		/// </summary>
		private ListBarItem editItem = null;
		/// <summary>
		/// The ListBarGroup currently being edited, if any
		/// </summary>
		private ListBarGroup editGroup = null;
		/// <summary>
		/// Are we scrolling a new group into view or not?
		/// </summary>
		private bool scrollingGroup = false;
		/// <summary>
		/// The index of the group which is currently selected
		/// when scrolling a new group into view:
		/// </summary>
		protected int indexCurrent = -1;
		/// <summary>
		/// The index of the newly selected group which will replace
		/// the selected index when scrolling a new group into view:
		/// </summary>
		protected int indexNew = -1;
		/// <summary>
		/// The Text Box used for editing an item's caption.
		/// </summary>
		private System.Windows.Forms.TextBox txtEdit;
		/// <summary>
		/// A class to determine when the TextBox used for
		/// editing should be cancelled:
		/// </summary>
		private PopupCancelNotifier popupCancel;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Events
		/// <summary>
		/// Raised before the selected group in the ListBar control is changed. Allows
		/// the group selection to be cancelled.
		/// </summary>
		[Description("Raised before the selected group in the ListBar control is changed.")]
		public event BeforeGroupChangedEventHandler BeforeSelectedGroupChanged;
		/// <summary>
		/// Raised when the selected group in a ListBar control has been
		/// changed.
		/// </summary>
		[Description("Raised once the selected group in the ListBar control has been changed.")]
		public event System.EventHandler SelectedGroupChanged;
		/// <summary>
		/// Raised before an item in a ListBar control is clicked.  Allows
		/// the item selection to be cancelled.
		/// </summary>
		[Description("Raised before an item in the ListBar control is clicked.")]
		public event BeforeItemClickedEventHandler BeforeItemClicked;
		/// <summary>
		/// Raised when an item has been clicked in the ListBar control.
		/// </summary>
		[Description("Raised once an item in the ListBar control has been clicked.")]
		public event ItemClickedEventHandler ItemClicked;
		/// <summary>
		/// Raised when an item has been double clicked in the ListBar control.
		/// </summary>
		[Description("Raised when an item has been double clicked in the ListBar control.")]
		public event ItemClickedEventHandler ItemDoubleClicked;
		/// <summary>
		/// Raised when a group has been clicked in the ListBar control.
		/// </summary>
		[Description("Raised when a group has been clicked in the ListBar control.")]
		public event GroupClickedEventHandler GroupClicked;
		/// <summary>
		/// Raised before an item's label is about to be edited in the ListBar
		/// control.  Allows the label edit to be cancelled.
		/// </summary>
		[Description("Raised before an item's label is about to be edited in the ListBar control.")]
		public event ListBarLabelEditEventHandler BeforeLabelEdit;
		/// <summary>
		/// Raised after an item's label has been edited in the ListBar control.
		/// Allows the new caption to be checked and the edit cancelled.
		/// </summary>
		[Description("Raised after an item's label has been edited but before the change is committed.")]
		public event ListBarLabelEditEventHandler AfterLabelEdit;
		#endregion

		#region Constructor and Dispose/Finalise
		/// <summary>
		/// Creates a new instance of a ListBar control.
		/// </summary>
		public ListBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Set up the control:
			this.SetStyle(
				ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.SupportsTransparentBackColor, 
				true);

			// Initialisation:
			groups = CreateListBarGroupCollection();
			btnUp = CreateListBarScrollButton(ListBarScrollButton.ListBarScrollButtonType.Up);
			btnDown = CreateListBarScrollButton(ListBarScrollButton.ListBarScrollButtonType.Down);
			
			// Scroll timer:
			buttonPressed.Interval = 350;
			buttonPressed.Enabled = false;
			buttonPressed.Tick += new EventHandler(buttonPressed_Tick);

			// Text box:
			txtEdit.KeyDown += new KeyEventHandler(txtEdit_KeyDown);
			
			popupCancel = new PopupCancelNotifier();
			popupCancel.PopupCancel += new PopupCancelEventHandler(popupCancel_PopupCancel);

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Responding to events
		/// <summary>
		/// Controls scrolling when the mouse is over and down on a scroll
		/// bar button.
		/// </summary>
		/// <param name="sender">The object which raised this event.</param>
		/// <param name="e">Arguments associated with this event.</param>
		private void buttonPressed_Tick(object sender, System.EventArgs e)
		{
			// check if the mouse is still over a scroll button
			// that's been pressed:
			if (activeButton != null)
			{
				// shorten the interval for the next scroll down
				// to 75ms:
				buttonPressed.Interval = 75;
				// Check if mouse in button:
				Point pos = Cursor.Position;
				pos = this.PointToClient(pos);
				if (activeButton.HitTest(pos))
				{
					// perform the scrolling:
					ScrollTo(activeButton, true);
				}				
			}
		}

		/// <summary>
		/// Scroll the control for the selected button.
		/// </summary>
		/// <param name="button">Button to scroll for.</param>
		/// <param name="fromTimer">Whether request to scroll from a 
		/// scroll button timer event.</param>
		private void ScrollTo(ListBarScrollButton button, bool fromTimer)
		{
			int direction = 
				(button.ButtonType == ListBarScrollButton.ListBarScrollButtonType.Up ? 1 : -1);
			ScrollTo(direction, fromTimer);
		}

		/// <summary>
		/// Scroll the control for the selected button.
		/// </summary>
		/// <param name="button">Button to scroll for.</param>
		private void ScrollTo(ListBarScrollButton button)
		{
			ScrollTo(button, false);
		}

		
		/// <summary>
		/// Scroll the control in the specified direction.
		/// </summary>
		/// <param name="direction">The direction to move in.  Note that this follows
		/// the direction of movement of an item: +1 scrolls up, -1 scrolls down.</param>
		private void ScrollTo(int direction)
		{
			ScrollTo(direction, false);
		}

		/// <summary>
		/// Scroll the control in the specified direction.
		/// </summary>
		/// <param name="direction">The direction to move in.  Note that this follows
		/// the direction of movement of an item: +1 scrolls up, -1 scrolls down.</param>
		/// <param name="fromTimer">Whether request to scroll from a 
		/// scroll button timer event.</param>
		private void ScrollTo(int direction, bool fromTimer)
		{
			// get the distance we must scroll to move one entire
			// item:
			ListBarGroup selGroup = SelectedGroup;
			int endScrollOffset = selGroup.ScrollOffset + 
				(direction * selGroup.Items[0].Height);
			if (endScrollOffset > 0)
			{
				endScrollOffset = 0;
			}

			// Get the invalidation rectangle:
			Rectangle rcInvalid = new Rectangle(
				new Point(1, selGroup.ButtonLocation.X + selGroup.ButtonHeight),
				new Size(this.Width - 2, 
				((groups.IndexOf(selGroup) == groups.Count - 1) ? 
				this.Height - (selGroup.ButtonLocation.Y + selGroup.ButtonHeight) :
				groups[groups.IndexOf(selGroup) + 1].ButtonLocation.Y)));			

			// Starting from the current point, scroll the selected
			// bar to the new point in ever increasing steps:
			int step = direction;
			if (fromTimer)
			{
				step *= selGroup.Items[0].Height / 4;
			}
			while (selGroup.ScrollOffset != endScrollOffset)
			{
				// determine the new scroll offset:
				int newOffset = selGroup.ScrollOffset + step;
				if (direction < 0)
				{
					if (newOffset < endScrollOffset)
					{
						newOffset = endScrollOffset;
					}
				}
				else
				{
					if (newOffset > endScrollOffset)
					{
						newOffset = endScrollOffset;
					}
				}
				selGroup.ScrollOffset = newOffset;
				
				// refresh the display:
				Invalidate();				
				this.Update();
				
				// Make the next step larger.
				step *= 2;
			}

			// Ensure that everything is shown in the right place
			DoResize();			
		}

		/// <summary>
		/// Raises the Resize event and performs internal
		/// sizing of the objects in the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			DoResize();
			base.OnResize(e);
		}

		/// <summary>
		/// Raises the SizeChanged event for this control
		/// and internally sizes the display.
		/// </summary>
		/// <param name="e">Event arguments associated with this
		/// event.</param>
		protected override void OnSizeChanged(EventArgs e)
		{			
			DoResize();
			this.Invalidate();

			base.OnSizeChanged(e);
		}

		private ListBarGroup ensureSelection()
		{
			ListBarGroup selectedGroup = SelectedGroup;			
			
			if ((selectedGroup == null)  || (!selectedGroup.Visible))
			{
				selectedGroup = null;
				if (groups.Count > 0)
				{
					for (int i = 0; i < groups.Count; i++)
					{
						if ((groups[i].Visible) && (selectedGroup == null))
						{
							groups[i].Selected = true;
							selectedGroup = groups[i];
						}
						else
						{
							if (groups[i].Selected)
							{
								groups[i].Selected = false;
							}
						}
					}
				}
			}
			return selectedGroup;
		}

		/// <summary>
		/// Called by the control's internal sizing mechanism.
		/// Returns the client size excluding the border of the
		/// control.
		/// </summary>
		/// <returns>A <c>Rectangle</c> providing the area to 
		/// draw the control into.</returns>
		protected virtual Rectangle GetClientRectangleExcludingBorder()
		{
			Rectangle rcClient = new Rectangle(
				this.ClientRectangle.Left + 1,
				this.ClientRectangle.Top + 1,
				this.ClientRectangle.Width - 2,
				this.ClientRectangle.Height - 2);
			return rcClient;
		}

		/// <summary>
		/// Called by the control's internal sizing mechanism.
		/// Returns the rectangle for a scroll button.
		/// </summary>
		/// <param name="buttonType">The scroll button to
		/// get the rectangle for.</param>
		/// <param name="selectedGroup">The Selected Group in the control.</param>
		/// <param name="internalGroupHeight">The internal height of the
		/// selected group</param>
		/// <returns>The Rectangle for the scroll button.</returns>
		protected virtual Rectangle GetScrollButtonRectangle(
			ListBarScrollButton.ListBarScrollButtonType buttonType,
			ListBarGroup selectedGroup,
			int internalGroupHeight
			)
		{
			Rectangle buttonRect;
			if (buttonType == ListBarScrollButton.ListBarScrollButtonType.Up)
			{
				buttonRect = new Rectangle(
					new Point(
					((this.RightToLeft == RightToLeft.Yes) ? 
					2 : 
					this.Width - 2 - btnUp.Rectangle.Width),
					selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 2),
					btnUp.Rectangle.Size);
			}
			else
			{				
				buttonRect = new Rectangle(
					new Point(
					((this.RightToLeft == RightToLeft.Yes) ? 
					2 : 
					this.Width - 2 - btnUp.Rectangle.Width),
					selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 
					internalGroupHeight - 2 - btnDown.Rectangle.Height),
					btnDown.Rectangle.Size);
			}
			return buttonRect;

		}
		

		private void DoResize()
		{
			if (this.redraw)
			{
				if (this.groups.Count > 0)
				{
					ListBarGroup selectedGroup = ensureSelection();
					if (selectedGroup != null)
					{						
						Rectangle rcClient = GetClientRectangleExcludingBorder();
						rcListView = new Rectangle(rcClient.Location, rcClient.Size);

						int lastVisibleGroup = 0;
						int firstVisibleGroup = groups.Count - 1;
						int nextVisibleGroup = firstVisibleGroup;

						for (int i = 0; i <= groups.IndexOf(selectedGroup); i++)
						{
							ListBarGroup group = groups[i];
						
							if (group.Visible)
							{
								int buttonWidth = GetGroupButtonWidth(group);
								group.SetLocationAndWidth(
									new Point(rcClient.Left, rcListView.Top), 
									buttonWidth);
								rcListView.Y += group.ButtonHeight;
								rcListView.Height -= group.ButtonHeight;

								if (i > lastVisibleGroup)
								{
									lastVisibleGroup = i;
								}
								if (i < firstVisibleGroup)
								{
									firstVisibleGroup = i;
								}
							}
						}

						int bottom = rcClient.Bottom;
						for (int i = groups.Count - 1; i > groups.IndexOf(selectedGroup); i--)
						{
							ListBarGroup group = groups[i];					
							if (group.Visible)
							{
								int buttonWidth = GetGroupButtonWidth(group);
								bottom -= group.ButtonHeight;
								rcListView.Height -= group.ButtonHeight;
								group.SetLocationAndWidth(
									new Point(rcClient.Left, bottom), 
									buttonWidth);

								if (i > lastVisibleGroup)
								{
									lastVisibleGroup = i;
								}
								if (i < nextVisibleGroup)
								{
									nextVisibleGroup = i;
								}
							}
						}				

						int size = selectedGroup.Items.Height;				
						int height = selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight;
						if (groups.IndexOf(selectedGroup) == lastVisibleGroup)
						{
							height = this.ClientRectangle.Height - height;
						}
						else
						{
							height = groups[nextVisibleGroup].ButtonLocation.Y - 
								height;
						}

						bool needUp = false;
						bool needDown = false;

						needUp = (selectedGroup.ScrollOffset < 0);
						needDown = ((size + selectedGroup.ScrollOffset) > height);

						Rectangle btnUpRect = GetScrollButtonRectangle(
							ListBarScrollButton.ListBarScrollButtonType.Up,
							selectedGroup,
							height);
						btnUp.SetRectangle(btnUpRect);
						btnUp.Visible = needUp;
						if (!needUp)
						{
							if (this.activeButton != null)
							{
								if (this.activeButton.Equals(btnUp))
								{
									buttonPressed.Enabled = false;
								}
							}
						}

						Rectangle btnDownRect = GetScrollButtonRectangle(
							ListBarScrollButton.ListBarScrollButtonType.Down,
							selectedGroup, height);							
						btnDown.SetRectangle(btnDownRect);
						btnDown.Visible = needDown;
						if (!needDown)
						{
							if (this.activeButton != null)
							{
								if (this.activeButton.Equals(btnDown))
								{
									buttonPressed.Enabled = false;
								}
							}
						}
					}
					else
					{
						btnUp.Visible = false;
						btnDown.Visible = false;
					}
								
					if (this.Width != lastWidth)
					{
						lastWidth = this.Width;
					}

					if (this.Height != lastHeight)
					{
						lastHeight = this.Height;
					}

				}
			}
		}

		/// <summary>
		/// Raises the Paint event and performs internal drawing of the
		/// control.	
		/// </summary>
		/// <param name="e">A PaintEventArgs object with details about the 
		/// paint event that must be performed.</param>
		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			if (scrollingGroup)
			{
				RenderScrollNewGroup(e);
			}
			else
			{
				Render(e);
			}
			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the double click event and performs internal double-click
		/// processing for the control.
		/// </summary>
		/// <param name="e"><see cref="EventArgs"/> associated with this
		/// double-click event.</param>
		protected override void OnDoubleClick( EventArgs e)
		{
			base.OnDoubleClick(e);
			Point pt = this.PointToClient(Cursor.Position);
			
			IMouseObject obj = HitTest(pt, false);
			if (obj != null)
			{
				if (typeof(ListBarItem).IsAssignableFrom(obj.GetType()))
				{
					ListBarItem item = (ListBarItem)obj;
					MouseButtons button = MouseButtons.Left; // TODO should use GetAsyncKeyState or whatever the Framework equivalent is
					ItemClickedEventArgs ice = new ItemClickedEventArgs(
						item, pt, button);
					OnItemDoubleClicked(ice);
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="ItemDoubleClicked"/> event for an item.
		/// </summary>
		/// <param name="e">The <see cref="ItemClickedEventArgs"/> details
		/// associated with the double click event.</param>
		protected virtual void OnItemDoubleClicked(ItemClickedEventArgs e)
		{
			if (this.ItemDoubleClicked != null)
			{
				this.ItemDoubleClicked(this, e);
			}
		}

		/// <summary>
		/// Raises the MouseDown event and performs internal mouse-down
		/// processing for the control.
		/// </summary>
		/// <param name="e">A MouseEventArgs object with details about the
		/// mouse event that has occurred.</param>
		protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			if (e.Button == MouseButtons.Left)
			{
				if (mouseTrack != null)
				{
					mouseDown = mouseTrack;
					mouseDown.MouseDown = true;
					mouseDown.MouseDownPoint = new Point(e.X, e.Y);

					// Check whether a scroll button has been pressed.
					// If it has, then start a timer to auto-scroll
					// more.
					if (typeof(ListBarScrollButton).IsAssignableFrom(mouseTrack.GetType()))
					{
						// Set the active scrolling button:
						activeButton = (ListBarScrollButton)mouseTrack;
						// perform the initial scroll:
						ScrollTo(activeButton);
						// initialise the timer:
						buttonPressed.Interval = 350;
						buttonPressed.Enabled = true;
					}
					else if (typeof(ListBarItem).IsAssignableFrom(mouseTrack.GetType()))
					{
						if (this.selectOnMouseDown)
						{
							MouseSelectItem((ListBarItem)mouseTrack, e);
						}
					}

					// Redraw the control:
					Invalidate();
				}
			}
			
		}

		/// <summary>
		/// Raises the MouseMove event and performs mouse move processing
		/// for the control.
		/// </summary>
		/// <param name="e">A MouseEventArgs object describing the mouse
		/// move event that has occurred.</param>
		protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// no motion during item editing
			if (editItem != null)
			{
				return;
			}
		
			// detect if the mouse is over anything:
			IMouseObject newMouseOver = HitTest(new Point(e.X, e.Y));

			if (newMouseOver == null) 
			{
				if (mouseTrack != null)
				{
					mouseTrack.MouseOver = false;
					mouseTrack = null;
					this.Cursor = Cursors.Default;
					Invalidate();
				}
				if (this.toolTip != null)
				{
					this.toolTip.SetToolTip(this, "");
				}
			}
			else
			{
				bool noChange = false;
				if (mouseTrack != null)
				{
					if (mouseTrack == newMouseOver)
					{
						// We're not tracking a new item.
						noChange = true;

						// However, if we mouse-downed on an item, then we 
						// should check if the new mouse position is sufficiently
						// far from the original position that a drag operation
						// is in order:
						if (this.allowDragItems)
						{
							if (typeof(ListBarItem).IsAssignableFrom(mouseTrack.GetType()))
							{
								if (mouseTrack.MouseDown)
								{
									int hysteresis = (SelectedGroup.View == ListBarGroupView.LargeIcons ? 4 : 2);
									if ((Math.Abs(mouseTrack.MouseDownPoint.X - e.X) > hysteresis) ||
										(Math.Abs(mouseTrack.MouseDownPoint.Y - e.Y) > hysteresis))
									{
										// time to start dragging:
										ListBarItem dragItem = (ListBarItem)mouseTrack;
										this.DoDragDrop(dragItem, DragDropEffects.Move);
										InternalDragDropComplete(dragItem, true);
										EnsureItemVisible(dragItem);
										return;
									}
								}
							}
						}
						if (this.allowDragGroups)
						{
							if (typeof(ListBarGroup).IsAssignableFrom(mouseTrack.GetType()))
							{
								if (mouseTrack.MouseDown)
								{
									if ((Math.Abs(mouseTrack.MouseDownPoint.X - e.X) > 4) ||
										(Math.Abs(mouseTrack.MouseDownPoint.Y - e.Y) > 4))
									{
										// time to start dragging:
										ListBarGroup dragGroup = (ListBarGroup)mouseTrack;
										this.DoDragDrop(dragGroup, DragDropEffects.Move);
										//InternalDragDropComplete(dragGroup);
										dragGroup.MouseOver = false;
										dragGroup.MouseDown = false;
										return;
									}
									
								}
							}
						}
					}
					else
					{
						mouseTrack.MouseOver = false;
					}
				}
				if (!noChange)
				{
					mouseTrack = newMouseOver;
					if (this.toolTip != null)
					{
						this.toolTip.SetToolTip(this, mouseTrack.ToolTipText);
					}
					mouseTrack.MouseOver = true;
					if (typeof(ListBarGroup).IsAssignableFrom(mouseTrack.GetType()))
					{
						this.Cursor = Cursors.Hand;
					}
					else
					{
						this.Cursor = Cursors.Default;
					}
					Invalidate();
				}
			}
		}

		
		/// <summary>
		/// Raises the MouseUp event and performs mouse up processing
		/// for the control.
		/// </summary>
		/// <param name="e">A MouseEventArgs object describing the mouse
		/// move event that has occurred.</param>
		protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseUp(e);
			
			if (e.Button == MouseButtons.Left)
			{
				if (mouseTrack != null)
				{
					if (mouseTrack.Equals(mouseDown))
					{

						if (typeof(ListBarGroup).IsAssignableFrom(mouseTrack.GetType()))
						{
							BeforeGroupChangedEventArgs bgc = new BeforeGroupChangedEventArgs(
								(ListBarGroup)mouseTrack);
							OnBeforeGroupChanged(ref bgc);
							if (!bgc.Cancel)
							{
								// group clicked.  Select the new group:
								SelectGroup((ListBarGroup)mouseTrack);
								OnSelectedGroupChanged(new System.EventArgs());
								GroupClickedEventArgs gce = new GroupClickedEventArgs(
									(ListBarGroup)mouseTrack,
									new Point(e.X, e.Y),
									e.Button);
								OnGroupClicked(gce);
							}
						}
						else if (typeof(ListBarScrollButton).IsAssignableFrom(mouseTrack.GetType()))
						{
							// don't need to do anything here, except be sure
							// we reset the active scroll button & timer later
						}
						else
						{
							if (activeButton == null)
							{
								if (!this.selectOnMouseDown)
								{
									MouseSelectItem((ListBarItem)mouseTrack, e);
								}
							}
						}
					}
				}
				
				// no more scrolling
				activeButton = null;
				buttonPressed.Enabled = false;
	
				if (mouseDown != null)
				{
					mouseDown.MouseDown = false;
					mouseDown.MouseOver = false;
				}
				if (mouseTrack != null)
				{
					mouseTrack.MouseOver = false;
				}
				Invalidate();
			}
		
			else if (e.Button == MouseButtons.Right)
			{
				if (mouseTrack != null)
				{
					// Right click?
					if (typeof(ListBarGroup).IsAssignableFrom(mouseTrack.GetType()))
					{
						GroupClickedEventArgs gce = new GroupClickedEventArgs(
							(ListBarGroup)mouseTrack,
							new Point(e.X, e.Y),
							e.Button);
						OnGroupClicked(gce);
					}
					else if (typeof(ListBarItem).IsAssignableFrom(mouseTrack.GetType()))
					{
						ItemClickedEventArgs ic = new ItemClickedEventArgs(
							(ListBarItem)mouseTrack,
							new Point(e.X, e.Y),
							e.Button );
						OnItemClicked(ic);
					}
					else
					{
						// no action currently
					}
				}
				else
				{
					// group right click:
					GroupClickedEventArgs gce = new GroupClickedEventArgs(
						SelectedGroup,
						new Point(e.X, e.Y),
						e.Button);
					OnGroupClicked(gce);
				}
			}
		}

		/// <summary>
		/// Raises the MouseLeave event and performs internal mouse
		/// track processing for the control.
		/// </summary>
		/// <param name="e">Event arguments associated with this event.</param>
		protected override void OnMouseLeave( System.EventArgs e)
		{
			base.OnMouseLeave(e);
			if (mouseTrack != null)
			{
				mouseTrack.MouseOver = false;
				mouseTrack = null;
				this.Cursor = Cursors.Default;
				Invalidate();
			}
		}

		/// <summary>
		/// Raises the MouseWheel event and performs mouse wheel 
		/// processing for the control.
		/// </summary>
		/// <param name="e">A MouseEventArgs object describing the mouse
		/// move event that has occurred.</param>
		protected override void OnMouseWheel ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseWheel(e);

			if ((e.Delta > 0) && (btnUp.Visible))
			{
				ScrollTo(1);	
			}			
			else if ((e.Delta < 0) && (btnDown.Visible))
			{
				ScrollTo(-1);
			}
			
		}

		private object GetBestDragDropFormat(DragEventArgs e)
		{
			object ret = null;
			object defaultFormat = null;
			foreach (string format in e.Data.GetFormats() )
			{
				object thisFormatData = e.Data.GetData(format);
				if (defaultFormat == null)
				{
					defaultFormat = thisFormatData;
				}

				if (typeof(ListBarItem).IsAssignableFrom(thisFormatData.GetType()))
				{
					ret  = thisFormatData;
					break;
				}
				else if (typeof(ListBarItem).IsAssignableFrom(thisFormatData.GetType()))
				{
					ret = thisFormatData;
					break;
				}
			}

			if (ret == null)
			{
				ret = defaultFormat;
			}

			return ret;
		}

		private object GetTypeOrSubClassFromData(DragEventArgs e, Type dataType)
		{
			object ret = null;
			foreach (string format in e.Data.GetFormats() )
			{
				if (dataType.IsAssignableFrom(e.Data.GetData(format).GetType()))
				{
					ret = e.Data.GetData(format);
					break;
				}
			}
			return ret;
		}

		private bool PerformAutoDrag(DragEventArgs e)
		{
			bool ret = false;
			if ((this.allowDragItems) || (this.allowDragGroups))
			{
				foreach (string format in e.Data.GetFormats() )
				{
					Type dataType = e.Data.GetData(format).GetType();
					if (typeof(ListBarItem).IsAssignableFrom(dataType))
					{
						ret = true;
						break;
					}
					else if (typeof(ListBarGroup).IsAssignableFrom(dataType))
					{
						ret = true;
						break;
					}
				}
			}
			return ret;
		}

		/// <summary>
		/// Raises the DragOver event and performs internal processing of 
		/// drag-drop to show the insertion point and navigate through
		/// the items in the control.
		/// </summary>
		/// <param name="e">A DragEventArgs object describing the drag
		/// over being performed.</param>
		protected override void OnDragOver(DragEventArgs e)
		{
			// perform the base operation:
			base.OnDragOver(e);	
			
			if (groups.Count > 0)
			{
				if (e.Effect != DragDropEffects.None) 
				{
					this.InternalDragOverProcess(e, true);
				}
				else if (this.PerformAutoDrag(e)) 
				{					
					this.InternalDragOverProcess(e, false);
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDragDrop(DragEventArgs e)
		{
			// perform the base operation:
			base.OnDragDrop(e);	

			if (groups.Count > 0)
			{
				object obj = GetBestDragDropFormat(e);
				
				if (e.Effect != DragDropEffects.None) 
				{
					bool move = (e.Effect == DragDropEffects.Move);
					this.InternalDragDropComplete(obj, move);
				}
				else if (this.PerformAutoDrag(e)) 
				{					
					this.InternalDragDropComplete(obj, true);
				}
			}

		}
		
		/// <summary>
		/// Raises the BeforeSelectedGroupChanged event.  This event enables
		/// the user to prevent a group selection.
		/// </summary>
		/// <param name="e">The BeforeGroupChangedEventArgs object associated
		/// with this event.</param>
		protected virtual void OnBeforeGroupChanged(ref BeforeGroupChangedEventArgs e)
		{
			if (this.BeforeSelectedGroupChanged != null)
			{
				this.BeforeSelectedGroupChanged(this, e);				
			}
		}
		/// <summary>
		/// Raises the BeforeItemClicked event.  This event enables
		/// the user to prevent an item from being selected.
		/// </summary>
		/// <param name="e">The BeforeItemClickedEventArgs object associated
		/// with this event.</param>
		protected virtual void OnBeforeItemClicked(ref BeforeItemClickedEventArgs e)
		{
			e.Cancel = (!e.Item.Enabled);
			if (this.BeforeItemClicked != null)
			{
				this.BeforeItemClicked(this, e);				
			}
		}

		/// <summary>
		/// Raises the <c>ItemClicked</c> event. 
		/// </summary>
		/// <param name="e">The <c>ItemClickedEventArgs</c> object associated 
		/// with this event.</param>
		protected virtual void OnItemClicked(ItemClickedEventArgs e)
		{
			if (this.ItemClicked != null)
			{
				this.ItemClicked(this, e);
			}
		}

		/// <summary>
		/// Raises the <c>GroupClicked</c> event.
		/// </summary>
		/// <param name="e">The <c>GroupClickedEventArgs</c> object
		/// associated with this event.</param>
		protected virtual void OnGroupClicked(GroupClickedEventArgs e)
		{
			if (this.GroupClicked != null)
			{
				this.GroupClicked(this, e);
			}
		}

		/// <summary>
		/// Raises the BeforeLabelEdit event for an item in the control.
		/// </summary>
		/// <param name="e">The LabelEditEventArgs describing the item
		/// that is about to be edited and allowing the edit action
		/// to be cancelled.</param>
		protected virtual void OnBeforeLabelEdit ( ListBarLabelEditEventArgs e )
		{
			if (BeforeLabelEdit != null)
			{
				this.BeforeLabelEdit(this, e);
			}
		}

		/// <summary>
		/// Raises the AfterLabelEdit event for an item in the control.
		/// </summary>
		/// <param name="e">The AfterEditEventArgs describing the item
		/// that has just been edited and allowing the edit action
		/// to be cancelled or the new caption to be changed.</param>
		protected virtual void OnAfterLabelEdit ( ListBarLabelEditEventArgs e )
		{
			if (AfterLabelEdit != null)
			{
				this.AfterLabelEdit(this, e);
			}
		}

		/// <summary>
		/// Raises the <c>SelectedGroupChanged</c> event.
		/// </summary>
		/// <param name="e">An EventArgs object associated with the event.</param>
		protected virtual void OnSelectedGroupChanged ( System.EventArgs e )
		{
			if (SelectedGroupChanged != null)
			{
				SelectedGroupChanged(this, e);
			}
		}

		private void txtEdit_TextChanged(object sender, System.EventArgs e)
		{
			if (editItem != null)
			{
				setTextBoxSize(editItem);
			}
		}

		private void txtEdit_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)			
			{
				case Keys.Return:
					// end editing:
					EndTextEdit(true);
					break;

				case Keys.Escape:
					// cancel editing:
					EndTextEdit(false);
					break;				
			}
		}

		private void popupCancel_PopupCancel(object sender, EventArgs e)
		{
			EndTextEdit(true);
		}

		#endregion

		#region Internal implementation
		private void EndTextEdit(bool commit)
		{
			if (this.editItem != null)
			{
				ListBarItem editedItem = this.editItem;
				this.editItem = null;
				
				if ((commit) && (editedItem != null))
				{
					ListBarGroup selectedGroup = SelectedGroup;

					ListBarLabelEditEventArgs lea = new ListBarLabelEditEventArgs(
						selectedGroup.Items.IndexOf(editItem), txtEdit.Text, editedItem);
					OnAfterLabelEdit(lea);

					if (!lea.CancelEdit )
					{
						if (editedItem != null) // may be shutting down...
						{
							editedItem.Caption = lea.Label;
						}
					}
				}
			}
			else if (this.editGroup != null)
			{
				ListBarGroup editedGroup = this.editGroup;
				this.editGroup = null;

				if ((commit) && (editedGroup != null))
				{
					ListBarLabelEditEventArgs lea = new ListBarLabelEditEventArgs(
						this.Groups.IndexOf(editedGroup), txtEdit.Text, editedGroup);
					OnAfterLabelEdit(lea);

					if (!lea.CancelEdit)
					{
						if (editedGroup != null)
						{
							editedGroup.Caption = lea.Label;
						}
					}
				}
			}

			txtEdit.Visible = false;
			Invalidate();
		}

		private void InternalDragDropComplete(
			object dragItem,
			bool move
			)
		{
			ListBarItem listBarDragItem = null;

			if (typeof(ListBarItem).IsAssignableFrom(dragItem.GetType()))
			{
				listBarDragItem = (ListBarItem)dragItem;
				listBarDragItem.MouseOver = false;
				listBarDragItem.MouseDown = false;
			}
			
			if (dragInsertPoint != null)
			{
				ListBarGroup groupTo = SelectedGroup;
				if (groupTo != null) // cannot happen...
				{
					ListBarGroup groupFrom = null;

					if (listBarDragItem != null)
					{
						// Check which bar we've come from
						// (it may be none, we may have come
						// from another control):
						
						foreach (ListBarGroup group in groups)
						{
							if (group.Items.Contains(listBarDragItem))
							{
								groupFrom = group;
								break;
							}
						}
					}

					if (groupFrom != null) // Dragged from this control
					{
						// moving to a new group: 
						if (move)
						{
							if (dragInsertPoint.ItemAfter != null)
							{
								if (dragInsertPoint.ItemAfter.Equals(listBarDragItem))
								{
									listBarDragItem = null;
								}
							}
							else if (dragInsertPoint.ItemBefore != null)
							{
								if (dragInsertPoint.ItemBefore.Equals(listBarDragItem))
								{
									listBarDragItem = null;
								}
							}
							if (listBarDragItem != null)
							{
								groupFrom.Items.Remove(listBarDragItem);							
							}
						}
						else
						{
							// Clone a new item to add:
							ListBarItem newItem = new ListBarItem(
								listBarDragItem.Caption, listBarDragItem.IconIndex, listBarDragItem.ToolTipText,
								listBarDragItem.Tag);
							listBarDragItem = newItem;
						}
					}
					else
					{
						// add a new item which represents what's been dragged
						if (listBarDragItem != null)
						{
							// there's an issue with which image to pick here
							listBarDragItem = new ListBarItem(
								listBarDragItem.Caption, listBarDragItem.IconIndex, 
								listBarDragItem.ToolTipText, listBarDragItem.Tag);
						}
						else
						{
							// Create a new item
							listBarDragItem = new ListBarItem(
								dragItem.ToString());
							((ListBarItem)dragItem).Tag = dragItem;
						}
					}

					if (listBarDragItem != null)
					{
						if (dragInsertPoint.ItemAfter != null)
						{
							groupTo.Items.InsertAfter(dragInsertPoint.ItemAfter, listBarDragItem);
						}
						else
						{
							groupTo.Items.InsertAfter(dragInsertPoint.ItemAfter, listBarDragItem);
						}
					}
				}		
			}

			dragInsertPoint = null;
			Invalidate();
		}

		private void SelectGroup(ListBarGroup group)
		{
			// first work out the scrolling logic:
			
			ListBarGroup selGroup = SelectedGroup;
			if (selGroup != group)
			{
				// Which groups are we moving between?
				this.indexNew = this.groups.IndexOf(group);
				this.indexCurrent = this.groups.IndexOf(selGroup);

				// Scrolling the new group into view:
				if (this.redraw)
				{
					this.scrollingGroup = true;

					if (this.indexNew > this.indexCurrent)
					{
						// the new index is below the current one.					
						// Scroll buttons from indexCurrent + 1 to indexNew
						// upwards
						int newIndexTargetPos = selGroup.ButtonLocation.Y + selGroup.ButtonHeight;
						for (int i = this.indexCurrent + 1; i <= this.indexNew - 1; i++)
						{
							if (this.groups[i].Visible)
							{
								newIndexTargetPos += this.groups[i].ButtonHeight;
							}
						}

						bool finished = false;
						int currentPos = group.ButtonLocation.Y;
						int step = -1;
						while (!finished)
						{
							currentPos += step;
							if (currentPos <= newIndexTargetPos)
							{
								step += (newIndexTargetPos - currentPos);
								currentPos = newIndexTargetPos;
								finished = true;
							}

							for (int i = this.indexCurrent + 1; i <= this.indexNew; i++)
							{
								ListBarGroup workGroup = this.groups[i];
								if (workGroup.Visible)
								{
									Point newLocation = workGroup.ButtonLocation;
									newLocation.Y += step;
									workGroup.SetLocationAndWidth(
										newLocation, workGroup.ButtonWidth);
								}
							}

							this.Invalidate();
							this.Update();

							step *= 2;
						}
													
					}
					else
					{
						// the new index is above the current one.
						// scroll buttons from indexNew + 1 to indexCurrent
						// downwards
						int lastIndex = indexCurrent;
						int nextIndex = this.Groups.Count -1;
						for (int i = indexCurrent + 1; i < this.Groups.Count; i++)
						{
							if (i > lastIndex)
							{
								lastIndex = i;
							}
							if (i < nextIndex)
							{
								nextIndex = i;
							}
						}
						int currentTargetPos = (indexCurrent == lastIndex ?
							this.ClientRectangle.Height :
							this.groups[nextIndex].ButtonLocation.Y);

						bool finished = false;
						int currentPos = selGroup.ButtonLocation.Y;
						int step = 1;
						while (!finished)
						{
							currentPos += step;
							if (currentPos >= currentTargetPos)
							{
								step -= (currentPos - currentTargetPos);
								currentPos = currentTargetPos;
								finished = true;
							}

							for (int i = indexNew + 1; i <= indexCurrent; i++)
							{
								ListBarGroup workGroup = this.groups[i];
								if (workGroup.Visible)
								{
									Point newLocation = workGroup.ButtonLocation;
									newLocation.Y += step;
									workGroup.SetLocationAndWidth(newLocation, workGroup.ButtonWidth);
								}
							}

							this.Invalidate();
							this.Update();

							step *= 2;

						}
					
					}

					this.scrollingGroup = false;
				}

				selGroup.Selected = false;
				group.Selected = true;
				DoResize();
			}

		}

		/// <summary>
		/// Selects an item in response to a mouse event.
		/// </summary>
		/// <param name="item">Item to be selected.</param>
		/// <param name="e"><see cref="System.Windows.Forms.MouseEventArgs"/> 
		/// details associated with the mouse event.</param>
		private void MouseSelectItem(ListBarItem item, MouseEventArgs e)
		{
			BeforeItemClickedEventArgs bic = new BeforeItemClickedEventArgs(
				(ListBarItem)mouseTrack);
			OnBeforeItemClicked(ref bic);
			if (!bic.Cancel)
			{
				// item clicked:
				SelectItem((ListBarItem)mouseTrack);
				ItemClickedEventArgs ic = new ItemClickedEventArgs(
					(ListBarItem)mouseTrack,
					new Point(e.X, e.Y),
					e.Button );
				OnItemClicked(ic);
			}
		}

		/// <summary>
		/// Selects an item in the selected bar and makes
		/// it visible.
		/// </summary>
		/// <param name="item">The item to select.</param>
		private void SelectItem(ListBarItem item)
		{
			BeginUpdate();
			foreach (ListBarItem otherItem in SelectedGroup.Items)
			{
				otherItem.Selected = false;
			}
			item.Selected = true;
			EndUpdate();
			EnsureItemVisible(item);
			Invalidate();
		}

		/// <summary>
		/// Starts editing the specified ListBarGroup.  Note this
		/// method is called from the StartEdit method of a ListBarGroup.
		/// </summary>
		/// <param name="group">The group to start editing.</param>
		protected internal void StartGroupEdit(ListBarGroup group)
		{
			// Fire the BeforeLabelEdit event:
			ListBarLabelEditEventArgs e = new ListBarLabelEditEventArgs(
				this.groups.IndexOf(group), group.Caption, group);
			OnBeforeLabelEdit(e);
			
			if (!e.CancelEdit)
			{
				editGroup = group;

				// Focus the control:
				this.Focus();

				// Set the edit text:
				txtEdit.Text = group.Caption;
				txtEdit.Font = (group.Font == null ? this.Font : group.Font);
				txtEdit.Location = group.ButtonLocation;
				txtEdit.Size = new Size(group.ButtonWidth, group.ButtonHeight);
				txtEdit.Visible = true;				
				txtEdit.BringToFront();
				txtEdit.Focus();

				popupCancel.StartTracking(txtEdit);
			}

		}

		/// <summary>
		/// Starts editing the specified <c>ListBarItem</c>.  Note this
		/// method is called from the <c>StartEdit</c> method of a 
		/// <c>ListBarItem</c>.
		/// <seealso cref="ListBarItem.StartEdit"/>
		/// </summary>
		/// <param name="item">The item to start editing.</param>
		protected internal void StartItemEdit(ListBarItem item)
		{

			// Get rectangle of item relative to control:
			ListBarGroup selectedGroup = SelectedGroup;

			// Check whether item is part of the selected
			// control:
			if (selectedGroup.Items.Contains(item))
			{
				// Fire the BeforeLabelEdit event:
				ListBarLabelEditEventArgs e = new ListBarLabelEditEventArgs(
					selectedGroup.Items.IndexOf(item), item.Caption, item);
				OnBeforeLabelEdit(e);
				if (!e.CancelEdit)
				{
					editItem = item;

					// Make sure we can see it:
					EnsureItemVisible(item);

					// Focus the control:
					this.Focus();

					// Set the edit text:
					txtEdit.Text = item.Caption;
					txtEdit.Font = (item.Font == null ? this.Font : item.Font);
					setTextBoxSize(editItem);
					int top = item.TextRectangle.Top;						
					txtEdit.Top = top;
					txtEdit.Visible = true;				
					txtEdit.BringToFront();
					txtEdit.Focus();

					popupCancel.StartTracking(txtEdit);
				}
			}
			else
			{
				throw new InvalidOperationException(
					"Editing is only possible on items belonging to the SelectedGroup in the control.");
			}


		}

		private void setTextBoxSize(ListBarItem editItem)
		{
			ListBarGroup selectedGroup = SelectedGroup;
			if (selectedGroup != null)
			{

				string text = txtEdit.Text;
				if (text.Length == 0)
				{
					text = "Xg";
				}
				
				int maxWidth = 0;
				if (selectedGroup.View == ListBarGroupView.SmallIcons)
				{
					maxWidth = this.ClientRectangle.Width - editItem.TextRectangle.Left - 1;
				}
				else
				{
					maxWidth = this.ClientRectangle.Width - 2;					
				}

				Graphics gfx = Graphics.FromHwnd(txtEdit.Handle);
				StringFormat fmt = new StringFormat(StringFormatFlags.LineLimit | 
					(txtEdit.RightToLeft == RightToLeft.Yes ? StringFormatFlags.DirectionRightToLeft : 0));
				fmt.Alignment = StringAlignment.Center;
				SizeF textSize = gfx.MeasureString(text, txtEdit.Font, maxWidth - 6, fmt);
				fmt.Dispose();
				gfx.Dispose();						
				
				if (textSize.Width < 24)
				{
					textSize.Width = 24;
				}
				textSize.Height += 2.0F;
				
				txtEdit.Size = new Size((int)textSize.Width + 6, (int)textSize.Height + 4);							
				if (selectedGroup.View == ListBarGroupView.SmallIcons)
				{
					txtEdit.Left = editItem.TextRectangle.Left + 1;
				}
				else
				{
					txtEdit.Left = 1 + (maxWidth - (int)textSize.Width) / 2;
				}
			}
		}

		/// <summary>
		/// Brings the specified <c>ListBarItem</c> into view if it is not already
		/// visible.  The <c>ListBarItem</c> must be in the selected group.
		/// <seealso cref="ListBarItem"/>
		/// <seealso cref="ListBar.SelectedGroup"/>
		/// </summary>
		/// <param name="item">Item to bring into view.</param>
		protected internal void EnsureItemVisible(ListBarItem item)
		{
			// Get rectangle of item relative to control:
			ListBarGroup selectedGroup = SelectedGroup;

			// Check whether item is part of the selected
			// group:
			if (selectedGroup.Items.Contains(item))
			{
				Rectangle rcVisible = new Rectangle(
					selectedGroup.ButtonLocation, 
					new Size(this.ClientRectangle.Width, 0)
					);
				
				ListBarGroup nextGroup = null;
				for (int i = this.groups.IndexOf(selectedGroup) + 1; i < this.groups.Count; i++)
				{
					if (this.groups[i].Visible)
					{
						nextGroup = this.groups[i];
						break;
					}
				}

				if (nextGroup == null)
				{
					rcVisible.Height = this.ClientRectangle.Height - 
						(selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight);
				}
				else
				{					
					rcVisible.Height = nextGroup.ButtonLocation.Y - rcVisible.Top; 
				}

				bool invisible = true;
				bool notFirstTime = false;
				while (invisible)
				{
					Rectangle rcItem = new Rectangle(item.Location,
						new Size(this.ClientRectangle.Width, item.Height));
					rcItem.Offset(0, 
						selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 
						selectedGroup.ScrollOffset);

					// Check if the item is too low:
					if (rcItem.Bottom > rcVisible.Bottom )
					{
						// need to scroll down until it can be seen:
						ScrollTo(-1, notFirstTime);
					}
					else if (rcItem.Top < rcVisible.Top)
					{
						// need to scroll up until it can be seen:
						ScrollTo(1, notFirstTime);
					}
					else
					{
						invisible = false;
					}
					notFirstTime = true;
				}
			}
		}

		/// <summary>
		/// Checks if there is an object which interacts with
		/// the mouse in the control under the specified point.
		/// </summary>
		/// <param name="pt">The point to test.</param>
		/// <returns>If there is a mouse object under the point 
		/// then its IMouseObject interface, otherwise null.</returns>
		private IMouseObject HitTest(Point pt)
		{
			return HitTest(pt, false);
		}

		/// <summary>
		/// Checks if there is an object which interacts with
		/// the mouse in the control under the specified point.
		/// </summary>
		/// <param name="pt">The point to test.</param>
		/// <returns>If there is a mouse object under the point 
		/// then its IMouseObject interface, otherwise null.</returns>
		/// <param name="forDragDrop">Whether the hit testing is
		/// being performed for a drag-drop operation or not.  During
		/// drag-drop, the hittest rectangle is relaxed so it includes
		/// the entire rectangle and not just the icon and text.
		/// </param>
		private IMouseObject HitTest(Point pt, bool forDragDrop)
		{
			// Default return value:
			IMouseObject mouseObject = null;
			ListBarGroup selectedGroup = SelectedGroup;

			// Over a scroll button?
			if (btnUp.HitTest(pt))
			{
				// over the scroll up button:
				mouseObject = btnUp;
			}
			else if (btnDown.HitTest(pt))
			{
				// over the scroll down button:
				mouseObject = btnDown;
			}
			else
			{
				if ((forDragDrop) && (selectedGroup != null))
				{
					// we test for any point with 6 pixels of
					// the scroll bars if the scroll buttons are on:
					if (btnUp.Visible)
					{
						Rectangle scrollTest = new Rectangle(
							selectedGroup.ButtonLocation.X, selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight,
							this.ClientRectangle.Width, 6);
						if (scrollTest.Contains(pt))
						{
							mouseObject = btnUp;
						}
					}
					if (btnDown.Visible)
					{
						ListBarGroup nextGroup = null;
						for (int i = this.groups.IndexOf(selectedGroup) + 1; i < this.groups.Count; i++)
						{
							if (this.groups[i].Visible)
							{
								nextGroup = this.groups[i];
								break;
							}
						}
						if (nextGroup != null)
						{
							Rectangle scrollTest = new Rectangle(
								nextGroup.ButtonLocation.X, nextGroup.ButtonLocation.Y - 6,
								this.ClientRectangle.Width, 6);
							if (scrollTest.Contains(pt))
							{
								mouseObject = btnDown;
							}						
						}
					}
				}
			}

			// Check whether we're over any group buttons:
			if (mouseObject == null)
			{
				foreach (ListBarGroup group in this.groups)
				{
					if (group.Visible)
					{
						Rectangle buttonRectangle = new Rectangle(
							group.ButtonLocation, new Size(group.ButtonWidth, group.ButtonHeight));
						if (buttonRectangle.Contains(pt))
						{
							// over a group:
							mouseObject = group;
							break;
						}
					}
				}
			}

			// Otherwise check whether we're over any list bar buttons:
			if (mouseObject == null)
			{
				// Is there a selected ListBar Group?
				if (selectedGroup != null)
				{
					// Check each item in this group:
					foreach (ListBarItem item in selectedGroup.Items)
					{
						Rectangle rcTest;
						if (forDragDrop)
						{
							// For drag drop the entire rectangle of the item
							// is taken into account:
							rcTest = new Rectangle(item.Location,
								new Size(item.Width, item.Height));
							rcTest.Offset(0, selectedGroup.ScrollOffset + selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight);
							if (rcTest.Contains(pt))
							{
								mouseObject = item;
								break;
							}

						}
						else
						{
							// Get the icon rectangle of the item within the group:
							rcTest = item.IconRectangle;
							// Check if the point is there:
							if (rcTest.Contains(pt))
							{
								// We're over an item:
								mouseObject = item;
								break;
							}
							// Otherwise try the text rectangle:
							rcTest = item.TextRectangle;
							if (rcTest.Contains(pt))
							{
								// We're over an item:
								mouseObject = item;
								break;
							}
						}
					}
				}
			}

			// Return the object the mouse is over if any
			return mouseObject;
		}

		
		/// <summary>
		/// Internal notification from a ListBarGroup that it has 
		/// been changed.
		/// </summary>
		/// <param name="group">The ListBarGroup which has been
		/// changed, or null the group has been removed.</param>
		/// <param name="addRemove">Whether the effect of the
		/// change will require the control to re-measured.</param>
		protected internal void GroupChanged(ListBarGroup group, bool addRemove)
		{
			// if we have changed the number of groups,
			// we need to redraw the entire control,
			// otherwise we just redraw this group
			if (addRemove)
			{	
				if (group != null)
				{
					group.SetButtonHeight(this.Font);
				}
				DoResize();
				PostResizeBarChanged();
			}			
			Invalidate();	
		}

		/// <summary>
		/// Internal notification from a ListBarItem that it has been
		/// changed.
		/// </summary>
		/// <param name="item">The ListBarItem which has been changed, 
		/// or null if the item has been removed.</param>
		/// <param name="addRemove">Whether the effect of the control
		/// will require the bar's contents to be remeasured.</param>
		protected internal void ItemChanged(ListBarItem item, bool addRemove)
		{
			ListBarGroup selGroup = SelectedGroup;
			ListBarGroup owningGroup = null;
			if (item != null)
			{
				// Which bar does it belong to
				foreach (ListBarGroup group in this.groups)
				{
					if (group.Items.Contains(item))
					{
						owningGroup = group;
						break;
					}
				}
				
				if (owningGroup != null)
				{
					Size imageSize = new Size(32, 32);
					if ((owningGroup.View == ListBarGroupView.LargeIcons) || (owningGroup.View == ListBarGroupView.LargeIconsOnly))
					{
						if (this.largeImageList != null)
						{
							imageSize = this.largeImageList.ImageSize;
						}
					}
					else
					{
						if (smallImageList != null)
						{
							imageSize = this.smallImageList.ImageSize;
						}
						else
						{
							imageSize = new Size(16, 16);
						}
					}

					// Tell the item to size itself
					item.SetSize(owningGroup.View, base.Font, imageSize);							
				}
				else
				{
					selGroup.SetLocationAndWidth(selGroup.ButtonLocation, selGroup.ButtonWidth);
				}
			}

			if (selGroup != null)
			{				
				if (item == null)
				{
					// need to assume it does
					if (addRemove)
					{
						DoResize();
						PostResizeBarChanged();
					}
					Invalidate();
				}
				else
				{
					if (selGroup.Items.Contains(item))
					{						
						// yes it does.  We need to modify the 
						// display:
						if (addRemove)
						{
							DoResize();
							PostResizeBarChanged();
						}
						Invalidate();
					}
					else
					{
						if (owningGroup == null)
						{
							if (addRemove)
							{
								DoResize();
								PostResizeBarChanged();
							}
							Invalidate();
						}
					}
				}
			}
		}

		/// <summary>
		/// Ensures the scroll bar isn't irrelevantly 
		/// begin displayed.
		/// </summary>
		private void PostResizeBarChanged()
		{
			// if the selected bar is scrolled,then we need 
			// to check in the new arrangement that there isn't
			// an unused space below the last item in the bar.
			
			// if there is we should check if it is possible
			// to scroll up by one or more items whilst still
			// ensuring the last item currently visible in the
			// view does not become any less visible.

			ListBarGroup selectedGroup = SelectedGroup;

			if (selectedGroup != null) 
			{
				if (selectedGroup.Items.Count > 0)
				{
					if (selectedGroup.ScrollOffset != 0)
					{
						bool finished = false;
						ListBarGroup nextGroup = null;
						for (int i = this.groups.IndexOf(selectedGroup) + 1; i < this.groups.Count; i++)
						{
							if (this.groups[i].Visible)
							{
								nextGroup = this.groups[i];
								break;
							}
						}
						
						while (!finished)
						{		
							ListBarItem lastItem = selectedGroup.Items[selectedGroup.Items.Count - 1];
							Rectangle rcItemLast = new Rectangle(lastItem.Location,
								new Size(this.ClientRectangle.Width, lastItem.Height));
							rcItemLast.Offset(0, selectedGroup.ScrollOffset + selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight );

							Rectangle rcView = new Rectangle(
								selectedGroup.ButtonLocation.X, selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight,
								this.ClientRectangle.Width, 0);
							if (nextGroup == null)
							{
								rcView.Height = this.ClientRectangle.Height - rcView.Top;
							}
							else
							{
								rcView.Height = nextGroup.ButtonLocation.Y - rcView.Top;
							}

							if (rcItemLast.Bottom < rcView.Bottom + rcItemLast.Height)
							{
								// we can scroll up:
								ScrollTo(1);
							}
							else
							{
								finished = true;
							}

							if (selectedGroup.ScrollOffset == 0)
							{
								finished = true;
							}
						}
					}
				}
			}
		}

		private void InternalDragOverProcess(DragEventArgs e, bool assumeItem)
		{
			// TODO: this method requires refactoring
			
			ListBarItem itemBefore = null;
			ListBarItem itemAfter = null;
			bool overEmptyBar = false;
			IMouseObject newDragHoverOver = null;
			bool overGroup = false;

			// see if the drag drop data contains a ListBarItem:		
			if ((GetTypeOrSubClassFromData(e, typeof(ListBarItem)) != null) 
				|| (assumeItem && (GetTypeOrSubClassFromData(e, typeof(ListBarGroup)) == null)))
			{
				// check if we're over an item:
				Point pt = new Point(e.X, e.Y);
				pt = this.PointToClient(pt);
				IMouseObject obj = HitTest(pt, true);
				newDragHoverOver = obj;
									
				if (obj != null)
				{
					// Do scrolling checks on this bar.  Scrolling
					// is rate limited to once per 250ms to assist
					// with usability
					System.TimeSpan diff = DateTime.Now.Subtract(lastScrollTime);
					if (diff.Milliseconds > 250)
					{
					
						// Firstly, check if we're over an actual scroll button:
						if (typeof(ListBarScrollButton).IsAssignableFrom(obj.GetType()))
						{
							// scroll in the appropriate direction:
							ListBarScrollButton scrollButton = (ListBarScrollButton)obj;
							ScrollTo(scrollButton, true);
							lastScrollTime = DateTime.Now;
						}
						else
						{
							// Otherwise, we may be within the boundary of the
							// scroll buttons:
							if (btnUp.Visible)
							{
								Rectangle rcBtnUp = btnUp.Rectangle;
								rcBtnUp.X = 0;
								rcBtnUp.Width = this.ClientRectangle.Width;
								if (rcBtnUp.Contains(pt))
								{
									ScrollTo(1,true);
									lastScrollTime = DateTime.Now;
								}
							}
							if (btnDown.Visible)
							{
								Rectangle rcBtnDown = btnDown.Rectangle;
								rcBtnDown.X = 0;
								rcBtnDown.Width = this.ClientRectangle.Width;
								if (rcBtnDown.Contains(pt))
								{
									ScrollTo(-1,true);
									lastScrollTime = DateTime.Now;
								}
							}

						}
					}


					// Now check for being over an item or an empty bar:
					if (typeof(ListBarItem).IsAssignableFrom(obj.GetType()))
					{
						ListBarItem item = (ListBarItem)obj;
						object objDragItem = GetTypeOrSubClassFromData(e, typeof(ListBarItem));
						bool itemEqualsDragItem = false;
						ListBarItem dragItem = null;
						if (objDragItem != null)
						{
							dragItem = (ListBarItem)objDragItem;
							itemEqualsDragItem = item.Equals(dragItem);
						}
					
						if (!itemEqualsDragItem) 
						{
							// we're over an item.
					
							// Get the rectangle relative to the bar:
							Rectangle rc = new Rectangle(item.Location, new Size(item.Width, item.Height));
							// Adjust the rectangle so it's relative to the control:
							ListBarGroup selectedGroup = SelectedGroup;
							rc.Offset(0, selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + selectedGroup.ScrollOffset);

							// The commented section here is an 8 pixel
							// margin from the top or bottom of an item,
							// as per the Outlook bar.  I found the control
							// more usable with a ListView style drag-drop
							// approach, where the before/after decision
							// is made depending on where you are relative
							// to the centre of the item.
								
							/* 
								** BEGIN: Outlook style drag-drop logic
							if (((pt.Y - rc.Top) > -8) && ((pt.Y - rc.Top) < 8))
							{
								itemBefore = item;
								// we can't go before the item which follows
								// the one we're dragging:
								if ((selectedGroup.Items.IndexOf(itemBefore) - 1) == 
									selectedGroup.Items.IndexOf(dragItem))
								{
									itemBefore = null;
								}

							}

							if (((rc.Bottom - pt.Y) > -8) && ((rc.Bottom - pt.Y) < 16))
							{
								itemAfter = item;
								// we can't go after the item which is before
								// the one we're dragging:
								if ((selectedGroup.Items.IndexOf(itemAfter) + 1) == 
									selectedGroup.Items.IndexOf(dragItem))
								{
									itemAfter = null;
								}
							}
								** END: Outlook style drag drop logic
								*/

							// ListView style drag insert point logic.
							if ((selectedGroup.View == ListBarGroupView.SmallIconsOnly) || (selectedGroup.View == ListBarGroupView.LargeIconsOnly))
							{
								int distRight = Math.Abs(rc.Right - pt.X);
								int distLeft = Math.Abs(pt.X - rc.Left);
								if (distRight < distLeft)
								{
									itemAfter = item;
								}
								else
								{
									itemBefore = item;
								}
							}
							else
							{
								int distBottom = Math.Abs(rc.Bottom - pt.Y);
								int distTop = Math.Abs(pt.Y - rc.Top);
								if (distBottom < distTop)
								{
									itemAfter = item;
								}
								else
								{
									itemBefore = item;
								}
							}

							if (itemAfter != null)
							{
								// we can't go after the item which is before
								// the one we're dragging:
								/*
								if ((selectedGroup.Items.IndexOf(itemAfter) + 1) == 
									selectedGroup.Items.IndexOf(dragItem))
								{
									itemAfter = null;
								}
								*/									
								if (itemAfter != null)
								{
									// check there isn't an appropriate item before:
									if (selectedGroup.Items.IndexOf(itemAfter) < selectedGroup.Items.Count - 1)
									{
										itemBefore = selectedGroup.Items[selectedGroup.Items.IndexOf(itemAfter) + 1];
									}
								}
							}
							else if (itemBefore != null)
							{
								// we can't go before the item which follows
								// the one we're dragging:
								/*
								if ((selectedGroup.Items.IndexOf(itemBefore) - 1) == 
									selectedGroup.Items.IndexOf(dragItem))
								{
									itemBefore = null;
								}
								*/					
								if (itemBefore != null)
								{
									// check there isn't an appropriate item after:
									if (selectedGroup.Items.IndexOf(itemBefore) > 0)
									{
										itemAfter = selectedGroup.Items[selectedGroup.Items.IndexOf(itemBefore) - 1];
									}
								}
							}
						}
					}
					else if (typeof(ListBarGroup).IsAssignableFrom(obj.GetType()))
					{
						overGroup = true;

						// over a group
						if (dragHoverOver != null)
						{
							if (dragHoverOver.Equals(obj))
							{
								System.TimeSpan overTime = DateTime.Now.Subtract(dragHoverOverStartTime);
								if (overTime.Milliseconds > 350)
								{
									// we should select this group:
									dragHoverOver = null;
									SelectGroup((ListBarGroup)obj);
									// Prevent the control from scrolling for a little bit.
									// TODO
									// Actually what we really want to do here is to say
									// that unless the mouse moves > 4 pixels from this
									// spot scrolling will not occur in the new bar
									lastScrollTime = DateTime.Now.AddMilliseconds(500);
								}
							}
						}
					}
				}
				else
				{
					// we may be over the bar section:
					ListBarGroup selectedGroup = SelectedGroup;
					
					if (selectedGroup != null) 
					{
						if (selectedGroup.Items.Count > 0)						
						{
							// we're not over an item.  Check if we're 
							// within the bar:
							if (obj == null)
							{							
								// Check if the selected group is the last group in
								// the control:
								ListBarGroup nextGroup = null;
								for (int i = this.groups.IndexOf(selectedGroup) + 1; i < this.groups.Count; i++)
								{
									if (this.groups[i].Visible)
									{
										nextGroup = this.groups[i];
										break;
									}
								}

								if (nextGroup == null)
								{
									// If so the bar area extends from the bottom
									// of the button rectangle to the bottom of the 
									// control:
									if ((pt.Y > (selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight)) && 
										(pt.Y < this.ClientRectangle.Height))
									{
										itemAfter = selectedGroup.Items[selectedGroup.Items.Count - 1];
									}
								}
								else
								{
									// Otherwise the bar area extends from the bottom
									// of the button rectangle of this control to the
									// top of the button rectangle of the next control:
									if ((pt.Y > (selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight)) && 
										(pt.Y < nextGroup.ButtonLocation.Y))
									{
										itemAfter = selectedGroup.Items[selectedGroup.Items.Count - 1];
									}
								}

							}
						}
						else
						{
							overEmptyBar = true;
						}
					}
				}
			}
			else if (GetTypeOrSubClassFromData(e, typeof(ListBarGroup)) !=  null)
			{
				itemAfter = null;
				itemBefore = null;				

				// Here we check if we should drag the list bar
				// into a new position
				// check if we're over an item:
				Point pt = new Point(e.X, e.Y);
				pt = this.PointToClient(pt);
				IMouseObject obj = HitTest(pt, true);
				newDragHoverOver = obj;
									
				if (obj != null)
				{
					if (typeof(ListBarGroup).IsAssignableFrom(obj.GetType()))
					{
						overGroup = true;

						ListBarGroup dragGroup = (ListBarGroup)GetTypeOrSubClassFromData(e, typeof(ListBarGroup));

						ListBarGroup dragOverGroup = (ListBarGroup)obj;
						if (!dragOverGroup.Equals(dragGroup))
						{
							bool reSelect = false;
							if (dragGroup.Selected)
							{
								reSelect = true;
							}
							bool isLastGroup = true;
							for (int i = groups.IndexOf(dragOverGroup) + 1; i < this.groups.Count; i++)
							{
								if (this.groups[i].Visible)
								{
									isLastGroup = false;
								}
							}
							this.groups.Remove(dragGroup);
							if (isLastGroup)
							{
								this.groups.Add(dragGroup);
							}
							else
							{
								this.groups.Insert(groups.IndexOf(dragOverGroup), dragGroup);
							}
							if (reSelect)
							{
								for (int i = 0; i < this.groups.Count; i++)
								{
									this.groups[i].Selected = false;
								}
								dragGroup.Selected = true;
							}
							DoResize();
							Invalidate();
						}
					}
				}
			
			}

			// Now check if we have any drag/drop insert position:
			if ((itemBefore != null) || (itemAfter != null) || (overEmptyBar))
			{
				e.Effect = DragDropEffects.Move;

				ListBarDragDropInsertPoint newInsertPoint = 
					new ListBarDragDropInsertPoint(itemBefore, itemAfter, overEmptyBar);
					
				// do we currently have an insert point?
				if (dragInsertPoint != null)
				{
					if (dragInsertPoint.CompareTo(newInsertPoint) == 0)
					{
						// we have nothing to do
						newInsertPoint = null;
					}
				}
					
				if (newInsertPoint != null)
				{
					Trace.WriteLine("Drag Insert Point has changed");
					dragInsertPoint = newInsertPoint;
					Invalidate();
				}
			}
			else
			{
				if (overGroup)
				{
					e.Effect = DragDropEffects.Move;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}

				// Clear the drag insert point if it's set:
				if (dragInsertPoint != null)
				{
					dragInsertPoint = null;
					// redraw the control:
					Invalidate();
				}
			}

			if ((newDragHoverOver != null) && (dragHoverOver != null))
			{								
				if (!newDragHoverOver.Equals(dragHoverOver))
				{
					dragHoverOver = newDragHoverOver;
					dragHoverOverStartTime = DateTime.Now;
				}
				// else we keep the drag hover over time.
			}
			else
			{
				dragHoverOver = newDragHoverOver;
				dragHoverOverStartTime = DateTime.Now;
			}

		}
		#endregion

		#region API
		/// <summary>
		/// Called by the control's internal sizing mechanism.
		/// Returns the size of a <see cref="ListBarGroup"/> button
		/// rectangle.
		/// </summary>
		/// <param name="group">The <see cref="ListBarGroup"/> to get the 
		/// button width for.</param>
		/// <returns>The width of the button.</returns>
		protected virtual int GetGroupButtonWidth(ListBarGroup group)
		{
			return this.ClientRectangle.Width - 2;
		}

		/// <summary>
		/// Gets/sets the default <see cref="System.Drawing.Font"/> used to 
		/// render text in the control.
		/// </summary>
		[Description("Gets/sets the Font used to render the text in this control.")]
		public override System.Drawing.Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				
				// Force all of the ListBar items to be measured
				foreach (ListBarGroup group in groups)
				{
					GroupChanged(group, true);
				}
			}
		}
		/// <summary>
		/// Gets/sets whether items are selected on MouseDown,
		/// rather than on MouseUp, which is the default.	
		/// </summary>
		[Description("Gets/sets whether items are selected on MouseDown, rather than on MouseUp.")]
		public bool SelectOnMouseDown
		{
			get
			{
				return this.selectOnMouseDown;
			}
			set
			{
				this.selectOnMouseDown = value;
			}
		}
	
		/// <summary>
		/// Gets/sets whether items will be dragged 
		/// in the control automatically.  The default
		/// is <c>True</c>.
		/// </summary>
		[Description("Gets/sets whether items will be dragged in the control automatically.  The default is True.")]
		public bool AllowDragItems
		{
			get
			{
				return this.allowDragItems;
			}
			set
			{
				this.allowDragItems = value;
			}
		}

		/// <summary>
		/// Gets/sets whether groups will be dragged 
		/// in the control automatically.  The default
		/// is <c>True</c>. (Note in MS Outlook
		/// Groups cannot be reordered by dragging, but
		/// they can in VS.NET).
		/// </summary>
		[Description("Gets/sets whether groups can be dragged automatically in the control.  The default is True.")]
		public bool AllowDragGroups
		{
			get
			{
				return this.allowDragGroups;
			}
			set
			{
				this.allowDragGroups = value;
			}
		}

		/// <summary>
		/// Sets the groups object associated with this control
		/// to a new group collection.
		/// </summary>
		/// <param name="groups">The <see cref="ListBarGroupCollection"/> object holding
		/// the new collection of groups to associate with this control.</param>
		[Description("Sets the internal collection holding the Groups associated with this control to a new object.")]
		public void SetGroups(ListBarGroupCollection groups)
		{
			this.BeginUpdate();
			groups.SetOwner(this);
			this.EndUpdate();
			this.groups = groups;			
			DoResize();
			this.Invalidate();
		}

		/// <summary>
		/// Gets the item which is currently being edited, if any,
		/// otherwise returns null.
		/// </summary>
		[Description("Gets the item which is currently being edited, if any, otherwise returns null.")]
		public ListBarItem EditItem
		{
			get
			{
				return this.editItem;
			}
		}

		/// <summary>
		/// Prevents the control from drawing until the 
		/// <see cref="EndUpdate"/> method is called.
		/// </summary>
		[Description("Prevents the control from drawing until the EndUpdate method is called.")]
		public void BeginUpdate()
		{
			this.redraw = false;
		}

		/// <summary>
		/// Resumes drawing of the control after drawing was suspended by the 
		/// <see cref="BeginUpdate"/> method.		
		/// </summary>
		[Description("Resumes drawing of the control after drawing was suspended by the BeginUpdate method.")]
		public void EndUpdate()
		{
			this.redraw = true;
			DoResize();
			Invalidate();
		}
		
		/// <summary>
		/// Renders the control when a new group is being scrolled
		/// into view.
		/// </summary>
		/// <param name="pe">The arguments associated with the paint
		/// event.</param>
		[Description("Renders the control as a new group is being scrolled into view")]
		protected virtual void RenderScrollNewGroup(PaintEventArgs pe)
		{
			int lastBar = 0;
			int currentNext = this.groups.Count -1;
			int newNext = this.groups.Count - 1;
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i].Visible)
				{
					if (i > lastBar)
					{
						lastBar = i;
					}
					if ((i > this.indexCurrent) && (i < currentNext))
					{
						currentNext = i;
					}
					if ((i > this.indexNew) && ( i < newNext))
					{
						newNext = i;
					}
				}
			}

			ListBarGroup currentBar = this.groups[this.indexCurrent];
			ListBarGroup newBar = this.groups[this.indexNew];

			// get the rectangle for currentBar:
			Rectangle currentBarBounds = new Rectangle(
				currentBar.ButtonLocation,
				new Size(currentBar.ButtonWidth, 0));
			// the height of the current bar rect is the height of the control
			// or the top of the rectangle of the next button along:
			if (this.indexCurrent == lastBar)
			{
				currentBarBounds.Height = this.ClientRectangle.Height -
					currentBarBounds.Top;
			}
			else
			{
				currentBarBounds.Height = this.groups[currentNext].ButtonLocation.Y - 
					currentBarBounds.Top;
			}

			// get the rectangle for newBar:
			Rectangle newBarBounds = new Rectangle(
				newBar.ButtonLocation,
				new Size(newBar.ButtonWidth, 0));
			// the height of the new bar is the height of the control or
			// the top of the rectangle of the next bar along:
			if (this.indexNew == lastBar)
			{
				newBarBounds.Height = this.ClientRectangle.Height - 
					newBarBounds.Top;
			}
			else
			{
				newBarBounds.Height = this.groups[newNext].ButtonLocation.Y -
					newBarBounds.Top;
			}
			
			// Draw the current bar contents:
			currentBar.DrawBar(
				pe.Graphics, currentBarBounds, 
				(currentBar.View == ListBarGroupView.LargeIcons ? largeImageList : smallImageList),
				this.Font,
				this.drawStyle, 
				this.Enabled);

			// Draw the new bar contents:
			newBar.DrawBar(
				pe.Graphics, newBarBounds, 
				(newBar.View == ListBarGroupView.LargeIcons ? largeImageList : smallImageList),
				this.Font,
				this.drawStyle,
				this.Enabled);

			// Draw the buttons:
			foreach (ListBarGroup group in this.groups)
			{
				group.DrawButton(pe.Graphics, this.Font, this.Enabled);
			}

			// Draw the border:
			RenderControlBorder(pe.Graphics);
		}

		/// <summary>
		/// Renders the control given the object passed to a Paint event.
		/// </summary>
		/// <param name="pe">The arguments associated with the paint
		/// event.</param>
		protected virtual void Render(PaintEventArgs pe)
		{
			if (this.redraw)
			{

				// background - does not need to be painted
				// as the control does it itself

				// draw the control elements:
				if (groups.Count > 0)
				{
					// First draw the items in the selected group,
					// if any:
					ListBarGroup selectedGroup = SelectedGroup;
					
					if ((selectedGroup != null) && (selectedGroup.Visible))
					{
						// Draw the items in the group:
						selectedGroup.DrawBar(pe.Graphics, rcListView,
							((selectedGroup.View == ListBarGroupView.LargeIcons ||
							selectedGroup.View == ListBarGroupView.LargeIconsOnly)
							? 
						largeImageList : smallImageList),
							this.Font,
							this.drawStyle,
							this.Enabled);
						
						// Render the drag-drop insertion point, if any:
						RenderDragInsertPoint(pe.Graphics,
							selectedGroup);					
					}

					// draw the scroll buttons:
					Color defaultBackColor = Color.FromKnownColor(KnownColor.Control);
					if (this.drawStyle == ListBarDrawStyle.ListBarDrawStyleNormal)
					{
						if (!this.BackColor.Equals(Color.FromKnownColor(KnownColor.ControlDark)))
						{
							defaultBackColor = this.BackColor;
						}
					}
					else if (this.DrawStyle == ListBarDrawStyle.ListBarDrawStyleOfficeXP)
					{
						defaultBackColor = this.BackColor;
					}

					btnUp.DrawItem(pe.Graphics, defaultBackColor, this.Enabled );
					btnDown.DrawItem(pe.Graphics, defaultBackColor, this.Enabled);

					// Now draw the group buttons, if any:
					foreach (ListBarGroup group in this.groups)
					{
						if (group.Visible)
						{
							group.DrawButton(pe.Graphics, this.Font, this.Enabled);
						}
					}
				}			

				// border:
				RenderControlBorder(pe.Graphics);
			}
		}

		/// <summary>
		/// Draw a border around the control.  The default
		/// implementation draws a 1 pixel inset border.
		/// </summary>
		/// <param name="gfx">The graphics object to drawn onto.</param>
		protected virtual void RenderControlBorder(
			Graphics gfx)
		{
			// draw the control's border
			Pen darkPen = new Pen(CustomBorderColor.ColorDark(this.BackColor));
			Pen lightPen = new Pen(CustomBorderColor.ColorLightLight(this.BackColor));
			gfx.DrawLine(darkPen, 
				this.ClientRectangle.Left, this.ClientRectangle.Bottom -2, 
				this.ClientRectangle.Left, this.ClientRectangle.Top);
			gfx.DrawLine(darkPen, 
				this.ClientRectangle.Left, this.ClientRectangle.Top, 
				this.ClientRectangle.Right -2, this.ClientRectangle.Top);
			gfx.DrawLine(lightPen, 
				this.ClientRectangle.Right - 1, this.ClientRectangle.Top, 
				this.ClientRectangle.Right -1, this.ClientRectangle.Bottom -1);
			gfx.DrawLine(lightPen, 
				this.ClientRectangle.Right - 1, this.ClientRectangle.Bottom - 1, 
				this.ClientRectangle.Left, this.ClientRectangle.Bottom -1);
			darkPen.Dispose();
			lightPen.Dispose();
		}

		/// <summary>
		/// Draws the drag insert point, if any.  The drag insert point is
		/// drawn using the same style as the Windows XP ListView drag
		/// insert point.
		/// 
		/// Note that the Outlook ListBar draws a single pixel drag insert
		/// point rather than a double width one.  I preferred the ListView 
		/// XP style so went with this.  The code can be overridden to
		/// use a single pixel border instead if desired. 
		/// </summary>
		/// <param name="gfx">The graphics object to draw onto.</param>
		/// <param name="selectedGroup">The currently selected ListBarGroup.</param>
		protected virtual void RenderDragInsertPoint(
			Graphics gfx,
			ListBarGroup selectedGroup
			)
		{
			if (dragInsertPoint != null)
			{				
				ListBarItem itemAfter = dragInsertPoint.ItemAfter;
				ListBarItem itemBefore = dragInsertPoint.ItemBefore;

				int offset = (selectedGroup.View == ListBarGroupView.LargeIcons ? 2 : 0);

				if (itemAfter != null)
				{
					// Get the bounding rectangle of the item after:
					Rectangle rcItem = new Rectangle(itemAfter.Location, new Size(itemAfter.Width, itemAfter.Height));
					// adjust the rectangle so it corresponds to the display:
					rcItem.Offset(0, 
						selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 
						selectedGroup.ScrollOffset);

					// Draw the insertion point line:
					if ((selectedGroup.View == ListBarGroupView.SmallIconsOnly) || (selectedGroup.View == ListBarGroupView.LargeIconsOnly))
					{
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Right, rcItem.Top + 2, 
							rcItem.Right, rcItem.Bottom - 1);
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Right + 1, rcItem.Top + 2, 
							rcItem.Right + 1, rcItem.Bottom - 1);

						// Draw triangles:
						if (itemBefore != null)
						{
							// left triangles:
							ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
								new Point(rcItem.Right + 1, rcItem.Top + 2), 5, 5);
							ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
								new Point(rcItem.Right + 1, rcItem.Bottom), 5, -6);
						}

						// right triangles:
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Right, rcItem.Top + 2), -4, 4);
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Right, rcItem.Bottom), -5, -6);

					}
					else
					{
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 7, rcItem.Bottom + offset, 
							rcItem.Right - 7, rcItem.Bottom + offset);
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 7, rcItem.Bottom + offset + 1, 
							rcItem.Right - 7, rcItem.Bottom + offset + 1);

						// Draw the triangles:
						if (itemBefore != null)
						{
							// below triangles:
							ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
								new Point(rcItem.Left + 7, rcItem.Bottom + offset + 1), 10, 5);
							ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText,
								new Point(rcItem.Right - 6, rcItem.Bottom + offset + 1), -10, 5);							
						}
						
						// above triangles
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Left + 7, rcItem.Bottom + offset), 10, -5);
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText,
							new Point(rcItem.Right - 6, rcItem.Bottom + offset), -10, -5);

					}
				}
				else
				{
					// before the first item:

					// Get the bounding rectangle of the item after:
					Rectangle rcItem ;
					if (itemBefore != null)
					{
						rcItem = new Rectangle(itemBefore.Location, new Size(this.Width, itemBefore.Height));
						// adjust the rectangle so it corresponds to the display:
						rcItem.Offset(0, 
							selectedGroup.ButtonLocation.Y + selectedGroup.ButtonHeight + 
							selectedGroup.ScrollOffset);
					}
					else
					{
						rcItem = new Rectangle(selectedGroup.ButtonLocation, 
							new Size(selectedGroup.ButtonWidth, selectedGroup.ButtonHeight));
						rcItem.Offset(0, selectedGroup.ButtonHeight);
					}

					// draw the insertion point line:
					if ((selectedGroup.View == ListBarGroupView.SmallIconsOnly) || (selectedGroup.View == ListBarGroupView.LargeIconsOnly))
					{
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 1, rcItem.Top + 2, 
							rcItem.Left + 1, rcItem.Bottom - 1);
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 2, rcItem.Top + 2, 
							rcItem.Left + 2, rcItem.Bottom - 1);						

						// left triangles:
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Left + 2, rcItem.Top + 2), 5, 5);
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Left + 2, rcItem.Bottom), 5, -6);
					}
					else
					{
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 7, rcItem.Top + offset, 
							rcItem.Right - 7, rcItem.Top + offset);
						gfx.DrawLine(SystemPens.WindowText,
							rcItem.Left + 7, rcItem.Top + offset + 1, 
							rcItem.Right - 7, rcItem.Top + offset + 1);

						// Now draw the triangles:
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText, 
							new Point(rcItem.Left + 7, rcItem.Top + offset + 1), 10, 5);
						ListBarUtility.FillRightAngleTriangle(gfx, SystemBrushes.WindowText,
							new Point(rcItem.Right - 6, rcItem.Top + offset + 1), -10, 5);
					}
				}
			}
		}

		/// <summary>
		/// Creates a new instance of the ListBarGroupCollection used by this
		/// control to store the ListBarGroups.  Fired during control 
		/// initialisation.
		/// </summary>
		/// <returns>A new instance of the ListBarGroupCollection to be used
		/// by the control to store ListBarGroups.</returns>
		protected virtual ListBarGroupCollection CreateListBarGroupCollection()
		{
			return new ListBarGroupCollection(this);
		}

		/// <summary>
		/// Creates a new instance of a ListBarScrollButton used by this control
		/// to draw the scroll buttons.  Fired during control initialisation
		/// </summary>
		/// <param name="buttonType">The type of scroll button (Up or Down)
		/// to create</param>
		/// <returns>A new ListBarScrollButton which is drawn when a ListBar
		/// contains more items than can be displayed.</returns>
		protected virtual ListBarScrollButton CreateListBarScrollButton(ListBarScrollButton.ListBarScrollButtonType buttonType)
		{
			return new ListBarScrollButton(buttonType);
		}

		/// <summary>
		/// Gets/sets how the ListBar control will be drawn.
		/// </summary>
		[Description("Gets/sets the style used to draw the ListBar control.")]
		public ListBarDrawStyle DrawStyle
		{
			get
			{
				return this.drawStyle;
			}
			set
			{
				this.drawStyle = value;
			}
		}

		/// <summary>
		/// Gets the collection of groups in the ListBar.
		/// </summary>
		[Description("Gets the collection of groups in the ListBar.")]
		public ListBarGroupCollection Groups
		{
			get
			{
				return this.groups;
			}
		}
		/// <summary>
		/// Gets/sets the tooltip object associated with this control.
		/// The control does not generate its own internal Tooltips
		/// and instead relies on an external ToolTip object to
		/// display tooltips.
		/// </summary>
		[Description("Gets/sets the tooltip object which will be used to show tooltips for groups and items in the control.")]
		public ToolTip ToolTip
		{
			get
			{
				return this.toolTip;
			}
			set
			{
				this.toolTip = value;				
			}
		}
		
		/// <summary>
		/// Gets/sets the large icon ImageList to be used for items 
		/// with the <see cref="ListBarGroupView.LargeIcons"/> and 
		/// <see cref="ListBarGroupView.LargeIconsOnly"/> view.
		/// </summary>
		[Description("Gets/sets the large icon ImageList to be used for items in groups with the LargeIcons or LargeIconsOnly view.")]
		public ImageList LargeImageList
		{
			get
			{
				return this.largeImageList;
			}
			set
			{
				this.largeImageList = value;
			}
		}

		/// <summary>
		/// Gets/sets the small icon ImageList to be used for ListBar groups
		/// using the <see cref="ListBarGroupView.SmallIcons"/> or 
		/// <see cref="ListBarGroupView.SmallIconsOnly "/> view.
		/// </summary>
		[Description("Gets/sets the small icon ImageList to be used for items in groups with the SmallIcons or SmallIconsOnly view.")]
		public ImageList SmallImageList
		{
			get
			{
				return this.smallImageList;
			}
			set
			{
				this.smallImageList = value;
			}
		}

		/// <summary>
		/// Returns the currently selected group in the ListBar control,
		/// if any.
		/// </summary>
		[Description("Gets the currently selected group in the ListBar control.")]
		public virtual ListBarGroup SelectedGroup
		{
			get
			{
				ListBarGroup selGroup = null;
				if (this.groups.Count > 0)
				{
					foreach (ListBarGroup group in this.groups)
					{
						if (group.Selected)
						{
							selGroup = group;
							break;
						}
					}					
				}
				return selGroup;
			}
		}
		#endregion 

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtEdit = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtEdit
			// 
			this.txtEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEdit.Location = new System.Drawing.Point(60, 92);
			this.txtEdit.Multiline = true;
			this.txtEdit.Name = "txtEdit";
			this.txtEdit.Size = new System.Drawing.Size(80, 44);
			this.txtEdit.TabIndex = 0;
			this.txtEdit.Text = "";
			this.txtEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtEdit.Visible = false;
			this.txtEdit.TextChanged += new System.EventHandler(this.txtEdit_TextChanged);
			// 
			// ListBar
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {			
																		  this.txtEdit});
			this.Name = "ListBar";
			this.ResumeLayout(false);

		}
		#endregion

	}	
	#endregion


  
}
