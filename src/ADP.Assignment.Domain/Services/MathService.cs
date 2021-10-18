using ADP.Assignment.Common.Extensions;
using ADP.Assignment.Common.Rest.Interfaces;
using ADP.Assignment.Domain.Interfaces;
using ADP.Assignment.Domain.Models;
using ADP.Assignment.Domain.Providers;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ADP.Assignment.Domain.Services
{
    public class MathService : IMathService
    {
        private readonly MathOptions _mathOptions;
        private readonly IRestService _restService;

        public MathService(IOptions<MathOptions> mathOptions, IRestService restService)
        {
            _mathOptions = mathOptions.Value ?? throw new ArgumentNullException(nameof(mathOptions));
            _restService = restService ?? throw new ArgumentNullException(nameof(restService));
        }

        public async Task<MathResult> CalculateInstructionAsync()
        {
            var mathOperation = await GetMathOperation();
            var mathOperationResult = ExecuteMathOperation(mathOperation);
            var mathResult = await SubmitMathResult(mathOperationResult);

            mathResult.MathOperation = mathOperation;
            mathResult.MathOperationResult = mathOperationResult;

            return mathResult;
        }

        private async Task<MathResult> SubmitMathResult(MathOperationResult mathOperationResult)
        {
            var httpResult = await _restService.PostRequestAsync(_mathOptions.UrlBase, _mathOptions.ResourceSubmitTask, mathOperationResult.ToJson());
            var mathResult = new MathResult();            

            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                mathResult.IsSuccess = true;
            }
            else
            {
                mathResult.IsSuccess = false;
                mathResult.ErrorInformation = $"Invalid http response '{httpResult.StatusCode}' -> '{httpResult.Content}'";
            }

            return mathResult;
        }

        private async Task<MathOperation> GetMathOperation()
        {
            var httpResult = await _restService.GetRequestAsync(_mathOptions.UrlBase, _mathOptions.ResourceGetTask);

            if (httpResult.StatusCode != HttpStatusCode.OK) throw new ApplicationException($"Invalid http response '{httpResult.StatusCode}' -> '{httpResult.Content}'");

            return httpResult.Content.ToObject<MathOperation>();
        }

        private MathOperationResult ExecuteMathOperation(MathOperation mathOperation)
        {
            var mathResult = new MathOperationResult();

            switch (mathOperation.Operation)
            {
                case "division":
                    mathResult.Id = mathOperation.Id;
                    mathResult.Result = (double)mathOperation.Left / (double)mathOperation.Right;
                    break;
                case "subtraction":
                    mathResult.Id = mathOperation.Id;
                    mathResult.Result = mathOperation.Left - mathOperation.Right;
                    break;
                case "multiplication":
                    mathResult.Id = mathOperation.Id;
                    mathResult.Result = (double)mathOperation.Left * (double)mathOperation.Right;
                    break;
                case "addition":
                    mathResult.Id = mathOperation.Id;
                    mathResult.Result = mathOperation.Left + mathOperation.Right;
                    break;
                case "remainder":
                    mathResult.Id = mathOperation.Id;
                    mathResult.Result = mathOperation.Left % mathOperation.Right;
                    break;
                default:
                    throw new ApplicationException($"Invalid operation '{mathOperation.Operation}'");
            }

            return mathResult;
        }
    }
}