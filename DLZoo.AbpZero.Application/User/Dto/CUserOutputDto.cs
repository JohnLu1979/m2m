using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.User.Dto
{
    [AutoMapFrom(typeof(Entities.CUser))]
    public class CUserOutputDto
    {
        public virtual string username { get; set; }
        public virtual  string password { get; set; }
        public virtual string address { get; set; }
        public virtual string tel { get; set; }
    }
}
