using NeteaseMusic.Helpers;
using NeteaseMusic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MusicDownloader
{
    public static class Xiami2
    {
        //一段时间之内只能请求一次，不然IP就会被封;目前还没找到解决方法 --废弃
        public static string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("http://api.xiami.com/web");
            hp.Referer = "http://h.xiami.com/";
            hp.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.75 Safari/537.36";
            hp.Data = new Dictionary<string, object>()
            {
                {"key" , HttpUtility.UrlEncode(name) },
                { "v" , "2.0" },
                {"app_key", "1" },
                {"r","search/songs" },
                { "page" , page },
                { "limit" , pageSize },
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        public static List<string> GetSongUrl(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            var songsUrl =  jObj["data"]["songs"].Select(e => (string)e["listen_file"]);
            //替换成更高硬质的Url
            songsUrl.Where(e => !e.Contains("_l.")).ToList().ForEach(e => e.Replace("m128", "m320"));
            return songsUrl.ToList();
        }

        public static List<string> GetSongUrl(string name, int page = 1, int pageSize = 1)
        {
            return GetSongUrl(Search(name, page, pageSize));
        }
    }
}
