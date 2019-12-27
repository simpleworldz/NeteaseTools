using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("backup", HelpText = "备份")]
    class BackupOptions
    {
        [Option('t', "type", Required = true, HelpText = "备份类型：p：Playlist;d:Detail;" +
            "df:从Playlist文件中获取Detaill信息")]
        public string Type { get; set; }
        [Option('n', "name", Required = true, HelpText = "t(p,d) Playlist Id;t(df) Playlist文件名")]
        public string Name { get; set; }
        [Option('f',  Required = false, HelpText = "保存文件名，为空则为默认文件名")]
        public string Filename { get; set; }
        [Option('i', Required = false, HelpText = "t(df),false:直接获取Playlist文件中的Detail信息;true:通过Playlist文件中的Id请求Detail信息。默认为true。歌曲数>1000时，只能为true")]
        [Value(0,Default = true)]
        public bool FromNetwork { get; set; }
    }
}
