using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManagementPortal.Enums;
using DeviceManagementPortal.Facade;
using DeviceManagementPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace DeviceManagementPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackendController : Controller
    {
        private readonly ILogger<BackendController> _logger;
        private readonly GlobalFacade _facade;
        public BackendController
            (ILoggerFactory logger,
            GlobalFacade facade)
        {
            _logger = logger.CreateLogger<BackendController>();
            _facade = facade;
        }

        [HttpGet("GetListMappingBackEnd/{Id}")]
        public JsonResult Get(string Id)
        {
            var returnData = new List<Backend>();
            try
            {
                returnData = _facade.GetListMappingBackEnd(Id);
                if (returnData == null)
                {
                    return Json(new ApiResult<List<Backend>>() { isSuccessful = false, Payload = returnData, Code = ApiErrorCode.DATA_NOT_FOUND });
                }
                else
                {
                    return Json(new ApiResult<List<Backend>>() { isSuccessful = true, Payload = returnData });
                }
            }
            catch (Exception ex)
            {
                return Json(new ApiResult<List<Backend>>() { isSuccessful = false, Payload = returnData, message = ex.Message, Code = ApiErrorCode.DATA_NOT_FOUND });
            }
        }

        [HttpPost]
        public JsonResult Post ([FromBody] string value)
        {
            return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true });
        }

        [HttpPut("{id}")]
        public JsonResult Put(int id, [FromBody] string value)
        {
            return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true });
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true });
        }
    }
}
