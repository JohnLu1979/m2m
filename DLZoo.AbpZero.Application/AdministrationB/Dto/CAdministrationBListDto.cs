using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.AdministrationB.Dto
{
    [AutoMapFrom(typeof(Entities.CAdministrationB))]
    public class CAdministrationBListDto //: Entity<string>
    {
        public virtual string addvname { get; set; }

        public virtual string parentcd { get; set; }

        public virtual int? leafflag { get; set; }

        public virtual int? levels { get; set; }

        public virtual string areaid { get; set; }
    }
}

