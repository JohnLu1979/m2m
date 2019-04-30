using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.StnParaR.Dto
{
    [AutoMapFrom(typeof(Entities.CStnParaR))]
    public class CStnParaRListDto : Entity<int>
    {
        public virtual string paraid { get; set; }
        public virtual int? stid { get; set; }        
        public virtual string paraTypeCode { get; set; }
        public virtual string attrCode1 { get; set; }
        public virtual string attrCode2 { get; set; }
        public virtual string attrName2 { get; set; }
        public virtual int? position { get; set; }
        public virtual string stcd { get; set; }
        public virtual string rtucode { get; set; }
    }
}
