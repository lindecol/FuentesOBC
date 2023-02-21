using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestSerializarTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Fecha", Type.GetType("System.DateTime")));
            DataRow rw = dt.NewRow();
            rw[0] = System.DateTime.Now;
            dt.Rows.Add(rw);
            DataRow rw2 = dt.NewRow();
            rw2[0] = System.DBNull.Value;
            rw2[0] = new System.DateTime();
            dt.Rows.Add(rw2);
            ds.Tables.Add(dt);
            string datso = Datascan.TextSerializer.TextSerializer.Serialize(ds);
        }
    }
}
