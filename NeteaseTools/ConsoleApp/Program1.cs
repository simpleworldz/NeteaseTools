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
    [Verb("check", HelpText = "检查")]
    class CheckOptions
    {
        [Value(0, HelpText = "一个 .sln 文件，一个或者多个 .csproj 文件。")]
        public IEnumerable<string> InputFiles { get; set; }
    }

    [Verb("fix", HelpText = "修复")]
    class FixOptions
    {
        [Value(0, HelpText = "一个 .sln 文件，一个或者多个 .csproj 文件。")]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('o', "outputFiles", Required = true, HelpText = "修复之后的文件集合。")]
        public IEnumerable<string> OutputFiles { get; set; }

        [Option(Required = false, HelpText = "是否自动决定版本号，这将使用冲突版本号中的最新版本。")]
        public bool AutoVersion { get; set; }
    }

    class Program1
    {


        //static int Main(string[] args)
        //{
        //    var exitCode = Parser.Default.ParseArguments<CheckOptions, FixOptions>(args)
        //        .MapResult(
        //            (CheckOptions o) => CheckSolutionOrProjectFiles(o),
        //            (FixOptions o) => FixSolutionOrProjectFiles(o),
        //            error => 1);
        //    return exitCode;
        //}
        //static int Main(string[] args)
        //{
        //    var result = CommandLine.Parser.Default.ParseArguments<AddOptions,FixOptions>(args)
        //       .MapResult(
        //         (AddOptions opts) => RunAddAndReturnExitCode(opts),
        //            (FixOptions o) => FixSolutionOrProjectFiles(o),
        //         //(CommitOptions opts) => RunCommitAndReturnExitCode(opts),
        //         //(CloneOptions opts) => RunCloneAndReturnExitCode(opts),
        //         errs => 1);
        //    return result;

        //}
        private static int CheckSolutionOrProjectFiles(CheckOptions options)
            {
                return 0;
            }

            private static int FixSolutionOrProjectFiles(FixOptions options)
            {
                return 0;
            }
        //static void Main(string[] args)
        //{
        //    CommandLine.Parser.Default.ParseArguments<AddOptions>(args)
        //      .WithParsed<AddOptions>(opts => RunOptionsAndReturnExitCode(opts))
        //      .WithNotParsed<AddOptions>((errs) => HandleParseError(errs));
        //}
       

        private static int RunAddAndReturnExitCode(AddOptions opts)
        {
            RunOptionsAndReturnExitCode( opts);
            return 3;
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
        }

        private static void RunOptionsAndReturnExitCode(AddOptions opts)
        {
            Console.WriteLine(opts.Test);
            Console.Write("input:");
            var xx = Console.ReadLine();
            var newOpts = xx.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //Main(newOpts);
        }
        //    static void Main(string[] args)
        //{
        //    //程序关闭时保存Cookie
        //    AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender, e) => { HttpHelper.WriteCookiesToDisk(); });

        //    var watch = System.Diagnostics.Stopwatch.StartNew();

        //    var playlistId = "320559879";
        //    var playlistFilename = "/PlayListDetail/320559879.json";
        //    var detailFilename = "/SongDetail/320559879.json";
        //    var queueFilename = "Queue/queue1105";
        //    var ids = new List<string>
        //    {
        //        "472607124",
        //        //"515143072",
        //        "1300210802",
        //    };

        //    string i = "Rqcu3wnN3sWlLzWf";
        //    string encSecKey = "a3ab8f757c3e19bf35a3e1b8f48f8a58e52f4dcef4dc4569f65b480ccf6e604f890ed95672f23d0940d6b4716e332e4b1286d9460153c3bf5dbcabe31bb5f06b440da1c3bc4b599c960534cc66d5cd28d58150e9bb278959a071bd4f2b163677ad4fbd15949b0f8b21b59c92eff7ee6654f9f77114de824c10446140ccb5cc08";
        //    string publicKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDgtQn2JZ34ZC28NWYpAUd98iZ37BUrX/aKzmFbt7clFSs6sXqHauqKWqdtLkF2KexO40H1YTX8z2lSgBBOAxLsvaklV8k4cBFK9snQXE9/DDaFt6Rr7iVZMldczhC0JNgTz+SHXT6CBHuX3e9SdB1Ua44oncaTWz7OBGLbCiK45wIDAQAB\n-----END PUBLIC KEY-----";
        //    //FileService.SaveDetailsAndPrivileges(playlistId);
        //    //var result = RequestService.Test( publicKey,Tools.ReverseString(i));
        //    //var result = RequestService.Test( i,publicKey);
        //    //var result = EncryptHelper.Encrypt(i);
        //    //os=pc

        //    // Referer: "https://music.163.com"
        //    //User-Agent:"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36"

        //    var result = RequestService.GetSongDetail(ids);
        //    //var result = RequestService.CellphoneLogin("18396359487", "qy19970108");
        //    //HttpHelper.CookieSerializeTest();
        //    watch.Stop();
        //    Console.WriteLine(watch.ElapsedMilliseconds);
        //    Console.ReadKey();
        //}
    }
}
