using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Relation.Dto
{
    [AutoMapFrom(typeof(Entities.CRelation))]
    public class CRelationListDto : Entity<int>
    {
        public virtual int customer_id { get; set; }
        public virtual int site_id { get; set; }
    }
}
