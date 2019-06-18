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
using MyTempProject.Base;
using MyTempProject.Base.Dto;
using MyTempProject.StnParaR.Dto;

namespace MyTempProject.StnParaR
{
    public class StnParaRAppService : CBaseAppService, IStnParaRAppService
    {
        private ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.CStnParaR, int> _stnParaRRepository;
        public StnParaRAppService(ISqlExecuter sqlExecuter,
            IRepository<Entities.CStnParaR, int> stnParaRRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository
            ) :base(CustomerRepository,IpRepository,VisitRecordRepository)
        {
            
            this._sqlExecuter = sqlExecuter;
            this._stnParaRRepository = stnParaRRepository;
        }

        public CDataResults<CStnParaRListDto> GetStnParaR(CStnParaRInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CStnParaRListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null,
                    Total = 0
                };
            }

            //Extract data from DB
            var query = this._stnParaRRepository.GetAll();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CStnParaRListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CStnParaRListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = query.Count()
            }; 
        }
        
    }
}
