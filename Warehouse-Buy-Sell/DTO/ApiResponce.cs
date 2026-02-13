namespace Warehouse_Buy_Sell.DTO
{
    public class ApiResponce<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponce<T> Ok(T data, string message = "Success") =>
            new ApiResponce<T> { Success = true, Message = message, Data = data };

        public static ApiResponce<T> Fail(string message) =>
            new ApiResponce<T> { Success = false, Message = message };
    }
}
