using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Base.Dto
{
    public class CBaseInput
    {
        public int customerId { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
    }
}