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

        private Tuple<int, List<CWmtRainDetailListDto>> GetRainTail(CWmtRainInput input)
        {
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
            return new Tuple<int, List<CWmtRainDetailListDto>>(totla, result);
        }

        public CDataResults<CWmtRainDetailListDto> GetWmtRainDetailFromMobile(CWmtRainInput input)
        {
            //Extract data from DB
            var result = GetRainTail(input);
            return new CDataResults<CWmtRainDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result.Item2,
                Total = result.Item1
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
            var result = GetRainTail(input);
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CWmtRainDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result.Item2,
                Total = result.Item1
            };
        }

        public CDataResults<CWmtRainTotalDto> GetWmtRainTotal(CWmtRainInput input)
        {
               var query = (from allData in (from region in _administrationBReposity.GetAll() where region.parentcd.Equals("2102")
                                          join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
                                          from cr in temp.DefaultIfEmpty()           
                                          join rain in _wmtRainRepository.GetAll().Where(c => c.collecttime >= input.fromTime && c.collecttime <= input.toTime) on cr.areaCode equals rain.stcd
                                          into relation
                                          from data in relation.DefaultIfEmpty()
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

            //var query = from region in _administrationBReposity.GetAll()
            //            join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
            //            from cr in temp.DefaultIfEmpty()
            //            join rain in _wmtRainRepository.GetAll() on cr.areaCode equals rain.stcd
            //            into relation
            //            from data in relation.DefaultIfEmpty()
            //            where data.collecttime >= input.fromTime && data.collecttime <= input.toTime
            //            && region.parentcd.StartsWith("2102")
            //            select new
            //            {

            //                addvname = region.addvname,
            //                paravalue = data.paravalue
            //            };
            //return null;
        }

        public CDataResults<CWmtRainTotalByHoursDto> GetWmtRainTotalByHours(CWmtRainInput input)
        {
            var now = new DateTime(2017, 8, 30);//DateTime.Now;//
            var beforeYesterday = now.AddDays(-2);
            var oneHourAgo = now.AddHours(-1);
            var threeHoursAgo = now.AddHours(-3);
            var sixHoursAgo = now.AddHours(-6);
            var twelveHoursAgo = now.AddHours(-12);
            var twentyFourHoursAgo = now.AddHours(-24);
            var addvcdArray = (input.addvcdArray == null) ? new string[] { } : input.addvcdArray.ToArray();
            var addvcdArrayLength = addvcdArray.Length;
            var query = from allData in (from site in _stnInfoBRepository.GetAll().Where(s => addvcdArrayLength == 0 || addvcdArray.Contains(s.addvcd))
                                         join rain in _wmtRainRepository.GetAll().Where(r => r.collecttime != null && r.collecttime >= beforeYesterday && r.collecttime < now) on site.areaCode equals rain.stcd into temp
                                         from cr in temp.DefaultIfEmpty()
                                         join admin in _administrationBReposity.GetAll() on site.addvcd equals admin.Id into relation
                                         from data in relation.DefaultIfEmpty()
                                         //where cr.collecttime != null && cr.collecttime >= beforeYesterday && cr.collecttime < now &&
                                            // (addvcdArrayLength == 0 || addvcdArray.Contains(site.addvcd))//((addvcdArray == null)? ((input.addvcd ==null) ? true : site.addvcd.StartsWith(input.addvcd)) : addvcdArray.Contains(site.addvcd))
                                         select new
                                         {
                                             areaCode = site.areaCode,
                                             areaName = site.areaName,
                                             addvcd = site.addvcd,
                                             addvname = data.addvname,
                                             collecttime = cr.collecttime,
                                             paravalue = cr.paravalue
                                         })
                        group allData by new { allData.areaName, allData.addvname } into lst
                        select new CWmtRainTotalByHoursDto
                        {
                            areaName = lst.Key.areaName,
                            addvname = lst.Key.addvname,
                            total_1 = lst.Where(t => t.collecttime > oneHourAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
                            total_3 = lst.Where(t => t.collecttime > threeHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
                            total_6 = lst.Where(t => t.collecttime > sixHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
                            total_12 = lst.Where(t => t.collecttime > twelveHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
                            total_24 = lst.Where(t => t.collecttime > twentyFourHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
                            total_48 = lst.Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue)
                        };
            var result = query.ToList();
            var totla = query.Count();
            return new CDataResults<CWmtRainTotalByHoursDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = totla
            };
        }
    }
}
