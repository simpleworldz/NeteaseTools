using NeteaseMusic.Helpers;
using NeteaseMusic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NeteaseMusic.Services
{
    public static class RequestService
    {
        #region data
        static readonly string iv = "0102030405060708";
        static readonly string g = "0CoJUm6Qyw8W8jud";
        //static readonly string i = "Rqcu3wnN3sWlLzWf";
        static readonly string e = "010001";
        static readonly string f = "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";
        //static readonly string g = "0CoJUm6Qyw8W8jud";
        //static readonly string encSecKey = "a3ab8f757c3e19bf35a3e1b8f48f8a58e52f4dcef4dc4569f65b480ccf6e604f890ed95672f23d0940d6b4716e332e4b1286d9460153c3bf5dbcabe31bb5f06b440da1c3bc4b599c960534cc66d5cd28d58150e9bb278959a071bd4f2b163677ad4fbd15949b0f8b21b59c92eff7ee6654f9f77114de824c10446140ccb5cc08";
        static readonly string publicKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDgtQn2JZ34ZC28NWYpAUd98iZ37BUrX/aKzmFbt7clFSs6sXqHauqKWqdtLkF2KexO40H1YTX8z2lSgBBOAxLsvaklV8k4cBFK9snQXE9/DDaFt6Rr7iVZMldczhC0JNgTz+SHXT6CBHuX3e9SdB1Ua44oncaTWz7OBGLbCiK45wIDAQAB\n-----END PUBLIC KEY-----";

        //hp.Url = "http://music.163.com/api/playlist/detail?id=320559879";
        //hp.Url = "https://music.163.com/weapi/v3/playlist/detail";
        //hp.Url = "https://music.163.com/api/v3/playlist/detail";
        //hp.Url = "http://interface3.music.163.com/eapi/v6/playlist/detail";
        #endregion
        /// <summary>
        /// 获取playlist中的所有歌曲的id，和前1000首歌的详细信息
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        public static string GetPlaylistDetail(string playlistId)
        {
            HttpParams hp = new HttpParams("https://music.163.com/weapi/v3/playlist/detail");
            //string d = "{\"id\":\"320559879\",\"offset\":\"10\",\"total\":\"true\",\"n\":\"1000\"}";
            var data = new { id = playlistId, n = 1000, s = 8 };
            return SendRequest(hp, data);
        }
        public static string GetSongDetail(List<string> ids)
        {
            HttpParams hp = new HttpParams("https://music.163.com/weapi/v3/song/detail");
            var x = (from a in ids select new { id = a }).ToList();
            var data = new { c = JsonConvert.SerializeObject(x) };
            return SendRequest(hp, data);

        }
        public static string CellphoneLogin(string phone, string password, string countrycode = "", bool rememberLogin = true)
        {
            HttpParams hp = new HttpParams("https://music.163.com/weapi/login/cellphone");
            //string d = "{\"id\":\"320559879\",\"offset\":\"10\",\"total\":\"true\",\"n\":\"1000\"}";
            password = EncryptHelper.MD5Encrypt(password);
            var data = new { phone, password, countrycode, rememberLogin };
            return SendRequest(hp, data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d">加密前的请求数据</param>
        /// <param name="hp"></param>
        /// <returns></returns>
        public static string SendRequest(HttpParams hp, object obj)
        {
            var data = JsonConvert.SerializeObject(obj);
            return SendRequest(hp, data);
        }
        public static string SendRequest(HttpParams hp, string data)
        {
            //当UserAgent为这个时如果没有cookie 不管改歌曲是否下架privilege的st都是-100，加了cookie后 下架 -200,非下架 0；
            //hp.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.79 Safari/537.36";
            //这个则不用cookie，privilege信息也正常;但无法获取从网易云盘中收藏的歌曲
            //https://github.com/Binaryify/NeteaseCloudMusicApi
            hp.UserAgent = "Mozilla / 5.0(iPad; CPU OS 10_0 like Mac OS X) AppleWebKit/602.1.38 (KHTML, like Gecko) Version/10.0 Mobile/14A300 Safari/602.1";
            string i = Tools.CreateRandomWords(16);
            string h = EncryptHelper.AesEncrypt(data, g, iv);
            string param = EncryptHelper.AesEncrypt(h, i, iv);
            #region
            //EncryptProvider.AESEncrypt key的length必须是32
            //string h = EncryptProvider.AESEncrypt(d, g, iv);
            //string param = EncryptProvider.AESEncrypt(h, i, iv);
            #endregion
            param = HttpUtility.UrlEncode(param);

            string ir = Tools.ReverseString(i);
            string encSecKey = EncryptHelper.RsaEncryptWithPublic(ir, publicKey);
            //string encSecKey = EncryptHelper.RsaEncryptWithEM(ir, e,f);
            hp.Data = new Dictionary<string, object>()
            {
                { "params",param
    },
                { "encSecKey",encSecKey
}
            };
            var task = HttpHelper.PostAsync(hp);
            return task.Result;
        }
    }

}
