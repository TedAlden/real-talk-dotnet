using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Maui.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public ErrorType ErrorType { get; set; }
    }

    public enum ErrorType
    {
        None,
        Timeout,
        ServerError,
        NoInternet,
        Unauthorized,
        SerializationError,
        Unknown,
        Cancelled
    }
}
