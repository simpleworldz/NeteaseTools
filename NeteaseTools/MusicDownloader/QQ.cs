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
using System.Xml;

namespace MusicDownloader
{
    public  class QQ:IMusic
    {
        public  string Search(string name, int page = 1, int pageSize = 1)
        {
            HttpParams hp = new HttpParams("http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp");
            hp.Referer = "http://m.y.qq.com/";
            hp.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
            hp.Data = new Dictionary<string, object>()
            {
                 //{"w" , HttpUtility.UrlEncode(name) },
                 {"w" , name },
                { "n" , pageSize },
                {"format", "json" },
                { "p" , page },
            };
            return HttpHelper.GetAsync(hp).Result;
        }
        private  List<string> GetSongMid(string jsonStr)
        {
            JObject jObj = JObject.Parse(jsonStr);
            return jObj["data"]["song"]["list"].Select(e => (string)e["songmid"]).ToList();
        }
        public static string GetSongVKey(string songmid)
        {
            HttpParams hp = new HttpParams("https://u.y.qq.com/cgi-bin/musicu.fcg");
            hp.Referer = "https://y.qq.com/portal/player.html";
            var data = "-=getplaysongvkey9303570918169626&g_tk=5381&loginUin=932266563&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0&data={\"req_0\":{\"module\":\"vkey.GetVkeyServer\",\"method\":\"CgiGetVkey\",\"param\":{\"guid\":\"6419485772\",\"songmid\":[\""+songmid+"\"],\"songtype\":[0],\"uin\":\"932266563\",\"loginflag\":1,\"platform\":\"20\"}},\"comm\":{\"uin\":932266563,\"format\":\"json\",\"ct\":24,\"cv\":0}}";
            hp.Url +="?"+ data;
            var jsonStr = HttpHelper.GetAsync(hp,"").Result;
            JObject jObj = JObject.Parse(jsonStr);
            return (string)jObj["req_0"]["data"]["midurlinfo"].Select(e => (string)e["purl"]).ToList()[0];
        }
        public static string GetSongUrl(string songmid)
        {
            //return $"http://27.152.180.23/amobile.music.tc.qq.com/C400{songmid}.m4a?guid=6419485772&vkey={GetSongVKey(songmid)}&uin=579&fromtag=66";
            return $"http://isure.stream.qqmusic.qq.com/{GetSongVKey(songmid)}";
        }
        public string GetFirstUrl(string name)
        {
          return   GetSongUrl(GetSongMid(Search(name, 1, 1))[0]);
        }
        #region 废弃
        public static string GetSongUrl2(string songmid)
        {
            //https://u.y.qq.com/cgi-bin/musicu.fcg?-=getplaysongvkey7394411021564253&g_tk=5381&loginUin=932266563&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0&data=%7B%22req_0%22%3A%7B%22module%22%3A%22vkey.GetVkeyServer%22%2C%22method%22%3A%22CgiGetVkey%22%2C%22param%22%3A%7B%22guid%22%3A%226419485772%22%2C%22songmid%22%3A%5B%220002yZ4D2Hx6ZZ%22%5D%2C%22songtype%22%3A%5B0%5D%2C%22uin%22%3A%22932266563%22%2C%22loginflag%22%3A1%2C%22platform%22%3A%2220%22%7D%7D%2C%22comm%22%3A%7B%22uin%22%3A932266563%2C%22format%22%3A%22json%22%2C%22ct%22%3A24%2C%22cv%22%3A0%7D%7D
            HttpParams hp = new HttpParams("https://u.y.qq.com/cgi-bin/musicu.fcg?");
            hp.Referer = "https://y.qq.com/portal/player.html";
            var data = "-=getplaysongvkey9303570918169626&g_tk=5381&loginUin=932266563&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0&data={\"req_0\":{\"module\":\"vkey.GetVkeyServer\",\"method\":\"CgiGetVkey\",\"param\":{\"guid\":\"6419485772\",\"songmid\":[\"" + songmid + "\"],\"songtype\":[0],\"uin\":\"932266563\",\"loginflag\":1,\"platform\":\"20\"}},\"comm\":{\"uin\":932266563,\"format\":\"json\",\"ct\":24,\"cv\":0}}";
            hp.Url += data;
            var jsonStr = HttpHelper.GetAsync(hp, "").Result;
            JObject jObj = JObject.Parse(jsonStr);
            return (string)jObj["req_0"]["data"]["midurlinfo"].Select(e => (string)e["vkey"]).ToList()[0];
        }
        public static string GetSongUrlNew(string songmid)
        {
            var guid = new Random().Next(1000000000, 2000000000);
            HttpParams hp = new HttpParams($"http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={guid}&format=json&json=3");
            var result = HttpHelper.GetAsync(hp, "").Result;
            return "http://dl.stream.qqmusic.qq.com/C400{songmid}.mp3?vkey={}&guid={3}&fromtag=1";


        }
       
        #region
        //https://segmentfault.com/a/1190000018645242
        public static string GetSongVKey1(string songmid)
        {
            HttpParams hp = new HttpParams("https://c.y.qq.com/base/fcgi-bin/fcg_music_express_mobile3.fcg?");
            hp.Referer = "http://m.y.qq.com/";
            //hp.Data = new Dictionary<string, object>()
            //{
            //    { "loginUin", 3051522991 },
            //    {"format", "jsonp" },
            //    {"platform", "yqq"},
            //    {"needNewCode", 0},
            //    {"cid", 205361747},
            //    {"uin", 3051522991},
            //    {"guid", 5931742855},
            //    {"songmid", songmid},
            //    {"filename", "C400"+songmid+".m4a"},
            //};
            hp.Data = new Dictionary<string, object>()
            {
                { "loginUin", 932266563 },
                {"format", "jsonp" },
                {"platform", "yqq"},
                {"needNewCode", 0},
                //没获取到cid
                {"cid", 205361747},
                {"uin", 932266563},
                {"guid", 6419485772},
                {"songmid", songmid},
                {"filename", "C400"+songmid+".m4a"},
            };
            JObject jObj = JObject.Parse(HttpHelper.GetAsync(hp).Result);
            return jObj["data"]["items"].Select(e => (string)e["vkey"]).ToList().FirstOrDefault();
        }
        #endregion

        //public static string GetSongUrlInfo(List<string> ids)
        //{
        //    HttpParams hp = new HttpParams("http://music.baidu.com/data/music/links");
        //    hp.Referer = "music.baidu.com/song/";
        //    hp.Data = new Dictionary<string, object>()
        //    {
        //        {"songIds",JsonConvert.SerializeObject(ids) }
        //    };
        //    return HttpHelper.GetAsync(hp).Result;

        //}

      
        //public static string GetSongVKey(string xmlStr)
        //{
        //    XmlDocument xml = new XmlDocument();
        //    xml.LoadXml(xmlStr);
        //    return xml["command-lable-xwl78-qq-music"]["cmd"]["info"].Attributes["key"].Value;
        //}
        //public static string GetSongUrl()
        //{
        //    HttpParams hp = new HttpParams("https://u.y.qq.com/cgi-bin/musicu.fcg?");
        //    hp.Referer = "http://m.y.qq.com/";
        //}
       
        #region
        public static string GetSongUrl1(string songmid)
        {
            //return $"http://dl.stream.qqmusic.qq.com/C400{songmid}.m4a?" +
            //        "fromtag=38" +
            //        "&guid=5931742855" +
            //        "&vkey=" + GetSongVKey(songmid);
            return $"http://dl.stream.qqmusic.qq.com/C400{songmid}.m4a?" +
                    "fromtag=38" +
                    "&guid=6419485772" +
                    "&vkey=" + GetSongVKey1(songmid);
        }

      
        #endregion
        #endregion
    }
}
