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
using MyTempProject.WmtSoilMoisture.Dto;

namespace MyTempProject.WmtSoilMoisture
{
    public class WmtSoilMoistureAppService : CBaseAppService, IWmtSoilMoistureAppService
    {
        
        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        private readonly IRepository<Entities.CWmtSoilMoisture, int> _wmtSoilMoistureRepository;
        private readonly IRepository<Entities.CRelation, int> _relationReposity;
        public WmtSoilMoistureAppService( 
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CWmtSoilMoisture, int> wmtSoilMoistureRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository,
            IRepository<Entities.CRelation, int> relationReposity
            ) :base(CustomerRepository,IpRepository,VisitRecordRepository)
        {        
            this._stnInfoBRepository = stnInfoBRepository;
            this._wmtSoilMoistureRepository = wmtSoilMoistureRepository;
            this._relationReposity = relationReposity;
        }

        public CDataResults<CWmtSoilMoistureListDto> GetWmtSoilMoisture(CWmtSoilMoistureInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CWmtSoilMoistureListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = this._wmtSoilMoistureRepository.GetAll();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CWmtSoilMoistureListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CWmtSoilMoistureListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            }; 
        }

        public CDataResults<CWmtSoilMoistureDetailListDto> GetWmtSoilMoistureDetail(CWmtSoilMoistureInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CWmtSoilMoistureDetailListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = from r in _wmtSoilMoistureRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        join res in _relationReposity.GetAll() on s.Id equals res.site_id
                        where res.customer_id == input.customerId
                        orderby r.collecttime
                        select new CWmtSoilMoistureDetailListDto
                        {
                            areaCode = s.areaCode,
                            areaName = s.areaName,
                            stcd = r.stcd,
                            paravalue = r.paravalue,
                            collecttime = r.collecttime,
                            systemtime = r.systemtime,
                            uniquemark = r.uniquemark,
                            gentm = r.gentm
                        };
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderByDescending(r => r.collecttime).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CWmtSoilMoistureDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
    }
}
