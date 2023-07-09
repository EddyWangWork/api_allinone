using Newtonsoft.Json;
using System.Net;

namespace demoAPI.Middleware
{
    public class ExMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ExMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.BadRequest;

            return (code, exception.Message);
        }
    }
}
