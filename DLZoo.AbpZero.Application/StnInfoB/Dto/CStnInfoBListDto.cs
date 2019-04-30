using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.StnInfoB.Dto
{
    [AutoMapFrom(typeof(Entities.CStnInfoB))]
    public class CStnInfoBListDto : Entity<int>
    {
        public override int Id { get; set; }
        public virtual string areaCode { get; set; }
        public virtual string areaName { get; set; }
        public virtual string areaDesc { get; set; }
        public virtual double? lgtd { get; set; }
        public virtual double? lttd { get; set; }
        public virtual string addvcd { get; set; }
        public virtual string iconurl { get; set; }
        public virtual string projectCode { get; set; }
        public virtual int? showLevel { get; set; }
        public virtual string stlc { get; set; }
        public virtual string posCode { get; set; }
        public virtual string posName { get; set; }
        public virtual string stType { get; set; }
        public virtual string photo { get; set; }
        public virtual int? parentid { get; set; }
        public virtual int? temporary { get; set; }
        public virtual int? artificial { get; set; }
        public virtual string contacter { get; set; }
        public virtual string telephone { get; set; }
        public virtual DateTime installdate { get; set; }
        public virtual double? holeDepth { get; set; }
    }
}
