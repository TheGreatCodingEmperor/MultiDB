using System.Data.Common;
using Debug.Controllers;
using MySql.Data.MySqlClient;

public class MariaDB : DBAbstractFactory {
    public override IDbConnection CreateConnection (string connstr) {
        if (connstr == "" || connstr == null) return null;
        DbConnection conn = new MySqlConnection ();
        conn.ConnectionString = connstr;
        var DBConn = new IDbConnection ();
        DBConn.connection = conn;
        return DBConn;
    }
    public override IDbCommand CreateCommand (IDbConnection con, string cmd) {
        DbCommand SqlCommand = new MySqlCommand (cmd, (MySqlConnection) con.connection);
        var DBCmd = new IDbCommand ();
        DBCmd.command = SqlCommand;
        return DBCmd;
    }
    public override IDataReader CreateDataReader (IDbConnection con, IDbCommand sqlCmd) {
        MySqlConnection conn = (MySqlConnection) con.connection;
        if (conn.State != System.Data.ConnectionState.Open) conn.Open ();
        DbCommand cmd = sqlCmd.command;
        DbDataReader data = cmd.ExecuteReader ();

        var read = new IDataReader ();

        read.data = this.CreateDataAdapter (data).data;

        conn.Close ();
        return read;
    }
}