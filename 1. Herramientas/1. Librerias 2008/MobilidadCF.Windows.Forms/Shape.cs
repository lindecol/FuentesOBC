using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

//******************************************************************************
// DSShape
//******************************************************************************

namespace MovilidadCF.Windows.Forms
{
	
	
	public class Shape : System.Windows.Forms.Control {

        #region Constructor, Dispose y Código Requerido por el diseñador
		
		// Required designer variable.
		private System.ComponentModel.Container components = null;
		
		public Shape() {
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the Code Editor.
		/// </summary>
		private void InitializeComponent() 
		{
			Enabled = false;
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

		#endregion

		#region Propiedades del control
		
		public enum ShapeStyle
		{
			Rectangle,
			TopLine,
			BottomLine,
			LeftLine,
			RightLine,
			GroupSeparator,
			TitleBox,
		}
		
		// Shape Type
		private ShapeStyle m_Style;

        /// <summary>
        /// Define el estilo del forma que se dibujara
        /// </summary>
		public ShapeStyle Style{
			get {
				return m_Style;
			}
			set {
				m_Style = value;
				this.Invalidate();
			}
		}		

		#endregion

		#region Implementacion funcionalidad del control		
		protected override void OnResize(EventArgs e) 
		{
			base.OnResize(e);
			this.Invalidate();
		}

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle r = this.ClientRectangle;
			int xIni, yIni, xFin, yFin;
			SizeF size;
			Pen pen = new Pen(ForeColor);
            Brush brush = new SolidBrush(ForeColor);
            r.Height = r.Height - 1;
			r.Width = r.Width-1;
			switch (Style) 
			{
				case ShapeStyle.Rectangle:
					e.Graphics.DrawRectangle(pen, r);
					r.X++;	r.Y++; r.Width -= 2; r.Height -= 2;
					e.Graphics.DrawRectangle(pen, r);
					break;
				case ShapeStyle.TopLine:
					e.Graphics.DrawLine(pen, r.X, r.Y, r.X+r.Width,	r.Y);
					r.Y++;
					e.Graphics.DrawLine(pen, r.X, r.Y, r.X+r.Width,	r.Y);
					break;
				case ShapeStyle.BottomLine:
					e.Graphics.DrawLine(pen, r.X, r.Y+r.Height, r.X+r.Width,	r.Y+r.Height);
					r.Y--;
					e.Graphics.DrawLine(pen, r.X, r.Y+r.Height, r.X+r.Width,	r.Y+r.Height);
					break;
				case ShapeStyle.LeftLine:
					e.Graphics.DrawLine(pen, r.X, r.Y, r.X,	r.Y + r.Height);
					r.X++;
					e.Graphics.DrawLine(pen, r.X, r.Y, r.X,	r.Y + r.Height);
					break;
				case ShapeStyle.RightLine:
					e.Graphics.DrawLine(pen, r.X +r.Width, r.Y, r.X +r.Width,	r.Y + r.Height);
					r.X--;
					e.Graphics.DrawLine(pen, r.X +r.Width, r.Y, r.X +r.Width,	r.Y + r.Height);
					break;
				case ShapeStyle.GroupSeparator:
					size = e.Graphics.MeasureString(Text, Font);
					r.Height = (int)size.Height;
					e.Graphics.DrawString(Text, Font, brush, r);					
					size.Width = Math.Min(size.Width, r.Width);
					xIni = r.X + (int)size.Width + 2;
					yIni = r.Y + (int)(size.Height/2);
					xFin = r.X + r.Width;
					yFin = yIni;                    					
					e.Graphics.DrawLine(pen, xIni, yIni, xFin, yFin);
					e.Graphics.DrawLine(pen, xIni, yIni+1, xFin, yFin+1);
					break;
				case ShapeStyle.TitleBox:
					e.Graphics.DrawRectangle(pen, r);
					r.X++;	r.Y++; r.Width -= 2; r.Height -= 2;
					e.Graphics.DrawRectangle(pen, r);
					r.X++;	r.Y++; r.Width -= 2; r.Height -= 2;
					size = e.Graphics.MeasureString(Text, Font);
					if ( size.Width < r.Width )
						r.X += (int)(r.Width - size.Width) / 2;
					if ( size.Height < r.Height )
						r.Y += (int)(r.Height - size.Height) /2;
					r.Height = (int)size.Height;
					e.Graphics.DrawString(Text, Font, brush, r);
					break;
			}
			pen.Dispose();
			brush.Dispose();
			base.OnPaint (e);
		}
		#endregion
	}
}
