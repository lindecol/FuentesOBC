using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;


// Constants.
namespace Desktop.Windows.Forms
{
	internal class ConstValues
	{
		// How many pixels a chart item expands.
		public static int ExpandSize = 10;
		
		// Size of 3D shadow.
		public static int ShadowSize = 3;
	}
	
	
	// Color table that is used for chart items. User of the control
	// can override the color table with methods exposed from the main
	// control class. Colors repeat if an index is specified that is
	// larger than the color table but the color is slightly brighter.
	internal class ChartColors
	{
		
		#region " Color Table "
		
		private Color[] m_List = null;
		
		#endregion
		
		// Override a color in the color table.
		public void SetColor (int Index, Color NewColor)
		{
			// Return right away if passed in an invalid index.
			if ((Index < 0) |(Index >= m_List.Length))
			{
				return;
			}
			m_List[Index] = NewColor;
		}
		
		// Return color from color table. Colors repeat but are slightly brighter.
		public Color GetColor(int Index)
		{
			Color result = m_List[(Index % m_List.Length)];
			int x = System.Convert.ToInt32(Index / m_List.Length);
			if (x > 0)
			{
				// Asked for a color larger than our internal table.
				result = Color.FromArgb(Math.Min(result.R + x, 255), Math.Min(result.G + x, 255), Math.Min(result.B + x, 255));
			}
			return result;
		}
	}
	
	
	// Base class that performs drawing functions. Shape classes
	// inherit from this class and perform shape-specific actions,
	// calculate position / size and draw shapes.
	internal class ChartDrawing
	{
		
		// Used to access main control properties.
		protected ChartControl m_Parent;
		
		// Constructor.
		public ChartDrawing(ChartControl Parent) {
			m_Parent = null;
			
			m_Parent = Parent;
		}
		
		// Draw the chart.
		public virtual void Draw (int Width, int Height, Rectangle rc, Graphics g, Graphics HitTest)
		{
			
			// Don't try drawing anything if area is too small.
			if ((rc.Width <= 1) |(rc.Height <= 1))
			{
				return;
			}
			
			// First, draw the 3D shadow.
			ChartItem item;
			int i;
			for (i = ConstValues.ShadowSize; i <= 0; i += - 1)
			{
				foreach (ChartItem tempLoopVar_item in m_Parent.List)
				{
					item = tempLoopVar_item;
					// Get drawing area for this item.
					Rectangle rcShadow = new Rectangle(rc.Location, rc.Size);
					rcShadow.Offset(i, i);
					
					if (item.IsExpanded)
					{
						// Draw expanded item shadow.
						Rectangle rcShift = new Rectangle(rcShadow.Location, rcShadow.Size);
						rcShift.Offset(item.ExpandOffset.X, item.ExpandOffset.Y);
						DrawShape(g, new HatchBrush(HatchStyle.Percent50, item.Color), rcShift, item);
					}
					else
					{
						// Draw item shadow.
						DrawShape(g, new HatchBrush(HatchStyle.Percent50, item.Color), rcShadow, item);
					}
				}
			}
			
			// Now go through and draw chart items.
			foreach (ChartItem tempLoopVar_item in m_Parent.List)
			{
				item = tempLoopVar_item;
				// Adjust the bounding rectangle for expanded items. If the
				// item is expanded, draw on a memory bitmap with non-expanded
				// area so the expanded area acts like it's part of the
				// shape (used when performing hit testing later).
				Rectangle rcShape = new Rectangle(rc.Location, rc.Size);
				if (item.IsExpanded)
				{
					rcShape.Offset(item.ExpandOffset.X, item.ExpandOffset.Y);
					DrawShape(HitTest, new SolidBrush(item.Color), rc, item);
				}
				
				// Use highlight brush if mouseover, otherwise use gradient brush.
				Brush brushShape = null;
				if (item.IsOver)
				{
					brushShape = new SolidBrush(item.HighlightColor);
				}
				else
				{
					brushShape = new LinearGradientBrush(rcShape, item.Color, item.HighlightColor, LinearGradientMode.Horizontal);
				}
				
				// Draw the shape.
				DrawShape(g, brushShape, rcShape, item);
				DrawShape(HitTest, new SolidBrush(item.Color), rcShape, item);
			}
			
			// Draw the shape labels.
			// Calculate border size used when drawing labels.
			int border = ConstValues.ExpandSize + ConstValues.ShadowSize + 1;
			DrawLabels(g, rc, border, m_Parent.List);
		}
		
		// Draws labels, values and percent in each shape. All three values can be
		// turned on / off through properties exposed by the main control class.
		private void DrawLabels (Graphics g, Rectangle rc, int Border, ArrayList List)
		{
			
			// Calculate total value of chart (sum of all items) that is
			// used when drawing percent.
			int total = m_Parent.TotalValue;
			
			// Go through each item and draw label for each item in chart.
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in List)
			{
				item = tempLoopVar_item;
				// Create the label string to display based on control properties.
				string labelStr = string.Empty;
				if (m_Parent.ShowLabel)
				{
					labelStr = item.Label;
				}
				
				// Create the value string to display based on control properties.
				string valueStr = string.Empty;
				if ((m_Parent.ShowValue == true) &(m_Parent.ShowPercent == false))
				{
					valueStr = item.Value.ToString();
				}
				else
				{
					if ((m_Parent.ShowValue == false) &(m_Parent.ShowPercent == true))
					{
						valueStr = string.Format("{0:0}%",(item.Value * 100) / total);
					}
					else
					{
						if ((m_Parent.ShowValue == true) &(m_Parent.ShowPercent == true))
						{
							valueStr = string.Format("{0} ({1:0}%)", item.Value,(item.Value * 100) / total);
						}
					}
				}
				
				// Calculate size of strings.
				SizeF labelSize = g.MeasureString(labelStr, m_Parent.Font);
				SizeF valueSize = g.MeasureString(valueStr, m_Parent.Font);
				
				// Calculate where to draw strings.
				Point pt = item.CenterPoint;
				Point offset = new Point(Border, Border);
				if (item.IsExpanded)
				{
					offset.X += item.ExpandOffset.X;
					offset.Y += item.ExpandOffset.Y;
				}
				
				// Draw strings in chart item.
				g.DrawString(labelStr, m_Parent.Font, new SolidBrush(m_Parent.ForeColor), pt.X - labelSize.Width / 2 + offset.X, pt.Y -(labelSize.Height + valueSize.Height) / 2 + offset.Y);
				g.DrawString(valueStr, m_Parent.Font, new SolidBrush(m_Parent.ForeColor), pt.X - valueSize.Width / 2 + offset.X, pt.Y -(labelSize.Height + valueSize.Height) / 2 + labelSize.Height + offset.Y);
			}
		}
		
		// Return bounding rectangle for entire chart.
		public virtual Rectangle GetChartRectangle(int Width, int Height)
		{
			return new Rectangle(0, 0, Width, Height);
		}
		
		// Override in derived class.
		public virtual void LayoutItems (Rectangle rc)
		{
			System.Diagnostics.Debug.Assert(false);
		}
		
		// Override in derived class.
		public virtual void DrawShape (Graphics g, Brush b, Rectangle rc, ChartItem item)
		{
			System.Diagnostics.Debug.Assert(false);
		}
		
		// Override in derived class.
		public virtual void DrawEmptyChart (Graphics g, Rectangle rc)
		{
			System.Diagnostics.Debug.Assert(false);
		}
	}
	
	
	// Pie chart class, calculates layout and draws pie shapes.
	internal class PieDrawing : ChartDrawing
	{
		
		// Constructor.
		public PieDrawing(ChartControl parent) : base(parent) {
		}
		
		// Calculate the position of each item in the chart.
		public override void LayoutItems (Rectangle rc)
		{
			// Return right away if area is too small.
			if ((rc.Width <= 1) |(rc.Height <= 1))
			{
				return;
			}
			
			float start = 0.0F;
			float sweep = 0.0F;
			ChartItem item;
			
			// Go through each item and calculate layout.
			foreach (ChartItem tempLoopVar_item in m_Parent.List)
			{
				item = tempLoopVar_item;
				// Calculate the sweep angle for this item.
				sweep = System.Convert.ToSingle(item.Value * 360) / m_Parent.TotalValue;
				
				// Calculate the offset when this item is expanded.
				Point shift = GetPoint(start + sweep / 2, rc.Width, rc.Height);
				
				// Relative to center of chart.
				shift.X -= System.Convert.ToInt32(rc.Width / 2);
				shift.Y -= System.Convert.ToInt32(rc.Height / 2);
				
				// Now convert to max offset.
				float x = System.Convert.ToSingle(ConstValues.ExpandSize) / System.Convert.ToSingle(Math.Max(Math.Abs(shift.X), Math.Abs(shift.Y)));
				shift.X = System.Convert.ToInt32(System.Convert.ToSingle(shift.X) * x);
				shift.Y = System.Convert.ToInt32(System.Convert.ToSingle(shift.Y) * x);
				item.ExpandOffset = shift;
				
				// Calculate center of pie slice.
				Point center = GetPoint(start + sweep / 2, rc.Width, rc.Height);
				center.X = System.Convert.ToInt32(((rc.Right - rc.Left) / 2 + center.X) / 2) + rc.Left;
				center.Y = System.Convert.ToInt32(((rc.Bottom - rc.Top) / 2 + center.Y) / 2);
				item.CenterPoint = center;
				
				// Starting position and sweep size, both in degrees.
				item.StartPos = start;
				item.SweepSize = sweep;
				
				start += sweep;
			}
		}
		
		// Draw a pie shape.
		public override void DrawShape (Graphics g, Brush br, Rectangle rc, ChartItem item)
		{
			// Make sure we have a valid area to draw on.
			if ((rc.Width > 1) &(rc.Height > 1))
			{
				//rc.Width = rc.Height
				g.FillPie(br, rc, item.StartPos, item.SweepSize);
			}
		}
		
		// Draw what is displayed when there are no items in the chart.
		public override void DrawEmptyChart (Graphics g, Rectangle rc)
		{
			if ((rc.Width > 1) &(rc.Height > 1))
			{
				g.DrawEllipse(new Pen(Color.DarkGray), rc);
			}
		}
		
		// Return bounding rectangle for entire chart.
		public override Rectangle GetChartRectangle(int Width, int Height)
		{
			
			// Make chart area a square so the pie chart is a circle and not ellipse.
			// Center the bounding rectangle in available space.
			int size = Math.Min(Width, Height);
			
			return new Rectangle(System.Convert.ToInt32((Width - size) / 2), System.Convert.ToInt32((Height - size) / 2), size, size);
		}
		
		
		// Return point on circle edge given an angle.
		private Point GetPoint(float Angle, int Width, int Height)
		{
			Point topCenter = new Point(System.Convert.ToInt32(Width / 2), 0);
			double rad = Math.PI * 2 * Angle / 360;
			Point pt = new Point();
			pt.X = System.Convert.ToInt32((Math.Cos(rad) * topCenter.X - Math.Sin(rad) * topCenter.Y) + Width / 2);
			pt.Y = System.Convert.ToInt32((Math.Sin(rad) * topCenter.X + Math.Cos(rad) * topCenter.Y) + Height / 2);
			return pt;
		}
	}
	
	
	// Stacked bar chart class, calculates layout and draws bar shapes.
	internal class BarDrawing : ChartDrawing
	{
		
		// Constructor.
		public BarDrawing(ChartControl parent) : base(parent) {
		}
		
		// Calculate the position of each item in the chart.
		public override void LayoutItems (Rectangle rc)
		{
			// Return right away if area is too small.
			if ((rc.Width <= 1) |(rc.Height <= 1))
			{
				return;
			}
			
			// Go through each item and calculate layout.
			float startPos = 0.0F;
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in m_Parent.List)
			{
				item = tempLoopVar_item;
				// The starting position (top of rectangle) and height of shape are in pixels.
				item.StartPos = startPos;
				item.SweepSize = System.Convert.ToSingle(item.Value * rc.Height) / m_Parent.TotalValue;
				
				// Calculate the offset when this item is expanded.
				item.ExpandOffset = new Point(ConstValues.ExpandSize, 0);
				
				// Center of this shape.
				item.CenterPoint = new Point(System.Convert.ToInt32(rc.Width / 2), rc.Height - System.Convert.ToInt32(item.StartPos + item.SweepSize / 2));
				
				startPos += item.SweepSize;
			}
		}
		
		// Draw bar shape.
		public override void DrawShape (Graphics g, Brush br, Rectangle rc, ChartItem item)
		{
			// Make sure we have a valid area to draw on
			if ((rc.Width > 1) &(rc.Height > 1))
			{
				g.FillRectangle(br, rc.Left, rc.Bottom - System.Convert.ToInt32(item.StartPos) - System.Convert.ToInt32(item.SweepSize), rc.Width, System.Convert.ToInt32(item.SweepSize));
			}
		}
		
		// Draw what is displayed when there are no items in the chart.
		public override void DrawEmptyChart (Graphics g, Rectangle rc)
		{
			if ((rc.Width > 1) &(rc.Height > 1))
			{
				g.DrawRectangle(new Pen(Color.DarkGray), rc);
			}
		}
	}
	
}
