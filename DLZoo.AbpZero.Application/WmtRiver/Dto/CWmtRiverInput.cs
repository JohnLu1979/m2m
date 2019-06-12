﻿using MyTempProject.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.WmtRiver.Dto
{
    public class CWmtRiverInput : CBaseInput
    {
        public string stcd { get; set; }
        public DateTime? fromTime { get; set; }
        public DateTime? toTime { get; set; }
    }
}
