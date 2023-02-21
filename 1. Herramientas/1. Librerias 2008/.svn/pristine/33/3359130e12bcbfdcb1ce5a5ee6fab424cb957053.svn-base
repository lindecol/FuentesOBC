using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Desktop.Windows.Forms
{
	public class SectionHeader : System.Windows.Forms.Control
	{
		
		#region " Windows Form Designer generated code "
		
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
			this.Name = "SectionHeader";
		}
		
		#endregion

		#region " Constantes, Variables y Constructor " 
		
		// Valores por defecto
		private class Consts
		{
			public const int PosOffset = 4;
		}
		
		// Objetos GDI utilizados
		private Pen m_PenTop;
		private Pen m_PenBottom;
		private SolidBrush m_brushText;
		private LinearGradientBrush m_brushBackground;


		// Constructor 
		public SectionHeader() 
		{
						// Llamada requerida por el diseñador
			InitializeComponent();
			
			// set double buffer styles
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

			this.BackColor = Color.FromArgb(175, 200, 245);
			this.ForeColor = Color.Black;

			// se crean los objetos GDI utilizados
			CreateBackBrush();
			CreateTextBrush();	
		}

		#endregion
		
		private void CreateBackBrush()
		{
			// can only create brushes when have a width and height
			if (this.Width > 0 && this.Height > 0)
			{
				if ( m_brushBackground != null )
					m_brushBackground.Dispose();
				m_brushBackground = new LinearGradientBrush(this.DisplayRectangle, 
					CustomBorderColor.ColorLight(BackColor), BackColor,  
					LinearGradientMode.Vertical);

				if ( m_PenTop != null )
					m_PenTop.Dispose();
                m_PenTop = new Pen(CustomBorderColor.ColorLight(BackColor));

				if ( m_PenBottom  != null )
					m_PenBottom.Dispose();
                //m_PenBottom = new Pen(Drawing.CustomBorderColor.ColorDark(BackColor));
				m_PenBottom = new Pen(SystemColors.InactiveCaption);
			}
		}

		private void CreateTextBrush()
		{
			if ( m_brushText != null )
				m_brushText.Dispose();
			m_brushText = new SolidBrush(this.ForeColor);
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 20);
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			Invalidate();
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			Draw(e.Graphics);
			base.OnPaint(e);
		}

		protected override void OnSizeChanged (System.EventArgs e)
		{
			base.OnSizeChanged(e);
			CreateBackBrush();
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
		
		// draw the caption
		private void Draw (Graphics g)
		{
			if (this.Width == 0 || this.Height == 0)
				return;

				
			// background
			g.FillRectangle(m_brushBackground, this.DisplayRectangle);
			
			// Texto
			g.DrawString(this.Text, this.Font, m_brushText, Consts.PosOffset,(this.Height - this.Font.Height) / 2);
			
			// top and bottom lines
			g.DrawLine(m_PenTop, 0, 0, this.Width - 1, 0);
			g.DrawLine(m_PenBottom, 0, this.Height - 1, this.Width - 1, this.Height - 1);
			
		}

		protected  override void OnResize(EventArgs e)
		{
			base.OnResize (e);
		}
	}
	
}
