using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Desktop.Windows.Forms
{
	/// <summary>
	/// Interfaz para controles validables
	/// </summary>
    /// 
   
   internal interface IInputControl
	{
        string HelpText {get;set;}

        string ErrorMessage {get;set;}

        bool Required { get; set;}

        bool IsValid { get;}

        bool HasInputChanged();

        bool IsInputEmpty();

        bool IsInputValid();        

	}
}
