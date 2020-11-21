using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeviceManagementPortal.Models
{
    [Table("DeviceBackend")]
    public class DeviceBackend
    {
        [Key]
        public string Id { get; set; }
        public string IdBackEnd { get; set; }
        public string IdDevice { get; set; }
        public DateTime MappedTime { get; set; }
    }
}
