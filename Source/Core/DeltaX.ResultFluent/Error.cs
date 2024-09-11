namespace DeltaX.ResultFluent;

public record Error(
    string Code,
    string Message,
    string? Detail = null)
{
    public static Error Create(string code, string message, string? detail = null) => new Error(code, message, detail);
    public static Error InvalidArgument(string message, string? detail = null) => new Error("InvalidArgument", message, detail);
    public static Error InternalError(string message, string? detail = null) => new Error("InternalError", message, detail);
    public static Error NotFound(string message, string? detail = null) => new Error("NotFound", message, detail);
    public static Error Unauthorized(string message, string? detail = null) => new Error("Unauthorized", message, detail);
    public static Error Forbidden(string message, string? detail = null) => new Error("Forbidden", message, detail);
    public static Error Conflict(string message, string? detail = null) => new Error("Conflict", message, detail);
    public static Error Unhandled(string message, string? detail = null) => new Error("Unhandled", message, detail);
}
