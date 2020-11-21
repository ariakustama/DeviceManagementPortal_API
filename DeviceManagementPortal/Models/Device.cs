 using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeviceManagementPortal.Models
{
    [Table("Device")]
    public class Device
    {
        [Key]
        public string Id { get; set; }
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string SimCardNo { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
