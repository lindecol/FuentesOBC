using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MovilidadCF.Location
{
    public class GPSCaptureDialog
    {

        GpsPosition position = null;

        public GpsPosition Position
        {
            get { return position; }
        }

        public LocationFormatType locationFormat = LocationFormatType.DecimalDegrees;
        public LocationFormatType LocationFormat
        {
            get { return locationFormat; }
            set { locationFormat = value; }
        }
        
        public DialogResult ShowDialog(string Title)
        {
            DialogResult result;
            GPSCaptureForm frm = new GPSCaptureForm();
            frm.Text = Title;
            frm.LocationFormat = locationFormat;
            result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                position = frm.Position;
            }
            frm.Dispose();
            return result;
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog("MovilidadCF Collect");
        }
    }
}
