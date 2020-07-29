using Newtonsoft.Json;

namespace JWTPolicyBasedAuthorization.Dtos
{
    public abstract class ResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class ResponseDto<T> : ResponseDto
    {
        public T Data { get; set; }
    }

    public class ErrorDto : ResponseDto
    {
        public int StatusCode { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }    
}