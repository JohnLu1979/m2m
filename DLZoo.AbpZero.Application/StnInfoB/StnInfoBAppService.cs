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
        private readonly IRepository<Entities.CStnParaR, int> _stnPararRepository;
        public StnInfoBAppService(ISqlExecuter sqlExecuter,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository,
            IRepository<Entities.CStnParaR, int> stnPararRepository
            ) :base(CustomerRepository,IpRepository,VisitRecordRepository)
        {
            
            this._sqlExecuter = sqlExecuter;
            this._stnInfoBRepository = stnInfoBRepository;
            this._stnPararRepository = stnPararRepository;
        }

        public CDataResults<CStnInfoBListDto> GetStnInfoB(CStnInfoBInput input)
        {
            //Check Ip & customer
            if (!checkCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CStnInfoBListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            //var query =  this._stnInfoBRepository.GetAll() ;
            //if (!string.IsNullOrEmpty(input.areaName)) {
            //    query.Where(r => r.areaName.Contains(input.areaName));
            //}
            var query = from s in _stnInfoBRepository.GetAll()
                        join p in this._stnPararRepository.GetAll() on s.areaCode equals  p.stcd into temp
                        from ur in temp.DefaultIfEmpty()
                        orderby s.areaName ascending
                        select new CStnInfoBListDto
                        {
                            Id = s.Id,
                            areaCode = s.areaCode,
                            areaName = s.areaName,
                            stType = ur.paraTypeCode,
                            stlc = s.stlc
                        };


            //if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            //{
            //    query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            //}

            var result = query.ToList();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CStnInfoBListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            }; 
        }

        public CDataResults<string> GetStType(CBaseInput input)
        {
            //Check Ip & customer
            if (!checkCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<string>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query =this._stnInfoBRepository.GetAll().Select(r => r.stType).Distinct();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<string>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
    }
}
