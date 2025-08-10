﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackton.Shared.FileServices.Settings
{
    public record AzureBlobOptions
    {
        public string ConnectionString { get; set; }
        public string VideoContainerName { get; set; }
    }
}
