
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO.Compression;

namespace demoAPI.Middleware
{
    /// <summary>
    /// Attribute for enabling Brotli/GZip/Deflate compression for specied action
    /// </summary>
    public class ResponseCompressionAttribute : ActionFilterAttribute
    {
        //private Stream _originStream = null;
        //private MemoryStream _ms = null;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding)) return;
            acceptEncoding = acceptEncoding.ToUpperInvariant();
            HttpResponse response = context.HttpContext.Response;
            //if (acceptEncoding.Contains("BR", StringComparison.OrdinalIgnoreCase))//Brotli 
            //{
            //    if (!(response.Body is BrotliStream))// avoid twice compression.
            //    {
            //        _originStream = response.Body;
            //        _ms = new MemoryStream();
            //        response.Headers.Add("Content-encoding", "br");
            //        response.Body = new BrotliStream(_ms, CompressionLevel.Optimal);
            //    }
            //}
            if (acceptEncoding.Contains("GZIP", StringComparison.OrdinalIgnoreCase))
            {
                if (!(response.Body is GZipStream))
                {
                    var _originStream = response.Body;
                    var _ms = new MemoryStream();

                    response.HttpContext.Items.Add("_originStream", _originStream);
                    response.HttpContext.Items.Add("_ms", _ms);

                    response.Headers.Add("Content-Encoding", "gzip");
                    response.Body = new GZipStream(_ms, CompressionLevel.Optimal);
                }
            }
            //else if (acceptEncoding.Contains("DEFLATE", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (!(response.Body is DeflateStream))
            //    {
            //        _originStream = response.Body;
            //        _ms = new MemoryStream();
            //        response.Headers.Add("Content-encoding", "deflate");
            //        response.Body = new DeflateStream(_ms, CompressionLevel.Optimal);                    
            //    }
            //}
            base.OnActionExecuting(context);
        }

        public override async void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                HttpResponse response = context.HttpContext.Response;

                if ((response.HttpContext.Items["_originStream"] != null) && (response.HttpContext.Items["_ms"] != null))
                {
                    Stream _originStream = (Stream)response.HttpContext.Items["_originStream"];
                    MemoryStream _ms = (MemoryStream)response.HttpContext.Items["_ms"];

                    await response.Body.FlushAsync();
                    _ms.Seek(0, SeekOrigin.Begin);
                    response.Headers.ContentLength = _ms.Length;
                    await _ms.CopyToAsync(_originStream);
                    response.Body.Dispose();
                    _ms.Dispose();
                    response.Body = _originStream;
                }
            }
            catch (Exception ex)
            {

            }
            
            base.OnResultExecuted(context);
        }
    }
}
