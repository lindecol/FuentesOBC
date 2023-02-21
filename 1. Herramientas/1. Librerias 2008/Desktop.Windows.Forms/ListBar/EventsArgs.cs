using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Desktop.Windows.Forms
{
	
	#region Event argument classes
	/// <summary>
	/// Provides details about an item which will undergo
	/// an edit operation.
	/// </summary>
	public class ListBarLabelEditEventArgs : LabelEditEventArgs
	{
		private object labelEditObject = null;

		/// <summary>
		/// Returns the object for which label editing has
		/// been requested.  Can either be a <see cref="ListBarItem"/> or
		/// a <see cref="ListBarGroup"/> (or a subclass of either).
		/// </summary>
		[Description("Gets the object for which label editing has been requested.  Either a ListBarItem or a ListBarGroup (or a subclass)")]
		public object LabelEditObject
		{
			get
			{
				return this.labelEditObject;
			}
		}

		/// <summary>
		/// Constructs a new instance of this object
		/// given the item, label and object.
		/// </summary>
		/// <param name="item">The index of the item being edited.</param>
		/// <param name="label">The label of the item being edited.</param>
		/// <param name="labelEditObject">The object being edited.</param>
		[Description("Constructs a new instance of this object.")]
		public ListBarLabelEditEventArgs(
			int item,
			string label,
			object labelEditObject
			) : base(item, label)
		{
			this.labelEditObject = labelEditObject;
		}		
	}

	/// <summary>
	/// Provides event arguments for the BeforeSelectedGroupChanged event
	/// raised by the control.  This object contains the group that
	/// would be selected and provides the opportunity to cancel the 
	/// group selection.
	/// </summary>
	public class BeforeGroupChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The ListBarGroup that would be selected.
		/// </summary>
		private ListBarGroup group;
		/// <summary>
		/// Whether to cancel the operation or not.
		/// </summary>
		private bool cancel = false;

		/// <summary>
		/// Gets the group that will be selected.
		/// </summary>
		[Description("Gets the group that will be selected.")]
		public ListBarGroup Group
		{
			get
			{
				return this.group;
			}
		}

		/// <summary>
		/// Gets/sets whether the group selection should be cancelled
		/// or not. By default the group selection is not cancelled.
		/// </summary>
		[Description("Gets/sets whether the group selection should be cancelled.")]
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		/// <summary>
		/// Constructs a new instance of this object.
		/// Called
		/// by the <see cref="ListBar"/> control before firing a 
		/// <c>BeforeSelectedGroupChanged</c> event.
		/// </summary>
		/// <param name="group">The group that will be selected</param>
		[Description("Constructs a new instance of this object.")]
		public BeforeGroupChangedEventArgs(
			ListBarGroup group
			)
		{
			this.group = group;
		}
	}

	/// <summary>
	/// This class is used with the BeforeItemClicked event and provides
	/// the item which is about to be clicked and the option to prevent
	/// the item being clicked by setting the Cancel property.
	/// </summary>
	public class BeforeItemClickedEventArgs : EventArgs
	{
		/// <summary>
		/// The ListBarItem which is about to be clicked.
		/// </summary>
		private ListBarItem item = null;
		/// <summary>
		/// Whether the click should be cancelled or not.
		/// </summary>
		private bool cancel = false;

		/// <summary>
		/// Gets/sets whether the click should be cancelled or not.
		/// </summary>
		[Description("Gets/sets whether the click should be cancelled or not.")]
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		/// <summary>
		/// Gets the ListBarItem that is about to be clicked.
		/// </summary>
		[Description("Gets the ListBarItem that is about to be clicked.")]
		public ListBarItem Item
		{
			get
			{
				return this.item;
			}

		}

		/// <summary>
		/// Constructor for this object. Called
		/// by the <see cref="ListBar"/> control before firing a 
		/// <see cref="BeforeItemClickedEventHandler"/> event.
		/// </summary>
		/// <param name="item">The item that's about to be clicked.</param>
		[Description("Constructs a new instance of this object.")]
		public BeforeItemClickedEventArgs(
			ListBarItem item
			)
		{
			this.item = item;
		}

	}

	/// <summary>
	/// This class is provides details of which item has been clicked
	/// and the mouse details of the click when the <c>ItemClicked</c> event
	/// is raised from a <c>ListBar</c>.
	/// <seealso cref="ListBar.ItemClicked"/>
	/// </summary>
	public class ItemClickedEventArgs : ObjectClickedEventArgs
	{
		/// <summary>
		/// The ListBarIem that has been clicked.
		/// </summary>
		private ListBarItem item = null;

		/// <summary>
		/// Gets the <see cref="ListBarItem"/> that has been clicked.
		/// </summary>
		[Description("Gets the ListBarItem that has been clicked.")]
		public ListBarItem Item
		{
			get
			{
				return this.item;
			}

		}

		/// <summary>
		/// Constructs a new instance of this object.  Called by the <see cref="ListBar"/>
		/// control when firing an <c>ItemClicked</c> event.
		/// </summary>
		/// <param name="item">The item that has been clicked</param>
		/// <param name="location">The mouse location relative to the 
		/// control for the click.</param>
		/// <param name="mouseButton">The mouse button used to click
		/// the item.</param>
		[Description("Constructs a new instance of this object")]
		public ItemClickedEventArgs(
			ListBarItem item,
			Point location,
			MouseButtons mouseButton
			) : base(location, mouseButton)
		{
			this.item = item;
		}

	}

	/// <summary>
	/// This class is provides details of which item has been clicked
	/// and the mouse details of the click when the <c>GroupClicked</c> event
	/// is raised from a <see cref="ListBar" /> control.
	/// </summary>
	public class GroupClickedEventArgs : ObjectClickedEventArgs
	{
		/// <summary>
		/// The ListBarGroup that has been clicked.
		/// </summary>
		private ListBarGroup group = null;

		/// <summary>
		/// Gets the <see cref="ListBarGroup"/> that has been clicked.
		/// </summary>
		[Description("Gets the ListBarGroup that has been clicked.")]
		public ListBarGroup Group
		{
			get
			{
				return this.group;
			}

		}

		/// <summary>
		/// Constructs a new instance of this object.  Called by the <see cref="ListBar"/>
		/// control when firing a <c>GroupClicked</c> event.
		/// </summary>
		/// <param name="group">The <see cref="ListBarGroup"/> that has been clicked</param>
		/// <param name="location">The mouse location relative to the 
		/// control for the click.</param>
		/// <param name="mouseButton">The mouse button used to click
		/// the item.</param>
		[Description("Constructs a new instance of this object.")]
		public GroupClickedEventArgs(
			ListBarGroup group,
			Point location,
			MouseButtons mouseButton
			) : base(location, mouseButton)
		{
			this.group = group;
		}

	}

	/// <summary>
	/// An abstract class used as the bases for the <c>ItemClicked</c>
	/// and <c>GroupClicked</c> events of the <see cref="ListBar"/> control.
	/// This class stores details of the mouse location and button.
	/// </summary>
	public abstract class ObjectClickedEventArgs : EventArgs
	{
		/// <summary>
		/// The location of the mouse when the item was clicked.
		/// </summary>
		private Point location;
		/// <summary>
		/// The mouse button that was used.
		/// </summary>
		private MouseButtons mouseButton = MouseButtons.Left;

		/// <summary>
		/// The Location of the mouse, relative to the control,
		/// when the item was clicked.
		/// </summary>
		[Description("The location of the mouse relative to the control when the item was clicked.")]
		public Point Location
		{
			get
			{
				return location;
			}
		}

		
		/// <summary>
		/// The MouseButton used to click the item.
		/// </summary>
		[Description("The mouse button used to click this item.")]
		public MouseButtons MouseButton
		{
			get
			{
				return this.mouseButton;
			}
		}

		/// <summary>
		/// When used in a subclass, constructs a new instance of the class with the specified
		/// mouse location and button.
		/// </summary>
		/// <param name="location">The location of the mouse.</param>
		/// <param name="mouseButton">The button which was pressed.</param>
		[Description("When used in a subclass, constructs a new instance of this class.")]
		public ObjectClickedEventArgs(
			Point location,
			MouseButtons mouseButton
			)
		{
			this.location = location;
			this.mouseButton = mouseButton;
		}

	}
	#endregion 

	#region Event delegates
	/// <summary>
	/// Represents the method that handles the BeforeSelectedGroupChanged event
	/// of a ListBar control.
	/// </summary>
	public delegate void BeforeGroupChangedEventHandler(
	object sender, 
	BeforeGroupChangedEventArgs e);
	/// <summary>
	/// Represents the method that handles the BeforeItemClicked event
	/// of a ListBar control.
	/// </summary>
	public delegate void BeforeItemClickedEventHandler(
	object sender, 
	BeforeItemClickedEventArgs e);
	/// <summary>
	/// Represents the method that handles the ItemClicked event of a
	/// ListBar control.
	/// </summary>
	public delegate void ItemClickedEventHandler(
	object sender,
	ItemClickedEventArgs e);
	/// <summary>
	/// Represents the method that handles the GroupClicked event of a
	/// ListBar control.
	/// </summary>
	public delegate void GroupClickedEventHandler(
	object sender,
	GroupClickedEventArgs e);
	
	/// <summary>
	/// Represents the method that handles the BeforeLabelEdit and AfterLabelEdit
	/// events of a ListBar control.
	/// </summary>
	public delegate void ListBarLabelEditEventHandler(object sender,ListBarLabelEditEventArgs e);	
	#endregion
	
}
