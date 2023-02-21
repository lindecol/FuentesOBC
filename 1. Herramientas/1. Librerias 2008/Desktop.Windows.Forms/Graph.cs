using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Desktop.Windows.Forms
{
	public class Graph : System.Windows.Forms.Control 
	{

		// Required designer variable.
		private System.ComponentModel.Container components = null;
		private Pen m_axisPen;
		private Brush m_blackBrush;
		private Font m_normalFont;
		private Pen m_gridPen;

		public Graph()
		{
			//Create dummy data for design-time
			m_data = new ArrayList();
			m_data.Add(new System.Drawing.Point(0, 80));
			m_data.Add(new System.Drawing.Point(1, 20));
			m_data.Add(new System.Drawing.Point(2, 0));
			m_data.Add(new System.Drawing.Point(3, 40));
			m_data.Add(new System.Drawing.Point(4, 25));

			m_axisPen = new System.Drawing.Pen(Color.FromArgb(0,0,0));
			m_graphPen = new System.Drawing.Pen(Color.FromArgb(0, 0, 0));
			m_gridPen = new System.Drawing.Pen(Color.FromArgb(127, 127, 127));
			m_blackBrush = new System.Drawing.SolidBrush(Color.FromArgb(0,0,0));
			m_normalFont = new Font("Arial", 8, FontStyle.Regular);

			// This call is required by the Windows.Forms Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		private int m_maxYValue = 100;
		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.DefaultValueAttribute(100)]
		[System.ComponentModel.Description("The maximum Y value in the graph.")]
		#endif
		//The lower Y bound of the graph
		public int MaximumY
		{
			get
			{
				return m_maxYValue;
			}
			set
			{
				m_maxYValue = value;
				this.Invalidate();
			}
		}
		private int m_maxXValue = 10;
		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.DefaultValueAttribute(10)]
		[System.ComponentModel.Description("The maximum X value in the graph.")]
		#endif
		
		//The Upper X bound of the graph
		public int MaximumX
		{
			get
			{
				return m_maxXValue;
			}
			set
			{
				m_maxXValue = value;
				this.Invalidate();
			}
		}

		private Pen m_graphPen;
		#if NETCFDESIGNTIME
		// These design-time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.Description("The Color of the Axis in the graph.")]
		#endif
		
		//The color of the line of the graph.
		public Color GraphLineColor
		{
			get
			{
				return m_graphPen.Color;
			}
			set
			{
				m_graphPen = new Pen(value);
				this.Invalidate();
			}
		}

		//The designer will generate code to access the collection from the resx file.  We don't want that so we don't build the Data property
		//at design-time.
		#if !NETCFDESIGNTIME
		//The actual Data used to draw the line on the graph
		public ICollection Data
		{
			get
			{
				return m_data;
			}
			set
			{
				m_data = new ArrayList(value);
				Rectangle rcClient = this.ClientRectangle;
				Rectangle rcGraphClient = new Rectangle(rcClient.X + 21, rcClient.Y + 5, rcClient.Width - 21, rcClient.Height - 21);
				this.Invalidate(rcGraphClient);
			}
		}
		#endif

		private bool m_showXScale = true;
		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.DefaultValueAttribute(true)]
		[System.ComponentModel.Description("Indicates whether the X scale will be drawn with the graph.")]
		#endif
		
		// If true, shows the X-Values on the bottom of the graph
		public bool ShowXScale
		{
			get
			{
				return m_showXScale;
			}
			set
			{
				m_showXScale = value;
				this.Invalidate();
			}
		}

		private bool m_showYScale = true;
		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.DefaultValueAttribute(true)]
		[System.ComponentModel.Description("Indicates whether the Y scale will be drawn with the graph.")]
		#endif
		
		// If true, shows the Y-Values on the left of the graph
		public bool ShowYScale
		{
			get
			{
				return m_showYScale;
			}
			set
			{
				m_showYScale = value;
				this.Invalidate();
			}
		}

		private bool m_showGrid = true;
		#if NETCFDESIGNTIME
		// These design time attributes affect appearance of this property in the property grid.
		[System.ComponentModel.Category("Graph")]
		[System.ComponentModel.DefaultValueAttribute(false)]
		[System.ComponentModel.Description("Indicates whether the grid should be drawn on the graph.")]
		#endif
																			
		// If true, shows horiztonal grid lines on the graph.
		public bool ShowGrid
		{
			get
			{
				return m_showGrid;
			}
			set
			{
				m_showGrid = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the Code Editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
		}
		#endregion

		private ArrayList m_data;

		protected override void OnResize(EventArgs e)
		{
			this.Refresh();
		}

		//This Paint function uses routines common to both platforms.
		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rcClient = this.ClientRectangle;
			Rectangle rcGraphClient = new Rectangle(rcClient.X + 20, rcClient.Y + 5, rcClient.Width - 20, rcClient.Height - 20);

			//Check if graph is too small
			if ((rcGraphClient.Width < 20) || (rcGraphClient.Height < 20))
			{
				return;
			}

			//Calculate how many numbers we want on the axis
			int numPointsOnY;
			int incrementOnY;
			int numPointsOnX;
			int incrementOnX;
			int yPixelIncrement = 0;
			int xPixelIncrement = 0;
			float yPixelsPerPoint = 0f;
			float xPixelsPerPoint = 0f;
			float fTemp1;
			float fTemp2;

			numPointsOnY = (rcGraphClient.Height / 20) + 1;
			incrementOnY = m_maxYValue / (numPointsOnY - 1);
			numPointsOnX = (rcGraphClient.Width / 20) + 1;
			incrementOnX = m_maxXValue / (numPointsOnX - 1);
			fTemp1 = rcGraphClient.Width;
			fTemp2 = m_maxXValue;
			xPixelsPerPoint = fTemp1/fTemp2;
			fTemp1 = rcGraphClient.Height;
			fTemp2 = m_maxYValue;
			yPixelsPerPoint = fTemp1/fTemp2;

			if (numPointsOnX > m_maxXValue)
			{
				incrementOnX = 1;
				numPointsOnX = m_maxXValue;
				xPixelIncrement = rcGraphClient.Width / m_maxXValue;
			}
			else
			{
				xPixelIncrement = 20;
			}
			if (numPointsOnY > m_maxYValue)
			{
				incrementOnY = 1;
				numPointsOnY = m_maxYValue;
				yPixelIncrement = rcGraphClient.Height / m_maxYValue;
			}
			else
			{
				yPixelIncrement = 20;
			}

			//Draw the grid
			if (m_showGrid)
			{
				//Draw the Y scale
				int yCoord = rcGraphClient.Y + rcGraphClient.Height - 20;

				for (int yScaleValue = incrementOnY + 1; yScaleValue < this.m_maxYValue; yScaleValue+= incrementOnY, yCoord -= yPixelIncrement)
				{
					e.Graphics.DrawLine(m_gridPen, rcGraphClient.X, yCoord, rcGraphClient.X + rcGraphClient.Width, yCoord);
				}
				e.Graphics.DrawLine(m_gridPen, rcGraphClient.X, yCoord, rcGraphClient.X + rcGraphClient.Width, yCoord);
			}

			//draw the actual line
			Point pLastPoint = new Point(0, 0);
			foreach(Object o in m_data)
			{
				Point p = (Point)o;
				if ((p.X >= 0) && (p.X <= this.m_maxXValue) && (p.Y >= 0) && (p.Y <= this.m_maxYValue))
				{
					p.X = (int)(20 + (p.X * xPixelsPerPoint));
					p.Y = (int)(rcGraphClient.Y + rcGraphClient.Height - (p.Y * yPixelsPerPoint));
					if (pLastPoint.X == 0)
					{
						pLastPoint = p;
					}
					e.Graphics.DrawLine(m_graphPen, pLastPoint.X, pLastPoint.Y, p.X, p.Y);
					e.Graphics.DrawLine(m_graphPen, pLastPoint.X, pLastPoint.Y - 1, p.X, p.Y - 1);
					e.Graphics.DrawLine(m_graphPen, pLastPoint.X, pLastPoint.Y + 1, p.X, p.Y + 1);
					pLastPoint = p;
				}
			}

			//Draw the axis
			e.Graphics.DrawLine(m_axisPen, rcGraphClient.X, rcGraphClient.Y , rcGraphClient.X, rcGraphClient.Y + rcGraphClient.Height );
			e.Graphics.DrawLine(m_axisPen, rcGraphClient.X , rcGraphClient.Y + rcGraphClient.Height , rcGraphClient.X + rcGraphClient.Width, rcGraphClient.Y + rcGraphClient.Height );

			if (m_showYScale)
			{
				//Draw the Y scale
				int yCoord = rcGraphClient.Y + rcGraphClient.Height;

				e.Graphics.DrawString("0", m_normalFont,m_blackBrush , rcClient.X , yCoord);
				yCoord -=20;

				for (int yScaleValue = incrementOnY + 1; yScaleValue < this.m_maxYValue; yScaleValue+= incrementOnY, yCoord -= yPixelIncrement)
				{
					e.Graphics.DrawString(yScaleValue.ToString(), m_normalFont, m_blackBrush, rcClient.X , yCoord);            
				}
				e.Graphics.DrawString(m_maxYValue.ToString(), m_normalFont, m_blackBrush, rcClient.X , rcGraphClient.Y);
			}

			if (m_showXScale)
			{
				//Draw the X scale
				int xCoord = rcGraphClient.X + 20;

				for (int xScaleValue = incrementOnX; xScaleValue < this.m_maxXValue; xScaleValue+= incrementOnX, xCoord += xPixelIncrement)
				{
					e.Graphics.DrawString(xScaleValue.ToString(), m_normalFont, m_blackBrush, xCoord -3, rcClient.Y + rcClient.Height - 12);            
				}
				e.Graphics.DrawString(m_maxXValue.ToString(), m_normalFont, m_blackBrush, rcClient.X + rcClient.Width - 10, rcClient.Height - 12);
			}
		}
	}
}
