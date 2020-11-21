using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManagementPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace DeviceManagementPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController
            (ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<HomeController>();
        }

        [HttpGet]
        public JsonResult Get()
        {
            var conStringBilder = new SqliteConnectionStringBuilder();
            conStringBilder.DataSource = "./DeviceManagement.db";

            using (var conn = new SqliteConnection(conStringBilder.ConnectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();
                tableCmd.CommandText = "CREATE TABLE DeviceBackEnd(Id VARCHAR(36));";
                tableCmd.ExecuteNonQuery();
            }
            return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true });
        }

        [HttpPost]
        public JsonResult Post ([FromBody] string value)
        {
            return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true });
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
