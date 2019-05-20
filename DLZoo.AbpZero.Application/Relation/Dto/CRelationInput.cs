using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTempProject.Base.Dto;

namespace MyTempProject.Relation.Dto
{
   public class CRelationInput : CBaseInput
    {
        public int customer_id { get; set; }
        public List<int> siteIdArr { get; set; }
    }
}
