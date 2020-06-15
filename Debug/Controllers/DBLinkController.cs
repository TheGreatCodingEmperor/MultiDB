using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Debug.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class DBLinkController : ControllerBase {
        private readonly ILogger<DBLinkController> _logger;
        private readonly MariaDB _mariaDB = new MariaDB ();
        private readonly PostGre _postGre = new PostGre ();
        private readonly SqliteDB _sqliteDB = new SqliteDB ();
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
        /// <param name="type">maria、sqlite、postgre</param>
        /// <param name="connectString">DB 連線方式</param>
        /// <param name="sql">sql 語法</param>
        /// <returns></returns>
        [HttpGet ("{type}")]
        public async Task<IActionResult> dblink (string type, [FromHeader] string connectString, [FromQuery] string sql) {
            DBAbstractFactory factory;
            switch(type.ToLower()){
                case "maria":{
                    factory = new MariaDB();
                    break;
                }
                case "sqlite":{
                    factory = new SqliteDB();
                    break;
                }
                case "postgre":{
                    factory = new PostGre();
                    break;
                }
                default: {
                    factory = null;
                    break;
                }
            }
            var data = getdatas(factory, WebUtility.UrlDecode(connectString) ,sql);
            await Task.CompletedTask;
            return Ok(data);
        }
        static List<Dictionary<string, object>> getdatas(DBAbstractFactory factory, string connectString, string sql){
            var conn = factory.CreateConnection (connectString);
            var cmd = factory.CreateCommand (conn, sql);
            var data = factory.CreateDataReader (conn, cmd).data;
            return data;
        }

        /// <summary>
        /// maria db ����
        /// </summary>
        /// <returns></returns>
        [HttpGet ("mariadb")]
        public async Task<IActionResult> Mariadb ([FromQuery] string sql, [FromQuery] string col = null, [FromQuery] string keyword = null) {
            var connectString = _config["ConnectionStrings:mariaDB"];
            if (col != null && keyword != null) {
                sql = sql + $" where {col} = '{keyword}' or {col} LIKE '%{keyword}%'";
            }
            var conn = _mariaDB.CreateConnection (connectString);
            var cmd = _mariaDB.CreateCommand (conn, sql);
            var data = _mariaDB.CreateDataReader (conn, cmd).data;
            await Task.CompletedTask;
            return Ok (data);
        }

        [HttpGet ("postgredb")]
        public async Task<IActionResult> PostGreDB ([FromQuery] string sql, [FromQuery] string col = null, [FromQuery] string keyword = null) {
            var connectString = _config["ConnectionStrings:postGresDB"];
            if (col != null && keyword != null) {
                sql = sql + $" where {col} = '{keyword}' or {col} LIKE '%{keyword}%'";
            }
            var conn = _postGre.CreateConnection (connectString);
            var cmd = _postGre.CreateCommand (conn, sql);
            var data = _postGre.CreateDataReader (conn, cmd).data;
            await Task.CompletedTask;

            return Ok (data);
        }

        [HttpGet ("sqlitedb")]
        public async Task<IActionResult> SqliteDB ([FromQuery] string sql, [FromQuery] string col = null, [FromQuery] string keyword = null) {
            var connectString = _config["ConnectionStrings:sqlLiteDB"];
            if (col != null && keyword != null) {
                sql = sql + $" where {col} = '{keyword}' or {col} LIKE '%{keyword}%'";
            }
            var conn = _sqliteDB.CreateConnection (connectString);
            var cmd = _sqliteDB.CreateCommand (conn, sql);
            var data = _sqliteDB.CreateDataReader (conn, cmd).data;
            await Task.CompletedTask;

            return Ok (data);
        }
    }
}