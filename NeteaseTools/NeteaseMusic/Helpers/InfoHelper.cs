using NeteaseMusic.Song.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeteaseMusic.Helpers
{
    public static class InfoHelper
    {
        public static IEnumerable<Detail> Queue2Detail(List<JToken> QueueJts)
        {
            return QueueJts.Select(t => new Detail
            {
                id = (string)t["track"]["id"],
                name = (string)t["track"]["name"],
                ar = t["track"]["artists"].Select(e => new Artist()
                {
                    id = (string)e["id"],
                    name = (string)e["name"]
                }).ToList(),
                al =  new Album()
                {
                    id = (string)t["track"]["album"]["id"],
                    name = (string)t["track"]["album"]["name"]
                }
            });
        }

        public static IEnumerable<Detail> Playlist2Detail(JObject playlistJObj)
        {
            var jToken = playlistJObj["playlist"]["tracks"];
            return Detail(jToken);
        }
        public static IEnumerable<Track> Playlist2Track(JObject playlistObj)
        {
            return playlistObj["playlist"]["trackIds"].Select(t => new Track
            {
                id = (string)t["id"]
            });
        }
        public static IEnumerable<Privilege> Privilege(JObject JObj)
        {
            return JObj["privileges"].Select(t => new Privilege
            {
                id = (string)t["id"],
                st = (string)t["st"]
            });
        }
        public static IEnumerable<Detail> Detail2Detail(JObject detailJObj)
        {
            var jToken = detailJObj["songs"];
            return Detail(jToken);
        }
        public static IEnumerable<Detail> Detail(JToken jToken)
        {
            return jToken.Select(t => new Detail
            {
                name = (string)t["name"],
                id = (string)t["id"],
                ar = t["ar"].Select( e => new Artist()
                {
                    id = (string)e["id"],
                    name = (string)e["name"]
                }).ToList(),
                al = new Album()
                {
                    id = (string)t["al"]["id"],
                    name = (string)t["al"]["name"]
                }
            });
        }
        public static DetailsAndPrivileges Detail2DAP(JObject jObj)
        {
            return new DetailsAndPrivileges()
            {
                songs = InfoHelper.Playlist2Detail(jObj),
                privileges = InfoHelper.Privilege(jObj)
            };
        }
    }
}
