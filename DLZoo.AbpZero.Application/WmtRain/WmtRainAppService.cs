﻿using Abp.AutoMapper;
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
using MyTempProject.WmtRain.Dto;

namespace MyTempProject.WmtRain
{
    public class WmtRainAppService : CBaseAppService, IWmtRainAppService
    {
        private ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        private readonly IRepository<Entities.CWmtRain, int> _wmtRainRepository;
        public WmtRainAppService(ISqlExecuter sqlExecuter,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CWmtRain, int> wmtRainRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository
            ) :base(CustomerRepository,IpRepository,VisitRecordRepository)
        {
            
            this._sqlExecuter = sqlExecuter;
            this._stnInfoBRepository = stnInfoBRepository;
            this._wmtRainRepository = wmtRainRepository;
        }

        public CDataResults<CWmtRainListDto> GetWmtRain(CWmtRainInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CWmtRainListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = this._wmtRainRepository.GetAll();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CWmtRainListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CWmtRainListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            }; 
        }

        public CDataResults<CWmtRainDetailListDto> GetWmtRainDetail(CWmtRainInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CWmtRainDetailListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = from r in _wmtRainRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode into rs
                        from rst in rs.DefaultIfEmpty()
                        orderby r.collecttime
                        select new CWmtRainDetailListDto
                        {
                            areaCode = rst.areaCode,
                            areaName = rst.areaName,
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
            return new CDataResults<CWmtRainDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
    }
}
