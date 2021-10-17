using ADP.Assignment.Common.Rest.Interfaces;
using Polly;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ADP.Assignment.Common.Rest.Services
{
    public class RestService : IRestService
    {
        private Policy<IRestResponse> _policy;
        private AsyncPolicy<IRestResponse> _policyAsync;

        public RestService(IRestPolicy policy)
        {
            // Rest Policy
            _policy = policy.RetryPolicy(2, 3);
            _policyAsync = policy.RetryPolicyAsync(2, 3);
        }

        public (string Content, HttpStatusCode StatusCode) GetRequest(string url, string resource = "")
        {
            RestClient restClient = CreateRestClient(url);

            RestRequest request = CreateRestRequest(resource, Method.GET);

            IRestResponse response = Rest(restClient, request);

            return (response.Content, response.StatusCode);
        }

        public (string Content, HttpStatusCode StatusCode) PostRequest(string url, string resource = "", object body = null)
        {
            RestClient restClient = CreateRestClient(url);

            RestRequest request = CreateRestRequest(resource, Method.POST, body);

            IRestResponse response = Rest(restClient, request);

            return (response.Content, response.StatusCode);
        }

        public async Task<(string Content, HttpStatusCode StatusCode)> GetRequestAsync(string url, string resource = "")
        {
            RestClient restClient = CreateRestClient(url);

            RestRequest request = CreateRestRequest(resource, Method.GET);

            IRestResponse response = await RestAsync(restClient, request);

            return (response.Content, response.StatusCode);
        }

        public async Task<(string Content, HttpStatusCode StatusCode)> PostRequestAsync(string url, string resource = "", object body = null)
        {
            RestClient restClient = CreateRestClient(url);

            RestRequest request = CreateRestRequest(resource, Method.POST, body);

            IRestResponse response = await RestAsync(restClient, request);

            return (response.Content, response.StatusCode);
        }

        private RestClient CreateRestClient(string url, (string UserName, string Password) basicAuth = default, int timeout = 0)
        {
            RestClient restClient = new RestClient
            {
                BaseUrl = new Uri(url),
                Timeout = timeout * 1000,
                Authenticator = basicAuth != default ? new HttpBasicAuthenticator(basicAuth.UserName, basicAuth.Password) : null
            };

            return restClient;
        }

        private RestRequest CreateRestRequest(string resource, Method method = Method.GET, object body = null, IDictionary<string, string> queryParams = null, IDictionary<string, string> header = null)
        {
            RestRequest request = new RestRequest(resource, method);
            request.AddHeader("Accept", "application/json; charset=utf-8");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");

            if (header != null)
            {
                header.ToList().ForEach(x =>
                {
                    request.AddHeader(x.Key, x.Value);
                });
            }

            if (queryParams != null)
            {
                queryParams.ToList().ForEach(x =>
                {
                    request.AddQueryParameter(x.Key, x.Value);
                });
            }

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            return request;
        }

        private IRestResponse Rest(RestClient restClient, RestRequest restRequest)
        {
            // Capture the exception so we can push it though the standard response flow.
            var policyResult = _policy.ExecuteAndCapture(() =>
            {
                var result = restClient.Execute(restRequest);
                return result;
            });

            IRestResponse rr = policyResult.Result;

            if (rr == null)
            {
                rr = policyResult.FinalHandledResult;
            }

            return rr;
        }

        private async Task<IRestResponse> RestAsync(RestClient restClient, RestRequest restRequest)
        {
            // Capture the exception so we can push it though the standard response flow.
            var policyResult = await _policyAsync.ExecuteAndCaptureAsync(async () =>
            {
                var result = await restClient.ExecuteAsync(restRequest);
                return result;
            });

            IRestResponse rr = policyResult.Result;

            if (rr == null)
            {
                rr = policyResult.FinalHandledResult;
            }

            return rr;
        }
    }
}