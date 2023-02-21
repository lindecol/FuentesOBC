using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Desktop.Windows.Forms
{
    #region ListBarDragDropInsertPoint class
    /// <summary>
    /// An internal class to manage the drag-drop insert point
    /// within the control.
    /// </summary>
    internal class ListBarDragDropInsertPoint : IComparable
    {
        /// <summary>
        /// The item before the drag-drop insert point, if any
        /// </summary>
        private ListBarItem itemBefore;
        /// <summary>
        /// The item after the drag-drop insert point, if any 
        /// </summary>
        private ListBarItem itemAfter;
        /// <summary>
        /// If we're over an empty bar.
        /// </summary>
        private bool overEmptyBar;

        /// <summary>
        /// Compares this object with another object of the same type.
        /// This implementation is only really useful for testing equality
        /// </summary>
        /// <param name="obj">Another ListBarDragDropInsertPoint object</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the comparands.  
        /// The return value has these meanings: 
        /// &lt; 0: This instance is less than obj.  
        /// 0: This instance is equal to obj.  
        /// &gt; 0: This instance is greater than obj. </returns>
        public virtual System.Int32 CompareTo(System.Object obj)
        {
            int ret = 1;
            ListBarDragDropInsertPoint compare = (ListBarDragDropInsertPoint)obj;
            if (compare.ItemBefore == this.ItemBefore)
            {
                if (compare.ItemAfter == this.ItemAfter)
                {
                    if (compare.OverEmptyBar == this.OverEmptyBar)
                    {
                        ret = 0;
                    }
                }
            }
            return ret;
        }


        /// <summary>
        /// Returns the item before the drag-drop point, if any.  At least one
        /// of the properties ItemBefore or ItemAfter will return an item.
        /// </summary>
        public ListBarItem ItemBefore
        {
            get
            {
                return this.itemBefore;
            }
        }
        /// <summary>
        /// Returns the item after the drag-drop point, if any.  At least one
        /// of the properties ItemBefore or ItemAfter will return an item.
        /// </summary>
        public ListBarItem ItemAfter
        {
            get
            {
                return this.itemAfter;
            }
        }

        /// <summary>
        /// Returns whether the drag point is over an empty bar
        /// or not.
        /// </summary>
        public bool OverEmptyBar
        {
            get
            {
                return this.overEmptyBar;
            }
        }

        /// <summary>
        ///  Constructs a new instance of this class, setting the items
        ///  before and after the drag-drop insertion point.
        /// </summary>
        /// <param name="itemBefore">Item before the drag-drop insertion
        /// point, or null if no item before.</param>
        /// <param name="itemAfter">Item after the drag-drop insertion
        /// point, or null if no item after.</param>
        /// <param name="overEmptyBar">Whether the drag-drop insertion
        /// point should be displayed in an empty bar.</param>
        public ListBarDragDropInsertPoint(
            ListBarItem itemBefore,
            ListBarItem itemAfter,
            bool overEmptyBar
            )
        {
            this.itemBefore = itemBefore;
            this.itemAfter = itemAfter;
            this.overEmptyBar = overEmptyBar;
        }
    }
    #endregion

}
