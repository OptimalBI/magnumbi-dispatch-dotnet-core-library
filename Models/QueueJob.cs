﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Optimal.MagnumMicroservices.Library.Models{
    public class QueueJob{
        public string AppId{ get; set; }
        public dynamic Data{ get; set; }
    }
}