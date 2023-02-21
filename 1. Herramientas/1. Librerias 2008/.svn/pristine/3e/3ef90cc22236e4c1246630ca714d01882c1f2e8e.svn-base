using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;



// Main chart control class. Exposes properties to user.
// Uses helper class to calculate and draw chart shapes.
namespace Desktop.Windows.Forms
{
	[DefaultEvent("ClickItem")]public class ChartControl : System.Windows.Forms.UserControl
	{
		
		// Internal fields.
		private ArrayList m_List;
		private bool m_ListDirty;
		private Bitmap m_HitTestImage;
		private bool m_ShowLabel;
		private bool m_ShowValue;
		private bool m_ShowPercent;
		private bool m_PieChart;
		private bool m_AutoExpand;
		
		// This is a weak-data-biding. That means it binds to a DataTable and
		// specified table field, its not as generic as other controls that
		// data-bind to various objects.
		private DataView m_DataView;
		private string m_DataMember;
		
		// A helper class is used to calculate layout and draw charts.
		// The helper class draws pie or bar charts depending on the
		// exposed property from this class.
		private ChartDrawing m_Drawing;
		private ChartColors m_ChartColors;
		
		// Event. Pass the item that was clicked to container.
		[Category("Action"), Description("A chart item was clicked.")]private ClickItemEventHandler ClickItemEvent;
		public event ClickItemEventHandler ClickItem
		{
			add
			{
				ClickItemEvent = (ClickItemEventHandler) System.Delegate.Combine(ClickItemEvent, value);
			}
			remove
			{
				ClickItemEvent = (ClickItemEventHandler) System.Delegate.Remove(ClickItemEvent, value);
			}
		}
		
		
		// Properties.
		[Browsable(false), Description("Table used for data binding.")]public DataTable DataTable
		{
			set
			{
				// Get view for table, do this so we can hookup to the ListChanged event
				// so the chart can be updated whenever the underlying data changes.
				m_DataView = value.DefaultView;
				m_DataView.ListChanged += new System.ComponentModel.ListChangedEventHandler(OnDataChanged);
				DataBind();
			}
		}
		
		[Browsable(false), Description("Field in table used for binding.")]public string DataMember
		{
			set
			{
				m_DataMember = value;
				DataBind();
			}
		}
		
		[Browsable(false), ReadOnly(true), Description("Return number of items in chart.")]public int Count
		{
			get{
				return m_List.Count;
			}
		}
		
		[Browsable(false), ReadOnly(true), Description("Sum of all items in chart.")]public int TotalValue
		{
			get{
				// Go through and add up values of all items in the chart.
				int Total = 0;
				ChartItem item;
				foreach (ChartItem tempLoopVar_item in m_List)
				{
					item = tempLoopVar_item;
					Total += item.Value;
				}
				return Total;
			}
		}
		
		[DefaultValue(true), Category("Appearance"), Description("Show item labels.")]public bool ShowLabel
		{
			get{
				return m_ShowLabel;
			}
			set
			{
				m_ShowLabel = value;
			}
		}
		
		[DefaultValue(false), Category("Appearance"), Description("Show item values.")]public bool ShowValue
		{
			get{
				return m_ShowValue;
			}
			set
			{
				m_ShowValue = value;
			}
		}
		
		[DefaultValue(false), Category("Appearance"), Description("Show item percent.")]public bool ShowPercent
		{
			get{
				return m_ShowPercent;
			}
			set
			{
				m_ShowPercent = value;
			}
		}
		
		[DefaultValue(true), Category("Appearance"), Description("Automatically expand clicked items.")]public bool AutoExpand
		{
			get{
				return m_AutoExpand;
			}
			set
			{
				m_AutoExpand = value;
			}
		}
		
		[DefaultValue(true), Category("Appearance"), Description("Use pie or stacked bar chart.")]public bool PieChart
		{
			get{
				return m_PieChart;
			}
			
			set
			{
				m_PieChart = value;
				
				// Create helper object that is used to draw the chart.
				if (m_PieChart)
				{
					m_Drawing = new PieDrawing(this);
				}
				else
				{
					m_Drawing = new BarDrawing(this);
				}
				
				// Update the chart.
				LayoutControls();
				m_ListDirty = true;
				Invalidate();
			}
		}
		
		// Internal properties. Used by helper drawing class.
		[Browsable(false), ReadOnly(true)]internal ArrayList List
		{
			get{
				return m_List;
			}
		}
		
		
		#region " Windows Form Designer generated code "
		
		public ChartControl() {
			m_List = new ArrayList();
			m_ListDirty = true;
			m_HitTestImage = null;
			m_ShowLabel = true;
			m_ShowValue = false;
			m_ShowPercent = false;
			m_PieChart = true;
			m_AutoExpand = true;
			m_DataView = null;
			m_DataMember = string.Empty;
			m_Drawing = null;
			m_ChartColors = new ChartColors();
			
			
			m_Drawing = new PieDrawing(this);
			
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			
			// we want to repaint when resized
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
		}
		
		//UserControl overrides dispose to clean up the component list.
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (!(components == null))
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		internal System.Windows.Forms.PictureBox chart;
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			this.chart = new System.Windows.Forms.PictureBox();
			base.Load += new System.EventHandler(CustomChart_Load);
			base.Resize += new System.EventHandler(CustomChart_Resize);
			base.Paint += new System.Windows.Forms.PaintEventHandler(CustomChart_Paint);
			chart.MouseMove += new System.Windows.Forms.MouseEventHandler(chart_MouseMove);
			chart.MouseLeave += new System.EventHandler(chart_MouseLeave);
			chart.Click += new System.EventHandler(chart_Click);
			this.SuspendLayout();
			//
			//chart
			//
			this.chart.BackColor = System.Drawing.Color.Transparent;
			this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chart.Name = "chart";
			this.chart.Size = new System.Drawing.Size(112, 112);
			this.chart.TabIndex = 0;
			this.chart.TabStop = false;
			//
			//CustomChart
			//
			this.Controls.AddRange(new System.Windows.Forms.Control[] {this.chart});
			this.Name = "CustomChart";
			this.Size = new System.Drawing.Size(112, 112);
			this.ResumeLayout(false);
			
		}
		
		#endregion
		
		
		// Public methods.
		
		// Add item to chart.
		public void Add (string Label, int Value, Color Color)
		{
			// Add item to internal list.
			m_List.Add(new ChartItem(Label, Value, Color));
			m_ListDirty = true;
		}
		
		// Override color table.
		public void SetColor (int Index, Color NewColor)
		{
			m_ChartColors.SetColor(Index, NewColor);
		}
		
		// Expand or collapse a chart item.
		public void ExpandItem (int Index, bool Expand)
		{
			if (Index < m_List.Count)
			{
				ChartItem item = ((ChartItem)(m_List[Index]));
				if (!(item == null))
				{
					item.IsExpanded = Expand;
					Invalidate();
				}
			}
		}
		
		// Expand or collapse all items in the chart.
		public void ExpandAll (bool Expand)
		{
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in m_List)
			{
				item = tempLoopVar_item;
				item.IsExpanded = Expand;
			}
			Invalidate();
		}
		
		// Return chart item.
		public ChartItem GetItem(int Index)
		{
			return ((ChartItem)(m_List[Index]));
		}
		
		
		// Event handlers.
		
		private void CustomChart_Load (System.Object sender, System.EventArgs e)
		{
			LayoutControls();
		}
		
		private void CustomChart_Resize (object sender, System.EventArgs e)
		{
			m_ListDirty = true;
			LayoutControls();
		}
		
		// Handle all painting, setup painting info and pass along to drawing helper object.
		private void CustomChart_Paint (object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Border space around chart.
			int Border = ConstValues.ExpandSize + ConstValues.ShadowSize + 1;
			
			// Calculate area to draw chart.
			Rectangle rc;
			if (m_PieChart)
			{
				rc = new Rectangle((chart.Width / 2) -(chart.Height / 2) + Border, Border, chart.Height - Border * 2, chart.Height - Border * 2);
			}
			else
			{
				rc = new Rectangle(Border, Border, chart.Width -(Border * 2), chart.Height -(Border * 2));
			}
			
			// See if need to update the layout of the chart items.
			if (m_ListDirty)
			{
				m_Drawing.LayoutItems(rc);
				m_ListDirty = false;
			}
			
			// Drawing surface for chart.
			Graphics g = Graphics.FromImage(chart.Image);
			g.Clear(this.BackColor);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			
			// Drawing surface for hit testing.
			Graphics hitTest = Graphics.FromImage(m_HitTestImage);
			hitTest.Clear(this.BackColor);
			hitTest.SmoothingMode = SmoothingMode.AntiAlias;
			
			// Draw the chart, pass along to helper drawing object.
			if (this.Count > 0)
			{
				m_Drawing.Draw(chart.Width, chart.Height, rc, g, hitTest);
			}
			else
			{
				m_Drawing.DrawEmptyChart(g, rc);
			}
		}
		
		private void chart_MouseMove (object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				// Update the mouse over state for the chart items.
				UpdateOver(m_HitTestImage.GetPixel(e.X, e.Y));
				Invalidate();
			}
			catch
			{
			}
		}
		
		private void chart_MouseLeave (object sender, System.EventArgs e)
		{
			// Clear the mouse over state for all chart items.
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in m_List)
			{
				item = tempLoopVar_item;
				item.IsOver = false;
			}
			Invalidate();
		}
		
		private void chart_Click (System.Object sender, System.EventArgs e)
		{
			// Notify container if user clicked on a chart item.
			int index = HitTest();
			if (index != - 1)
			{
				// See if we should auto expand the clicked item.
				if (AutoExpand)
				{
					// Toggle the expand state of this item,
					// only expand one item at a time
					ChartItem item = GetItem(index);
					if (item.IsExpanded)
					{
						ExpandItem(index, false);
					}
					else
					{
						ExpandAll(false);
						ExpandItem(index, true);
					}
				}
				
				// Dispatch the event.
				OnClickItem(new ClickItemEventArgs(index));
			}
		}
		
		// Dispatch the event to delegates. Event argument
		// contains the chart item that was clicked.
		protected virtual void OnClickItem (ClickItemEventArgs e)
		{
			if (ClickItemEvent != null)
			ClickItemEvent(this, e);
		}
		
		// Private methods.
		
		// Called when underlying data changes.
		private void OnDataChanged (object sender, ListChangedEventArgs args)
		{
			// Update items in the chart based on info from data source.
			DataBind();
		}
		
		// Update items in chart based on data source.
		private void DataBind ()
		{
			// Return right away if data binding was not setup.
			if ((m_DataView == null) |(m_DataMember.Length == 0))
			{
				return;
			}
			
			// Clear list since we will be repopulating the chart.
			m_List.Clear();
			
			try
			{
				// Go through and get count of items, list will contain
				// key / value pairs, one for each different item in database.
				SortedList list = new SortedList();
				DataRowView row;
				foreach (DataRowView tempLoopVar_row in m_DataView)
				{
					row = tempLoopVar_row;
					// Increment count for this item, set to 0 if first time.
					string key = row[m_DataMember].ToString();
					
					// Special case, check for percent values and
					// treat as numbers so they sort correctly.
					if (key.EndsWith("%"))
					{
						// Convert percent string to number and that as the key.
						double keyNum = System.Convert.ToDouble(key.Replace("%", ""));
						object o = list[keyNum];
						int count = System.Convert.ToInt32(Interaction.IIf(o == null, 0, System.Convert.ToInt32(o)));
						list[keyNum] = System.Convert.ToInt32(count + 1);
					}
					else
					{
						// All other text.
						object o = list[key];
						int count = System.Convert.ToInt32(Interaction.IIf(o == null, 0, System.Convert.ToInt32(o)));
						list[key] = System.Convert.ToInt32(count + 1);
					}
				}
				
				// Now go through list and add items to chart.
				int colorIndex = System.Convert.ToInt32(Interaction.IIf(this.PieChart, 0, 3));
				IDictionaryEnumerator enumerator = list.GetEnumerator();
				while (enumerator.MoveNext())
				{
					this.Add(enumerator.Key.ToString(), System.Convert.ToInt32(enumerator.Value), m_ChartColors.GetColor(colorIndex));
					colorIndex = colorIndex + 1;
				}
			}
			catch
			{
			}
			
			// Done updating chart.
			m_ListDirty = true;
			Invalidate();
		}
		
		// Calculate size / position of chart and bitmap objects used when drawing chart.
		private void LayoutControls ()
		{
			// Get bounding rectangle of chart area.
			Rectangle rc = m_Drawing.GetChartRectangle(this.Width, this.Height);
			chart.Left = rc.Left;
			chart.Top = rc.Top;
			chart.Width = rc.Width;
			chart.Height = rc.Height;
			
			// Create bitmaps to draw chart. There are two bitmaps; one that
			// is displayed and another that is used for hit testing.
			if (chart.Width > 0 & chart.Height > 0)
			{
				chart.Image = new Bitmap(chart.Width, chart.Height);
				m_HitTestImage = new Bitmap(chart.Width, chart.Height);
			}
		}
		
		// Return the chart item that is beneath the cursor, -1 if none.
		// Use colors on memory bitmap for hit testing. That allows
		// precise hit testing for pie shapes.
		private int HitTest()
		{
			// Get cursor position relative to the chart.
			Point pt = chart.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
			Color Color = m_HitTestImage.GetPixel(pt.X, pt.Y);
			
			// Loop through each item and see if matching color.
			int Index = 0;
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in m_List)
			{
				item = tempLoopVar_item;
				if (item.Color.ToArgb() == Color.ToArgb())
				{
					// Found chart item.
					return Index;
				}
				Index += 1;
			}
			
			// Did not find match.
			return - 1;
		}
		
		// Set the mouse over state for chart items if they have the specified color.
		private void UpdateOver (Color color)
		{
			// Go through each item and see if it matches the specified color.
			ChartItem item;
			foreach (ChartItem tempLoopVar_item in m_List)
			{
				item = tempLoopVar_item;
				item.IsOver = (bool)Interaction.IIf(item.Color.ToArgb() == color.ToArgb(), true, false);
			}
		}
		
	}
	
}
