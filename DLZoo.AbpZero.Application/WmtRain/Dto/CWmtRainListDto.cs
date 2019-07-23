using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.WmtRain.Dto
{
    [AutoMapFrom(typeof(Entities.CWmtRain))]
    public class CWmtRainListDto : Entity<int>
    {
        public virtual string stcd { get; set; }
        public virtual double? paravalue { get; set; }
        public virtual DateTime collecttime { get; set; }
        public virtual DateTime systemtime { get; set; }
        public virtual string uniquemark { get; set; }
        public virtual DateTime gentm { get; set; }
    }
    public class CWmtRainDetailListDto : Entity<int>
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
 

    public class CWmtRainTotalDto
    {
        public virtual string addvname { get; set; }
        public virtual double? total { get; set; }
    }
    public class CWmtRainTotalByHoursDto
    {
        public virtual string areaName { get; set; }
        public virtual string addvname { get; set; }
        public virtual double? total_1 { get; set; }
        public virtual double? total_3 { get; set; }
        public virtual double? total_6 { get; set; }
        public virtual double? total_12 { get; set; }
        public virtual double? total_24 { get; set; }
        public virtual double? total_48 { get; set; }
    }
}

