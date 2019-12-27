using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Verb("login", HelpText = "手机账号登陆")]
    class LoginOptions
    {
        [Option('p', "phone", Required = true, HelpText = "手机号")]
        public string Phone { get; set; }
        [Option('w', "pwd", Required = true, HelpText = "密码")]
        public string Password { get; set; }
        [Option('c', Required = false, HelpText = "国家编码，默认中国")]
        public string CountryCode { get; set; }
    }
}
