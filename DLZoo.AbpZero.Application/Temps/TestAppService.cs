using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MyTempProject.Configuration;
using MyTempProject.EntityFramework;
using MyTempProject.Temps.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyTempProject.Temps
{
    public class TestAppService : ITestAppService
    {
        private ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.Temp.CTableClass, long> _tableRepository;
        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        public TestAppService(ISqlExecuter sqlExecuter, 
            IRepository<Entities.Temp.CTableClass, long> tableRepository,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository
            ) {
            this._sqlExecuter = sqlExecuter;
            this._tableRepository = tableRepository;
            this._stnInfoBRepository = stnInfoBRepository;
        }
        public List<CTableListDto> FindColumnAFromTable()
        {
            ////HttpContext.Current
            //var ab = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
            //               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //var sss = Dns.GetHostEntry(IPAddress.Parse(ab)).AddressList;
            //foreach (var item in sss)
            //{
            //    var sssss = item;
            //}
            //string sql = "select Column_A from dbo.[Table]";
            //var result = _sqlExecuter.SqlQuery<string>(sql).ToList();
            //return result;


            //var results = _tableRepository.GetAll().ToList();
            //return results.MapTo<List<CTableListDto>>();

            var results = this._stnInfoBRepository.GetAll().ToList();
            return new List<CTableListDto>();
        }
    }
}
