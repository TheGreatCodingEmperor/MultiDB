using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Debug.Controllers {

    /// <summary>
    ///   抽象 DataBase 物件
    /// </summary>
    public abstract class DBAbstractFactory {
        /// <summary>
        /// 建立 DB 連線物件
        /// </summary>
        /// <param name="connstr">連線方式字串</param>
        /// <returns>DB 連線物件</returns>
        public abstract IDbConnection CreateConnection (string connstr);
        /// <summary>
        /// 建立 DB 查詢物件
        /// </summary>
        /// <param name="con">連線物件</param>
        /// <param name="cmd">查詢字串</param>
        /// <returns>查詢物件</returns>
        public abstract IDbCommand CreateCommand (IDbConnection con, string cmd);
        /// <summary>
        ///   建立讀取物件，讀取 DB 回傳 json
        /// </summary>
        /// <param name="con">連線物件</param>
        /// <param name="cmd">查詢物件</param>
        /// <returns></returns>
        public abstract IDataReader CreateDataReader (IDbConnection con, IDbCommand cmd);
        /// <summary>
        ///   將 DB command 物件回傳 DBDataReader 轉成 json
        /// </summary>
        /// <param name="DBdata" > DBDataReader </param>
        /// <returns></returns>
        public virtual IDbDataAdapter CreateDataAdapter (DbDataReader DBdata) {
            var result = new List<Dictionary<string, object>> ();
            int count = 0;
            while (DBdata.Read ()) {
                if (result.Count <= count + 1) {
                    result.Add (new Dictionary<string, object> ());
                }
                for (int i = 0; i < DBdata.FieldCount; i++) {
                    var key = DBdata.GetName (i);
                    if (result[count].ContainsKey (DBdata.GetName (i)))
                        key = this.copyVersion (result[count], key, key, 0);
                    var value = DBdata[i];
                    result[count][key] = Convert.IsDBNull (value) ? null : value;
                }
                count++;
            }
            var adapter = new MyDBDataAdapter ();
            adapter.data = result;
            return adapter;
        }
        public virtual string copyVersion (Dictionary<string, object> dic, string key, string oriKey, int version) {
            if (dic.ContainsKey (key)) {
                version++;
                return this.copyVersion (dic, $"{oriKey}({version})", oriKey, version);
            } else {
                return key;
            }
        }
    }
}