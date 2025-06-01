using System.Net;
using System.Text.Json;

namespace MyBlog.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                httpContext,
                HttpStatusCode.InternalServerError,
                ex);
        }
    }
    private async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        }.ToString());
    }
}
public class ErrorDetails
{
    public required int StatusCode { get; set; }
    public required string Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}