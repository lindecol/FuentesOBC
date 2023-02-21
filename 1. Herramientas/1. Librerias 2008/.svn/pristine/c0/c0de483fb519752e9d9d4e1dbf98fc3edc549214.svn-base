using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// 
	/// </summary>
	public class OneInstance: MessageWindow
	{
		private IntPtr HWND_BROADCAST = (IntPtr)65535;
		private const int ERROR_ALREADY_EXISTS = 183;
        private int ourActivateMessage;
		private Form mainForm;
		private bool closing = false;

		// p/invokes
		//
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpString"></param>
		/// <returns></returns>
		[DllImport("CoreDll.Dll")]
		private static extern int RegisterWindowMessage( string lpString );

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[DllImport("CoreDll.dll", EntryPoint="GetLastError")]
		private static extern int GetLastError();		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpMutexAttributes"></param>
		/// <param name="InitialOwner"></param>
		/// <param name="MutexName"></param>
		/// <returns></returns>
		[DllImport("CoreDll.dll", EntryPoint="CreateMutexW")]		
		private static extern int CreateMutex( IntPtr lpMutexAttributes, bool InitialOwner, string MutexName );
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool IsInstanceRunning()	
		{				
			string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			if( CreateMutex(IntPtr.Zero, true, applicationName) != 0 )
			{
				return (GetLastError() == ERROR_ALREADY_EXISTS);
			}			
			return false;		
		}

		/// <summary>
		/// 
		/// </summary>
		private bool ActivateInstance()
		{
            if (this.IsInstanceRunning())
            {
                this.closing = true;
                Message activateMessage = Message.Create(HWND_BROADCAST,
                    this.ourActivateMessage,
                    IntPtr.Zero,
                    IntPtr.Zero);
                MessageWindow.PostMessage(ref activateMessage);
                return false;
            }
            else
                return true;
		}

        private static OneInstance instance = null;

        public static void Run(Form mainForm)
        {
            instance = new OneInstance();
            instance.mainForm = mainForm;
            string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            instance.ourActivateMessage = RegisterWindowMessage(applicationName);
            if (instance.ActivateInstance())
                Application.Run(mainForm);
            
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		protected OneInstance( )
		{
			
		}
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		protected override void WndProc( ref Message msg )
		{
			if( !this.closing && msg.Msg == this.ourActivateMessage )
			{
				this.mainForm.BringToFront();
			}
 
			// call the base class WndProc for default message handling
			//
			base.WndProc( ref msg );
		}
	}
}
