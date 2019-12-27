using NeteaseMusic.Helpers;
using NeteaseMusic.Song.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeteaseMusic.Services
{
    public static class AppService
    {
        
        public static DetailsAndPrivileges GetSongsDetail(IEnumerable<Track> tracks)
        {
            int begin = 0;
            IEnumerable<Detail> allDetails =  new  List<Detail>();
            IEnumerable<Privilege> allPrivileges = new List<Privilege>();
            while (true)
            {
                var ids =  tracks.Skip(begin).Take(1000).Select(t => t.id).ToList();
                var songsDetail =  RequestService.GetSongDetail(ids);
                var jObj = JObject.Parse(songsDetail);
                var details = InfoHelper.Detail2Detail(jObj);
                var Privileges = InfoHelper.Privilege(jObj);
                allDetails = allDetails.Union(details);
                allPrivileges = allPrivileges.Union(Privileges);
                if (details.Count() < 1000) break;
                begin += 1000;
            }
            return new DetailsAndPrivileges()
            {
                songs = allDetails,
                privileges = allPrivileges,
            };
        }
        /// <summary>
        /// 从Detail文件中获取没有版权的歌曲信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IEnumerable<Detail> GetNoCopyrightSongs(string filename)
        {
            var dp = FileService.GetDAP(filename);
            return GetNoCopyrightSongs(dp);
        }
        private static IEnumerable<Detail> GetNoCopyrightSongs(DetailsAndPrivileges dp)
        {
            return from d in dp.songs
                   join p in dp.privileges on d.id equals p.id
                   where p.st == "-200"
                   select d;
        }
        /// <summary>
        /// 请求歌曲信息，为404的
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static IEnumerable<Privilege> Get404(DetailsAndPrivileges dp)
        {
            var songsIds = dp.songs.Select(e => e.id).ToList();
            return dp.privileges.Where(e => !songsIds.Contains(e.id)); 
        }
        /// <summary>
        /// 对比Queue文件和Detail文件
        /// ‪Queue文件为网易云音乐PC客户端中的正在播放Playlist，文件默认位置：%localappdata%\Netease\CloudMusic\webdata\file\queue
        /// </summary>
        /// <param name="queueFilename"></param>
        /// <param name="detailFilename"></param>
        /// <returns></returns>
        public static object ChangeQD(string queueFilename,string detailFilename)
        {
            return Change(FileService.GetDetailFromQueueFile(queueFilename), FileService.GetDetailFromDetailFile(detailFilename));
        }
        /// <summary>
        /// 对比Queue文件
        /// </summary>
        /// <param name="filepathOld"></param>
        /// <param name="filepathNew"></param>
        /// <returns></returns>
        public static object ChangeDD(string filepathOld, string filepathNew)
        {
            return Change(FileService.GetDetailFromDetailFile(filepathOld), FileService.GetDetailFromDetailFile(filepathNew));
        }
        public static object ChangePP(string filepathOld, string filepathNew)
        {
            return Change(FileService.GetDetailFromPlaylistFile(filepathOld), FileService.GetDetailFromPlaylistFile(filepathNew));
        }
        public static object ChangePD(string filepathOld, string filepathNew)
        {
            return Change(FileService.GetDetailFromPlaylistFile(filepathOld), FileService.GetDetailFromDetailFile(filepathNew));
        }
        public static object ChangeDP(string filepathOld, string filepathNew)
        {
            return Change(FileService.GetDetailFromDetailFile(filepathOld), FileService.GetDetailFromPlaylistFile(filepathNew));
        }
        private static object Change(IEnumerable<Detail> dOld,IEnumerable<Detail> dNew)
        {
            var idsOld = dOld.Select(e => e.id).ToList();
            var idsNew = dNew.Select(e => e.id).ToList();
            var add = dNew.Where(e => !idsOld.Contains(e.id));
            var reduce = dOld.Where(e => !idsNew.Contains(e.id));
            return new { add, reduce };
        }
    }
}
