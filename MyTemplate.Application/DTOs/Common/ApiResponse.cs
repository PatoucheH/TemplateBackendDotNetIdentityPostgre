namespace MyTemplate.Application.DTOs.Common;

/// <summary>
/// Réponse API standardisée.
/// Utilisé pour uniformiser les réponses de l'API.
/// </summary>
/// <typeparam name="T">Type des données retournées</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indique si la requête a réussi
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message de résultat
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Données retournées
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Liste des erreurs (si échec)
    /// </summary>
    public List<string> Errors { get; set; } = [];

    // ============================================================
    // FACTORY METHODS
    // ============================================================

    public static ApiResponse<T> SuccessResult(T data, string message = "Opération réussie")
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
/// Réponse API sans données.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indique si la requête a réussi
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message de résultat
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Liste des erreurs (si échec)
    /// </summary>
    public List<string> Errors { get; set; } = [];

    public static ApiResponse SuccessResponse(string message = "Opération réussie")
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
