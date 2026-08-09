using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Core;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IActionFilter
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
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterNeme}.{MethodName] method",
                nameof(PersonsListActionFilter),
                nameof(OnActionExecuted)
            );
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterNeme}.{MethodName] method",
                nameof(PersonsListActionFilter),
                nameof(OnActionExecuting)
            );

            context.HttpContext.Response.Headers[_key] = _value; 
        }
    }
}
