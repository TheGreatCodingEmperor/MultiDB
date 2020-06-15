using System.Data.Common;
using Debug.Controllers;
using Npgsql;

public class PostGre : DBAbstractFactory
    {
        public override IDbConnection CreateConnection(string connstr)
        {
            if (connstr == "" || connstr == null) return null;
            DbConnection  conn = new NpgsqlConnection();
            conn.ConnectionString = connstr;
            var DBConn = new MyDBConnection();
            DBConn.connection = conn;
            return DBConn;
        }
        public override IDbCommand CreateCommand(IDbConnection con, string cmd)
        {
            DbCommand SqlCommand = new NpgsqlCommand(cmd, (NpgsqlConnection)con.connection);
            var DBCmd = new MyDBCommand();
            DBCmd.command = SqlCommand;
            return DBCmd;
        }
        public override IDataReader CreateDataReader(IDbConnection con, IDbCommand sqlCmd)
        {
            DbConnection conn = (NpgsqlConnection)con.connection;
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            DbCommand cmd = sqlCmd.command;
            DbDataReader data = cmd.ExecuteReader();

            var read = new MyDataReader();

            read.data = this.CreateDataAdapter(data).data;

            conn.Close();
            return read;
        }
    }