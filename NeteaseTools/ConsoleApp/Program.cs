using CommandLine;
using NeteaseMusic.Helpers;
using NeteaseMusic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender, e) => { HttpHelper.WriteCookiesToDisk(); });

            Parser.Default.ParseArguments<BackupOptions, CompareOptions,InfoOptions, LoginOptions>(args).MapResult
                (
                    (BackupOptions opts) => Backup(opts),
                    (CompareOptions opts) => Compare(opts),
                    (InfoOptions opts) => Info(opts),
                    (LoginOptions opts) => Login(opts),
                    errs => 9
                );

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
            Console.WriteLine("备份完成：" + path.Replace("/","\\"));
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
    }
}
