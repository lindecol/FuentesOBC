using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Design;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Desktop.Windows.Forms;



namespace Desktop.Windows.Forms
{
	[Designer(typeof(XPanderListDesigner)), DesignTimeVisibleAttribute(true)]
	public class XPanderList : System.Windows.Forms.UserControl
	{
		
		#region "Constants"
		private const int c_InitialVerticalSpacing = 10;
		private const int c_HorizontalSpacing = 8;
		private const int c_VerticalSpacing = 14; // Y gap between XPander controls
		#endregion
		
		private XPanderComparer m_XPanderComparer = null;
		private SortedList m_ControlList;
		private int m_NextControlKey;
		private Brush m_brushBackground;
		
		#region " Windows Form Designer generated code "
		
		public XPanderList() {
			BackColor = Color.FromArgb(99, 117, 222);

			m_ControlList = new SortedList(m_XPanderComparer);
			m_NextControlKey = 0;
			
			
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			
			//Add any initialization after the InitializeComponent() call
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			// Create GDI Objects
			CreateBackBrush();
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
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			//
			//XPanderList
			//
			this.AutoScroll = true;
			base.Load += new System.EventHandler(XPanderList_Load);
			base.SizeChanged += new System.EventHandler(XPanderList_SizeChanged);
			this.Name = "XPanderList";
			
		}
		
		#endregion
		
		// Public Properties
		
		
		private void XPanderList_Load (object sender, System.EventArgs e)
		{
			m_NextControlKey = m_ControlList.Count;
		}
		
		private int FixRGB(int RGBValue)
		{
			if (RGBValue >= 0 & RGBValue <= 255)
			{
				return RGBValue;
			}
			else if (RGBValue < 0)
			{
				return 0;
			}
			else
			{
				return 255;
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			CreateBackBrush();
		}


		private void CreateBackBrush ()
		{
			// can only create brushes when have a width and height
			if (this.Width > 0 && this.Height > 0)
			{
				if ( m_brushBackground != null )
					m_brushBackground.Dispose();
				m_brushBackground = new LinearGradientBrush(this.DisplayRectangle, 
					CustomBorderColor.ColorLightLight(BackColor), BackColor, 
					LinearGradientMode.Vertical);
			}
		}
		
		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			// Draw gradient background
			Rectangle rc = new Rectangle(0, 0, this.Width, this.Height);
			e.Graphics.FillRectangle(m_brushBackground, rc);
		}
		
		private void XPanderList_SizeChanged (object sender, System.EventArgs e)
		{
			Invalidate();
		}
		
		private int GetNextTopPosition()
		{
			Control ctl;
			int max = c_InitialVerticalSpacing;
			int YPos = 0;
			
			// The next top position is the highest top value + that controls height, with a
			// little vertical spacing thrown in for good measure
			IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ctl = ((Control)(enumerator.Value));
				YPos = ctl.Top + ctl.Height;
				if (YPos > max)
				{
					max = YPos;
				}
			}
			
			if (max != c_InitialVerticalSpacing)
			{
				max += c_VerticalSpacing;
			}
			
			return max;
		}
		
		protected override void OnControlAdded (System.Windows.Forms.ControlEventArgs e)
		{
			base.OnControlAdded(e);
			
			// We'll only keep track of XPander controls in our list.
			if (e.Control.GetType() == typeof(XPander))
			{
				
				if (e.Control.Width <= this.Width)
				{
					e.Control.Left = c_HorizontalSpacing;
					e.Control.Width = this.Width - 2 * c_HorizontalSpacing;
					e.Control.Top = GetNextTopPosition();
					e.Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
				}
				
				// Add a handler so we know when the control is expanded and collapsed
				XPander x = ((XPander)(e.Control));
				x.XPanderCollapsed += new  XPander.XPanderCollapsedHandler(ControlCollapsed);
				x.XPanderExpanded += new XPander.XPanderExpandedHandler(ControlExpanded);
				
				// Add the control to our control list so we can keep track of it
				// Store the key of the control in it's tag property so we can reference it
				// later when we remove it
				e.Control.Tag = m_NextControlKey;
				m_NextControlKey += 1;
				m_ControlList.Add(e.Control.Tag, e.Control);
			}
		}
		
		protected override void OnControlRemoved (System.Windows.Forms.ControlEventArgs e)
		{
			Control ctl;
			int prevTop = e.Control.Top;
			int newTop = 0;
			
			if (e.Control.GetType() == typeof(XPander))
			{
				IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ctl = ((Control)(enumerator.Value));
					if (ctl.Top > prevTop)
					{
						newTop = prevTop;
						prevTop = ctl.Top;
						ctl.Top = newTop;
					}
				}
				
				XPander x = ((XPander)(e.Control));
				
				// Remove the custom event handlers for this control
				x.XPanderCollapsed -= new XPander.XPanderCollapsedHandler(ControlCollapsed);
				x.XPanderExpanded -= new XPander.XPanderExpandedHandler(ControlExpanded);
				
				// Remove the control from the list
				m_ControlList.Remove(e.Control.Tag);
			}
		}
		
		public void ControlExpanding (XPander x, int delta)
		{
			Control ctl;
			
			IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ctl = ((Control)(enumerator.Value));
				if (ctl.Top > x.Top)
				{
					ctl.Top += delta;
				}
			}
		}
		
		public void ControlCollapsing (XPander x, int delta)
		{
			Control ctl;
			
			IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ctl = ((Control)(enumerator.Value));
				if (ctl.Top > x.Top)
				{
					ctl.Top -= delta;
				}
			}
		}
		
		public void ControlExpanded (XPander x)
		{
			Control ctl;
			
			IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ctl = ((Control)(enumerator.Value));
				if (ctl.Top > x.Top)
				{
					ctl.Top += x.ExpandedHeight - x.CaptionHeight;
				}
			}
		}
		
		public void ControlCollapsed (XPander x)
		{
			Control ctl;
			
			IDictionaryEnumerator enumerator = m_ControlList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ctl = ((Control)(enumerator.Value));
				if (ctl.Top > x.Top)
				{
					ctl.Top -= x.ExpandedHeight - x.CaptionHeight;
				}
			}
		}
	}
	
	public class XPanderComparer : IComparer
	{
		
		// If x < y, a -1 is returned
		// If x = y, 0 is returned
		// If x > y, 1 is returned
		public int Compare(object x, object y)
		{
			XPander xp1 = ((XPander)(x));
			XPander xp2 = ((XPander)(y));
			int result = 0;
			
			if (xp1.Top < xp2.Top)
			result = - 1;
			if (xp1.Top > xp2.Top)
			result = 1;
			
			return result;
		}
	}
}
