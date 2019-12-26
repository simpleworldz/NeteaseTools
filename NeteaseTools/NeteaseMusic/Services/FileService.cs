using NeteaseMusic.Helpers;
using NeteaseMusic.Song.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeteaseMusic.Services
{
    public static class FileService
    {
        /// <summary>
        /// 从Detail文件总获取DetailsAndPrivileges 数据
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DetailsAndPrivileges GetDAP(string filename)
        {
            var jObj = FileHelper.ReadJsonFile2JObj(filename);
            return new DetailsAndPrivileges()
            {
                songs = InfoHelper.Detail2Detail(jObj),
                privileges = InfoHelper.Privilege(jObj)
            };
        }
        /// <summary>
        ///通过Playlist文件，获取Detail信息并保存
        /// </summary>
        /// <param name="playlistFilename"></param>
        /// <param name="filename">保存文件名</param>
        /// <param name="fromNetwork">false：直接获取Playlist中的Detail；True获取playlist中的id并通过网络请求获取Detail。当playlist中歌曲数大于1000时，只能后者</param>
        public static void SaveDetailsAndPrivileges(string playlistFilename, bool fromNetwork ,string filename = null)
        {
            var jObj = FileHelper.ReadJsonFile2JObj(playlistFilename);
            var tracks = InfoHelper.Playlist2Track(jObj);
            DetailsAndPrivileges dp = (fromNetwork || tracks.Count() > 1000)? AppService.GetSongsDetail(tracks) : InfoHelper.Detail2DAP(jObj);
            SaveDetailsAndPrivileges(dp, filename);
        }
        /// <summary>
        /// 通过playlistId获取Detail并保存
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="filename"></param>
        public static void SaveDetailsAndPrivileges(string playlistId, string filename = null)
        {
            string playlistDetail = RequestService.GetPlaylistDetail(playlistId);
            var jObj = JObject.Parse(playlistDetail);
            var tracks = InfoHelper.Playlist2Track(jObj);
            DetailsAndPrivileges dp = tracks.Count() > 1000? AppService.GetSongsDetail(tracks): InfoHelper.Detail2DAP(jObj);
            if (string.IsNullOrEmpty(filename)) filename = playlistId + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            SaveDetailsAndPrivileges(dp, filename);
        }

       

        public static void SaveDetailsAndPrivileges(List<string> ids, string filename = null)
        {
            var tracks = ids.Select(e => new Track { id = e });
            var dp = AppService.GetSongsDetail(tracks);
            SaveDetailsAndPrivileges(dp, filename);
        }
        private static void SaveDetailsAndPrivileges(DetailsAndPrivileges dp, string filename = null)
        {
            var dpo = new { dp.songs, dp.privileges };
            var dpoStr = JsonConvert.SerializeObject(dpo);
            if (string.IsNullOrEmpty(filename)) filename ="9999999"+ DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = "SongDetail/" + filename + ".json";
            FileHelper.SaveJsonFile(dpoStr, path);
        }
        public static void SavePlaylist(string playlistId, string filename = null)
        {
            string path;
            if (string.IsNullOrEmpty(filename))
            {
                path = "PlaylistDetail/" + playlistId+"_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
            }
            else
            {
                path = "PlaylistDetail/" + filename + ".json";

            }
            string playlistDetail = RequestService.GetPlaylistDetail(playlistId);
            FileHelper.SaveJsonFile(playlistDetail, path);
        }
        /// <summary>
        /// 读取Queue文件
        /// ‪Queue文件为网易云音乐PC客户端中的正在播放Playlist，文件默认位置：%localappdata%\Netease\CloudMusic\webdata\file\queue
        /// </summary>
        /// <param name="filename"></param>
        public static IEnumerable<Detail> GetDetailFromQueueFile(string filename)
        {
            return InfoHelper.Queue2Detail(FileHelper.ReadJsonFile2JTokens(filename));
        }

    }
}
