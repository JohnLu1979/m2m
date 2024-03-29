﻿using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.WmtRsvr.Dto
{
    [AutoMapFrom(typeof(Entities.CWmtRsvr))]
    public class CWmtRsvrListDto : Entity<int>
    {
        public virtual string stcd { get; set; }
        public virtual double? paravalue { get; set; }
        public virtual DateTime collecttime { get; set; }
        public virtual DateTime systemtime { get; set; }
        public virtual string uniquemark { get; set; }
        public virtual DateTime gentm { get; set; }
    }
    public class CWmtRsvrDetailListDto : Entity<int>
    {
        //s.areaCode,s.areaName,r.[stcd],[paravalue],[collecttime] ,[systemtime],[uniquemark],[gentm]
        public virtual string areaCode { get; set; }
        public virtual string areaName { get; set; }
        public virtual string stcd { get; set; }
        public virtual double? paravalue { get; set; }
        public virtual DateTime collecttime { get; set; }
        public virtual DateTime systemtime { get; set; }
        public virtual string uniquemark { get; set; }
        public virtual DateTime gentm { get; set; }
    }
}

