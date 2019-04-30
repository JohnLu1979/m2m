using MyTempProject.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.StnInfoB.Dto
{
    public class CStnInfoBInput : CBaseInput
    {
        public string areaName { get; set; }
        public int customerId { get; set; }
    }
}
