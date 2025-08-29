using System.Net;

namespace SchoolMgmtAPI.Models
{
    public class APIResponseModel<T>
    {
        public HttpStatusCode code {  get; set; }
        public string message { get; set; }
        public T data { get; set; }

    }
}
