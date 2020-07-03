using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using Debug.Controllers;
using Npgsql;

class PostGreDBFactory: DBAbstractFactory,ISerializable
    {
        private string drivertype { get; set; }
        public PostGreDBFactory() { this.drivertype = null; }
        public override IDbConnection CreateConnection(string connstr)
        {
            if (connstr == null || connstr.Length == 0)
            {
                return null;
            }
            return new NpgsqlConnection(connstr);
        }
        public override IDbCommand CreateCommand(IDbConnection con, string cmd)
        {
            if (con == null || cmd == null || cmd.Length == 0)
            {
                return null;
            }
            if(con is NpgsqlConnection)
            {
                return new NpgsqlCommand(cmd, (NpgsqlConnection)con);
            }
            return null;
        }
        public override IDbDataAdapter CreateDbAdapter(IDbCommand cmd)
        {
            if(cmd == null) { return null; }
            if(cmd is NpgsqlCommand)
            {
                return new NpgsqlDataAdapter((NpgsqlCommand)cmd);   
            }
            return null;
        }
        public override IDataReader CreateDataReader(IDbCommand cmd)
        {
            if (cmd == null) { return null; }
            if(cmd is NpgsqlCommand)
            {
                return (NpgsqlDataReader)cmd.ExecuteReader();
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        protected PostGreDBFactory(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }