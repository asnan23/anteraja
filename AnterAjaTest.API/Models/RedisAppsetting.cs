﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnterAjaTest.API.Models
{
    public class RedisAppsetting
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
        public string ssl { get; set; }
    }
}
