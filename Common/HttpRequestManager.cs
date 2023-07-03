using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Http请求类型
    /// </summary>
    public enum HttpType
    {
        POST,
        GET,
        DELETE,
        PUT,
        OPTIONS
    }

    /// <summary>
    /// Http请求管理类
    /// </summary>
    public class HttpRequestManager
    {
        /// <summary>
        /// 发送HTTP请求并返回结果
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <param name="httpType">http请求方式[POST,GET,DELETE]</param>
        /// <param name="contenType">响应类型</param>
        /// <param name="data">参数</param>
        /// <param name="token">token</param>
        /// <param name="_successCallBack">成功回调函数</param>
        /// <param name="_failCallBack">失败回调函数</param>
        public static void HttpRespons(
            string url,
            HttpType httpType,
            string contenType,
            out string _successResult,
            out string _failResult,
            string data = "",
            string token = null
            )
        {
            _successResult = string.Empty;
            _failResult = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = httpType.ToString();
                request.ContentType = contenType;
                if (token != null)
                {
                    request.Headers.Add("Authorization", "Bearer " + token);
                }
                // Send the data.   

                ASCIIEncoding encoding = new ASCIIEncoding();
                Stream newStream = null;
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] postdata = encoding.GetBytes(data);
                    newStream = request.GetRequestStream();
                    newStream.Write(postdata, 0, data.Length);
                    newStream.Close();
                }
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    _successResult=result;
                }
            }
            catch(Exception ex)
            {
                _failResult=ex.Message;
            }
        }
    }
}
