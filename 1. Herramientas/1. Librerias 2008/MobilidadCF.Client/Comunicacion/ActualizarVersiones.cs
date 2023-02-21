using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MovilidadCF.Windows.Forms;

namespace MovilidadCF.Client.Comunicacion
{
    public partial class ActualizarVersiones : Form
    {
        private string m_Programa;
        private string m_VersionActual;
        private OpenNETCF.Threading.Thread2 m_Thread;
        private DescargaVersiones m_Descargar;
        private delegate void ActualizarMensajeHandler(string sData);

        public ActualizarVersiones()
        {
            InitializeComponent();
        }

        public static bool Run(string Programa, string VersionActual)
        {
            bool result = false;

            // Si asi se especifica se intenta primero establecer la conexión GPRS
            UIHandler.StartWait();            
            try
            {
                // Se crea la instancia de la forma y se inicia el proceso
                ActualizarVersiones frm = new ActualizarVersiones();
                frm.m_Programa = Programa;
                frm.m_VersionActual = VersionActual;
                frm.m_Descargar = new DescargaVersiones();
                if (frm.m_Descargar.RevisarVersiones(frm.m_VersionActual, frm.m_Programa))
                {
                    result = (UIHandler.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK);
                }
                frm.Dispose();                
                return result;
            }
            catch (Exception ex)
            {
                MovilidadCF.Logging.Logger.Write(ex);
                return false;
            }
            finally
            {
                UIHandler.EndWait();
            }
        }

        private void ActualizarVersiones_Load(object sender, EventArgs e)
        {            
            // Se inicializan controles
            UiHandler1.Parent = this;            

            // Se inicia el hilo de ejecución de la integración de datos y se habilita el timer
            // de reset de estado Idle del sistema
            tmProgreso.Enabled = true;
            m_Thread = new OpenNETCF.Threading.Thread2(ObtenerVersion);
            m_Thread.Start();
            this.CargarImagenes();
            UIHandler.EndWait();
            this.tmProgreso.Enabled = true;
        }

        private void ObtenerVersion()
        {
            IniciarEnvioDatos("Descargando Nueva Versión");
            if (m_Descargar.DescargarVersion())
            {
                MostrarProcesoTerminado("DESCARGA COMPLETA!!");
            }
            else
            {
                if (m_Thread != null && m_Thread.State != OpenNETCF.Threading.ThreadState.Stopped)
                {
                    if (!(m_Thread.Join(5000)))
                    {
                        try
                        {
                            m_Thread.Abort();
                        }
                        catch
                        {
                        }
                    }
                }
                UIHandler.StartWait();
                tmProgreso.Enabled = false;
                m_Descargar.BorrarArchivoTemporal();
                this.DialogResult = DialogResult.Cancel;
            }            
        }

        private void CargarImagenes()
        {
            System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.AnimateCtl1.Image = new System.Drawing.Bitmap(_assembly.GetManifestResourceStream("MovilidadCF.Client.Imagenes.AnimateCtl1.Image.jpg"));
        }       

        private void ActualizarVersiones_Closed(object sender, EventArgs e)
        {
            this.tmProgreso.Dispose();
        }

        private void tmProgreso_Tick(object sender, EventArgs e)
        {
            this.lblProgreso.Text = m_Descargar.NumeroBytes.ToString();
            this.Refresh();
        }

        private void InternalIniciarEnvioDatos(string sMensaje)
        {            
            pnlEnviar.Visible = true;
            AnimateCtl1.StartAnimation();
            lblFase.Text = sMensaje;
        }

        private void IniciarEnvioDatos(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalIniciarEnvioDatos);
            this.Invoke(funcHandler, sMensaje);
        }

        private void InternalMostrarProcesoTerminado(string sMensaje)
        {
            this.tmProgreso.Enabled = false;
            this.lblProgreso.Text = sMensaje;
            this.Refresh();
        }

        private void MostrarProcesoTerminado(string sMensaje)
        {
            ActualizarMensajeHandler funcHandler = new ActualizarMensajeHandler(InternalMostrarProcesoTerminado);
            this.Invoke(funcHandler, sMensaje);
            if (System.IO.File.Exists(m_Descargar.CabInstalacion()))
            {
                try
                {
                    InstallHelper.InstallCabFile(m_Descargar.CabInstalacion());
                }
                catch(Exception ex)
                {
                    MovilidadCF.Logging.Logger.Write(ex);
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {            
            lblFase.Text = "Cancelando...";         
            btnCancelar.Enabled = false;
            Application.DoEvents();
            if (m_Thread != null && m_Thread.State != OpenNETCF.Threading.ThreadState.Stopped)
            {
                if (!(m_Thread.Join(5000)))
                {
                    try
                    {
                        m_Thread.Abort();
                    }
                    catch
                    {
                    }
                }
            }
            UIHandler.StartWait();
            tmProgreso.Enabled = false;
            m_Descargar.BorrarArchivoTemporal();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}