using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Base.Dto
{
    public class CDataResults<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public List<T> Data { get; set; }

        public int Total { get; set; }
    }

    public class CDataResult<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
