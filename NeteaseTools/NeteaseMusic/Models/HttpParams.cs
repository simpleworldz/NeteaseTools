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
        public Dictionary<string,string> Data{ get; set; }
        //public string Method { get; set; }
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        //当UserAgent为这个时如果没有cookie 不管改歌曲是否下架privilege的st都是-100，加了cookie后 下架 -200,非下架 0；
        //public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.79 Safari/537.36";
        //这个则不用cookie，privilege信息也正常;但无法获取从网易云盘中收藏的歌曲
        //https://github.com/Binaryify/NeteaseCloudMusicApi
        public string UserAgent { get; set; } = "Mozilla / 5.0(iPad; CPU OS 10_0 like Mac OS X) AppleWebKit/602.1.38 (KHTML, like Gecko) Version/10.0 Mobile/14A300 Safari/602.1";
        public string Referer { get; set; } 
    }
}
