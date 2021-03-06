using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using Debug.Controllers;
using Microsoft.Data.SqlClient;

[Serializable()]
    class SQLServerDbFactory: DBAbstractFactory,ISerializable
    {
        private string drivertype { get; set; }
        public SQLServerDbFactory() { this.drivertype = null; }
        public override IDbConnection CreateConnection(string connstr)
        {
            if (connstr == null || connstr.Length == 0)
            {
                return null;
            }
            return new SqlConnection(connstr);
        }
        public override IDbCommand CreateCommand(IDbConnection con, string cmd)
        {
            if (con == null || cmd == null || cmd.Length == 0)
            {
                return null;
            }
            if(con is SqlConnection)
            {
                return new SqlCommand(cmd, (SqlConnection)con);
            }
            return null;
        }
        public override IDbDataAdapter CreateDbAdapter(IDbCommand cmd)
        {
            if(cmd == null) { return null; }
            if(cmd is SqlCommand)
            {
                return new SqlDataAdapter((SqlCommand)cmd);   
            }
            return null;
        }
        public override IDataReader CreateDataReader(IDbCommand cmd)
        {
            if (cmd == null) { return null; }
            if(cmd is SqlCommand)
            {
                return (SqlDataReader)cmd.ExecuteReader();
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        protected SQLServerDbFactory(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }