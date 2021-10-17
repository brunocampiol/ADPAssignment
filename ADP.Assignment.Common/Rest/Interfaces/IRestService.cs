using System.Net;
using System.Threading.Tasks;

namespace ADP.Assignment.Common.Rest.Interfaces
{
    public interface IRestService
    {
        //(string Content, HttpStatusCode StatusCode) GetRequest(string url, string resource = "", IDictionary<string, string> queryParams = null, IDictionary<string, string> headerParams = null);
        //(string Content, HttpStatusCode StatusCode) PostRequest(string url, string resource = "", object body = null, IDictionary<string, string> queryParams = null, IDictionary<string, string> headerParams = null);

        (string Content, HttpStatusCode StatusCode) GetRequest(string url, string resource = "");
        (string Content, HttpStatusCode StatusCode) PostRequest(string url, string resource = "", object body = null);

        Task<(string Content, HttpStatusCode StatusCode)> GetRequestAsync(string url, string resource = "");
        Task<(string Content, HttpStatusCode StatusCode)> PostRequestAsync(string url, string resource = "", object body = null);
    }
}