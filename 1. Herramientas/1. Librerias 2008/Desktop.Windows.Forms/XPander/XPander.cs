using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;




namespace Desktop.Windows.Forms
{
	[Designer(typeof(XPanderDesigner)), DesignTimeVisible(true)]
	public class XPander : System.Windows.Forms.UserControl
	{
		
		#region " Constants "
		private const int c_ExpandCollapseHeight = 1;
		private const int c_ExpandCollapseAlpha = 2;
		#endregion
		
		private int m_CaptionHeight;
		private string m_CaptionText;
		private Color m_CaptionTextColor;
		private Color m_CaptionTextHighlightColor;
		private Font m_CaptionFont;
		private Color m_CaptionColor;
		private bool m_Expanded;
		//private bool m_ExpandedAtLoad = false;
		private bool m_IsCaptionHighlighted;
		private int m_Height;
		
		private Bitmap[] m_Images;
		
		#region " Custom Events "

		// Repeatedly fired as the XPander is being collapsed
		public delegate void XPanderCollapsingHandler(XPander x, int delta);
		private XPanderCollapsingHandler XPanderCollapsingEvent;
		public event XPanderCollapsingHandler XPanderCollapsing
		{
			add
			{
				XPanderCollapsingEvent = (XPander.XPanderCollapsingHandler) System.Delegate.Combine(XPanderCollapsingEvent, value);
			}
			remove
			{
				XPanderCollapsingEvent = (XPander.XPanderCollapsingHandler) System.Delegate.Remove(XPanderCollapsingEvent, value);
			}
		}
		
		
		// Fired when the XPander is completely collapsed
		public delegate void XPanderCollapsedHandler(XPander x);
		private XPanderCollapsedHandler XPanderCollapsedEvent;
		public event XPanderCollapsedHandler XPanderCollapsed
		{
			add
			{
				XPanderCollapsedEvent = (XPanderCollapsedHandler) System.Delegate.Combine(XPanderCollapsedEvent, value);
			}
			remove
			{
				XPanderCollapsedEvent = (XPanderCollapsedHandler) System.Delegate.Remove(XPanderCollapsedEvent, value);
			}
		}
		
		
		delegate void XPanderExpandingHandler(XPander x, int delta);
		private XPanderCollapsingHandler XPanderExpandingEvent;
		public event XPanderCollapsingHandler XPanderExpanding
		{
			add
			{
				XPanderExpandingEvent = (XPanderCollapsingHandler) System.Delegate.Combine(XPanderExpandingEvent, value);
			}
			remove
			{
				XPanderExpandingEvent = (XPanderCollapsingHandler) System.Delegate.Remove(XPanderExpandingEvent, value);
			}
		}
		
		
		public delegate void XPanderExpandedHandler(XPander x);
		private XPanderExpandedHandler XPanderExpandedEvent;
		public event XPanderExpandedHandler XPanderExpanded
		{
			add
			{
				XPanderExpandedEvent = (XPanderExpandedHandler) System.Delegate.Combine(XPanderExpandedEvent, value);
			}
			remove
			{
				XPanderExpandedEvent = (XPanderExpandedHandler) System.Delegate.Remove(XPanderExpandedEvent, value);
			}
		}
		
		#endregion
		
		#region " Windows Form Designer generated code "
		
		public XPander() {
			m_CaptionHeight = 25;
			m_CaptionText = "";
			m_CaptionTextColor = Color.FromArgb(33, 93, 198);
			m_CaptionTextHighlightColor = Color.FromArgb(66, 142, 255);
			m_CaptionFont = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
			m_CaptionColor = Color.FromArgb(198, 210, 248);
			m_Expanded = true;
			//m_ExpandedAtLoad = true;
			m_IsCaptionHighlighted = false;
			m_Images = new Bitmap[5];
			
			
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			
			//Add any initialization after the InitializeComponent() call
			// We draw everything, and repaint when resized.
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ContainerControl, true);
			
			this.BackColor = Color.FromArgb(214, 223, 247); // Default
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
			//XPander
			//
			this.Name = "XPander";
			base.Load += new System.EventHandler(XPander_Load);
			this.Size = new System.Drawing.Size(8, 136);
			
		}
		
		#endregion
		
		#region " Public Properties "
		// properties
		[Description("Height of caption."), DefaultValue(25), Category("Appearance")]public int CaptionHeight
		{
			get{
				return m_CaptionHeight;
			}
			
			set
			{
				m_CaptionHeight = value;
				Invalidate();
			}
		}
		
		[Description("Caption text."), DefaultValue(""), Category("Appearance"), Localizable(true)]public string CaptionText
		{
			get{
				return m_CaptionText;
			}
			
			set
			{
				m_CaptionText = value;
				Invalidate();
			}
		}
		
		[Description("Caption color."), DefaultValue("180,190,240"), Category("Appearance")]public Color CaptionColor
		{
			get{
				return m_CaptionColor;
			}
			
			set
			{
				m_CaptionColor = value;
				Invalidate();
			}
		}
		
		[Description("Caption text color."), DefaultValue("33,93,198"), Category("Appearance")]public Color CaptionTextColor
		{
			get{
				return m_CaptionTextColor;
			}
			
			set
			{
				m_CaptionTextColor = value;
				Invalidate();
			}
		}
		
		[Description("Caption text color when the mouse is hovering over it."), DefaultValue("66, 142, 255"), Category("Appearance")]public Color CaptionTextHighlightColor
		{
			get{
				return m_CaptionTextHighlightColor;
			}
			
			set
			{
				m_CaptionTextHighlightColor = value;
				Invalidate();
			}
		}
		
		[Description("Caption font."), Category("Appearance")]public Font CaptionFont
		{
			get{
				return m_CaptionFont;
			}
			
			set
			{
				m_CaptionFont = value;
				Invalidate();
			}
		}
		
		//<Description("Expanded state of control at load time."),     ' DefaultValue("True"),     ' DesignOnly(True),     ' Category("Appearance")>     'Public Property ExpandedAtLoad() As Boolean
		//    Get
		//        Return m_ExpandedAtLoad
		//    End Get
		//    Set(ByVal Value As Boolean)
		//        m_ExpandedAtLoad = Value
		//        ChangeHeight()
		//        'Invalidate()
		//    End Set
		//End Property
		
		[Browsable(false), DesignOnly(true)]public int ExpandedHeight
		{
			get{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}
		#endregion
		
		private void XPander_Load (object sender, System.EventArgs e)
		{
			// Load images from the .dll
			m_Images[1] = GetBitmapFromResource("Images.Collapse.jpg");
            m_Images[2] = GetBitmapFromResource("Images.Collapse_h.jpg");
            m_Images[3] = GetBitmapFromResource("Images.Expand.jpg");
            m_Images[4] = GetBitmapFromResource("Images.Expand_h.jpg");
			this.DockPadding.Top = m_CaptionHeight;
		}

        private Bitmap GetBitmapFromResource(string sResourceName)
        {
            Stream s = GetResourceStream(sResourceName);
            Bitmap bmp = new Bitmap(s);
            s.Close();
            return bmp;
        }

        private Stream GetResourceStream(string sResourceName)
        {
            return Assembly.GetCallingAssembly().GetManifestResourceStream(this.GetType().Namespace + "." + sResourceName);
        }
		
		protected override void OnPaint (PaintEventArgs e)
		{
			Rectangle rc = new Rectangle(0, 0, this.Width, CaptionHeight);
			LinearGradientBrush b = new LinearGradientBrush(rc, Color.White, CaptionColor, LinearGradientMode.Horizontal);
			Size size = e.Graphics.MeasureString(CaptionText, CaptionFont).ToSize();
			
			// Now draw the caption areas with the rounded corners at the top
			e.Graphics.FillRectangle(b, rc);
			
			// Draw the outline around the work area
			if (this.Height > m_CaptionHeight)
			{
				e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.HighlightText)), 0, this.CaptionHeight, 0, this.Height);
				e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.HighlightText)), this.Width - 1, this.CaptionHeight, this.Width - 1, this.Height);
				e.Graphics.DrawLine(new Pen(Color.FromKnownColor(KnownColor.HighlightText)), 0, this.Height - 1, this.Width - 1, this.Height - 1);
			}
			
			// Caption text.
			if (m_IsCaptionHighlighted)
			{
				e.Graphics.DrawString(CaptionText, CaptionFont, new SolidBrush(m_CaptionTextHighlightColor), System.Convert.ToSingle(10), System.Convert.ToSingle((this.CaptionHeight - 2 - size.Height) / 2));
			}
			else
			{
				e.Graphics.DrawString(CaptionText, CaptionFont, new SolidBrush(m_CaptionTextColor), System.Convert.ToSingle(10), System.Convert.ToSingle((this.CaptionHeight - 2 - size.Height) / 2));
			}
			
			// Expand / Collapse Icon
			if (m_Expanded)
			{
				if (m_IsCaptionHighlighted)
				{
					e.Graphics.DrawImage(m_Images[2], this.Width - m_Images[1].Width - 8, 4);
				}
				else
				{
					e.Graphics.DrawImage(m_Images[1], this.Width - m_Images[1].Width - 8, 4);
				}
			}
			else
			{
				if (m_IsCaptionHighlighted)
				{
					e.Graphics.DrawImage(m_Images[4], this.Width - m_Images[2].Width - 8, 3);
				}
				else
				{
					e.Graphics.DrawImage(m_Images[3], this.Width - m_Images[2].Width - 8, 3);
				}
			}
		}
		
		protected override void OnMouseMove (System.Windows.Forms.MouseEventArgs e)
		{
			// Change cursor to hand when over caption area.
			if (e.Y <= this.CaptionHeight)
			{
				Cursor.Current = Cursors.Hand;
				m_IsCaptionHighlighted = true;
			}
			else
			{
				Cursor.Current = Cursors.Default;
				m_IsCaptionHighlighted = false;
			}
			Invalidate();
		}
		
		protected override void OnMouseDown (System.Windows.Forms.MouseEventArgs e)
		{
			
			//			int delta;
			//			Control ctl;
			
			// Don't do anything if did not click on caption.
			if (e.Y > this.CaptionHeight)
			{
				return;
			}
			
			// Toggle expanded flag.
			m_Expanded = ! m_Expanded;
			
			// Expand or collapse the control based on its current state
			ChangeHeight();
			this.Refresh();
		}
		
		protected override void OnMouseLeave (System.EventArgs e)
		{
			if (m_IsCaptionHighlighted)
			{
				m_IsCaptionHighlighted = false;
				Invalidate();
			}
		}
		
		private void ChangeHeight ()
		{
			if (! m_Expanded)
			{
				// Remember height so we can restore it later.
				m_Height = this.Height;
				
				// Set the new height and let others know we have been collapsed
				this.Height = CaptionHeight;
				if (XPanderCollapsedEvent != null)
				XPanderCollapsedEvent(this);
			}
			else
			{
				this.Height = m_Height;
				if (XPanderExpandedEvent != null)
				XPanderExpandedEvent(this);
			}
		}
		
		public void Expand ()
		{
			if (! m_Expanded)
			{
				m_Expanded = true;
				ChangeHeight();
			}
		}
		
		public void Collapse ()
		{
			if (m_Expanded)
			{
				m_Expanded = false;
				ChangeHeight();
			}
		}
		
		public object IsExpanded
		{
			get{
				return m_Expanded;
			}
		}
	}
	
}
