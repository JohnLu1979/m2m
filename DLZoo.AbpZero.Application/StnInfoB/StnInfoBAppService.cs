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
using MyTempProject.StnInfoB.Dto;
using MyTempProject.Base;
using MyTempProject.Base.Dto;

namespace MyTempProject.StnInfoB
{
    public class StnInfoBAppService : CBaseAppService, IStnInfoBAppService
    {
        private ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        private readonly IRepository<Entities.CCustomer, int> _wmtCustomerRepository;
        private readonly IRepository<Entities.CIp, int> _wmtIpRepository;
        private readonly IRepository<Entities.CVisitRecord, int> _wmtVisitRecordRepository;
        public StnInfoBAppService(ISqlExecuter sqlExecuter,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CCustomer, int> wmtCustomerRepository,
            IRepository<Entities.CIp, int> wmtIpRepository,
            IRepository<Entities.CVisitRecord, int> wmtVisitRecordRepository
            ) :base(wmtCustomerRepository,wmtIpRepository,wmtVisitRecordRepository)
        {
            
            this._sqlExecuter = sqlExecuter;
            this._stnInfoBRepository = stnInfoBRepository;
        }

        public CDataResults<CStnInfoBListDto> GetStnInfoB(CStnInfoBInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CStnInfoBListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = this._stnInfoBRepository.GetAll();
            if (!string.IsNullOrEmpty(input.areaName)) {
                query.Where(r => r.areaName.Contains(input.areaName));
            }
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query.Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CStnInfoBListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CStnInfoBListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            }; 
        }
    }
}
