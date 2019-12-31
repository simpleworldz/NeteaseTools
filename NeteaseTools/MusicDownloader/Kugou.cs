using NeteaseMusic.Helpers;
using NeteaseMusic.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MusicDownloader
{
   public class Kugou
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
        //public override MusicInfo[] GetSongsByIds(string[] ids)
        //{
        //    List<MusicInfo> mis = new List<MusicInfo>();
        //    foreach (string id in ids)
        //    {
        //        Task<string> songRes = GetSongByIdRAsync(id);
        //        Task<string> songLrc = GetLrcRAsync(id);
        //        JObject jo = JObject.Parse(songRes.Result);
        //        if (!jo["error"].ToString().Equals(""))
        //        {
        //            continue;
        //        }
        //        mis.Add(new MusicInfo()
        //        {
        //            url = jo["url"].ToString(),
        //            title = jo["songName"].ToString(),
        //            author = jo["singerName"].ToString(),
        //            link = "http://www.kugou.com/song/#hash=" + id,
        //            songid = id,
        //            type = "kugou",
        //            pic = !jo["imgUrl"].Equals("") ? jo["imgUrl"].ToString().Replace("{size}", "150") : jo["album_img"].ToString().Replace("{size}", "150"),
        //            lrc = songLrc.Result
        //        });
        //    }
        //    return mis.Count > 0 ? mis.ToArray() : null;
        //}
    }
}
