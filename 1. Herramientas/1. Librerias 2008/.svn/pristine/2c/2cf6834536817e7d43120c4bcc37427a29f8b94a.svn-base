using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Desktop.Windows.Forms
{
    #region ListBarItemCollection class
    /// <summary>
    /// This class manages a collection of items within a ListBarGroup.
    /// </summary>
    [SerializableAttribute()]
    public class ListBarItemCollection : CollectionBase, ISerializable
    {
        /// <summary>
        /// The owning ListBar control.
        /// </summary>
        private ListBar owner = null;

        /// <summary>
        /// Sorts the items in this collection using the specified
        /// comparer.
        /// </summary>
        /// <param name="comparer">IComparer implementation specifying
        /// how to sort the objects.</param>
        [Description("Sorts the items in this collection using the specified comparer")]
        public virtual void Sort(IComparer comparer)
        {
            this.InnerList.Sort(comparer);
            owner.ItemChanged(null, true);
        }
        /// <summary>
        /// Sorts the items in this collection using the default comparison
        /// operation (alphabetic).
        /// </summary>
        [Description("Sorts the items in this collection alphabetically.")]
        public virtual void Sort()
        {
            this.InnerList.Sort();
            owner.ItemChanged(null, true);
        }

        /// <summary>
        /// Draws the items within this collection.
        /// </summary>
        /// <param name="gfx">The graphics object to draw onto.</param>
        /// <param name="bounds">The bounding rectangle within which
        /// to draw the items.</param>
        /// <param name="ils">The ImageList to use when drawing the icons.</param>
        /// <param name="defaultFont">The default <see cref="System.Drawing.Font"/> to use.</param>
        /// <param name="style">The Style to draw the items using.</param>
        /// <param name="view">The view to use when drawing the items.</param>
        /// <param name="enabled">Whether the owning group is enabled or not.</param>
        /// <param name="scrollOffset">The scrolled offset at which to start
        /// drawing the items.</param>				
        public virtual void Draw(
            Graphics gfx,
            Rectangle bounds,
            ImageList ils,
            Font defaultFont,
            ListBarDrawStyle style,
            ListBarGroupView view,
            bool enabled,
            int scrollOffset
            )
        {
            bool skipDraw = false;
            ListBarItem editItem = this.owner.EditItem;

            gfx.SetClip(bounds);
            foreach (ListBarItem item in this.InnerList)
            {
                skipDraw = false;
                if (editItem != null)
                {
                    skipDraw = editItem.Equals(item);
                }
                int itemTop = item.Location.Y;
                itemTop += scrollOffset;
                if (
                    ((itemTop >= bounds.Top) && (itemTop <= bounds.Bottom)) ||
                    (((itemTop + item.Height) <= bounds.Bottom) && ((itemTop + item.Height) > bounds.Top))
                    )
                {
                    item.DrawButton(gfx, ils, defaultFont,
                        style, view, scrollOffset, enabled, skipDraw);
                }
            }
            gfx.ResetClip();
        }

        /// <summary>
        /// Gets the height of all the items within this collection.
        /// </summary>
        [Description("Gets the overall height of all the items in the collection.")]
        public virtual int Height
        {
            get
            {
                int maxHeight = 0;
                foreach (ListBarItem item in this.InnerList)
                {
                    int itemBottom = item.Location.Y + item.Height;
                    if (itemBottom > maxHeight)
                    {
                        maxHeight = itemBottom;
                    }
                }
                return maxHeight;
            }
        }

        /// <summary>
        /// Adds a <see cref="ListBarItem"/> object to the group.
        /// </summary>
        /// <param name="item">The ListBarItem to add.</param>
        [Description("Adds a ListBarItem object to the items in the group.")]
        public virtual void Add(
            ListBarItem item
            )
        {
            this.InnerList.Add(item);
            item.SetOwner(owner);
            EnsureSingleSelection(item);
            NotifyOwner(item, true);
        }

        /// <summary>
        /// Constructs a new <see cref="ListBarItem"/> object using the specified
        /// caption, adds it to the bar and returns it.
        /// </summary>
        /// <param name="caption">The caption to use for the ListBarItem.</param>
        /// <returns>The newly added ListBarItem object.</returns>
        [Description("Constructs a new ListBarItem object and adds it to the group.")]
        public virtual ListBarItem Add(
            string caption
            )
        {
            ListBarItem item = new ListBarItem(caption);
            this.InnerList.Add(item);
            item.SetOwner(owner);
            NotifyOwner(item, true);
            return item;
        }

        /// <summary>
        /// Constructs a new ListBarItem object using the specified
        /// caption and icon, adds it to the bar and returns it.
        /// </summary>
        /// <param name="caption">The caption to use for the ListBarItem.</param>
        /// <param name="iconIndex">The 0-based index of the icon for the ListBarItem
        /// within an ImageList</param>
        /// <returns>The newly added ListBarItem object.</returns>
        [Description("Constructs a new ListBarItem object and adds it to the group.")]
        public virtual ListBarItem Add(
            string caption,
            int iconIndex
            )
        {
            ListBarItem item = new ListBarItem(caption, iconIndex);
            this.InnerList.Add(item);
            item.SetOwner(owner);
            NotifyOwner(item, true);
            return item;
        }

        /// <summary>
        /// Adds a range of <see cref="ListBarItem"/> objects to the bar from an array.
        /// </summary>
        /// <param name="values">The array of ListBarItem objects to
        /// add.</param>
        [Description("Adds of range of ListBarItem objects to the bar.")]
        public virtual void AddRange(
            ListBarItem[] values
            )
        {
            foreach (ListBarItem item in values)
            {
                this.InnerList.Add(item);
                item.SetOwner(owner);
            }
            EnsureSingleSelection(this[0]);
            NotifyOwner(values[0], true);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified <see cref="ListBarItem "/> is contained
        /// within this collection, otherwise <c>false</c>.
        /// </summary>
        /// <param name="item">The ListBarItem to check.</param>
        /// <returns>True if the specified ListBarItem is contained
        /// within this collection, False otherwise.</returns>
        [Description("Returns true if the specified ListBarItem is found in this collection, otherwise false.")]
        public virtual bool Contains(ListBarItem item)
        {
            return this.InnerList.Contains(item);
        }

        /// <summary>
        /// Returns the 0-based index of the specified item in the
        /// collection if present, -1 otherwise.
        /// </summary>
        /// <param name="item">The ListBarItem to check.</param>
        /// <returns>The 0-based index of the specified item in the
        /// collection if present, -1 otherwise.</returns>
        [Description("Returns the 0-based index of the specified item in the collection")]
        public virtual int IndexOf(ListBarItem item)
        {
            return this.InnerList.IndexOf(item);
        }

        /// <summary>
        /// Inserts a <see cref="ListBarItem"/> at the specified index in the bar.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="item">The ListBarItem to insert.</param>
        [Description("Inserts a ListBarItem at the specified index in the bar.")]
        public virtual void Insert(int index, ListBarItem item)
        {
            this.InnerList.Insert(index, item);
            item.SetOwner(this.owner);
            EnsureSingleSelection(item);
            NotifyOwner(item, true);
        }

        /// <summary>
        /// Inserts a <see cref="ListBarItem"/> immediately before the specified ListBarItem.
        /// </summary>
        /// <param name="itemBefore">ListBarItem to insert before.</param>
        /// <param name="item">Item to insert.</param>
        [Description("Inserts a ListBarItem immediately before the specified ListBarItem.")]
        public virtual void InsertBefore(ListBarItem itemBefore, ListBarItem item)
        {
            this.InnerList.Insert(this.InnerList.IndexOf(itemBefore), item);
            EnsureSingleSelection(item);
            NotifyOwner(item, true);
        }

        /// <summary>
        /// Inserts a <see cref="ListBarItem"/> immediately after the specified ListBarItem.
        /// </summary>
        /// <param name="itemAfter">ListBarItem to insert after.</param>
        /// <param name="item">Item to insert.</param>
        [Description("Inserts a ListBarItem immediately after the specified ListBarItem.")]
        public virtual void InsertAfter(ListBarItem itemAfter, ListBarItem item)
        {
            int index = this.InnerList.IndexOf(itemAfter);
            if (index == this.InnerList.Count - 1)
            {
                this.Add(item);
            }
            else
            {
                this.Insert(index + 1, item);
            }
            NotifyOwner(item, true);
        }

        /// <summary>
        /// Removes the specified <see cref="ListBarItem"/> from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        [Description("Removes the specified ListBarItem from the collection.")]
        public virtual void Remove(ListBarItem item)
        {
            this.InnerList.Remove(item);
            NotifyOwner(item, true);
        }

        /// <summary>
        /// Gets the <see cref="ListBarItem"/> at the specified 0-based index.
        /// </summary>
        [Description("Gets the ListBarItem at the specified 0-based index.")]
        public ListBarItem this[int index]
        {
            get
            {
                return (ListBarItem)this.InnerList[index];
            }
        }

        /// <summary>
        /// Gets the <see cref="ListBarItem"/> with the specified key.
        /// </summary>
        [Description("Gets the ListBarItem at the specified key.")]
        public ListBarItem this[string key]
        {
            get
            {
                ListBarItem ret = null;
                foreach (ListBarItem item in this.InnerList)
                {
                    if (item.Key.Equals(key))
                    {
                        ret = item;
                        break;
                    }
                }
                return ret;
            }
        }

        private void EnsureSingleSelection(ListBarItem newItem)
        {
            bool foundSelectedItem = false;
            if (newItem.Selected)
            {
                foundSelectedItem = true;
            }

            foreach (ListBarItem item in this.InnerList)
            {
                if (!item.Equals(newItem))
                {
                    if (item.Selected)
                    {
                        if (foundSelectedItem)
                        {
                            item.Selected = false;
                        }
                        else
                        {
                            foundSelectedItem = true;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Notifies the owner control that the items have been
        /// cleared.
        /// </summary>
        protected override void OnClearComplete()
        {
            NotifyOwner(null, true);
        }
        /// <summary>
        /// Notifies the owner control after an item has been inserted.
        /// </summary>
        /// <param name="index">Index of inserting item</param>
        /// <param name="value">Item which has been inserted.</param>
        protected override void OnInsertComplete(System.Int32 index, System.Object value)
        {
            NotifyOwner((ListBarItem)value, true);
        }
        /// <summary>
        /// Notifies the owner control after an item has been removed.
        /// </summary>
        /// <param name="index">Index of inserting item</param>
        /// <param name="value">Item which has been inserted.</param>
        protected override void OnRemoveComplete(System.Int32 index, System.Object value)
        {
            NotifyOwner((ListBarItem)value, true);
        }
        /// <summary>
        /// Notifies the owner control after an item has been changed using set.
        /// </summary>
        /// <param name="index">Index of inserting item</param>
        /// <param name="oldValue">Old item which was there.</param>
        /// <param name="newValue">New Item which has been set.</param>
        protected override void OnSetComplete(System.Int32 index, System.Object oldValue, System.Object newValue)
        {
            NotifyOwner((ListBarItem)newValue, true);
        }

        /// <summary>
        /// Notifies the owning control of a change in this item.
        /// </summary>
        /// <param name="addRemove">Set to true if the change
        /// that has been made requires the size of the display
        /// to be recalculated.</param>
        /// <param name="item">The Item which has been changed
        /// (or null if the item itm is invalid)</param>
        protected virtual void NotifyOwner(ListBarItem item, bool addRemove)
        {
            if (owner != null)
            {
                owner.ItemChanged(item, addRemove);
            }
        }

        /// <summary>
        /// 
        /// TODO: This method has not been implemented yet.
        /// 
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
            //
            // TODO: This method has not been implemented yet.
            // 
        }

        /// <summary>
        /// Enables a deserialized object graph to be associated with a ListBar
        /// control.
        /// </summary>
        /// <param name="owner">The ListBar control which will own
        /// this collection of items.</param>
        public virtual void SetOwner(ListBar owner)
        {
            this.owner = owner;
            foreach (ListBarItem item in this.InnerList)
            {
                item.SetOwner(owner);
            }
        }

        /// <summary>
        /// 
        /// TODO: This method has not been implemented yet.
        /// 
        /// Constructs this object from a serialized representation.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo 
        /// containing the serialized data to build this object from.</param>
        /// <param name="context">The destination (see 
        /// System.Runtime.Serialization.StreamingContext) for this serialization.</param>
        public ListBarItemCollection(
            SerializationInfo info,
            StreamingContext context)
        {
            // 
            // TODO: This method has not been implemented yet.
            // 
        }

        /// <summary>
        /// Constructs a new instance of this collection and sets
        /// the owner.  Typically this is performed by the owning ListBar
        /// control.
        /// </summary>
        /// <param name="owner">The ListBar which owns this collection</param>
        public ListBarItemCollection(ListBar owner)
        {
            this.owner = owner;
        }

    }
    #endregion	

}
