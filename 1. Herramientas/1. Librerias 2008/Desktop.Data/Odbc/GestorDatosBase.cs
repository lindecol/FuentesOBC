using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Reflection;

namespace Desktop.Data.Odbc
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {

        public GestorDatosBase() : base() { }

        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {
            return new OdbcConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return new OdbcCommand();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            OdbcCommandBuilder.DeriveParameters((OdbcCommand)cmd);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new OdbcDataAdapter();
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new OdbcCommandBuilder();
        }

        public new OdbcConnection Connection
        {
            get
            {
                return (OdbcConnection)base.Connection;
            }
        }

        public new OdbcTransaction Transaction
        {
            get
            {
                return (OdbcTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            OdbcCommand cmdOdbc = (OdbcCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdOdbc.Parameters.AddWithValue(String.Format("p{0}", (I + 1)), Parameters[I]);
            }
        }
    }
}
