using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("add", HelpText = "Add file contents to the index.")]
    class AddOptions
    {
        [Option('t', "test", Required = true, HelpText = "Test")]
        public string Test { get; set; }
        [Option('r', "read", Required = false, HelpText = "Input files to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

        //Omitting long name, defaults to name of property, ie "--verbose"
        [Option(
          Default = false,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option("stdin",
          Default = false,
          HelpText = "Read from stdin")]
        public bool stdin { get; set; }

        [Value(0, MetaName = "offset", HelpText = "File offset.")]
        public long? Offset { get; set; }
    }
  
}
