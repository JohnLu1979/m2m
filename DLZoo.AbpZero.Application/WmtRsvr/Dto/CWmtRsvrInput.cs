﻿using MyTempProject.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.WmtRsvr.Dto
{
    public class CWmtRsvrInput : CBaseInput
    {
        public string stcd { get; set; }
        public DateTime? fromTime { get; set; }
        public DateTime? toTime { get; set; }
    }
}
