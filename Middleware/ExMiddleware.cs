using demoAPI.Model.Exceptions;
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

            switch (exception)
            {
                case KeyNotFoundException
                    or FileNotFoundException
                    or NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException
                    or InvalidOperationException
                    or BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            return (code, exception.Message);
        }
    }
}
