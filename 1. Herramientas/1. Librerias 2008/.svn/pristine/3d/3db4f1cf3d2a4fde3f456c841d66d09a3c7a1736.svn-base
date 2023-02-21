using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Reflection;

namespace Desktop.Data.OracleClient
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {
        public GestorDatosBase() : base() { }

        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {
            return new OracleConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            OracleCommandBuilder.DeriveParameters((OracleCommand)cmd);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new OracleDataAdapter();
        }
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new OracleCommandBuilder();
        }

        public new OracleConnection Connection
        {
            get
            {
                return (OracleConnection)base.Connection;
            }
        }

        public new OracleTransaction Transaction
        {
            get
            {
                return (OracleTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            OracleCommand cmdOracle = (OracleCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdOracle.Parameters.AddWithValue(String.Format("p{0}", (I + 1)), Parameters[I]);
            }
        }
    }
}
