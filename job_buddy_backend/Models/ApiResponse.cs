namespace job_buddy_backend.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public List<string> Errors { get; private set; }

        // Private constructor ensures that ApiResponse cannot be created directly
        private ApiResponse(bool success, string message, T data = default, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        // Factory method for success response where we pass data and a optional message
        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>(true, message, data);
        }

        // Factory method for failure response where we send error message
        public static ApiResponse<T> FailureResponse(string errorMessage)
        {
            return new ApiResponse<T>(false, errorMessage, default, new List<string> { errorMessage });
        }

        // Factory method for failure with multiple errors messages
        public static ApiResponse<T> FailureResponse(List<string> errors)
        {
            return new ApiResponse<T>(false, string.Join(", ", errors), default, errors);
        }
    }

}
