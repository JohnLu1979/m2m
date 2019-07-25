using MyTempProject.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.WmtRain.Dto
{
    public class CWmtRainInput : CBaseInput
    {
        public string stcd { get; set; }
        public string addvcd { get; set; }
        public List<string> addvcdArray { get; set; }
        public DateTime? fromTime { get; set; }
        public DateTime? toTime { get; set; }
        public string visitType { get; set; }
    }
}
