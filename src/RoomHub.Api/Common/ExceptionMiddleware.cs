namespace RoomHub.Api.Common;

// Business errors are Result values; this only catches what nobody expected.
public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException ex)
        {
            // In Development, minimal APIs throw for unreadable request bodies; keep that a 400, not a 500.
            await Results.Problem(
                    detail: ex.Message,
                    statusCode: ex.StatusCode)
                .ExecuteAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await Results.Problem(
                    title: "An unexpected error occurred.",
                    statusCode: StatusCodes.Status500InternalServerError)
                .ExecuteAsync(context);
        }
    }
}
