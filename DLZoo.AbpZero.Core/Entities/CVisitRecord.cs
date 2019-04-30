using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Entities
{
    [Table("visit_record")]
    public class CVisitRecord:Entity<int>
    {
        //   [customerId] INT NULL,   
        public virtual int? customerId { get; set; }
        //   [visit_dateTime] DATETIME NULL,  
        public virtual DateTime visit_dateTime { get; set; }
        //   [ip]             NVARCHAR(50) NULL,
        [MaxLength(50)]
        public virtual string ip { get; set; }
        //   [flag]    INT NULL,
        public virtual int? flag { get; set; }
    }
    public enum VisitRecordFlag {
        Black = 0,
        White = 1
    }
}
