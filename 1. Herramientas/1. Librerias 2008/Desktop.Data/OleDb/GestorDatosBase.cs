using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Reflection;

namespace Desktop.Data.OleDb
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {

        public GestorDatosBase() : base() { }

        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {
            return new OleDbConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return new OleDbCommand();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            OleDbCommandBuilder.DeriveParameters((OleDbCommand)cmd);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new OleDbCommandBuilder();
        }

        public new OleDbConnection Connection
        {
            get
            {
                return (OleDbConnection)base.Connection;
            }
        }

        public new OleDbTransaction Transaction
        {
            get
            {
                return (OleDbTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            OleDbCommand cmdOleDb = (OleDbCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdOleDb.Parameters.AddWithValue(String.Format("p{0}", (I + 1)), Parameters[I]);
            }
        }
    }
}
