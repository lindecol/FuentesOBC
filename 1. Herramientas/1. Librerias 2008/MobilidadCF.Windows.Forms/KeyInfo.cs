using System;
using System.Windows.Forms;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// Descripción breve de KeyInfo.
	/// </summary>
	public class KeyInfo
	{
		private int m_KeyCode;
		private char m_KeyChar;

		public KeyInfo()
		{
			m_KeyChar = '\0';
			m_KeyCode = 0;
		}

		public KeyInfo(int keycode, char keychar)
		{
			m_KeyCode = keycode;
			m_KeyChar = keychar;
		}

		public int KeyCode
		{
			get
			{
				return m_KeyCode;
			}
			set
			{
				m_KeyCode = value;
			}
		}

		public char KeyChar
		{
			get
			{
				return m_KeyChar;
			}
			set
			{
				m_KeyChar = value;
			}
		}

		public void Change(Keys keycode, char keychar)
		{
			KeyCode = (int)keycode;
			KeyChar = keychar;
		}

		public void Change(int keycode, char keychar)
		{
			KeyCode = keycode;
			KeyChar = keychar;
		}
	}
}
