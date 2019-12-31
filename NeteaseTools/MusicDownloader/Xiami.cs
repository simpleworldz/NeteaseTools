using NeteaseMusic.Helpers;
using NeteaseMusic.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDownloader
{
   public static class Xiami
    {
        //https://github.com/LIU9293/musicAPI
        public static string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("https://music-api-jwzcyzizya.now.sh/api/search/song/xiami");
            hp.Referer = "http://h.xiami.com/";
            hp.UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.75 Safari/537.36";
            hp.Data = new Dictionary<string, object>()
            {
                {"key" , name },
                { "page" , page },
                { "limit" , pageSize },
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        public static List<string> GetSongUrl(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            var songsUrl = jObj["songList"].Select(e => (string)e["file"]);
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
