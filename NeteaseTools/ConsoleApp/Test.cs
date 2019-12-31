using MusicDownloader;
using NeteaseMusic.Helpers;
using NeteaseMusic.Services;
using NeteaseMusic.Song.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Test
    {
        static void Main()
        {
            var watch = Stopwatch.StartNew();
            IMusic music = new Netease();
            //string result =   Netease.Search("月", 1, 1);

            music = new Baidu();
           // var result =music.GetFirstUrl("月");

            //var result = Baidu.Search("月");
            //var result =   Baidu.GetSongUrl("月", 1, 1);

            //var result = Xiami.Search("hah");
            //var result =   Xiami.GetSongUrl("月亮", 1, 1);

            //var result = QQ.Search("月");
            //var result = QQ.GetSongVKey("000BULz507Tlkq");
            //var result =   QQ.GetSongUrl("004O62qD2LVPAu");
            //var result = QQ.GetSongUrl("0002yZ4D2Hx6ZZ");
            //var result =   QQ.GetSongUrl("月", 1, 1);

            //var result = FileService.GetDetail("SongDetail/2012429627_20191227_134946.json");


            DownLoad();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
        static void DownLoad()
        {
            //先拿5个 测试下
            var detail5 = FileService.GetDetailFroNoCopyRight("NoCopyright/NoCopyright_20191227_140115.json").Take(5);
            DownLoad(detail5, "Netease");
            DownLoad(detail5, "Baidu");
            //netesae
        }
        static void DownLoad(IEnumerable<Detail> details, string type)
        {

            IMusic music = new Netease();
            switch (type)
            {
                case "Netease":
                    music = new Netease();
                    break;
                case "Baidu":
                    music = new Baidu();
                    break;
            }
            foreach (var detail in details)
            {
                try
                {
                    string filename = detail.name;
                  var url =   music.GetFirstUrl(detail.name + " " + detail.ar[0].name);
                    var extension = GetExtension(url);
                    var path = type + "/" + filename + extension;
                    HttpHelper.DownloadFile(url, path);
                }
                catch (Exception ex)
                {
                }
            }

        }
        public static string GetExtension(string url)
        {
            var index = url.IndexOf('?');
            if (index != -1)
            {
                url = url.Substring(0, index);
            }
            return url.Substring(url.LastIndexOf('.'));
        }
    }
}
