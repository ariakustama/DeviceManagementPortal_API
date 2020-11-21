using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManagementPortal.Enums;
using DeviceManagementPortal.Facade;
using DeviceManagementPortal.Models;
using DeviceManagementPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeviceManagementPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly GlobalFacade _facade;
        public DeviceController
            (ILoggerFactory logger,
            GlobalFacade facade)
        {
            _logger = logger.CreateLogger<DeviceController>();
            _facade = facade;
        }

        [HttpGet("GetDeviceById/{Id}")]
        public JsonResult Get(string Id)
        {
            var returnData = new Device();
            try
            {
                returnData = _facade.GetDeviceById(Id);
                if (returnData == null)
                {
                    return Json(new ApiResult<Device>() { isSuccessful = false, Payload = returnData, Code = ApiErrorCode.DATA_NOT_FOUND });
                }
                else
                {
                    return Json(new ApiResult<Device>() { isSuccessful = true, Payload = returnData });
                }
            }
            catch (Exception ex)
            {
                return Json(new ApiResult<Device>() { isSuccessful = false, Payload = returnData, message = ex.Message, Code = ApiErrorCode.DATA_NOT_FOUND });
            }
        }

        [HttpPost("SaveDevicesAndBackEndMapping")]
        public JsonResult SaveDevicesAndBackEndMapping([FromBody] DevicesSubmitViewModel param)
        {
            bool returnData = false;
            try
            {
                returnData = _facade.InsertDevicesAndBackEndBackEnd(param);
                return Json(new ApiResult<bool>() { isSuccessful = true, Payload = true });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult<bool>() { isSuccessful = false, Payload = true, message = ex.Message });
            }
        }

        [HttpPost("UpdateDevicesAndBackEndMapping")]
        public JsonResult UpdateDevicesAndBackEndMapping([FromBody] DevicesSubmitViewModel param)
        {
            try
            {
                bool returnData = _facade.UpdateDevicesAndBackEndBackEnd(param);
                return Json(new ApiResult<bool>() { isSuccessful = true, Payload = true });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult<bool>() { isSuccessful = false, Payload = false, message = ex.Message });
            }
        }

        [HttpPost("GetListDevices")]
        public JsonResult GetListDevices([FromBody] ParamGetListDevices param)
        {
            var returnData = new DeviceListViewModel();
            try
            {
                returnData = _facade.getListDevices(param);
                return Json(new ApiResult<DeviceListViewModel>() { isSuccessful = true, Payload = returnData });
            }
            catch (Exception ex)
            {
                return Json(new ApiResult<DeviceListViewModel>() { isSuccessful = false, Payload = returnData, message = ex.Message });
            }
        }

        [HttpDelete("DeleteDeviceData/{Id}")]
        public JsonResult DeleteDeviceData(string Id)
        {
            try
            {
                bool returnData = _facade.DeleteDevice(Id);
                return Json(new ApiResult<bool>() { isSuccessful = true, Payload = true });
            }
            catch (Exception)
            {
                return Json(new ApiResult<bool>() { isSuccessful = false, Payload = false });
            }
            
        }
    }
}
