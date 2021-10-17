using Polly;
using RestSharp;

namespace ADP.Assignment.Common.Rest.Interfaces
{
    public interface IRestPolicy
    {
        Policy<IRestResponse> RetryPolicy(int retryCount, int retrySeconds);

        AsyncPolicy<IRestResponse> RetryPolicyAsync(int retryCount, int retrySeconds);
    }
}