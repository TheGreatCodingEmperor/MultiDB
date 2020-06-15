using System.Collections.Generic;
using System.Data.Common;

namespace Debug.Controllers {
    /// <summary>
    ///   DB 連線方式
    /// </summary>
    public interface IDbConnection {
        /// <summary>
        ///   DB 連線方式
        /// </summary>
        public DbConnection connection { get; set; }
    }
    /// <summary>
    ///   DB sql 指令
    /// </summary>
    public interface IDbCommand {
        /// <summary>
        ///   DB sql 指令
        /// </summary>
        public DbCommand command { get; set; }
    }
    /// <summary>
    ///   DB 回傳 Data
    /// </summary>
    public interface IDataReader {
        /// <summary>
        ///   DB 回傳 Data
        /// </summary>
        public List<Dictionary<string, object>> data { get; set; }
    }
    /// <summary>
    ///   DB Data 轉成 json 格式
    /// </summary>
    public interface IDbDataAdapter {
        /// <summary>
        ///   DB Data 轉成其他格式
        /// </summary>
        public List<Dictionary<string, object>> data { get; set; }
    }

    public class MyDBConnection : IDbConnection {
        public MyDBConnection () { }
        private DbConnection _connection;
        public DbConnection connection {
            get => _connection;
            set => _connection = value;
        }
    }
    public class MyDBCommand : IDbCommand {
        public MyDBCommand () { }
        private DbCommand _command;
        public DbCommand command {
            get => _command;
            set => _command = value;
        }
    }
    public class MyDataReader : IDataReader {
        public MyDataReader(){}
        private List<Dictionary<string,object>> _data;
        public List<Dictionary<string, object>> data {
            get =>
                _data;
            set =>
                _data = value;
        }
    }
    public class MyDBDataAdapter : IDbDataAdapter {
        public MyDBDataAdapter(){ }
        private List<Dictionary<string,object>> _data;
        public List<Dictionary<string, object>> data {
            get =>
                _data;
            set =>
                _data = value;
        }
    }
}