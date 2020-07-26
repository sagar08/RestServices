namespace JWTPolicyBasedAuthorization.Dtos
{
    public class ResponseDto<T>
    {
        public T ResponseData { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class ResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}