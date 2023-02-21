using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Desktop.Windows.Forms
{

    /// <summary>
    /// A class to hold the collection of groups in the ListBar control.
    /// </summary>
    [SerializableAttribute()]
    public class ListBarGroupCollection : CollectionBase, ISerializable
    {
        /// <summary>
        /// The ListBar which owns this collection
        /// </summary>
        private ListBar owner = null;

        /// <summary>
        /// Adds a new <see cref="ListBarGroup"/> to the control.
        /// </summary>
        /// <param name="group">The group to add to the control</param>
        /// <returns>The index at which the group was added.</returns>
        [Description("Adds a new ListBarGroup to the control.")]
        public virtual int Add(ListBarGroup group)
        {
            int ret = this.InnerList.Add(group);
            group.SetOwner(owner);
            return ret;
        }

        /// <summary>
        /// Adds a new <see cref="ListBarGroup"/> with the specified caption to
        /// the control and returns a reference to it.
        /// </summary>
        /// <param name="caption">The caption for the new ListBarGroup.</param>
        /// <returns>The ListBarGroup added to the control.</returns>
        [Description("Adds a new ListBarGroup with the specified caption to the control and returns a reference to it.")]
        public virtual ListBarGroup Add(
            string caption
            )
        {
            ListBarGroup group = new ListBarGroup(caption);
            this.InnerList.Add(group);
            group.SetOwner(owner);
            return group;
        }

        /// <summary>
        /// Adds a series of <see cref="ListBarGroup"/> objectss based on the supplied captions.
        /// </summary>
        /// <param name="captions">The array of captions to use when creating
        /// the <see cref="ListBarGroup"/> objects.</param>
        [Description("Adds a series of ListBarGroups with the specified captions to the control.")]
        public virtual void AddRange(
            string[] captions
            )
        {
            foreach (string caption in captions)
            {
                Add(caption);
            }
        }

        /// <summary>
        /// Adds a range of previously defined <see cref="ListBarGroup" /> objects.
        /// </summary>
        /// <param name="values">The array of ListBarGroups to add
        /// to the control.</param>
        [Description("Adds a range of previously defined ListBarGroup objects.")]
        public virtual void AddRange(
            ListBarGroup[] values
            )
        {
            foreach (ListBarGroup group in values)
            {
                this.InnerList.Add(group);
                group.SetOwner(owner);
            }
        }

        /// <summary>
        /// Determines whether a <see cref="ListBarGroup"/> element is contained within 
        /// the control's collection of groups.
        /// </summary>
        /// <param name="group">The ListBarGroup to check if present.</param>
        /// <returns>True if the ListBarGroup is contained within the control's
        /// collection, False otherwise.</returns>
        [Description("Determins whether a ListBarGroup element is contained within the control's collection of groups.")]
        public virtual bool Contains(ListBarGroup group)
        {
            return this.InnerList.Contains(group);
        }

        /// <summary>
        /// Gets the 0-based index of the specified <see cref="ListBarGroup"/> within this
        /// collection.
        /// </summary>
        /// <param name="group">The group to find the index for.</param>
        /// <returns>The 0-based index of the group, if found, otherwise - 1.</returns>
        [Description("Gets the 0-based index of the specified ListBarGroup within this collection.")]
        public virtual int IndexOf(ListBarGroup group)
        {
            return this.InnerList.IndexOf(group);
        }

        /// <summary>
        /// Inserts a group at the specified 0-based index in the collection
        /// of groups.
        /// </summary>
        /// <param name="index">The 0-based index to insert the group at.</param>
        /// <param name="group">The ListBarGroup to add.</param>
        [Description("(Inserts a group at the specified 0-based index in the collection of groups.")]
        public virtual void Insert(int index, ListBarGroup group)
        {
            this.InnerList.Insert(index, group);
            group.SetOwner(owner);
        }
        /// <summary>
        /// Inserts a group immediately before the specified <see cref="ListBarGroup"/>.
        /// </summary>
        /// <param name="groupBefore">ListBarGroup to insert before.</param>
        /// <param name="group">Group to insert.</param>
        [Description("(Inserts a group immediately before the specified ListBarGroup object.")]
        public virtual void InsertBefore(ListBarGroup groupBefore, ListBarGroup group)
        {
            this.InnerList.Insert(this.InnerList.IndexOf(groupBefore), group);
            group.SetOwner(owner);
        }
        /// <summary>
        /// Inserts a <see cref="ListBarGroup"/> immediately after the specified ListBarGroup.
        /// </summary>
        /// <param name="groupAfter">ListBarGroup to insert after.</param>
        /// <param name="group">Group to insert.</param>
        [Description("(Inserts a group immediately after the specified ListBarGroup object.")]
        public virtual void InsertAfter(ListBarGroup groupAfter, ListBarGroup group)
        {
            int index = this.InnerList.IndexOf(groupAfter);
            if (index == this.InnerList.Count - 1)
            {
                this.Add(group);
            }
            else
            {
                this.Insert(index + 1, group);
            }
        }


        /// <summary>
        /// Removes the specified <see cref="ListBarGroup"/>.
        /// </summary>
        /// <param name="group">The group to remove.</param>
        [Description("(Removes the specified ListBarGroup object.")]
        public virtual void Remove(ListBarGroup group)
        {
            this.InnerList.Remove(group);
            NotifyOwner(group, true);
        }

        /// <summary>
        /// Gets the <see cref="ListBarGroup"/> at the specified 0-based index.
        /// </summary>
        [Description("(Gets the ListBarGroup at the specified 0-based index.")]
        public ListBarGroup this[int index]
        {
            get
            {
                return (ListBarGroup)this.InnerList[index];
            }
        }

        /// <summary>
        /// Gets the <see cref="ListBarGroup"/> with the specified string key.
        /// </summary>
        [Description("(Gets the ListBarGroup with the specified string key.")]
        public ListBarGroup this[string key]
        {
            get
            {
                ListBarGroup ret = null;
                foreach (ListBarGroup group in this.InnerList)
                {
                    if (group.Key.Equals(key))
                    {
                        ret = group;
                        break;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Notifies the owning ListBar control of any changes to a group.
        /// </summary>
        /// <param name="group">The Group which has changed.</param>
        /// <param name="addRemove">Whether the control should resize
        /// all groups associated with the ListBar.</param>
        protected virtual void NotifyOwner(ListBarGroup group, bool addRemove)
        {
            if (this.owner != null)
            {
                this.owner.GroupChanged(group, addRemove);
            }
        }

        /// <summary>
        /// Notifies the control after clearing all groups.
        /// </summary>
        protected override void OnClearComplete()
        {
            NotifyOwner(null, true);
        }

        /// <summary>
        /// Notifies the control after inserting a new ListBarGroup.
        /// </summary>
        protected override void OnInsertComplete(System.Int32 index, System.Object value)
        {
            NotifyOwner((ListBarGroup)value, true);
        }

        /// <summary>
        /// Notifies the control after removing a new ListBarGroup.
        /// </summary>
        protected override void OnRemoveComplete(System.Int32 index, System.Object value)
        {
            NotifyOwner(null, true);
        }

        /// <summary>
        /// Notifies the control after setting a ListBarGroup to another ListBarGroup.
        /// </summary>
        protected override void OnSetComplete(System.Int32 index, System.Object oldValue, System.Object newValue)
        {
            NotifyOwner((ListBarGroup)newValue, false);
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
            // TODO
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
            foreach (ListBarGroup group in this.InnerList)
            {
                group.SetOwner(owner);
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
        public ListBarGroupCollection(
            SerializationInfo info,
            StreamingContext context)
        {

        }

        /// <summary>
        /// Creates a new instance of the ListBarGroup collection and associates
        /// it with the control which owns it.
        /// </summary>
        /// <param name="owner">The owning ListBar control.</param>
        public ListBarGroupCollection(ListBar owner)
        {
            this.owner = owner;
        }

    }

}
