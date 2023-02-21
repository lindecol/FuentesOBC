using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Desktop.Windows.Forms
{

    #region Utility class (static methods)
    /// <summary>
    /// An internal class holding static utility methods for the ListBar
    /// control.
    /// </summary>
    internal class ListBarUtility
    {
        /// <summary>
        /// Private constructor - all methods are intended to be static
        /// so you shouldn't be able to create an instance of the class.
        /// </summary>
        private ListBarUtility()
        {
            // intentionally blank
        }

        /// <summary>
        /// Fills a right-angled triangle using the specified brush.  The
        /// origin of the triangle is taken to be the right-angle corner.
        /// </summary>
        /// <param name="gfx">Graphics object to draw onto.</param>
        /// <param name="brush">Brush to fill the right-angled triangle with.</param>
        /// <param name="origin">Location of the right-angle corner of the triangle.</param>
        /// <param name="adjacent">The length of the adjacent side of the triangle.</param>
        /// <param name="opposite">The length of the opposite side of the triangle.</param>
        public static void FillRightAngleTriangle(
            Graphics gfx,
            Brush brush,
            Point origin,
            int adjacent,
            int opposite
            )
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(origin.X, origin.Y, origin.X + adjacent, origin.Y);
            path.AddLine(origin.X + adjacent, origin.Y, origin.X, origin.Y + opposite);
            path.CloseFigure();
            gfx.FillPath(brush, path);
            path.Dispose();
        }

        /// <summary>
        /// Blends two colours together using the specified alpha amount.
        /// </summary>
        /// <param name="colorFrom">Base colour</param>
        /// <param name="colorTo">Colour to blend with the base colour.</param>
        /// <param name="alpha">Alpha amount to use when blending the colours.</param>
        /// <returns>The blended colour.</returns>
        public static Color BlendColor(
            Color colorFrom,
            Color colorTo,
            int alpha
            )
        {
            Color retColor = Color.FromArgb(
                ((colorFrom.R * alpha) / 255) + ((colorTo.R * (255 - alpha)) / 255),
                ((colorFrom.G * alpha) / 255) + ((colorTo.G * (255 - alpha)) / 255),
                ((colorFrom.B * alpha) / 255) + ((colorTo.B * (255 - alpha)) / 255)
                );
            return retColor;
        }
    }
    #endregion

}
