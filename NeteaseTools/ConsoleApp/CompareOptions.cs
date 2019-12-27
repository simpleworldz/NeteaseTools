using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("compare", HelpText = "对比")]
    class CompareOptions
    {
        [Option('t', "type", Required = true, HelpText = "对比类型（dd,dp,pp,pd)，d：Detail; p:Playlist。如：dd则为Detail文件和Detail文件对比")]
        public string Type { get; set; }
        [Option('o', "old", Required = true, HelpText = "旧文件")]
        public string OldFile { get; set; }
        [Option('n', "new", Required = true, HelpText = "新文件")]
        public string NewFile { get; set; }
        [Option('s', "save", Required = false, HelpText = "保存文件名，为空则为默认文件名")]
        public string SaveFilename { get; set; }
    }
}
