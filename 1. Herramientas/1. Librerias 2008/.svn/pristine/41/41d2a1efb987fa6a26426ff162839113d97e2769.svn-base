using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Desktop.Windows.Forms 
{
    public class DataGridView : System.Windows.Forms.DataGridView
    {
        protected override bool  ProcessDialogKey(Keys keyData)
        {
            if ( keyData == Keys.Enter)
            {
                 int fila = this.CurrentCell.RowIndex;
                 int col = this.CurrentCell.ColumnIndex;

                 // Se posiciona en la siguiente columna que no sea de lectura
                 for (int j = fila; j < this.RowCount; j++)
                 {
                     for (int i = col + 1; i < this.ColumnCount; i++)
                     {
                         if (!this[i, j].ReadOnly && this[i, j].Visible)
                         {
                             try
                             {
                                 this.CurrentCell = this[i, j];
                             }
                             catch { }
                             return true;
                         }
                     }
                     col = 0;
                 }
                 return true;
             }
             else if (keyData == Keys.Escape)
             {
                 return true;
             }
             else
                 return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int fila = this.CurrentCell.RowIndex;
                int col = this.CurrentCell.ColumnIndex;

                // Se posiciona en la siguiente columna que no sea de lectura
                for (int j = fila; j < this.RowCount; j++)
                {
                    for (int i = col + 1; i < this.ColumnCount; i++)
                    {
                        if (!this[i, j].ReadOnly && this[i,j].Visible )
                        {
                            try
                            {
                                this.CurrentCell = this[i, j];
                            }
                            catch { }
                            return;
                        }
                    }
                    col = 0;
                }
                return;
            }
            else
                base.OnKeyDown(e);
        }

        private bool m_LeftKeyPressed = false;

        public bool LeftKeyPressed
        {
            get { return m_LeftKeyPressed; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
                m_LeftKeyPressed = true;
            else
                m_LeftKeyPressed = false;
            bool bRes = base.ProcessCmdKey(ref msg, keyData);
            return bRes;
            //m_LeftKeyPressed = false;
        }

        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);
            m_LeftKeyPressed = false;
        }
    

    }
}
