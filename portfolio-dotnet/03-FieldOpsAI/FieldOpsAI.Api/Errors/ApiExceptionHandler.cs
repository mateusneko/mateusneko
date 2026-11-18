using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FieldOpsAI.Core.Exceptions;
namespace FieldOpsAI.Api.Errors;
public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var status = exception is AppException appError ? appError.HttpStatus : 500;
        if (status == 500) logger.LogError(exception, "Erro inesperado");
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = status == 500 ? "Erro interno" : "Requisição não concluída",
            Detail = status == 500 ? "Ocorreu um erro inesperado." : exception.Message
        }, cancellationToken: ct);
        return true;
    }
}
