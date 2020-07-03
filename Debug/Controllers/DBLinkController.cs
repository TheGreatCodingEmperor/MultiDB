using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Debug.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class DBLinkController : ControllerBase {
        private readonly ILogger<DBLinkController> _logger;
        private IConfiguration _config;
        public DBLinkController (ILogger<DBLinkController> logger, IConfiguration config) {
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> hello () {
            await Task.CompletedTask;
            return Ok ("hello");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">maria、sqlite、postgre、oracle、sqlserver</param>
        /// <param name="connectString">DB 連線方式</param>
        /// <param name="sql">sql 語法</param>
        /// <returns></returns>
        [HttpGet ("{type}")]
        public async Task<IActionResult> dblink (string type, [FromHeader] string connectString, [FromQuery] string sql) {
            DBAbstractFactory factory;
            switch (type.ToLower ()) {
                case "maria":
                    {
                        factory = new MySQLDBFactory ();
                        break;
                    }
                case "sqlite":
                    {
                        factory = new SqliteDBFactory ();
                        break;
                    }
                case "postgre":
                    {
                        factory = new PostGreDBFactory ();
                        break;
                    }
                case "oracle":
                    {
                        factory = new OracleDBFactory ();
                        break;
                    }
                case "sqlserver":
                    {
                        factory = new SQLServerDbFactory ();
                        break;
                    }
                default:
                    {
                        factory = null;
                        break;
                    }
            }
            var data = getdatas (factory, WebUtility.UrlDecode (connectString), sql);
            await Task.CompletedTask;
            return Ok (data);
        }
        static string getdatas (DBAbstractFactory factory, string connectString, string sql) {
            var db = factory;
            var connect = db.CreateConnection (connectString);
            try {
                connect.Open ();
                var cmd = db.CreateCommand ((DbConnection) connect, sql);
                var reader = db.CreateDataReader ((DbCommand) cmd);

                //MySqlDataAdapter adapter = (MySqlDataAdapter)maria.CreateDbAdapter(cmd);
                DataTable ds = new DataTable ();

                var columns = new List<string> ();
                foreach (DataColumn column in ds.Columns) {
                    columns.Add (column.ColumnName);
                }

                //adapter.Fill(ds);
                ds.Load (reader);

                var rows = ds.AsEnumerable ()
                    // .Where (i => (dynamic) (i["id"]) == 2)
                    // .Where(i => ColValue(i,"image_no") == 34)

                    #region 【json】
                    .Select (r => r.Table.Columns.Cast<DataColumn> ()
                        .Select (c => new KeyValuePair<string, object> (c.ColumnName, r[c.Ordinal])).ToDictionary (z => z.Key, z => z.Value)
                    ).ToList ();
                #endregion【json】

                #region 【data list】
                // .Select(i => i.ItemArray).ToList();
                #endregion 【data list】

                // Console.WriteLine (JsonConvert.SerializeObject (rows));
                connect.Close ();
                return JsonConvert.SerializeObject (rows);

                // Console.WriteLine(JsonConvert.SerializeObject(rows));
            } catch (Exception e) {
                // Console.WriteLine (e.ToString ());
                connect.Close ();
                return e.ToString ();
            }
        }
    }
}