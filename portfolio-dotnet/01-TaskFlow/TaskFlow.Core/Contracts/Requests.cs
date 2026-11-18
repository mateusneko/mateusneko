using System.ComponentModel.DataAnnotations;
using TaskFlow.Core.Domain;
namespace TaskFlow.Core.Contracts;
public sealed record RegisterRequest(
    [property: Required, StringLength(80, MinimumLength = 2)] string Name,
    [property: Required, EmailAddress] string Email,
    [property: Required, MinLength(8)] string Password);
public sealed record LoginRequest([property: Required] string Email, [property: Required] string Password);
public sealed record AuthResponse(string AccessToken, string TokenType, DateTime ExpiresAtUtc);
public sealed record CreateTaskRequest([property: Required, StringLength(120)] string Title,
    [property: StringLength(1000)] string? Description, DateTime? DueAtUtc);
public sealed record UpdateTaskRequest([property: Required, StringLength(120)] string Title,
    [property: StringLength(1000)] string? Description, DateTime? DueAtUtc);
public sealed record ChangeStatusRequest(WorkStatus Status);
public sealed record TaskResponse(Guid Id, string Title, string? Description, WorkStatus Status,
    DateTime CreatedAtUtc, DateTime? DueAtUtc);
public sealed record PageResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int Total);
