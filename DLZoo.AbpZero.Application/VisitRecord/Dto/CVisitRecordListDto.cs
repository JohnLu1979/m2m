using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.VisitRecord.Dto
{
    [AutoMapFrom(typeof(Entities.CVisitRecord))]
    public class CVisitRecordListDto : Entity<int>
    {
        public virtual int? customerId { get; set; }
        public virtual DateTime visit_dateTime { get; set; }
        public virtual string ip { get; set; }
        public virtual int? flag { get; set; }
    }
    public class CVisitRecordDetailListDto : Entity<int>
    {
        public virtual int? customerId { get; set; }
        public virtual string customerName { get; set; }
        public virtual DateTime visit_dateTime { get; set; }
        public virtual string ip { get; set; }
        public virtual int? flag { get; set; }
    }

}

