using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("download", HelpText = "下载")]
    class DownloadOptions
    {
        [Option('t', "type", Required = true, HelpText = "下载时加载的文件类型：d:Detail; n:NoCopyright")]
        public string Type { get; set; }
        [Option('n', "name", Required = true, HelpText = "文件名")]
        public string Name { get; set; }
        [Option('p', "platform", Required = false, HelpText = "n:Netease; q:QQ; b:Baidu; k:Kugou; x:Xiami  默认：nqbkx（所有平台）")]
        [Value(0,Default = "nqbkx")]
        public string Platform { get; set; }
       
    }
}
