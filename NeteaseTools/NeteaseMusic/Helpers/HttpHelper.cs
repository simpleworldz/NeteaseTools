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

namespace NeteaseMusic.Helpers
{
    public static class HttpHelper
    {
        static CookieContainer cookieContainer = new CookieContainer();
        static string  cookieFile = "Cookie/Test.ck";
        static HttpHelper()
        {
            //var cookieCon3 = ReadCookiesFromDisk();
            cookieContainer.Add(new Uri("https://music.163.com"), new Cookie("Lang", "English"));
        }
        public static async Task<string> PostAsync(HttpParams hp)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hp.Url);
            MappingHeader(request, hp);
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            //要吗 不能直接这样，会把 = 一并给encode了
            //string data = HttpUtility.UrlEncode(hp.Data); 
            byte[] daby = Encoding.ASCII.GetBytes(DataDic2Str(hp.Data));
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(daby, 0, daby.Length);
            }
            //以下貌似也可以合并。
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            //保存cookie
            response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            var xx1 = cookieContainer;
            WriteCookiesToDisk();
            var xx = ReadCookiesFromDisk();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
            //Stream myResposeStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResposeStream);
            //string responseStr = myStreamReader.ReadToEnd();
            //myStreamReader.Close();
            //myResposeStream.Close();
        }

        public static void CookieSerializeTest()
        {
            CookieContainer cookieCon = new CookieContainer();
            cookieCon.Add(new Uri("http://www.example.com"), new Cookie("name", "value"));
            //var cookieStr = JsonConvert.SerializeObject(cookieCon);
            //var cookieCon2 = JsonConvert.DeserializeObject<CookieContainer>(cookieStr);
            //var file = "Cookie/Test.ck";
            //WriteCookiesToDisk(file, cookieCon);
            //var cookieCon3 = ReadCookiesFromDisk(file);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="connect">连接字符 ？ or #</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(HttpParams hp, string connect = "?")
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hp.Url + connect + DataDic2Str(hp.Data));
            //request.AllowAutoRedirect = true;
            request.Method = "GET";
            MappingHeader(request, hp);
            //request.ContentType = "text/html; charset=utf-8";
            //request.Referer = hp.Referer;
            //request.UserAgent = hp.Agent;
            //request.Headers.Add(HttpRequestHeader.Cookie, "user_from=2;XMPLAYER_addSongsToggler=0;XMPLAYER_isOpen=0;_xiamitoken=cb8bfadfe130abdbf5e2282c30f0b39a");
            // request.Timeout = 15000;
            request.CookieContainer = cookieContainer;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public static async Task<bool> TeskAsync(HttpParams hp)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hp.Url);
            request.AddRange(1);
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                request.Abort();
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static void MappingHeader(HttpWebRequest request, HttpParams hp)
        {
            request.ContentType = hp.ContentType;
            request.Referer = hp.Referer;
            request.UserAgent = hp.UserAgent;
        }
        public static string DataDic2Str(Dictionary<string, string> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
