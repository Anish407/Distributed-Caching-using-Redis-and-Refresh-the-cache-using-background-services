using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HostedServices.Models
{
    public class CacheSettings
    {
        public bool Enabled { get; set; } = true;
        public string ConnectionString { get; set; }
    }
}
