using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTempProject.Base.Dto;


namespace MyTempProject.User.Dto
{
   public class CUserInput : CBaseInput
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
