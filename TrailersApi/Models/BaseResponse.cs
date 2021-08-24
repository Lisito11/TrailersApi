using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrailersApi.Models {
    public class BaseResponse {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }


        public BaseResponse() { }

        public BaseResponse(string message, object data, bool ok = false) {
            Ok = ok;
            Message = message;
            Data = data;
        }
    }
}
