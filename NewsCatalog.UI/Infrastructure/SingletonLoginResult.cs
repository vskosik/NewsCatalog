﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.UI.Infrastructure
{
    public static class SingletonLoginResult
    {
        public static bool IsLogged { get; set; } = false;
    }
}
