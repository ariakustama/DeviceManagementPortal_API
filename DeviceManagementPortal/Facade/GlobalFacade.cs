using DeviceManagementPortal.Models;
using DeviceManagementPortal.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementPortal.Facade
{
    public class GlobalFacade
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<GlobalFacade> _logger;
        public GlobalFacade(DatabaseContext db, ILoggerFactory logger) 
        {
            _db = db;
            _logger = logger.CreateLogger<GlobalFacade>();
        }
        public DeviceListViewModel getListDevices(ParamGetListDevices param) 
        {
            var returndata = new DeviceListViewModel();
            returndata.listDevices = new List<DeviceViewModel>();
            try
            {
                returndata.CountData = _db.Devices.Count();
                returndata.listDevices = (from a in _db.Devices
                                          select new DeviceViewModel
                                          {
                                              Id = a.Id,
                                              IMEI = a.IMEI,
                                              Model = a.Model,
                                              SimCardNo = a.SimCardNo,
                                              Enabled = a.Enabled,
                                              CreatedDate = a.CreatedDate,
                                              CreatedDateFormat = a.CreatedDate.ToString("dd MMM yyyy"),
                                              CreatedBy = a.CreatedBy
                                          }).OrderBy(x => x.CreatedDate).Skip((param.page - 1) * param.itemPerPage).Take(param.itemPerPage).ToList();
                return returndata;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }

        }

        public bool InsertDevicesAndBackEndBackEnd(DevicesSubmitViewModel param) 
        {
            _db.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Populating Object Device");
                var dataDevice = new Device();
                dataDevice.Id = Guid.NewGuid().ToString();
                dataDevice.IMEI = param.IMEI;
                dataDevice.Model = param.Model;
                dataDevice.SimCardNo = param.SimCardNo;
                dataDevice.Enabled = param.Enabled;
                dataDevice.CreatedDate = param.CreatedDate;
                dataDevice.CreatedBy = param.CreatedBy;
                _db.Devices.Add(dataDevice);
                _logger.LogInformation("Add Data Device");

                if (param.listBackEnd.Any())
                {
                    _logger.LogInformation("Start Loop detail Backend");
                    for (int i = 0; i < param.listBackEnd.Count(); i++)
                    {
                        _logger.LogInformation("Populating Object BackEnd");
                        var dataBackEnd = new Backend();
                        dataBackEnd.Id = Guid.NewGuid().ToString();
                        dataBackEnd.Name = param.listBackEnd[i].Name;
                        dataBackEnd.Address = param.listBackEnd[i].Address;
                        _db.Backends.Add(dataBackEnd);
                        _logger.LogInformation("Add Data BackEnd");

                        _logger.LogInformation("Populating Object DeviceBackEnd");
                        var dataDeviceMappingBackEnd = new DeviceBackend();
                        dataDeviceMappingBackEnd.Id = Guid.NewGuid().ToString();
                        dataDeviceMappingBackEnd.IdBackEnd = dataBackEnd.Id;
                        dataDeviceMappingBackEnd.IdDevice = dataDevice.Id;
                        dataDeviceMappingBackEnd.MappedTime = DateTime.Now;
                        _db.DeviceBackends.Add(dataDeviceMappingBackEnd);
                        _logger.LogInformation("Add Data DeviceBackEnd");
                    }
                }

                _logger.LogInformation("Start Save Change");
                _db.SaveChanges();
                _db.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _db.Database.RollbackTransaction();
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }
        }

        public bool UpdateDevicesAndBackEndBackEnd(DevicesSubmitViewModel param)
        {
            _db.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Start Search Data Device");
                var dataDevice = _db.Devices.Where(x => x.Id == param.Id).FirstOrDefault();
                if (dataDevice == null)
                {
                    _logger.Log(LogLevel.Critical, "Data Device NOT FOUND");
                    throw new ArgumentException("Data Device Not Found");
                }

                _logger.LogInformation("Start Populating Object Device");
                dataDevice.IMEI = param.IMEI;
                dataDevice.Model = param.Model;
                dataDevice.SimCardNo = param.SimCardNo;
                _db.Devices.Update(dataDevice);
                _logger.LogInformation("Update Data Device");

                #region Remove Backend And Mapping Back End
                var listIdBackEndFromUI = new List<string>();
                listIdBackEndFromUI = param.listBackEnd.Where(x => (x.Id != null || x.Id != string.Empty)).Select(x => x.Id).ToList();
                if (listIdBackEndFromUI.Any())
                {
                    _logger.LogInformation("Start Search Data DeviceBackends");
                    var dataBackEndMapping = _db.DeviceBackends.Where(x => x.IdDevice == param.Id && !listIdBackEndFromUI.Contains(x.IdBackEnd)).ToList();
                    if (dataBackEndMapping.Any()) {
                        _db.DeviceBackends.RemoveRange(dataBackEndMapping);
                        _logger.LogInformation("Remove Data DeviceBackends");
                    }
                }
                else
                {
                    _logger.LogInformation("Start Search Data DeviceBackends ALL  by device id");
                    var dataBackEndMapping = _db.DeviceBackends.Where(x => x.IdDevice == param.Id).ToList();
                    if (dataBackEndMapping.Any())
                        _db.DeviceBackends.RemoveRange(dataBackEndMapping);

                    _logger.LogInformation("Remove Data DeviceBackends ALL  by device id");
                }
                #endregion

                foreach (var item in param.listBackEnd.Where(x => (x.Id == null || x.Id == string.Empty)).ToList())
                {
                    _logger.LogInformation("Start Populating Object Backend");
                    var dataBackEnd = new Backend();
                    dataBackEnd.Id = Guid.NewGuid().ToString();
                    dataBackEnd.Name = item.Name;
                    dataBackEnd.Address = item.Address;
                    _db.Backends.Add(dataBackEnd);
                    _logger.LogInformation("Add Data BackEnd");

                    _logger.LogInformation("Start Populating Object DeviceBackend");
                    var dataDeviceMappingBackEnd = new DeviceBackend();
                    dataDeviceMappingBackEnd.Id = Guid.NewGuid().ToString();
                    dataDeviceMappingBackEnd.IdBackEnd = dataBackEnd.Id;
                    dataDeviceMappingBackEnd.IdDevice = dataDevice.Id;
                    dataDeviceMappingBackEnd.MappedTime = DateTime.Now;
                    _db.DeviceBackends.Add(dataDeviceMappingBackEnd);
                    _logger.LogInformation("Add Data DeviceBackEnd");
                }

                foreach (var item in param.listBackEnd.Where(x => x.Id != null).ToList())
                {
                    _logger.LogInformation("Start Search Data Backends");
                    var dataBackEnd = _db.Backends.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (dataBackEnd == null)
                    {
                        _logger.Log(LogLevel.Critical, "Data Backends NOT FOUND");
                        throw new ArgumentException("Data Backend Not Found");
                    }

                    _logger.LogInformation("Start Populating Object Backend");
                    dataBackEnd.Name = item.Name;
                    dataBackEnd.Address = item.Address;
                    _db.Backends.Update(dataBackEnd);
                    _logger.LogInformation("Update Data Backends");
                }

                _logger.LogInformation("Start Save Change");
                _db.SaveChanges();
                _db.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _db.Database.RollbackTransaction();
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }
        }
        public Device GetDeviceById(string id) 
        {
            try
            {
                return _db.Devices.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }
        }

        public List<Backend> GetListMappingBackEnd(string DeviceId)
        {
            try
            {
                return (from a in _db.DeviceBackends
                        join b in _db.Backends on a.IdBackEnd equals b.Id
                        where a.IdDevice == DeviceId
                        select b).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }
        }

        public bool DeleteDevice(string DeviceId) 
        {
            _db.Database.BeginTransaction();
            try
            {
                _logger.LogInformation("Start Search Data Device");
                var dataDevice = _db.Devices.Where(x => x.Id == DeviceId).FirstOrDefault();
                if (dataDevice == null)
                {
                    _logger.Log(LogLevel.Critical, "Data Device NOT FOUND");
                    throw new ArgumentException("Data Device Not Found");
                }

                _logger.LogInformation("Start Search Data DeviceBackends");
                var listIdBackENd = _db.DeviceBackends.Where(x => x.IdDevice == DeviceId).Select(x => x.IdBackEnd).ToList();

                _db.Devices.Remove(dataDevice);
                _logger.LogInformation("Remove Data DeviceBackends");
                if (listIdBackENd.Any())
                {
                    _logger.LogInformation("Start Search Data Backends");
                    var listdataBackEnd = _db.Backends.Where(x => listIdBackENd.Contains(x.Id)).ToList();
                    if (listdataBackEnd.Any())
                    {
                        _db.Backends.RemoveRange(listdataBackEnd);
                        _logger.LogInformation("Remove Data Backends");
                    }
                }

                _logger.LogInformation("Start Save Change");
                _db.SaveChanges();
                _db.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _db.Database.RollbackTransaction();
                _logger.Log(LogLevel.Critical, ex.Message.ToString());
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
