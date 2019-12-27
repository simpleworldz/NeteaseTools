using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("info", HelpText = "获取信息")]
    class InfoOptions
    {
        [Option('t',"type", Required = true, HelpText = "信息类型，nc:没有版权的歌曲")]
        public string Type { get; set; }
        [Option('f',"file", Required = true, HelpText = "保存文件名，为空则为默认文件名")]
        public string Filename { get; set; }
        [Option('s',"Save", Required = false, HelpText = "保存文件名，为空则为默认文件名")]
        public string SaveFilename { get; set; }
    }
}
