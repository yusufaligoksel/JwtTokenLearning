using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JwtTokenLearning.Business.Dto
{
    public class Response<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        /// <summary>
        /// Projede kendi kullanımımız için
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful { get; set; }
        public ErrorDto Error { get; set; }
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        //boş data dönmek için
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(ErrorDto error, int statusCode)
        {
            return new Response<T> { Error = error, StatusCode = statusCode, IsSuccessful = false };
        }

        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var error = new ErrorDto(errorMessage, isShow);

            return new Response<T> { Error = error, IsSuccessful = false, StatusCode = statusCode };

        }
    }
}
