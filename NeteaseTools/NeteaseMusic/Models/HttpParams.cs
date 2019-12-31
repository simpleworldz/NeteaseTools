using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NeteaseMusic.Models
{
   public  class HttpParams
    {
        public HttpParams(string url)
        {
            Url = url;
        }
        public HttpParams()
        {
        }
        public string Url { get; set; }
        //public WebHeaderCollection Headers { get; set; }
        public Dictionary<string,object> Data{ get; set; }
        //public string Method { get; set; }
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.79 Safari/537.36";
        public string Referer { get; set; } //= "https://music.163.com";
    }
}
