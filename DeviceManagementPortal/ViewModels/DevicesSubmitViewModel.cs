using DeviceManagementPortal.Models;
using System;
using System.Collections.Generic;

namespace DeviceManagementPortal.ViewModels
{
    public class DevicesSubmitViewModel
    {
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string SimCardNo { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<Backend> listBackEnd { get; set; }
    }
}
