using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public class Response<T>
    {
        public Response()
        {
            Header = new Header(); ;
            Header.Code = 200;
            Header.Message = "Successfull request";
        }
        public Header Header { get; set; }
        public T? Data { get; set; }
    }

    public class Header
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
