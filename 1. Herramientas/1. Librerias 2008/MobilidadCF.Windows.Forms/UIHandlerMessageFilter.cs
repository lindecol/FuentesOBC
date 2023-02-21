using System;
using OpenNETCF.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using System.Windows.Forms;
using OpenNETCF.Win32;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// Descripción breve de MessageFilter.
	/// </summary>
	internal class UIHandlerMessageFilter: IMessageFilter
	{
		// Valores de mensajes constantes
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;
		private const int WM_CHAR = 0x0102;
		private const int WM_COMMAND = 0x111;
		private const int WM_LBUTTONDOWN = 0x0201;
		private const int WM_LBUTTONUP = 0x202;
		private const int WM_LBUTTONDBLCLK = 0x0203;
		private const int WM_MOUSEWHEEL = 0x020A;
		private const int WM_ENTERMENULOOP = 0x0211;
		private const int WM_EXITMENULOOP = 0x0212;
		
		public UIHandlerMessageFilter()
		{
			
		}

		
		// Función de filtro de mensajes, para control de mensajes de teclado
		// y mouse (touch screen )
		public  bool PreFilterMessage(ref Message m )
		{
			bool bResult = false;
			switch(m.Msg)
			{
				case WM_KEYDOWN:
				case WM_KEYUP:
					if ( UIHandler.m_bDoWait)
						bResult = true;
					else if ( m.WParam.ToInt32() == UIHandler.TabKey.KeyCode)
					{
						m.WParam = (IntPtr) Keys.Tab;
						Win32Window.SendMessage(m.HWnd, m.Msg, m.WParam, m.LParam.ToInt32());	
						bResult = true;										
					}
					break;
			
				case WM_CHAR:
					if ( UIHandler.m_bDoWait)
						bResult = true;
					else if ( m.WParam.ToInt32() == UIHandler.TabKey.KeyChar)
					{
						m.WParam = (IntPtr) Keys.Tab;
						Win32Window.SendMessage(m.HWnd, m.Msg, m.WParam, m.LParam.ToInt32());
						bResult = true;										
					}
					break;

				case WM_LBUTTONDOWN:
				case WM_LBUTTONUP:
				case WM_LBUTTONDBLCLK:
				case WM_MOUSEWHEEL:
					if ( UIHandler.m_bDoWait)
						bResult = true;
					break;
				case WM_ENTERMENULOOP:
					UIHandler.m_bMenuLoop = true;
					break;
				case WM_EXITMENULOOP:
					UIHandler.m_bMenuLoop = false;
					break;
			}			
			
			return bResult;
		}

        
	}
}
