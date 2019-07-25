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
using MyTempProject.WmtRain.Dto;

namespace MyTempProject.WmtRain
{
    public class WmtRainAppService : CBaseAppService, IWmtRainAppService
    {

        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        private readonly IRepository<Entities.CWmtRain, int> _wmtRainRepository;
        private readonly IRepository<Entities.CRelation, int> _relationReposity;
        private readonly IRepository<Entities.CAdministrationB, string> _administrationBReposity;

 
        public WmtRainAppService(
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CWmtRain, int> wmtRainRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository,
            IRepository<Entities.CRelation, int> relationReposity,
            IRepository<Entities.CAdministrationB, string> administrationBReposity
            ) : base(CustomerRepository, IpRepository, VisitRecordRepository)
        {

            this._stnInfoBRepository = stnInfoBRepository;
            this._wmtRainRepository = wmtRainRepository;
            this._relationReposity = relationReposity;
            this._administrationBReposity = administrationBReposity;
        }

        public CDataResults<CWmtRainListDto> GetWmtRain(CWmtRainInput input)
        {
            //Check Ip & customer
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CWmtRainListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null,
                    Total = 0
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
                Data = result,
                Total = query.Count()
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
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        join res in _relationReposity.GetAll() on s.Id equals res.site_id
                        where res.customer_id == input.customerId
                        orderby r.collecttime
                        select new CWmtRainDetailListDto
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
            if (input.fromTime != null)
            {
                query = query.Where(r => r.collecttime > input.fromTime);
            }
            if (input.toTime != null)
            {
                query = query.Where(r => r.collecttime < input.toTime);
            }
            if (!string.IsNullOrEmpty(input.stcd))
            {
                query = query.Where(r => r.stcd == input.stcd);
            }

            var totla = query.Count();
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
                Data = result,
                Total = totla
            };
        }

        public CDataResults<CWmtRainTotalDto> GetWmtRainTotal(CWmtRainInput input)
        {
            #region
            //var query = (from allData in (from site in _stnInfoBRepository.GetAll()
            //                              join rain in _wmtRainRepository.GetAll() on site.areaCode equals rain.stcd into temp
            //                              from cr in temp.DefaultIfEmpty()
            //                              join admin in _administrationBReposity.GetAll() on site.addvcd equals admin.Id into relation
            //                              from data in relation.DefaultIfEmpty()
            //                              where cr.collecttime >= input.fromTime && cr.collecttime <= input.toTime
            //                              select new
            //                              {
            //                                  areaCode = site.areaCode,
            //                                  areaName = site.areaName,
            //                                  addvcd = site.addvcd,
            //                                  addvname = data.addvname,
            //                                  paravalue = cr.paravalue
            //                              })
            //             group allData by allData.addvname into lst
            //             select new CWmtRainTotalDto
            //             {
            //                 addvname = lst.Key,
            //                 total = lst.Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue)
            //             }).OrderByDescending(t => t.total);
            //var result = query.ToList();
            //var totla = query.Count();
            //return new CDataResults<CWmtRainTotalDto>()
            //{
            //    IsSuccess = true,
            //    ErrorMessage = null,
            //    Data = result,
            //    Total = totla
            //};

            //var query = from region in _administrationBReposity.GetAll()
            //         join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
            //         //from cr in temp.DefaultIfEmpty()
            //         //where region.parentcd == "2012"
            //         select new CWmtRainTotalDto
            //         {
            //             addvname = region.addvname,
            //             total=0
            //                                  };
            #endregion
            var query = (from allData in (from region in _administrationBReposity.GetAll()
                                          join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
                                          from cr in temp.DefaultIfEmpty()
                                          join rain in _wmtRainRepository.GetAll() on cr.areaCode equals rain.stcd
                                          into relation
                                          from data in relation.DefaultIfEmpty()
                                          where data.collecttime >= input.fromTime && data.collecttime <= input.toTime 
                                          && region.parentcd == "2012"
                                          select new
                                          {
                                              addvname = region.addvname,
                                              paravalue = data.paravalue
                                          })
                         group allData by allData.addvname into lst
                         select new CWmtRainTotalDto
                         {
                             addvname = lst.Key,
                             total = lst.Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue)
                         }).OrderByDescending(t => t.total);
            var result = query.ToList();
            var totla = query.Count();
            return new CDataResults<CWmtRainTotalDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = totla
            };
        }
    }
}
