using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MovilidadCF.Data.SQLClient
{
    public class GestorDatosBase : System.ComponentModel.Component
    {
        private SqlConnection m_Connection = null;
        private SqlTransaction m_Transaction = null;
        private int m_OpenCount = 0;
        private string m_sConnectionString;

        public GestorDatosBase(string sConnectionString)
        {
            m_sConnectionString = sConnectionString;
        }

        protected SqlConnection Connection
        {
            get 
            {
                if (m_Connection == null)
                {
                    m_Connection = new SqlConnection(m_sConnectionString);
                }
                return m_Connection;
            }
        }

        protected SqlTransaction Transaction
        {
            get
            {
                return m_Transaction;
            }
            set
            {
                m_Transaction = value;
            }
        }

        public void OpenConnection()
        {
            if (m_OpenCount == 0)
            {
                Connection.Open();
                m_OpenCount++;
            }
        }

        public void CloseConnection()
        {
            m_OpenCount--;
            if (m_OpenCount == 0)
                Connection.Close();
        }

        public void BeginTransaction()
        {
            OpenConnection();

            // Create the transaction and assign it to the Transaction property
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        // --------------------------------------------------------------------
        public void CommitTransaction()
        {
            if (Transaction == null)
                throw new InvalidOperationException("No se ha abierto ninguna transacción");

            // Commit the transaction
            Transaction.Commit();
            Transaction = null;

            // Close the connection
            CloseConnection();
        }

        // --------------------------------------------------------------------
        public void RollbackTransaction()
        {
            if (Transaction == null)
                throw new InvalidOperationException("No se ha abierto ninguna transacción");

            // Rollback the transaction
            Transaction.Rollback();
            Transaction = null;

            // Close the connection
            CloseConnection();
        }
        
        protected void PrepareAdapter(System.ComponentModel.Component DataAdapter)
        {
            SqlDataAdapter Adapter;
            if (DataAdapter is SqlDataAdapter)
            {
                // Es un dataupdater sql base 
                Adapter = (SqlDataAdapter)DataAdapter;
            }
            else 
            { 
                // Se trata un Typed DataAdapter

                // se asigna la conexión a la base de datos
                DataAdapter.GetType().GetProperty("Connection", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(DataAdapter, Connection, null);

                // Se obtienen los objetos principales a ser configurados
                Adapter = (SqlDataAdapter)DataAdapter.GetType().GetProperty("Adapter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(DataAdapter, null);
                SqlCommand[] CommandCollection = (SqlCommand[])DataAdapter.GetType().GetProperty("CommandCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(DataAdapter, null); ;

                // attach or reset transaction to all commands of this adapter:
                if (CommandCollection != null)
                {
                    foreach (SqlCommand command in CommandCollection)
                    {
                        command.Transaction = m_Transaction;
                    }
                }
            }

            // Se configura el SQL Data Adapter
            if (Adapter.InsertCommand != null)
            {
                Adapter.InsertCommand.Connection = Connection;
                Adapter.InsertCommand.Transaction = m_Transaction;
            }
            if (Adapter.UpdateCommand != null)
            {
                Adapter.UpdateCommand.Connection = Connection;
                Adapter.UpdateCommand.Transaction = m_Transaction;
            }
            if (Adapter.DeleteCommand != null)
            {
                Adapter.DeleteCommand.Connection = Connection;
                Adapter.DeleteCommand.Transaction = m_Transaction;
            }            
        }
    }
}
