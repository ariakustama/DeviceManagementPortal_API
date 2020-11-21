using DeviceManagementPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementPortal.ViewModels
{
    public class DeviceViewModel
    {
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string SimCardNo { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormat { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DeviceListViewModel
    {
        public List<DeviceViewModel> listDevices { get; set; }
        public int CountData { get; set; }
    }
}
