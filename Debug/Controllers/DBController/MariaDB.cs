using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Debug.Controllers;
using MySql.Data.MySqlClient;

class MySQLDBFactory: DBAbstractFactory,ISerializable
    {
        private string drivertype { get; set; }
        public MySQLDBFactory() { this.drivertype = null; }
        public override IDbConnection CreateConnection(string connstr)
        {
            if (connstr == null || connstr.Length == 0)
            {
                return null;
            }
            return new MySqlConnection(connstr);
        }
        public override IDbCommand CreateCommand(IDbConnection con, string cmd)
        {
            if (con == null || cmd == null || cmd.Length == 0)
            {
                return null;
            }
            if(con is MySqlConnection)
            {
                return new MySqlCommand(cmd, (MySqlConnection)con);
            }
            return null;
        }
        public override IDbDataAdapter CreateDbAdapter(IDbCommand cmd)
        {
            if(cmd == null) { return null; }
            if(cmd is MySqlCommand)
            {
                return new MySqlDataAdapter((MySqlCommand)cmd);   
            }
            return null;
        }
        public override IDataReader CreateDataReader(IDbCommand cmd)
        {
            if (cmd == null) { return null; }
            if(cmd is MySqlCommand)
            {
                return (MySqlDataReader)cmd.ExecuteReader();
            }
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        protected MySQLDBFactory(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }