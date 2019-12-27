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
        #region SaveDetailsAndPrivileges
        /// <summary>
        ///通过Playlist文件，获取Detail信息并保存
        /// </summary>
        /// <param name="playlistFilename"></param>
        /// <param name="filename">保存文件名</param>
        /// <param name="fromNetwork">false：直接获取Playlist中的Detail；True获取playlist中的id并通过网络请求获取Detail。当playlist中歌曲数大于1000时，只能后者</param>
        public static string SaveDetailsAndPrivileges(string playlistFilename, bool fromNetwork, string filename = null)
        {
            var jObj = FileHelper.ReadJsonFile2JObj(playlistFilename);
            var tracks = InfoHelper.Playlist2Track(jObj);
            DetailsAndPrivileges dp = (fromNetwork || tracks.Count() > 1000) ? AppService.GetSongsDetail(tracks) : InfoHelper.Detail2DAP(jObj);
            return SaveDetailsAndPrivileges(dp, filename);
        }
        /// <summary>
        /// 通过playlistId获取Detail并保存
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="filename"></param>
        public static string SaveDetailsAndPrivileges(string playlistId, string filename = null)
        {
            string playlistDetail = RequestService.GetPlaylistDetail(playlistId);
            var jObj = JObject.Parse(playlistDetail);
            var tracks = InfoHelper.Playlist2Track(jObj);
            DetailsAndPrivileges dp = tracks.Count() > 1000 ? AppService.GetSongsDetail(tracks) : InfoHelper.Detail2DAP(jObj);
            if (string.IsNullOrEmpty(filename)) filename = playlistId + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return SaveDetailsAndPrivileges(dp, filename);
        }



        public static string SaveDetailsAndPrivileges(List<string> ids, string filename = null)
        {
            var tracks = ids.Select(e => new Track { id = e });
            var dp = AppService.GetSongsDetail(tracks);
            return SaveDetailsAndPrivileges(dp, filename);
        }
        private static string SaveDetailsAndPrivileges(DetailsAndPrivileges dp, string filename = null)
        {
            var dpo = new { dp.songs, dp.privileges };
            var dpoStr = JsonConvert.SerializeObject(dpo);
            string path = FileHelper.PathJointForSave(filename, "SongDetail", "SongDetail");
            return FileHelper.SaveJsonFile(dpoStr, path);
        }
        #endregion
        public static string SavePlaylist(string playlistId, string filename = null)
        {
            string path = FileHelper.PathJointForSave(filename, "Playlist", playlistId);
            string playlistDetail = RequestService.GetPlaylistDetail(playlistId);
            return FileHelper.SaveJsonFile(playlistDetail, path);
        }
        #region GetDetail
        /// <summary>
        /// 读取Queue文件
        /// ‪Queue文件为网易云音乐PC客户端中的正在播放Playlist，文件默认位置：%localappdata%\Netease\CloudMusic\webdata\file\queue
        /// </summary>
        /// <param name="filename"></param>
        public static IEnumerable<Detail> GetDetailFromQueueFile(string filename)
        {
            return InfoHelper.Queue2Detail(FileHelper.ReadJsonFile2JTokens(filename));
        }
        public static IEnumerable<Detail> GetDetailFromDetailFile(string filepath)
        {
            return InfoHelper.Detail2Detail(FileHelper.ReadJsonFile2JObj(filepath));
        }
        public static IEnumerable<Detail> GetDetailFromPlaylistFile(string filepath)
        {
            return InfoHelper.Playlist2Detail(FileHelper.ReadJsonFile2JObj(filepath));
        }
        #endregion

        public static string SaveNoCopyrightSongsFromDetail(string filename,string saveFilename)
        {
            var result =  AppService.GetNoCopyrightSongsFromDetail(filename);
            var path = FileHelper.PathJointForSave(saveFilename, "NoCopyright", "NoCopyright");
            return FileHelper.SaveJsonFile(result, path);
        }
        //public static string SaveNoCopyrightSongsFromPlaylist(string filename, string saveFilename)
        //{
        //    var result = AppService.GetNoCopyrightSongsFromPlaylist(filename);
        //    var path = FileHelper.PathJointForSave(saveFilename, "NoCopyright", "NoCopyright");
        //    return FileHelper.SaveJsonFile(filename, path);
        //}
    }
}
