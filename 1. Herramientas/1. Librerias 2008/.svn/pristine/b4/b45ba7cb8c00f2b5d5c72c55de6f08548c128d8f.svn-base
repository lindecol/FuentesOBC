using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;


// An instance of this class is created by the designer for the XPanderList control at
// design time.  That instance controls the behavior of the control at design-time.
namespace Desktop.Windows.Forms
{
	public class XPanderListDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		
		private Pen _borderPen;
		private XPanderList _control;
		
		public XPanderListDesigner() {
			_borderPen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark));
			
			_borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
		}
		
		public override void Initialize (System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
			_control = ((XPanderList)(this.Control));
			
			// Disable the autoscroll feature for the control during design time.  The control
			// itself sets this property to true when it initializes at run time.  Trying to position
			// controls in this control with the autoscroll property set to True is problematic.
			_control.AutoScroll = false;
		}
		
		protected override void OnPaintAdornments (PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			pe.Graphics.DrawRectangle(_borderPen, 0, 0, _control.Width - 2, _control.Height - 2);
		}
	}
	
}
