namespace MyTemplate.Application.DTOs.Common;

/// <summary>
/// Standardized API response.
/// Used to uniformize API responses.
/// </summary>
/// <typeparam name="T">Type of returned data</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Returned data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// List of errors (if failed)
    /// </summary>
    public List<string> Errors { get; set; } = [];

    // ============================================================
    // FACTORY METHODS
    // ============================================================

    public static ApiResponse<T> SuccessResult(T data, string message = "Operation successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> FailureResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? []
        };
    }
}

/// <summary>
/// API response without data.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// List of errors (if failed)
    /// </summary>
    public List<string> Errors { get; set; } = [];

    public static ApiResponse SuccessResponse(string message = "Operation successful")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    public static ApiResponse Failure(string message, List<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? []
        };
    }
}
