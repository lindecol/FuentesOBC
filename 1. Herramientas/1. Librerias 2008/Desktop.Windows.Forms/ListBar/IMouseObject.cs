using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Desktop.Windows.Forms
{
    #region IMouseObject interface
    /// <summary>
    /// An internal interface specifying the properties and methods which must
    /// be supported by an object in the control which interacts with the
    /// mouse.
    /// TODO: think of a better name for this interface
    /// </summary>
    internal interface IMouseObject
    {
        /// <summary>
        /// Gets/sets the point at which the mouse button was
        /// pressed.
        /// </summary>
        Point MouseDownPoint
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets the tooltip text for this object.
        /// </summary>
        string ToolTipText
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets whether the mouse is over the object or not.
        /// </summary>
        bool MouseOver
        {
            get;
            set;
        }
        /// <summary>
        /// Gets/sets whether the mouse was pressed on the object or not.
        /// </summary>
        bool MouseDown
        {
            get;
            set;
        }
    }
    #endregion

}
