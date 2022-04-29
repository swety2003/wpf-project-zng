﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DefaultWidgets.Utils
{
    public class Request
    {
        private HttpClient httpClient;

        public Request()
        {
            HttpClientHandler handler = new HttpClientHandler() { UseCookies = true };
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.74 Safari/537.36 Edg/99.0.1150.46");

            //httpClient.DefaultRequestHeaders.Add("Cookie", User.Cookie);

            httpClient.Timeout = new TimeSpan(0, 0, 0, 300); //超时时间             
        }

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public async Task<string> Get(string url, Dictionary<string, string> dic = null)

        {
            HttpResponseMessage response;
            //参数添加
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic != null)
            {

                if (dic.Count > 0)
                {
                    builder.Append("?");
                    int i = 0;
                    foreach (var item in dic)
                    {
                        if (i > 0)
                            builder.Append("&");
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                        i++;
                    }
                }
            }

            try
            {

                response = await httpClient.GetAsync(new Uri(builder.ToString()));
                Console.WriteLine("URL:" + url);
            }
            catch
            {
                return null;
            }
            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("result-->" + result);

            return result;
        }
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public async Task<string> Post(string url, List<KeyValuePair<String, String>> paramList)
        {

            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList));
                Console.WriteLine("URL:" + url);
            }
            catch
            {
                return null;
            }
            string result = await response.Content.ReadAsStringAsync();

            if (!result.Contains("status"))
            {
                return null;
            }
            return result;
        }
        /// <summary>
        /// json格式上传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<string> PostJson(string url, string json)
        {
            string responseBody;
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(url), content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
            return responseBody;
        }

    }
}
