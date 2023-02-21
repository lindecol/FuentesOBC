using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

// Event argument that is passed to the container. Event is
// sent when the chart is clicked and the argument contains
// the index of the chart item that was clicked.

namespace Desktop.Windows.Forms
{
	public delegate void ClickItemEventHandler(object sender, ClickItemEventArgs e);
	
	public class ClickItemEventArgs : EventArgs
	{
		
		// Internal fields. Chart item that was clicked.
		private int m_SelItem;
		
		// Constructor.
		public ClickItemEventArgs(int item) {
			SelectedItem = item;
		}
		
		// Properties.
		public int SelectedItem
		{
			get{
				return m_SelItem;
			}
			set
			{
				m_SelItem = value;
			}
		}
	}
	
}
