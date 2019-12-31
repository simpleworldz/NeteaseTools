using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using NeteaseMusic.Models;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Reflection;

namespace NeteaseMusic.Helpers
{
    public static class HttpHelper
    {
        static CookieContainer cookieContainer = new CookieContainer();
        static string  cookieFile = "Cookie/Cookie.ck";
        static HttpHelper()
        {
            cookieContainer = ReadCookiesFromDisk();
            //var cookies = cookieContainer.List();
            //cookieContainer.Add(new Uri("https://music.163.com"), new Cookie("Lang", "English"));
            //cookieContainer.Add(new Uri("https://music.163.com"), new Cookie("os", "pc"));
        }
       
        public static async Task<string> PostAsync(HttpParams hp)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hp.Url);
            MappingHeader(request, hp);
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            //request.Proxy = new WebProxy(new Uri("http://127.0.0.1:1080"));
            //不能直接这样，会把 “=” 一并给encode了
            //string data = HttpUtility.UrlEncode(hp.Data); 
            byte[] daby = Encoding.ASCII.GetBytes(DataDic2Str(hp.Data));
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(daby, 0, daby.Length);
            }
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            //保存cookie
            response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="connect">连接字符 ？ or #</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(HttpParams hp, string connect = "?")
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hp.Url + connect + DataDic2Str(hp.Data));
            request.Method = "GET";
            MappingHeader(request, hp);
            request.CookieContainer = cookieContainer;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
       
        //https://stackoverflow.com/questions/1777203/c-writing-a-cookiecontainer-to-disk-and-loading-back-in-for-use
        public static void WriteCookiesToDisk()
        {
            WriteCookiesToDisk(cookieFile,cookieContainer);
        }
        public static void WriteCookiesToDisk(string file, CookieContainer cookieJar)
        {
            file = FileHelper.PathHander(file,true);
            using (Stream stream = File.Create(file))
            {
                try
                {
                    //Console.Out.Write("Writing cookies to disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                    //Console.Out.WriteLine("Done.");
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
                }
            }
        }
        public static CookieContainer ReadCookiesFromDisk()
        {
            return ReadCookiesFromDisk(cookieFile);
        }
        public static CookieContainer ReadCookiesFromDisk(string file)
        {

            try
            {
                file = FileHelper.PathHander(file);
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    // Console.Out.Write("Reading cookies from disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    // Console.Out.WriteLine("Done.");
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
                return new CookieContainer();
            }
        }
      
        private static void MappingHeader(HttpWebRequest request, HttpParams hp)
        {
            request.ContentType = hp.ContentType;
            request.Referer = hp.Referer;
            request.UserAgent = hp.UserAgent;
        }
        public static string DataDic2Str(Dictionary<string, object> data)
        {
            if (data == null || data.Count == 0 )
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        //https://stackoverflow.com/questions/13675154/how-to-get-cookies-info-inside-of-a-cookiecontainer-all-of-them-not-for-a-spe
        public static List<Cookie> List(this CookieContainer container)
        {
            var cookies = new List<Cookie>();

            var table = (Hashtable)container.GetType().InvokeMember("m_domainTable",
                BindingFlags.NonPublic |
                BindingFlags.GetField |
                BindingFlags.Instance,
                null,
                container,
                null);

            foreach (string key in table.Keys)
            {
                var item = table[key];
                var items = (ICollection)item.GetType().GetProperty("Values").GetGetMethod().Invoke(item, null);
                foreach (CookieCollection cc in items)
                {
                    foreach (Cookie cookie in cc)
                    {
                        cookies.Add(cookie);
                    }
                }
            }

            return cookies;
        }
    }
}
