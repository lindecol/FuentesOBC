using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;
using System.Reflection;

namespace Desktop.Data.PostgreSQL
{
    public class GestorDatosBase : Desktop.Data.GestorDatosBase
    {
        public GestorDatosBase() : base() { }

        public GestorDatosBase(String ConnectionString)
            : base(ConnectionString) { }

        internal override DbConnection CreateConnection(string ConnectionString)
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public override DbCommand CreateCommand()
        {
            return new NpgsqlCommand();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new NpgsqlCommandBuilder();
        }

        public override void DeriveParameters(DbCommand cmd)
        {
            NpgsqlCommandBuilder.DeriveParameters((NpgsqlCommand)cmd);
        }

        public new NpgsqlConnection Connection
        {
            get
            {
                return (NpgsqlConnection)base.Connection;
            }
        }

        public new NpgsqlTransaction Transaction
        {
            get
            {
                return (NpgsqlTransaction)base.Transaction;
            }
            set
            {
                base.Transaction = value;
            }
        }

        protected override void SetCommandParameters(DbCommand cmd, object[] Parameters)
        {
            NpgsqlCommand cmdNpgsql = (NpgsqlCommand)cmd;
            for (int I = 0; I < Parameters.Length; I++)
            {
                cmdNpgsql.Parameters.AddWithValue(String.Format("p{0}", (I + 1)), Parameters[I]);
            }
        }
    }
}
