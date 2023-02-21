using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.ComponentModel;

// Custom control that draws the caption for each pane. Contains an active
// state and draws the caption different for each state. Caption is drawn
// with a gradient fill and antialias font.


namespace Desktop.Windows.Forms
{
	public class PaneCaption : System.Windows.Forms.Control
	{

		#region " Windows Form Designer generated code "
		
		//UserControl overrides dispose to clean up the component list.
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (components != null)
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
			//PaneCaption
			//
			this.Name = "SecctionHeader";
			this.Size = new System.Drawing.Size(150, 30);
		}
		
		#endregion
		
		// const values
		private class Consts
		{
			public const string DefaultFontName = "Tahoma";
			public const int DefaultFontSize = 12;
			public const int PosOffset = 4;
		}
		
		// internal members
		private bool m_antiAlias;
		
		// gdi objects
		private SolidBrush m_brushText;
		private LinearGradientBrush m_brushBackground;
		private StringFormat m_format;
		
		
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 30);
			}
		}
	
		
		// Constructor
		public PaneCaption() {
			m_antiAlias = true;
			
			// this call is required by the Windows Form Designer
			InitializeComponent();
			
			// set double buffer styles
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
			
			// format used when drawing the text
			m_format = new StringFormat();
			m_format.FormatFlags = StringFormatFlags.NoWrap;
			m_format.LineAlignment = StringAlignment.Center;
			m_format.Trimming = StringTrimming.EllipsisCharacter;
			
			// init the font
			this.Font = new Font(Consts.DefaultFontName, Consts.DefaultFontSize, FontStyle.Bold);
			this.BackColor = Color.FromArgb(3, 55, 145);
			this.ForeColor = Color.White;
			
			// create gdi objects
			CreateBackBrush();
			CreateTextBrush();			
		}

		// Dibujar texto con AntiAlias
		[Description("Dibjuar texto con AntiAlias"), Category("Appearance"), 
		DefaultValue(true)]
		public bool AntiAlias
		{
			get
			{
				return m_antiAlias;
			}
			set
			{
				m_antiAlias = value;
				Invalidate();
			}
		}		
		
		protected override void OnSizeChanged (System.EventArgs e)
		{
			base.OnSizeChanged(e);
			CreateBackBrush();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			Invalidate();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged (e);
			Invalidate();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged (e);
			CreateTextBrush();
			Invalidate();
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged (e);
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

		private void CreateTextBrush()
		{
			if ( m_brushText != null )
				m_brushText.Dispose();
			m_brushText = new SolidBrush(ForeColor);
		}	
		
		
		// the caption needs to be drawn
		protected override void OnPaint (PaintEventArgs e)
		{
			Draw(e.Graphics);
			base.OnPaint(e);
		}


		// draw the caption
		private void Draw(Graphics g)
		{
			// background
			g.FillRectangle(this.m_brushBackground, this.DisplayRectangle);
			
			// caption
			if (m_antiAlias)
			{
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			}
			
			// need a rectangle when want to use ellipsis
			RectangleF bounds = new RectangleF(Consts.PosOffset, 0, this.DisplayRectangle.Width - Consts.PosOffset, this.DisplayRectangle.Height);
			g.DrawString(Text, Font, m_brushText, bounds, m_format);
		}

		
		
	}
	
}
