using MusicDownloader;
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
            //string result =   Netease.Search("月", 1, 1);
            //var result =   Netease.GetSongUrl("月", 1, 1);

            //var result = Baidu.Search("月");
            //var result =   Baidu.GetSongUrl("月", 1, 1);

            //var result = Xiami.Search("hah");
            var result =   Xiami.GetSongUrl("月亮", 1, 1);

            //var result = QQ.Search("月");
            //var result = QQ.GetSongVKey("000BULz507Tlkq");
            //var result =   QQ.GetSongUrl("004O62qD2LVPAu");
            //var result = QQ.GetSongUrl("0002yZ4D2Hx6ZZ");
            //var result =   QQ.GetSongUrl("月", 1, 1);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
        static void Exec()
        {

        }
    }
}
