﻿namespace Api.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    private ApiResponse()
    {

    }

    public static ApiResponse<T> BuildSuccess(T data)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Error = string.Empty,
            Message = string.Empty,
            Success = true
        };
    }
}
