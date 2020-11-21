using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceManagementPortal.Models
{
    public class ApiResult
    {
        public bool isSuccessful { get; set; }
        public string Error { get; set; }
        public string message { get; set; }
        public string Code { get; set; }
        public void SetResult(bool Status, string Message, string Code = "")
        {
            this.isSuccessful = true;
            this.message = Message;
            this.Code = Code;
        }
    }
    public class ApiResult<T>
    {
        public bool isSuccessful { get; set; }
        public string Error { get; set; }
        public string message { get; set; }
        public string Code { get; set; }
        public T Payload { get; set; }

        public void SetResult(bool Status, string Message, string Code = "")
        {
            this.isSuccessful = true;
            this.message = Message;
            this.Code = Code;
        }
    }
}
