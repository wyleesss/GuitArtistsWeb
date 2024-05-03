using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

[AttributeUsage(AttributeTargets.Method)]
public class RequestLimitAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _seconds;
    private readonly Dictionary<string, DateTime> _requestDictionary = new Dictionary<string, DateTime>();

    public RequestLimitAttribute(int seconds)
    {
        _seconds = seconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
        var key = $"{ipAddress}-{context.ActionDescriptor.DisplayName}";

        if (_requestDictionary.TryGetValue(key, out var lastRequestTime) && DateTime.Now < lastRequestTime.AddSeconds(_seconds))
        {
            context.Result = new ContentResult { Content = "Too many requests. Please try again later.", StatusCode = 429 };
            return;
        }

        _requestDictionary[key] = DateTime.Now;

        await next();
    }
}