using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeteaseMusic.Song.Models
{
    public class Detail
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Artist> ar { get; set; }
        public Album al { get; set; }
    }
}
