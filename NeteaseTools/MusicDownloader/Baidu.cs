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
   public  class Baidu:IMusic
    {
        public  string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("http://musicapi.qianqian.com/v1/restserver/ting");
            hp.Referer = "http://music.baidu.com/";
            hp.Data = new Dictionary<string, object>()
            {
                { "method" , "baidu.ting.search.common" },
                //{"query" , HttpUtility.UrlEncode(name) },
                {"query" , name },
                {"format", "json" },
                {"page_size",pageSize },
                { "page_no" , page }
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        public  string GetSongUrlInfo(List<int> ids)
        {
            HttpParams hp = new HttpParams("http://music.baidu.com/data/music/links");
            hp.Referer = "music.baidu.com/song/";
            hp.Data = new Dictionary<string, object>()
            {
                {"songIds",JsonConvert.SerializeObject(ids) }
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        private  List<int> GetSongId(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["song_list"].Select(e => (int)e["song_id"]).ToList();
        }
        private  List<string> GetSongUrl(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["data"]["songList"].Select(e => (string)e["songLink"]).ToList();
        }

        public  List<string> GetSongUrl(string name, int page = 1, int pageSize = 1)
        {
            return GetSongUrl(GetSongUrlInfo(GetSongId(Search(name, page, pageSize))));
        }

        public string GetFirstUrl(string name)
        {
            return GetSongUrl(name,1,1)[0];
        }

    }
}
