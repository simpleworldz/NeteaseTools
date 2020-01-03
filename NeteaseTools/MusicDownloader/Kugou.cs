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
   public class Kugou:IMusic
    {
        public string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("http://songsearch.kugou.com/song_search_v2");
            hp.Referer = "http://www.kugou.com/yy/html/search.html";
            hp.Data = new Dictionary<string, object>()
            {
                { "platform" , "WebFilter" },
                {"keyword" , name},
                {"format", "json" },
                {"pagesize",pageSize },
                { "page" , page }
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        private List<string> GetSongId(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["data"]["lists"].Select(e => 
            !string.IsNullOrEmpty((string)e["HQFileHash"])? (string)e["HQFileHash"]: (string)e["FileHash"]
            ).ToList();
        }
        public string GetSongUrlInfo(string id)
        {
            HttpParams hp = new HttpParams("http://m.kugou.com/app/i/getSongInfo.php");
            hp.Referer = "http://m.kugou.com/play/info/" + id;
            hp.Data = new Dictionary<string, object>()
            {
                { "cmd" , "playInfo" },
                {"hash" , id},
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        private string GetSongUrl(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return (string)jObj["url"];
        }
        //public string GetSongUrl(string name, int page = 1, int pageSize = 1)
        //{
        //    return GetSongUrl(GetSongUrlInfo(GetSongId(Search(name, page, pageSize))));
        //}

        public string GetFirstUrl(string name)
        {
           return GetSongUrl(GetSongUrlInfo(GetSongId(Search(name, 1, 1))[0]));
        }
    }
}
