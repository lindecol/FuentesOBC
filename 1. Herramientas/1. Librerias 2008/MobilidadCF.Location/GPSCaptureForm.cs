using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MovilidadCF.Location
{
    internal partial class GPSCaptureForm : Form
    {
        private Gps gps = null;
        private int nIntentos = 0;

        private GpsPosition position = null;
        public GpsPosition Position
        {
            get { return position; }
        }

        public LocationFormatType LocationFormat = LocationFormatType.DecimalDegrees;

        public GPSCaptureForm()
        {
            InitializeComponent();
        }


        private void GPSCaptureDialog_Load(object sender, EventArgs e)
        {
            try
            {
                pnlError.Visible = false;
                pnlPosicion.Visible = true;
                gps = new Gps(this);
                gps.DeviceStateChanged += new DeviceStateChangedEventHandler(gps_DeviceStateChanged);
                gps.LocationChanged += new LocationChangedEventHandler(gps_LocationChanged);
                gps.Open();
                timerPortOpening.Enabled = true;
            }
            catch
            {
                pnlPosicion.Visible = false;
                pnlError.Visible = true;
                mitemAceptar.Enabled = false;
            }
        }

        void gps_LocationChanged(object sender, LocationChangedEventArgs args)
        {
            position = args.Position;

            if (position.LatitudeValid && position.LongitudeValid)
            {
                nIntentos = 0;
                if (LocationFormat == LocationFormatType.Degrees)
                {
                    lblLatitud.Text = position.Latitude.ToString();
                    lblLongitud.Text = position.Longitude.ToString();
                }
                else
                {
                    lblLatitud.Text = position.LatitudeInDegreesMinutesSeconds.ToString();
                    lblLongitud.Text = position.LongitudeInDegreesMinutesSeconds.ToString();
                }
            }
            else
            {
                nIntentos++;
                if (nIntentos == 15)
                    nIntentos = 1;
                lblLongitud.Text = lblLatitud.Text = new String('.', nIntentos);
            }

            if (position.SatellitesInSolutionValid &&
                position.SatellitesInViewValid &&
                position.SatelliteCountValid)
            {
                lblSatelites.Text = "Satelites en uso: " + position.GetSatellitesInSolution().Length + " de " +
                    position.GetSatellitesInView().Length + position.SatelliteCount + "\n";
            }
            else
                lblSatelites.Text = "No hay satelites a la vista";
        }

        void gps_DeviceStateChanged(object sender, DeviceStateChangedEventArgs args)
        {
            lblEstado.Text = args.DeviceState.FriendlyName + ": " + args.DeviceState.DeviceState.ToString() +
                "\nEstado servicio: " + args.DeviceState.ServiceState.ToString();
        }

        private void menuOK_Click(object sender, EventArgs e)
        {
            if (CloseGPS())
                DialogResult = DialogResult.OK;
        }

        private void menuCancel_Click(object sender, EventArgs e)
        {
            if (CloseGPS())
                DialogResult = DialogResult.Cancel;
        }

        private bool CloseGPS()
        {
            try
            {
                if (gps != null)
                {
                    if (gps.Opened)
                        gps.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cerrando GPS: " + ex.Message);
                return false;
            }
        }

        private void timerPortOpening_Tick(object sender, EventArgs e)
        {
            timerPortOpening.Enabled = false;
            this.Menu = menuPrincipal;
        }


    }
}