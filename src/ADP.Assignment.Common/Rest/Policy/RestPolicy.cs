using ADP.Assignment.Common.Extensions;
using ADP.Assignment.Common.Rest.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ADP.Assignment.Common.Rest.Policy
{
    public class RestPolicy : IRestPolicy
    {
        private readonly ILogger<RestPolicy> _logger;
        private readonly int[] _noRetryHttpStatusCodes = { 400, 401, 403 };

        public RestPolicy(ILogger<RestPolicy> logger)
        {
            _logger = logger;
        }

        public Policy<IRestResponse> RetryPolicy(int retryCount, int retrySeconds)
        {
            var retry = Polly.Policy
                        .Handle<Exception>()
                        .OrResult<IRestResponse>(result => !(((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299) ||
                                                            (_noRetryHttpStatusCodes.Contains((int)result.StatusCode))))
                        .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retrySeconds, retryAttempt)),
                        (result, timeSpan, context) =>
                        {
                            if (result.Exception != null)
                            {
                                _logger.LogError(result.Exception, result.Exception.GetErrorMsg());
                            }
                            if (result.Result != null)
                            {
                                LogHttpResponseMessage(result.Result);
                            }
                        });

            return retry;
        }

        public AsyncPolicy<IRestResponse> RetryPolicyAsync(int retryCount, int retrySeconds)
        {
            var retry = Polly.Policy
                        .Handle<Exception>()
                        .OrResult<IRestResponse>(result => !(((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299) ||
                                                            (_noRetryHttpStatusCodes.Contains((int)result.StatusCode))))
                        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(retrySeconds, retryAttempt)),
                         async (result, timeSpan, context) =>
                         {
                             if (result.Exception != null)
                             {
                                 _logger.LogError(result.Exception, result.Exception.GetErrorMsg());
                             }
                             if (result.Result != null)
                             {
                                 LogHttpResponseMessage(result.Result);
                             }
                         });

            return retry;
        }

        private static (bool IsError, string Message) GetHttpResponseLog(IRestResponse response)
        {
            var message = String.Join(String.Empty,
                                        "[RestRetry] ==> ",
                                        $"HTTP({response.StatusCode}) ",
                                        $"'{response.ResponseUri.OriginalString}' : ",
                                        $"'{response.Content}'");

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                message = $"{message} [HTTP ERROR]: '{response.ErrorMessage}'";
                return (true, message);
            }
            else
            {
                return (false, message);
            }
        }

        private void LogHttpResponseMessage(IRestResponse response)
        {
            var result = GetHttpResponseLog(response);

            if (result.IsError)
            {
                _logger.LogError(result.Message);
            }
            else
            {
                _logger.LogWarning(result.Message);
            }
        }
    }
}