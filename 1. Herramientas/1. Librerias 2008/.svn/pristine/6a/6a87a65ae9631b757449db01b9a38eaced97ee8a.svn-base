using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

// Properties of a single item in the chart.
namespace Desktop.Windows.Forms
{
	public class ChartItem
	{
		
		#region " Internal fields "
		
		// Internal fields.
		private string m_Label;
		private int m_Value;
		private Color m_Color;
		private Color m_HighlightColor;
		private bool m_Over;
		private bool m_Expand;
		private Point m_ExpandOffset;
		private Point m_CenterPoint;
		private float m_StartPos;
		private float m_SweepSize;
		
		#endregion
		
		// Constructor.
		public ChartItem(string Label, int Value, Color Color) {
			this.Label = Label;
			this.Value = Value;
			this.Color = Color;
		}
		
		// Properties.
		public string Label
		{
			get{
				return m_Label;
			}
			set
			{
				m_Label = value;
			}
		}
		
		public int Value
		{
			get{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}
		
		public Color Color
		{
			get{
				return m_Color;
			}
			set
			{
				m_Color = value;
				
				// Create a lighter color for highlight that
				// is used later when drawing item.
				HighlightColor = Color.FromArgb(255, Math.Min(255, System.Convert.ToInt32(System.Convert.ToSingle(value.R) * System.Convert.ToSingle(1.5))), Math.Min(255, System.Convert.ToInt32(System.Convert.ToSingle(value.G) * System.Convert.ToSingle(1.5))), Math.Min(255, System.Convert.ToInt32(System.Convert.ToSingle(value.B) * System.Convert.ToSingle(1.5))));
			}
		}
		
		// Highlight color is used to gradient fill the item and
		// highlight when mouse-over.
		public Color HighlightColor
		{
			get{
				return m_HighlightColor;
			}
			set
			{
				m_HighlightColor = value;
			}
		}
		
		// Starting position of the item. This is relative to the chart type.
		// For example, this is degrees for pie chart and pixels for bar chart.
		public float StartPos
		{
			get{
				return m_StartPos;
			}
			set
			{
				m_StartPos = value;
			}
		}
		
		// Size of the item. This is relative to the chart type. For example,
		// this is degrees for pie chart and pixels for bar chart.
		public float SweepSize
		{
			get{
				return m_SweepSize;
			}
			set
			{
				m_SweepSize = value;
			}
		}
		
		// How far the chart item is expanded. For pie charts, this
		// depends where the item is positioned within the circle.
		public Point ExpandOffset
		{
			get{
				return m_ExpandOffset;
			}
			set
			{
				m_ExpandOffset = value;
			}
		}
		
		// Center to chart shape. Used when drawing labels.
		public Point CenterPoint
		{
			get{
				return m_CenterPoint;
			}
			set
			{
				m_CenterPoint = value;
			}
		}
		
		// If the chart item is expanded or not.
		public bool IsExpanded
		{
			get{
				return m_Expand;
			}
			set
			{
				m_Expand = value;
			}
		}
		
		// Internal properties. If the mouse is currently over the chart item.
		internal bool IsOver
		{
			get{
				return m_Over;
			}
			set
			{
				m_Over = value;
			}
		}
	}
	
}
