using System.ComponentModel.DataAnnotations;
using FieldOpsAI.Core.Domain;
namespace FieldOpsAI.Core.Contracts;
public sealed record CreateIncidentRequest(
    [property: Required, StringLength(160)] string Location,
    [property: Required, StringLength(2000)] string Description);
public sealed record IncidentResponse(Guid Id, string Location, string Description,
    DateTime CreatedAtUtc, IncidentStatus Status, IncidentPriority? Priority,
    IncidentCategory? Category, string? Summary, string? Guidance, string? TriageSource);
