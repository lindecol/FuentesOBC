using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Desktop.Windows.Forms
{
	/// <summary>
	/// Represents the method that handles the <see cref="PopupCancelNotifier.PopupCancel"/> event
	/// raised by this class.
	/// </summary>
	internal delegate void PopupCancelEventHandler(object sender, EventArgs e);

	#region PopupCancelNotifier
	/// <summary>
	/// 
	/// A class which provides the functionality required to 
	/// cancel a popup window.  This class wraps two pieces of 
	/// functionality:
	/// 
	/// <list type="number">Firstly, it checks whether the form (or the form owner
	/// for the control) receives a <c>WM_ACTIVATE</c> message with
	/// wParam = 0.  This indicates the window has gone out
	/// of focus because the user has clicked on another one.</list>
	/// <list type="number">Secondly, it installs a Win32 Mouse Hook and checks
	/// for mouse presses anywhere else in the application.
	/// This is the same technique that's used in the Framework
	/// Class Library to implement drop-down designer windows.</list>
	/// 
	/// However, this functionality may cause a problem because 
	/// the CLR will mark this code as non-type safe owing to 
	/// the unmanaged code in the Hook.  If you don't need in-place 
	/// editing of items, then you can remove this code and any 
	/// reference to it from the <c>ListBar</c> control and recompile 
	/// for a 100% managed type-safe version of the control.
	/// 
	/// <remarks>
	/// Copyright &#169; 2003 Steve McMahon for vbAccelerator.com.
	/// vbAccelerator is a Trade Mark of vbAccelerator Ltd.  All Rights
	/// Reserved.  Please visit http://vbaccelerator.com/ for more
	/// on this and other VB and .NET Framework code.
	/// </remarks>
	/// 
	/// </summary>
	internal class PopupCancelNotifier : NativeWindow, IDisposable
	{

		/// <summary>
		/// The PopupCancel event is raised whenever the popup should be
		/// cancelled.
		/// </summary>
		public event PopupCancelEventHandler PopupCancel;
		
		private MouseHook mouseHook = new MouseHook();

		/// <summary>
		/// Message sent to a window when activation state
		/// changes
		/// </summary>
		private const int WM_ACTIVATE = 0x6;
		/// <summary>
		/// Window handle to track for popup cancellation
		/// </summary>
		private IntPtr trackHandle = IntPtr.Zero;
		/// <summary>
		/// Whether this object has been disposed or not
		/// </summary>
		private bool disposed = false;

		/// <summary>
		/// Start tracking for a popup cancellation.
		/// </summary>
		/// <param name="ctl">The <c>Control</c> or <c>Form</c>
		/// to use when tracking Window inactivation messages. This can
		/// either be a control or a Form.</param>
		public void StartTracking(Control ctl)
		{
			IntPtr handle = IntPtr.Zero;

			Control ctlOwnerForm = ctl;
			Control ctlTest = null;
			while (!typeof(Form).IsAssignableFrom(ctlOwnerForm.GetType()))
			{
				ctlTest = ctlOwnerForm.Parent;
				if (ctlTest == null)
				{
					break;
				}
				else
				{
					ctlOwnerForm = ctlTest;
				}
			}
						
			this.trackHandle = ctlOwnerForm.Handle;
			this.AssignHandle(trackHandle);
			this.mouseHook.Install();
			this.mouseHook.MouseHookEvent += new MouseHookEventHandler(mouseHook_MouseHookEvent);
		}

		private void mouseHook_MouseHookEvent(object sender, ref MouseHookEventArgs e)
		{
			if (e.Button != MouseButtons.None)
			{
				OnPopupCancel(new EventArgs());
			}
		}

		/// <summary>
		/// Check for the WM_ACTIVATE message and stop
		/// tracking if the window is inactivated.
		/// </summary>
		/// <param name="msg">Message details for this window procedure
		/// event.</param>
		protected override void WndProc(ref Message msg)
		{
			base.WndProc(ref msg);
			if (msg.Msg == WM_ACTIVATE)
			{
				if (((int)msg.WParam) == 0)
				{
					OnPopupCancel(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Stop tracking. Called automatically if this class determines
		/// the popup should be cancelled.
		/// </summary>
		public void StopTracking()
		{
			if (!this.trackHandle.Equals(IntPtr.Zero))
			{
				this.ReleaseHandle();
				this.trackHandle = IntPtr.Zero;
				this.mouseHook.MouseHookEvent -= new MouseHookEventHandler(mouseHook_MouseHookEvent);
				this.mouseHook.Uninstall();
			}
		}

		/// <summary>
		/// Notify when the popup should be cancelled,
		/// and uninstall tracking.
		/// </summary>
		protected virtual void OnPopupCancel(EventArgs e)
		{
			if (PopupCancel != null)
			{
				PopupCancel(this, e);
			}
			StopTracking();
		}

		/// <summary>
		/// Create a new instance of the PopupCancel class
		/// </summary>
		public PopupCancelNotifier()
		{
			// intentionally blank
		}
		
		/// <summary>
		/// Finalises the class and clears up resources if the
		/// Dispose() method has not been called.
		/// </summary>
		~PopupCancelNotifier()
		{
			if (!disposed)
			{
				StopTracking();
			}
		}

		/// <summary>
		/// Ensures any resources associated with this object are
		/// cleared up.
		/// </summary>
		public void Dispose()
		{
			StopTracking();
			disposed = true;
			GC.SuppressFinalize(this);
		}

	}
	#endregion

	#region LocalWindowsHook
	//
	// The LocalWindowsHook code is based mainly on 
	// Dino Esposito's Cutting Edge column in the MSDN October 
	// 2002 issue, "Cutting Edge: Windows Hooks in the .NET Framework".
	// Changes:
	// 1) Change the hook event handling to an override-based hook 
	//    mechanism rather than an event-based one.
	// 2) The event information needs to be by ref so we can modify the
	//    details returned to Windows.
	// 3) Some half-hearted documentation and  field renaming.
	//

	#region Class HookEventArgs
	/// <summary>
	/// Arguments for the Hook event
	/// </summary>
	/// <remarks>This code is based on code published by Dino Esposito
	/// in the article "Cutting Edge: Windows Hooks in the .NET Framework"
	/// published in the October 2002 edition of MSDN and available online
	/// at http://msdn.microsoft.com/
	/// </remarks>
	internal class HookEventArgs : EventArgs
	{
		/// <summary>
		/// Hook code
		/// </summary>
		public int HookCode;	
		/// <summary>
		/// WPARAM argument
		/// </summary>
		public IntPtr wParam;	
		/// <summary>
		/// LPARAM argument
		/// </summary>
		public IntPtr lParam;	
	}
	#endregion

	#region Enum HookType
	/// <summary>
	/// Hook Types available under Windows. TODO: documentation
	/// </summary>
	/// <remarks>This code is based on code published by Dino Esposito
	/// in the article "Cutting Edge: Windows Hooks in the .NET Framework"
	/// published in the October 2002 edition of MSDN and available online
	/// at http://msdn.microsoft.com/
	/// </remarks>
	internal enum HookType : int
	{
		WH_JOURNALRECORD = 0,
		WH_JOURNALPLAYBACK = 1,
		WH_KEYBOARD = 2,
		WH_GETMESSAGE = 3,
		WH_CALLWNDPROC = 4,
		WH_CBT = 5,
		WH_SYSMSGFILTER = 6,
		WH_MOUSE = 7,
		WH_HARDWARE = 8,
		WH_DEBUG = 9,
		WH_SHELL = 10,
		WH_FOREGROUNDIDLE = 11,
		WH_CALLWNDPROCRET = 12,		
		WH_KEYBOARD_LL = 13,
		WH_MOUSE_LL = 14
	}
	#endregion

	/// <summary>
	/// This class defines the core functionality to implement
	/// a Windows Hook.  Specific implementations can subclass this
	/// class and override <see cref="OnHookInvoked"/>, or respond to the
	/// <see cref="HookInvoked"/> event.
	/// </summary>	
	/// <remarks>This code is based on code published by Dino Esposito
	/// in the article "Cutting Edge: Windows Hooks in the .NET Framework"
	/// published in the October 2002 edition of MSDN and available online.  
	/// It has been changed as follows:
	/// <list type="number">Change the hook event handling to an override-based hook 
	///  mechanism rather than an event-based one.</list>
	/// <list type="number">The event information needs to be by ref so we can modify the
	/// details returned to Windows.</list>
	/// <list type="number">Added some documentation and renamed some fields.</list>
	/// </remarks>
	internal abstract class LocalWindowsHook : IDisposable
	{
		#region Unmanaged code
		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowsHookEx(
			HookType code, 
			HookProc func,
			IntPtr hInstance,
			int threadID);
		[DllImport("user32.dll")]
		public static extern int UnhookWindowsHookEx(IntPtr hhook);
		[DllImport("user32.dll")]
		protected static extern int CallNextHookEx(IntPtr hhook, 
			int code, IntPtr wParam, IntPtr lParam);
		#endregion

		/// <summary>
		/// Filter function delegate
		/// </summary>
		public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

		#region Properties
		/// <summary>
		/// The handle to the Windows hook.
		/// </summary>
		protected IntPtr HookHandle = IntPtr.Zero;
		/// <summary>
		/// The hook filter function.
		/// </summary>
		protected HookProc FilterFunc = null;
		/// <summary>
		/// The type of hook installed.
		/// </summary>
		protected HookType HookType;
		#endregion
		private bool disposed = false;

		/// <summary>
		/// Represents the method that handles the HookInvoked event
		/// raised by this class.
		/// </summary>
		public delegate void HookInvokedEventHandler(object sender, ref HookEventArgs e);

		/// <summary>
		/// The HookInvoked event is raised whenever the hook fires.
		/// </summary>
		public event HookInvokedEventHandler HookInvoked;
		
		/// <summary>
		/// Raises the HookInvoked event. This method can be overriden
		/// for particular implementations of a hook, or an implementation
		/// can respond to the HookInvoked event.
		/// </summary>
		/// <param name="e">The HookEventArgs for this hook
		/// event.</param>
		protected virtual void OnHookInvoked(ref HookEventArgs e)
		{
			if (HookInvoked != null)
			{
				HookInvoked(this, ref e);
			}
		}


		/// <summary>
		/// Constructs a new instance of this class with
		/// the specified Hook Type.
		/// </summary>
		/// <param name="hookType">Hook type to create</param>
		public LocalWindowsHook(HookType hookType)
		{
			this.HookType = hookType;
			this.FilterFunc = new HookProc(this.CoreHookProc); 
		}
		
		/// <summary>
		/// Default filter function.
		/// </summary>
		/// <param name="code">Hook code</param>
		/// <param name="wParam">Hook wParam</param>
		/// <param name="lParam">Hook lParam</param>
		/// <returns></returns>
		private int CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
		{
			// According to MSDN docs, if code < 0 then you must call
			// the next hook in the chain:
			if (code < 0)
			{
				return CallNextHookEx(this.HookHandle, code, wParam, lParam);
			}
			else
			{
				// Call the event:
				HookEventArgs e = new HookEventArgs();
				e.HookCode = code;
				e.wParam = wParam;
				e.lParam = lParam;				
				OnHookInvoked(ref e);

				// Yield to the next hook in the chain:
				return CallNextHookEx(this.HookHandle, e.HookCode, e.wParam, e.lParam);
			}
		}

		/// <summary>
		/// Install the hook.
		/// </summary>
		public void Install()
		{
			this.HookHandle = SetWindowsHookEx(
				this.HookType, 
				this.FilterFunc, 
				IntPtr.Zero, 
				(int) System.Threading.Thread.CurrentThread.ManagedThreadId);
			Trace.Assert((!this.HookHandle.Equals(IntPtr.Zero)), "Failed to install hook!");
		}

		/// <summary>
		/// Uninstall the hook.
		/// </summary>
		public void Uninstall()
		{
			if (this.HookHandle != IntPtr.Zero)
			{
				UnhookWindowsHookEx(this.HookHandle); 
			}
			this.HookHandle = IntPtr.Zero;
		}

		/// <summary>
		/// Clear up any resources associated with the hook if 
		/// Dispose() has not been called.
		/// </summary>
		~LocalWindowsHook()
		{
			if (!this.disposed)
			{
				Uninstall();
			}
		}

		/// <summary>
		/// Clear up any resources associated with this hook.
		/// </summary>
		public void Dispose()
		{
			if (!this.disposed)
			{
				Uninstall();
				this.disposed = true;
				GC.SuppressFinalize(this);
			}
		}

	}
	#endregion

	#region Mouse Hook Implementation

	#region MOUSEHOOKSTRUCT for interop with the Windows Mouse Hook
	/// <summary>
	/// The Windows API <c>MOUSEHOOKSTRUCT</c> which is passed in the 
	/// <c>lParam</c> of a Mouse Hook event.
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential)]
	internal struct MOUSEHOOKSTRUCT 
	{
		/// <summary>
		/// Mouse X Position
		/// </summary>
		public int x;
		/// <summary>
		/// Mouse Y Position
		/// </summary>
		public int y;
		/// <summary>
		/// Handle of window mouse is over
		/// </summary>
		public IntPtr handle;
		/// <summary>
		/// Hit test code returned
		/// </summary>
		public int wHitTestCode;
		/// <summary>
		/// Other information about the mouse event
		/// </summary>
		public int dwExtraInfo;
		
		// note that under Windows2000 and XP there is 
		// also an additional mouseData DWORD which supplies
		// the mouse wheel information

		/// <summary>
		/// Provides a human-readable string displaying the contents of
		/// this structure.
		/// </summary>
		/// <returns>A string containing details of the contents of
		/// this structure.</returns>
		public override string ToString()
		{
			return String.Format("{0} x={1},y={2},hWnd={3},wHitTestCode={4},dwExtraInfo={5}",
				typeof(MOUSEHOOKSTRUCT).FullName,
				this.x,
				this.y,
				this.handle,
				this.wHitTestCode,
				this.dwExtraInfo 
				);
		}
	}
	#endregion

	#region Enumerations associated with the Mouse Hook class

	/// <summary>
	/// Types of MouseHook events which are recorded:
	/// </summary>
	internal enum MouseHookEventType : int
	{
		/// <summary>
		/// The mouse has moved
		/// </summary>
		MouseMove,
		/// <summary>
		/// A mouse button has been depressed
		/// </summary>
		MouseDown,
		/// <summary>
		/// A mouse button has been released
		/// </summary>
		MouseUp,
		/// <summary>
		/// A mouse wheel action has occurred
		/// </summary>
		MouseWheel,
		/// <summary>
		/// A mouse button has been double-clicked
		/// </summary>
		DblClick
	}

	/// <summary>
	/// The location of the mouse when a mouse hook event is recorded.
	/// </summary>
	internal enum MouseHookEventLocation : int
	{
		/// <summary>
		/// The mouse event occurred in the client area.
		/// </summary>
		Client,
		/// <summary>
		/// The mouse event occurred in a non-client area.
		/// </summary>
		NonClient
	}
	#endregion


	#region MouseHookEventArgs class
	/// <summary>
	/// Information about a Mouse Hook event
	/// which has occured.
	/// TODO: X buttons
	/// </summary>
	internal class MouseHookEventArgs : EventArgs
	{
		private MouseHookEventType eventType;
		private MouseHookEventLocation eventLocation;
		private MouseButtons button;
		private int x;
		private int y;
		private IntPtr handle;
		private int hitTestCode;
		private int extraData;

		private const int  WM_MOUSEMOVE                    = 0x0200;
		private const int  WM_LBUTTONDOWN                  = 0x0201;
		private const int  WM_LBUTTONUP                    = 0x0202;
		private const int  WM_LBUTTONDBLCLK                = 0x0203;
		private const int  WM_RBUTTONDOWN                  = 0x0204;
		private const int  WM_RBUTTONUP                    = 0x0205;
		private const int  WM_RBUTTONDBLCLK                = 0x0206;
		private const int  WM_MBUTTONDOWN                  = 0x0207;
		private const int  WM_MBUTTONUP                    = 0x0208;
		private const int  WM_MBUTTONDBLCLK                = 0x0209;
		private const int  WM_MOUSEWHEEL                   = 0x020A;
		private const int  WM_XBUTTONDOWN                  = 0x020B;
		private const int  WM_XBUTTONUP                    = 0x020C;
		private const int  WM_XBUTTONDBLCLK                = 0x020D;
		private const int  WM_NCLBUTTONDOWN                = 0x00A1;
		private const int  WM_NCLBUTTONUP                  = 0x00A2;
		private const int  WM_NCLBUTTONDBLCLK              = 0x00A3;
		private const int  WM_NCRBUTTONDOWN                = 0x00A4;
		private const int  WM_NCRBUTTONUP                  = 0x00A5;
		private const int  WM_NCRBUTTONDBLCLK              = 0x00A6;
		private const int  WM_NCMBUTTONDOWN                = 0x00A7;
		private const int  WM_NCMBUTTONUP                  = 0x00A8;
		private const int  WM_NCMBUTTONDBLCLK              = 0x00A9;
		private const int  WM_NCXBUTTONDOWN                = 0x00AB;
		private const int  WM_NCXBUTTONUP                  = 0x00AC;
		private const int  WM_NCXBUTTONDBLCLK              = 0x00AD;

		/// <summary>
		/// Gets the type of mouse event.
		/// </summary>
		public MouseHookEventType EventType
		{
			get
			{
				return this.eventType;
			}
		}

		/// <summary>
		/// Gets the location in which the mouse event
		/// occurred.
		/// </summary>
		public MouseHookEventLocation EventLocation
		{
			get
			{
				return this.eventLocation;
			}
		}

		/// <summary>
		/// Gets the button which is involved in the action
		/// (or MouseButtons.None if no button is used).
		/// </summary>
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		/// <summary>
		/// Returns the X location of the mouse when the event
		/// occurred.
		/// </summary>
		public int X
		{
			get
			{
				return this.x;
			}
		}

		/// <summary>
		/// Returns the Y location of the mouse when the event
		/// occurred.
		/// </summary>
		public int Y
		{
			get
			{
				return this.Y;
			}
		}

		/// <summary>
		/// Gets the window handle of the object the mouse
		/// was over.
		/// </summary>
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}


		/// <summary>
		/// Constructs a new MouseHookEvent
		/// </summary>
		/// <param name="wParam">The <c>wParam</c> (Message code) for the
		/// Mouse Hook event</param>
		/// <param name="mhs">The <c>MOUSEHOOKEVENT</c> structure
		/// for the hook event.</param>
		public MouseHookEventArgs(
			IntPtr wParam,
			MOUSEHOOKSTRUCT mhs
			)
		{
			switch ((int)wParam)
			{
				case WM_MOUSEMOVE:
					this.eventType = MouseHookEventType.MouseMove;
					// we could check if we're over a non-client
					// area here etc
					this.button = MouseButtons.None;
					break;

				case WM_LBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_LBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_LBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_MBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_MBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_MBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_RBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_RBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_RBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.Client;
					break;

				case WM_NCLBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCLBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCLBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Left;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCMBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCMBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCMBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Middle;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCRBUTTONDOWN:
					this.eventType = MouseHookEventType.MouseDown;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCRBUTTONUP:
					this.eventType = MouseHookEventType.MouseUp;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;

				case WM_NCRBUTTONDBLCLK:
					this.eventType = MouseHookEventType.DblClick;
					this.button = MouseButtons.Right;
					this.eventLocation = MouseHookEventLocation.NonClient;
					break;
			}

			this.x = mhs.x;
			this.y = mhs.y;

			this.handle = mhs.handle;
			
			this.hitTestCode = mhs.wHitTestCode;
			this.extraData = mhs.dwExtraInfo;
		}
	}
	#endregion


	#region MouseHook class delegates
	/// <summary>
	/// Represents the method that handles the HookInvoked event
	/// raised by this class.
	/// </summary>
	internal delegate void MouseHookEventHandler(object sender, ref MouseHookEventArgs e);
	#endregion

	#region MouseHook Class
	internal class MouseHook : LocalWindowsHook
	{

		/// <summary>
		/// The HookInvoked event is raised whenever the hook fires.
		/// </summary>
		public event MouseHookEventHandler MouseHookEvent;


		/// <summary>
		/// Override for the generic hook's invoked event to
		/// convert to a strongly typed MouseHookEvent:
		/// </summary>
		/// <param name="e">Generic Hook event argument details</param>
		protected override void OnHookInvoked(ref HookEventArgs e)
		{
			// Convert into mouse details:
			MOUSEHOOKSTRUCT mhs = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure(
				e.lParam, typeof(MOUSEHOOKSTRUCT));

			MouseHookEventArgs mhe = new MouseHookEventArgs(
				e.wParam, mhs);
			OnMouseHookEvent(ref mhe);

		}

		/// <summary>
		/// Raises the MouseHookEvent event.
		/// </summary>
		/// <param name="e">The MouseHook event details associated
		/// with this mouse hook event.</param>
		protected virtual void OnMouseHookEvent(ref MouseHookEventArgs e)
		{
			if (MouseHookEvent != null)
			{
				MouseHookEvent(this, ref e);
			}

		}

		/// <summary>
		/// Constructs a new instance of a MouseHook.
		/// </summary>
		public MouseHook() : base(HookType.WH_MOUSE)
		{
			// intentionally blank
		}

	}
	#endregion

	#endregion

}
