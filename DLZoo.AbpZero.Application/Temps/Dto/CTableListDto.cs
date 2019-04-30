using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Temps.Dto
{
    [AutoMapFrom(typeof(Entities.Temp.CTableClass))]
    public class CTableListDto : Entity<int>
    {        
        //public virtual int Id { get; set; }
        public virtual string Column_A { get; set; }
        public virtual decimal Column_B { get; set; }
    }
}
