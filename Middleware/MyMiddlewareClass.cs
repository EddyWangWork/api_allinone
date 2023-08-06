using demoAPI.BLL.Member;
using demoAPI.BLL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace demoAPI.Middleware
{
    public class MyMiddlewareClass
    {
        RequestDelegate _next;

        public MyMiddlewareClass(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IMemberBLL memberBLL)
        {
            List<string> whitePages = new List<string>
            {
                "/Member/login",
                "/Member/signup",
                "/Common/getDSTransTypes",
                "/Common/getTodolistTypes",
            };

            if (whitePages.Contains(context.Request.Path.Value))
            {
                await _next.Invoke(context);
                return;
            }

            StringValues authorizationToken = string.Empty;
            context.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            if (authorizationToken.Count > 0)
            {
                var token = authorizationToken.ToString();
                var member = memberBLL.GetMemberByToken(token);

                if (member == null || IsExpires(member.LastLoginDate))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Token expired, Please re-login."
                    }.ToString());

                    return;
                }

                BaseBLL.MemberId = member.ID;
                memberBLL.UpdateMemberSession();
            }
            else
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Please Login"
                }.ToString());

                return;
            }

            await _next.Invoke(context);
        }

        private bool IsExpires(DateTime loginDatetime)
        {
            return loginDatetime.AddHours(1) <= DateTime.Now;
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddlewareClass>();
        }
    }
}
