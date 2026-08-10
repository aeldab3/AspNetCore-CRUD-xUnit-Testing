using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Core;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
        {
            _logger = logger;
            _key = key;
            _value = value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterNeme}.{MethodName] method - before ",
                nameof(PersonsListActionFilter),
                nameof(OnActionExecutionAsync)
            );

            await next();

            _logger.LogInformation("{FilterNeme}.{MethodName] method - after",
                nameof(PersonsListActionFilter),
                nameof(OnActionExecutionAsync)
            );

            context.HttpContext.Response.Headers[_key] = _value;
        }
    }
}
