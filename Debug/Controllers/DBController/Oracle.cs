using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using Debug.Controllers;
using Oracle.ManagedDataAccess.Client;

class OracleDBFactory: DBAbstractFactory,ISerializable
    {
        private string drivertype { get; set; }
        public OracleDBFactory() { this.drivertype = null; }
        public override IDbConnection CreateConnection(string connstr)
        {
            if (connstr == null || connstr.Length == 0)
            {
                return null;
            }
            return new OracleConnection(connstr);
        }
        public override IDbCommand CreateCommand(IDbConnection con, string cmd)
        {
            if (con == null || cmd == null || cmd.Length == 0)
            {
                return null;
            }
            if(con is OracleConnection)
            {
                return new OracleCommand(cmd, (OracleConnection)con);
            }
            return null;
        }
        public override IDbDataAdapter CreateDbAdapter(IDbCommand cmd)
        {
            if(cmd == null) { return null; }
            if(cmd is OracleCommand)
            {
                return new OracleDataAdapter((OracleCommand)cmd);   
            }
            return null;
        }
        public override IDataReader CreateDataReader(IDbCommand cmd)
        {
            if (cmd == null) { return null; }
            if(cmd is OracleCommand)
            {
                return (OracleDataReader)cmd.ExecuteReader();
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        protected OracleDBFactory(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }