﻿using log4net;
using MusicDownloader;
using NeteaseMusic.Helpers;
using NeteaseMusic.Services;
using NeteaseMusic.Song.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Test
    {
        static void Main1()
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

            music = new Kugou();
            //var result = music.GetFirstUrl("月亮");
            //ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //log.Error("test" + ": " );
            string filename = "NoCopyright/NoCopyright_20191227_140115.json";
            var downloadType = new List<string>()
            {
                "Netease",
                "Baidu",
                "Kugou",
                "QQ",
                "Xiami",
            };
            DownLoad(filename,downloadType);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
        static void DownLoad(string filename,List<string> downloadType)
        {
            //先拿5个 测试下
            var detail5 = FileService.GetDetailFroNoCopyRight(filename).Take(5);
          
            downloadType.ForEach(e => DownLoad(detail5, e));
        }
        static void DownLoad(IEnumerable<Detail> details, string type)
        {
            var dateStr =  DateTime.Now.ToString("yyyyMMdd_HHmmss");
            IMusic music = new Netease();
            switch (type)
            {
                case "Netease":
                    music = new Netease();
                    break;
                case "Baidu":
                    music = new Baidu();
                    break;
                case "Kugou":
                    music = new Kugou();
                    break;
                case "QQ":
                    music = new QQ();
                    break;
                case "Xiami":
                    music = new Xiami();
                    break;
            }

            foreach (var detail in details)
            {
                var keywords = new List<string>()
                {
                    detail.name + " " + detail.ar[0].name,
                    detail.name
                };
                foreach (var keyword in keywords)
                {
                    try
                    {
                        string filename = detail.name;
                        var url = music.GetFirstUrl(keyword);
                        if (!string.IsNullOrEmpty(url))
                        {
                            var extension = GetExtension(url);
                            var path = "Download/"+dateStr +"/" + type + "/" + filename + extension;
                            HttpHelper.DownloadFile(url, path);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                        log.Error(type + ": "+ keyword, ex);
                    }
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
