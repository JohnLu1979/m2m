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
        private readonly IRepository<Entities.CStnParaR, int> _stnParaRRepository;
        private readonly IRepository<Entities.CWmtRain, int> _wmtRainRepository;
        private readonly IRepository<Entities.CWmtRiver, int> _wmtRiverRepository;
        private readonly IRepository<Entities.CWmtRsvr, int> _wmtRsvrRepository;
        private readonly IRepository<Entities.CWmtSoilMoisture, int> _wmtSoilMoistureRepository;
        private readonly IRepository<Entities.CCustomer, int> _wmtCustomerRepository;
        private readonly IRepository<Entities.CIp, int> _wmtIpRepository;
        private readonly IRepository<Entities.CVisitRecord, int> _wmtVisitRecordRepository;
        public TestAppService(ISqlExecuter sqlExecuter, 
            IRepository<Entities.Temp.CTableClass, long> tableRepository,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CStnParaR, int> stnParaRRepository,
            IRepository<Entities.CWmtRain, int> wmtRainRepository,
            IRepository<Entities.CWmtRiver, int> wmtRiverRepository,
            IRepository<Entities.CWmtRsvr, int> wmtRsvrRepository,
            IRepository<Entities.CWmtSoilMoisture, int> wmtSoilMoistureRepository,
            IRepository<Entities.CCustomer, int> wmtCustomerRepository,
            IRepository<Entities.CIp, int> wmtIpRepository,
            IRepository<Entities.CVisitRecord, int> wmtVisitRecordRepository
            ) {
            this._sqlExecuter = sqlExecuter;
            this._tableRepository = tableRepository;
            this._stnInfoBRepository = stnInfoBRepository;
            this._stnParaRRepository = stnParaRRepository;
            this._wmtRainRepository = wmtRainRepository;
            this._wmtRiverRepository = wmtRiverRepository;
            this._wmtRsvrRepository = wmtRsvrRepository;
            this._wmtSoilMoistureRepository = wmtSoilMoistureRepository;
            this._wmtCustomerRepository = wmtCustomerRepository;
            this._wmtIpRepository = wmtIpRepository;
            this._wmtVisitRecordRepository = wmtVisitRecordRepository;
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

            var results = this._stnParaRRepository.GetAll().ToList();
            var results1 = this._wmtCustomerRepository.GetAll().ToList();
            var results2 = this._wmtIpRepository.GetAll().ToList();
            var results3 = this._wmtVisitRecordRepository.GetAll().ToList();
            return new List<CTableListDto>();
        }
    }
}
