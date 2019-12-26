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
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((sender,e) => { HttpHelper.WriteCookiesToDisk(); });
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var playlistId = "320559879";
            var playlistFilename = "/PlayListDetail/320559879.json";
            var detailFilename = "/SongDetail/320559879.json";
            var queueFilename = "Queue/queue1105";
            var ids = new List<string>
            {
                "472607124",
                "515143072",
                //"26466734",
            };

            string i = "Rqcu3wnN3sWlLzWf";
            string encSecKey = "a3ab8f757c3e19bf35a3e1b8f48f8a58e52f4dcef4dc4569f65b480ccf6e604f890ed95672f23d0940d6b4716e332e4b1286d9460153c3bf5dbcabe31bb5f06b440da1c3bc4b599c960534cc66d5cd28d58150e9bb278959a071bd4f2b163677ad4fbd15949b0f8b21b59c92eff7ee6654f9f77114de824c10446140ccb5cc08";
            string publicKey = "-----BEGIN PUBLIC KEY-----\nMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDgtQn2JZ34ZC28NWYpAUd98iZ37BUrX/aKzmFbt7clFSs6sXqHauqKWqdtLkF2KexO40H1YTX8z2lSgBBOAxLsvaklV8k4cBFK9snQXE9/DDaFt6Rr7iVZMldczhC0JNgTz+SHXT6CBHuX3e9SdB1Ua44oncaTWz7OBGLbCiK45wIDAQAB\n-----END PUBLIC KEY-----";
            FileService.SaveDetailsAndPrivileges(playlistId);
            //var result = RequestService.Test( publicKey,Tools.ReverseString(i));
            //var result = RequestService.Test( i,publicKey);
            //var result = EncryptHelper.Encrypt(i);

            //var result = RequestService.GetSongDetail(ids);
            //var result = RequestService.CellphoneLogin("18397789765","yiuyi");
            //HttpHelper.CookieSerializeTest();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
