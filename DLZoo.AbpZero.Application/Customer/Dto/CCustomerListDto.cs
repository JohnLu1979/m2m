using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Customer.Dto
{
    [AutoMapFrom(typeof(Entities.CCustomer))]
    public class CCustomerListDto : Entity<int>
    {
        public virtual string customerName { get; set; }
        public virtual string address { get; set; }
        public virtual string tel { get; set; }
    }

}

