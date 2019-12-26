using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeteaseMusic.Song.Models
{
    public class DetailsAndPrivileges
    {
        public IEnumerable<Detail> songs { get; set; }
        public IEnumerable<Privilege> privileges { get; set; }
    }
}
