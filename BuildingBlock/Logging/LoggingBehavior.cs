using BuildingBlock.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAppLogger<LoggingBehavior<TRequest, TResponse>> _logger;

        //private readonly Serilog.ILogger _logger;

        public LoggingBehavior(IAppLogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);

            var requestName = typeof(TRequest).Name;
            var responseName = typeof(TResponse).Name;
            var requestData = request;

            _logger.LogInformation("[{@Prefix}] Handle request={@RequestName} and response={@responseName} with requestData={@RequestData}",
                prefix, requestName, responseName, requestData.ToJsonV2());

            var timer = new Stopwatch();
            timer.Start();

            var responseData = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
                _logger.LogWarning("[{@PerfPossible}] The request {@XRequestData} took {@TimeTaken} seconds.",
                    prefix, typeof(TRequest).Name, timeTaken.Seconds);

            _logger?.LogInformation("[{@Prefix}] Handled request={@RequestName} and  response={@ResponseName}, responsedata={@ResponseData}",
               prefix, requestName, responseName, responseData.ToJsonV2());

            return responseData;
        }
    }

}
