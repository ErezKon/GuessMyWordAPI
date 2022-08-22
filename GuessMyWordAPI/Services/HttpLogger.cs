using GuessMyWordAPI.IServices;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace GuessMyWordAPI.Services
{
    public class HttpLogger
    {
        private readonly IMyLogger _logger;

        private string path;

        public HttpLogger(IMyLogger logger)
        {
            _logger = logger;

            

        }
        public void Log(ActionExecutingContext context)
        {
            var action = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;
            var reqPath = $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            var IP = $"{context.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}:{context.HttpContext.Connection.RemotePort}";

            var headerKeys = context.HttpContext.Request.Headers.Keys;
            var headers = new Dictionary<string, string>();
            foreach (var key in headerKeys)
            {
                headers[key] = context.HttpContext.Request.Headers[key];
            }

            var data = JsonConvert.SerializeObject(new
            {
                Path = reqPath,
                IP = IP,
                Headers = headers,
            }, Formatting.Indented);

            CreateFile(action);

            File.WriteAllText(path, data);
        }
        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            string requestObject;
            context.Request.EnableBuffering();
            context.Response.HttpContext.Request.EnableBuffering();
            var stream = new MemoryStream();
            context.Response.HttpContext.Request.Body.Position = 0;
            await context.Response.HttpContext.Request.Body.CopyToAsync(stream);
            stream.Position = 0;
            using (var bodyReader = new StreamReader(stream))
            {
                requestObject = await bodyReader.ReadToEndAsync();
            }
            return requestObject;
        }
        public async Task LogAsync(ActionExecutedContext context)
        {
            string responseObject;
            if (context.Result != null)
            {
                responseObject =
                    JsonConvert.SerializeObject(((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value, Formatting.Indented);
            } else
            {
                responseObject = "{}";
            }
            string requestObject = await ReadRequestBodyAsync(context.HttpContext);
            
            var action = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;
            var reqPath = $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            var IP = $"{context.HttpContext.Connection.RemoteIpAddress.MapToIPv4()}:{context.HttpContext.Connection.RemotePort}";
            
            var reqHeaders = ReadHeaders(context.HttpContext.Request.Headers);

            var reqData = JsonConvert.SerializeObject(new
            {
                Path = reqPath,
                IP = IP,
                Headers = reqHeaders,
                Request = JsonConvert.DeserializeObject(requestObject),
                Response = JsonConvert.DeserializeObject(responseObject)
            }, Formatting.Indented);

            CreateFile(action);

            _ = File.WriteAllTextAsync(path, reqData);
            
        }

        private Dictionary<string, string> ReadHeaders(IHeaderDictionary headers)
        {
            var keys = headers.Keys;
            var headersDic = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                headersDic[key] = headers[key];
            }
            return headersDic;
        }

        private void CreateFile(string action)
        {
            var logsDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "HTTP");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            path = Path.Combine(logsDir, $"{action}-{DateTime.Now:ddMMyyyy.HHmmss.fff}.txt");
            _logger.Info($"Creating HTTP Log at path: {path}");
            try
            {
                var file = File.Create(path);
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create file:\n{ex.ToString()}");
            }
        }
    }
}
