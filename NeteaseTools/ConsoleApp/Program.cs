using CommandLine;
using log4net;
using MusicDownloader;
using NeteaseMusic.Helpers;
using NeteaseMusic.Services;
using NeteaseMusic.Song.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {


                AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender, e) => { HttpHelper.WriteCookiesToDisk(); });

                Parser.Default.ParseArguments<BackupOptions, CompareOptions, InfoOptions, LoginOptions, DownloadOptions>(args).MapResult
                    (
                        (BackupOptions opts) => Backup(opts),
                        (CompareOptions opts) => Compare(opts),
                        (InfoOptions opts) => Info(opts),
                        (LoginOptions opts) => Login(opts),
                        (DownloadOptions opts) => Download(opts),
                        errs => 9
                    );
            }
            catch (Exception ex)
            {

                ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                log.Error(ex);
                throw;
            }
        }
        static int Backup(BackupOptions opts)
        {
            string path = string.Empty;
            switch (opts.Type.ToUpper())
            {
                case "P":
                    path = FileService.SavePlaylist(opts.Name, opts.SaveFilename);
                    break;
                case "D":
                    path = FileService.SaveDetailsAndPrivileges(opts.Name, opts.SaveFilename);
                    break;
                case "DF":
                    path = FileService.SaveDetailsAndPrivileges(opts.Name, opts.FromNetwork, opts.SaveFilename);
                    break;
                default:
                    Console.WriteLine("类型错误");
                    return 1;
            }
            Console.WriteLine("备份完成：" + path.Replace("/", "\\"));
            return 1;
        }
        static int Compare(CompareOptions opts)
        {
            string type = opts.Type.ToUpper();
            string path = FileHelper.PathJointForSave(opts.SaveFilename, "Change", "Change" + type);
            object result = null;
            switch (type)
            {
                case "DD":
                    result = AppService.ChangeDD(opts.OldFile, opts.NewFile);
                    break;
                case "DP":
                    result = AppService.ChangeDP(opts.OldFile, opts.NewFile);
                    break;
                case "PP":
                    result = AppService.ChangePP(opts.OldFile, opts.NewFile);
                    break;
                case "PD":
                    result = AppService.ChangePD(opts.OldFile, opts.NewFile);
                    break;
                default:
                    Console.WriteLine("类型错误");
                    return 2;
            }
            path = FileHelper.SaveJsonFile(result, path);
            Console.WriteLine("对比完成：" + path.Replace("/", "\\"));
            return 2;

        }
        static int Info(InfoOptions opts)
        {
            string path = string.Empty;
            switch (opts.Type.ToUpper())
            {
                case "NC":
                    path = FileService.SaveNoCopyrightSongsFromDetail(opts.Filename, opts.SaveFilename);
                    break;
                default:
                    Console.WriteLine("类型错误");
                    return 1;
            }
            Console.WriteLine("获取完成：" + path.Replace("/", "\\"));
            return 3;
        }
        static int Login(LoginOptions opts)
        {
            //Console.WriteLine(opts.CountryCode == null);
            //Console.WriteLine(opts.CountryCode == "");
            var result = RequestService.CellphoneLogin(opts.Phone, opts.Password, opts.CountryCode);
            Console.WriteLine(result);
            return 3;

        }
        #region Download
        static int Download(DownloadOptions opts)
        {
            var dateStr = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            List<Detail> detail = new List<Detail>();
            switch (opts.Type.ToUpper())
            {
                case "N":
                    detail = FileService.GetDetailFroNoCopyRight(opts.Name);
                    break;
                case "D":
                    detail = FileService.GetDetailFromDetail(opts.Name);
                    break;
                case "DI":
                    detail = FileService.GetDetailFromDetail(opts.Name);
                    DownloadNetease(detail, dateStr);
                    return 5;
            }

            var dic = new Dictionary<string, string>()
            {
                {"N","Netease" },
                {"Q","QQ" },
                {"B","Baidu" },
                {"K","Kugou" },
                {"X","Xiami" },
            };

            var platform = (!string.IsNullOrEmpty(opts.Platform) ? opts.Platform : "nqbkx").ToUpper();
            var downloadType = dic.Where(e => platform.Contains(e.Key)).Select(e => e.Value).ToList();
            downloadType.ForEach(e => DownLoad(detail, e, dateStr));
            return 5;
        }
        static void DownLoad(IEnumerable<Detail> details, string type, string dateStr)
        {
            IMusic music = new Netease();
            switch (type)
            {
                case "Netease":
                    music = new Netease();
                    break;
                case "QQ":
                    music = new QQ();
                    break;
                case "Baidu":
                    music = new Baidu();
                    break;
                case "Kugou":
                    music = new Kugou();
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
                    var alert = type + ": " + keyword;
                    try
                    {
                        string filename = detail.name;
                        var url = music.GetFirstUrl(keyword);
                        if (!string.IsNullOrEmpty(url))
                        {
                            var extension = GetExtension(url);
                            var path = "Download/" + dateStr + "/" + type + "/" + filename + extension;
                            HttpHelper.DownloadFile(url, path);
                            Console.WriteLine(alert + " Dwonload Success!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine(alert + " Dwonload Fail!");
                        }
                    }
                    catch (Exception ex)
                    {
                        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                        log.Info(alert, ex);
                        Console.WriteLine(alert + " Dwonload Fail!");
                    }
                }
            }

        }
        static void DownloadNetease(IEnumerable<Detail> details, string dateStr)
        {
            var netease = new Netease();
            var ids = details.Select(e => e.id).ToList();
            var idNamedic = details.ToDictionary(e => e.id, e => e.name);
            var idUrlMap = netease.GetSongUrl(ids);

            foreach (var idUrl in idUrlMap)
            {
                string id = idUrl.Key;
                string url = idUrl.Value;
                string alert = id + " " + idNamedic[id];
                try
                {
                    var extension = GetExtension(url);
                    var path = "Download/" + dateStr + "/Netease/" + idNamedic[id] + extension;
                    HttpHelper.DownloadFile(url, path);
                    Console.WriteLine(alert + " Dwonload Success!");
                }
                catch (Exception ex)
                {
                    ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                    log.Info(alert, ex);
                    Console.WriteLine(alert + " Dwonload Fail!");
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
        #endregion
    }
}
