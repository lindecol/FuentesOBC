using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

	/// <summary>
	/// Descripción breve de _Default.
	/// </summary>
	public partial class OpcionesConfiguracion : System.Web.UI.Page
	{
		WebAplicationSettings m_Configuracion = new WebAplicationSettings();

		protected System.Web.UI.WebControls.Button btnIngresar;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox txtUsuario;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.HtmlControls.HtmlInputText txtContraseña;
		protected System.Web.UI.WebControls.Label lblAdvertencia;
		/*protected System.Web.UI.WebControls.Image imgConexion;
		protected System.Web.UI.WebControls.Label Label30;
		protected System.Web.UI.WebControls.Button btnModificarOracle;
		protected System.Web.UI.WebControls.Label lblAdvertenciaOracle;
		protected System.Web.UI.WebControls.Panel pnConexionBaseDatos;
		protected System.Web.UI.WebControls.Label lblDSN;
		protected System.Web.UI.WebControls.TextBox txtDSN;
		protected System.Web.UI.WebControls.TextBox txtUsuarioOracle;
		protected System.Web.UI.WebControls.TextBox txtContrasenaOracle;
		protected System.Web.UI.WebControls.Label lblUsuario;
		protected System.Web.UI.WebControls.Label lblContraseña;
		protected System.Web.UI.WebControls.Panel pnInicio;*/
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!(this.IsPostBack))
			{
				CargarDatosConexion();
			}
		}

		// TIPO FUNCION : METODO DE LA CLASE
		// DESCRIPCION: METODO PARA CARGAR LOS DATOS DE CONEXION CON UNA BASE DE DATOS ORACLE
		// PARAMETROS: NINGUNO
		private void CargarDatosConexion()
		{
			if (m_Configuracion.ConexionOracle != "")
			{
				this.txtDSN.Text = m_Configuracion.DesEncriptar(m_Configuracion.ConexionOracle);
            }
			if (m_Configuracion.UsuarioOracle != "")
			{
                this.txtUsuarioOracle.Text = m_Configuracion.DesEncriptar(m_Configuracion.UsuarioOracle);
			}
			if (m_Configuracion.ContrasenaOracle != "")
			{
				this.txtContrasenaOracle.Text = m_Configuracion.DesEncriptar(m_Configuracion.ContrasenaOracle);
            }
            if (Convert.ToString(m_Configuracion.CodigoPrograma) != "")
            {
                this.txtCodigoPrograma.Text = Convert.ToString(m_Configuracion.CodigoPrograma);
            }
		}


		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnModificarOracle.Click += new System.EventHandler(this.btnModificarOracle_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnIngresar_Click(object sender, System.EventArgs e)
		{
			ValidarInicioSesion();
		}

		private void ValidarInicioSesion()
		{
			//VARIABLE PARA EL ACCEDER A LA CONFIGURACION DEL SERVICIO WEB
			WebAplicationSettings fConfiguracion = new WebAplicationSettings();
			if (fConfiguracion.Revisarinformacion("", this.txtUsuario.Text) && fConfiguracion.Revisarinformacion("", this.txtContraseña.Value))
			{
				Response.Redirect("OpcionesConfiguracion.aspx");
			}
			else
			{
				this.lblAdvertencia.Visible = true;
			}
		}

		public void btnModificarOracle_Click(object sender, System.EventArgs e)
		{
			if (this.txtDSN.Text == "")
			{
				this.lblAdvertenciaOracle.Text = "Debe Ingresar Nombre DSN!!!";
				this.lblAdvertenciaOracle.Visible = true;
				return;
			}
			if (this.txtUsuarioOracle.Text == "")
			{
				this.lblAdvertenciaOracle.Text = "Debe Ingresar usuario de conexión!!!";
				this.lblAdvertenciaOracle.Visible = true;
				return;
			}
			if (this.txtContrasenaOracle.Text == "")
			{
				this.lblAdvertenciaOracle.Text = "Debe Ingresar Contraseña Conexión!!!";
				this.lblAdvertenciaOracle.Visible = true;
				return;
			}
            if (this.txtCodigoPrograma.Text == "")
            {
                this.lblAdvertenciaOracle.Text = "Debe Ingresar Código Programa!!!";
                this.lblAdvertenciaOracle.Visible = true;
                return;
            }
			this.lblAdvertenciaOracle.Visible = true;
			m_Configuracion.ConexionOracle = m_Configuracion.Encriptar(txtDSN.Text).ToBase64();
			m_Configuracion.UsuarioOracle = m_Configuracion.Encriptar(txtUsuarioOracle.Text).ToBase64();
			m_Configuracion.ContrasenaOracle = m_Configuracion.Encriptar(txtContrasenaOracle.Text).ToBase64();
            m_Configuracion.CodigoPrograma = Convert.ToInt32(txtCodigoPrograma.Text);
			this.lblAdvertenciaOracle.Text = "Datos Modificados Satisfactoriamente!!!";
		}
        
}

