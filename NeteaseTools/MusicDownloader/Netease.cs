using NeteaseMusic.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeteaseMusic.Services;

namespace MusicDownloader
{
    public static class Netease
    {
        public static string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("https://music.163.com/weapi/cloudsearch/get/web");
            var data = new
            {
                s = name,
                type = 1,
                offset = (page - 1) * pageSize,
                limit = pageSize,
                total = false
            };
            return RequestService.SendRequest(hp, data);
        }
        public static string GetSongUrlInfo(List<string> ids, int br = 320000)
        {
            HttpParams hp = new HttpParams("https://music.163.com/weapi/song/enhance/player/url");
            var data = new
            {
                ids = ids,
                br = br,
            };
            return RequestService.SendRequest(hp, data);
        }
        public static List<string> GetSongId(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["result"]["songs"].Select(e => (string)e["id"]).ToList();
        }
        public static List<string> GetSongUrl(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["data"].Select(e => (string)e["url"]).ToList();
        }

        public static List<string> GetSongUrl(string name,int page = 1,int pageSize = 1)
        {
         return GetSongUrl( GetSongUrlInfo(GetSongId(Search(name, page, pageSize))));
        }
    }
}
