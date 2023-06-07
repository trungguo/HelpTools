using System.Collections.Generic;

namespace SaleDXGui.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Response(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public T Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseList<T> : Response
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public ResponseList(bool success, string message, List<T> data, long total) : base(success, message)
        {
            Data = data;
            Total = total;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public List<T> Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public long Total { get; set; }
    }
}
