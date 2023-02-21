using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Desktop.Windows.Forms
{
	public class XPanderDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		
		private Pen m_BorderPen;
		private XPander m_Control;
		
		public XPanderDesigner() {
			m_BorderPen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark));
			
			m_BorderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
		}
		
		public override void Initialize (System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
			m_Control = ((XPander)(this.Control));
		}
		
		protected override void OnPaintAdornments (PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			pe.Graphics.DrawRectangle(m_BorderPen, 0, 0, m_Control.Width - 2, m_Control.Height - 2);
			m_Control.ExpandedHeight = m_Control.Height;
		}
	}
	
}
