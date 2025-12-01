namespace LoanManagementSystem.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public T? Data { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public static ApiResponse<T> CreateSuccess(T data, string messsage = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = messsage,
                Data = data
            };
        }

        public static ApiResponse<T> CreateError(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message
            };
        }

    }


    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public static ApiResponse CreateSuccess(string message = "")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        public static ApiResponse CreateError(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }
    }

   


}
