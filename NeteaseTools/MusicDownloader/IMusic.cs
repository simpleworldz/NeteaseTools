﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDownloader
{
    public interface IMusic
    {
        string GetFirstUrl(string name);
    }
}
