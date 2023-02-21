using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;



namespace Desktop.Windows.Forms
{

#if NETCFDESIGNTIME
	[ToolboxItemFilter("NETCF",ToolboxItemFilterType.Require),
	ToolboxItemFilter("System.CF.Windows.Forms", ToolboxItemFilterType.Custom)]
#endif
	/// <summary>
	/// Descripción breve de EnumComboBox.
	/// </summary>
	public class EnumComboBox : ComboBoxBase
	{
		#region Constructor y código generado por el Diseñador de componentes

		public EnumComboBox()
		{
			/// <summary>
			/// Requerido para la compatibilidad con el Diseñador de composiciones de clases Windows.Forms
			/// </summary>
			InitializeComponent();
			
		}

		
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			Required = true;
		}
		#endregion

		#region Propiedades del control
		
		private Type m_EnumType = null;

		public Type EnumType
		{
			get
			{
				return m_EnumType;
			}
			set
			{
#if !NETCFDESIGNTIME
				if ( value != null )
				{
					if ( value.IsEnum )
					{
						string[] items = Enum.GetNames(value);
						this.Items.Clear();
						foreach(string item in items)
							this.Items.Add(item);
					}
					else
						throw new ArgumentException("EnumComboBox.EnumType: el tipo especificado debe ser una enumeración");
				}
				m_EnumType = value;
#endif
			}
		}
				

		public new object SelectedValue
		{
			get
			{
				if ( EnumType != null && SelectedIndex >= 0 )
					return Enum.Parse(EnumType, SelectedItem.ToString());
				else
					return null;
			}
			set
			{
				if ( EnumType != null )
				{
					if ( Enum.IsDefined(EnumType, value) )
						this.SelectedIndex = this.Items.IndexOf(Enum.GetName(EnumType, value));
				}
				else
					throw new InvalidOperationException("EnumComboBox.SelectedValue: no se ha especificado el tipo de enumeración");
			}
		}

		#endregion

		#region Funcionalidad del control
        
		

		#endregion


	}
}
