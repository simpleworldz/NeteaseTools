using CommandLine;
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
            Parser.Default.ParseArguments<BackupOptions, CompareOptions, LoginOptions>(args)
      .MapResult(
        (BackupOptions opts) => Backup(opts),
        (CompareOptions opts) => Compare(opts),
        (LoginOptions opts) => Login(opts),
        errs => 9);

        }
        static int Backup(BackupOptions opts)
        {
            //string path = string.Empty;
            switch (opts.Type)
            {
                case "p":
                    FileService.SavePlaylist(opts.Name, opts.Filename);
                    break;
                case "d":
                    FileService.SaveDetailsAndPrivileges(opts.Name, opts.Filename);
                    break;
                case "df":
                    FileService.SaveDetailsAndPrivileges(opts.Name,opts.FromNetwork, opts.Filename);
                    break;
            }
            return 1;
        }
        static int Compare(CompareOptions opts)
        {
            return 2;

        }
        static int Login(LoginOptions opts)
        {
            return 3;

        }
        //SaveDetailsAndPrivileges(string playlistId, string filename = null)
        //SaveDetailsAndPrivileges(string playlistFilename, bool fromNetwork ,string filename = null)
        //ChangeDD(string filepathOld, string filepathNew)
        //CellphoneLogin(string phone, string password, string countrycode = "", bool rememberLogin = true)
        //SavePlaylist
        //ChangePP
    }
}
