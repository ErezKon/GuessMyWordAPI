using GuessMyWordAPI.IServices;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace GuessMyWordAPI.Services
{
    public class HttpLoggerActionFilter: IActionFilter
    {
        private readonly IMyLogger _logger;
        public HttpLoggerActionFilter(IMyLogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var httpLogger = new HttpLogger(_logger);
            //httpLogger.Log(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpLogger = new HttpLogger(_logger);
            
            httpLogger.LogAsync(context);
        }
    }
}
