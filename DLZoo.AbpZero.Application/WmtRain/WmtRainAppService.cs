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

        private readonly IRepository<Entities.CStnInfoB, int> _stnInfoBRepository;
        private readonly IRepository<Entities.CWmtRain, int> _wmtRainRepository;
        private readonly IRepository<Entities.CWmtRain_FiveMinute, int> _wmtRainFiveMinutesRepository;
        private readonly IRepository<Entities.CRelation, int> _relationReposity;
        private readonly IRepository<Entities.CAdministrationB, string> _administrationBReposity;

        public WmtRainAppService(
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CWmtRain, int> wmtRainRepository,
            IRepository<Entities.CWmtRain_FiveMinute, int> wmtRainFiveMinutesRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository,
            IRepository<Entities.CRelation, int> relationReposity,
            IRepository<Entities.CAdministrationB, string> administrationBReposity

            ) : base(CustomerRepository, IpRepository, VisitRecordRepository)
        {

            this._stnInfoBRepository = stnInfoBRepository;
            this._wmtRainRepository = wmtRainRepository;
            this._wmtRainFiveMinutesRepository = wmtRainFiveMinutesRepository;
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

        public CDataResult<CWmtRainRegionDetailDto> GetWmtRainRegionDetailFromMobile(CWmtRainInput input)
        {
            CWmtRainRegionDetailDto result = new CWmtRainRegionDetailDto();
            //Get Site Count and rain site count
            result.stationCount = this._stnInfoBRepository.GetAll().Where(c => c.addvcd == input.addvcd).Count();
            var query = from r in _wmtRainFiveMinutesRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        where ((input.fromTime == null || r.tm > input.fromTime) && s.addvcd == input.addvcd)
                        select new { areaCode = s.areaCode };
            result.stationCountRain = query.Distinct().Count();

            // Max Rain
            var query1 = from r in _wmtRainFiveMinutesRepository.GetAll()
                         join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                         where ((input.fromTime == null || r.tm > input.fromTime) && s.addvcd == input.addvcd)
                         select new
                         {
                             areaCode = s.areaCode,
                             areaName = s.areaName,
                             paravalue = r.drp,
                             time = r.tm
                         };
            var dataList = query1.ToList();
            List<CWmtRainDetailListDto> hourDataList = new List<CWmtRainDetailListDto>();
            var timelist = dataList.GroupBy(c => new { c.areaCode, c.areaName, c.time.Date }).ToList();
            foreach (var item in timelist)
            {
                CWmtRainDetailListDto dto = new CWmtRainDetailListDto()
                {
                    areaCode = item.Key.areaCode,
                    areaName = item.Key.areaName,
                    collecttime = item.Key.Date,
                    paravalue = item.Sum(c => c.paravalue)
                };
                hourDataList.Add(dto);
            }
            if (hourDataList.Count() > 0)//.Count() > 0
            {
                var maxResult = hourDataList.OrderByDescending(r => r.paravalue).FirstOrDefault();
                result.maxParavalue = maxResult.paravalue;
                result.maxParaStation = maxResult.areaName;
            }
            else
            {
                result.maxParavalue = 0;
                result.maxParaStation = "";
            }
            // Max Rain Hour
            var query2 = from r in _wmtRainFiveMinutesRepository.GetAll()
                         join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                         where ((input.fromTime == null || r.tm > input.fromTime) && s.addvcd == input.addvcd)
                         select new
                         {
                             areaCode = s.areaCode,
                             areaName = s.areaName,
                             paravalue = r.drp,
                             time = r.tm
                         };
            var dataList2 = query2.ToList();
            List<CWmtRainDetailListDto> hourDataList2 = new List<CWmtRainDetailListDto>();

            var timelist2 = dataList2.GroupBy(c => new { areaCode = c.areaCode, areaName = c.areaName, Date = c.time.Date.AddHours(c.time.Hour) }).ToList();
            foreach (var item in timelist2)
            {
                CWmtRainDetailListDto dto = new CWmtRainDetailListDto()
                {
                    areaCode = item.Key.areaCode,
                    areaName = item.Key.areaName,
                    collecttime = item.Key.Date,
                    paravalue = item.Sum(c => c.paravalue)
                };
                hourDataList2.Add(dto);
            }
            if (hourDataList2.Count() > 0)
            {
                var maxResult = hourDataList2.OrderByDescending(r => r.paravalue).FirstOrDefault();
                result.maxParavalueHour = maxResult.paravalue;
                result.maxParaStationHour = maxResult.areaName;
                result.maxParaTimeHour = maxResult.collecttime;
            }
            else
            {
                result.maxParavalueHour = 0;
                result.maxParaStationHour = "";
                result.maxParaTimeHour = null;
            }
            return new CDataResult<CWmtRainRegionDetailDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
        public CDataResult<CWmtRainDetailListDto> GetMaxWmtRainDayTotalFromMobile(CWmtRainInput input)
        {
            var query = from r in _wmtRainFiveMinutesRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        where (input.fromTime == null || r.tm > input.fromTime)
                        select new
                        {
                            areaCode = s.areaCode,
                            areaName = s.areaName,
                            paravalue = r.drp,
                            time = r.tm
                        };
            var dataList = query.ToList();
            List<CWmtRainDetailListDto> hourDataList = new List<CWmtRainDetailListDto>();
            var timelist = dataList.GroupBy(c => new { c.areaCode, c.areaName, c.time.Date }).ToList();
            foreach (var item in timelist)
            {
                CWmtRainDetailListDto dto = new CWmtRainDetailListDto()
                {
                    areaCode = item.Key.areaCode,
                    areaName = item.Key.areaName,
                    collecttime = item.Key.Date,
                    paravalue = item.Sum(c => c.paravalue)
                };
                hourDataList.Add(dto);
            }
            //if (query.Count() > 0)
            //    if (dataList != null)
            //    {
            //        foreach (var item in dataList)
            //        {
            //            var time = item.time.Date.AddHours(item.time.Hour);
            //            if (hourDataList.Any(r => r.collecttime == time && r.areaCode == item.areaCode))
            //            {
            //                hourDataList.Find(r => r.collecttime == time).paravalue += item.paravalue;
            //            }
            //            else
            //            {
            //                hourDataList.Add(new CWmtRainDetailListDto()
            //                {
            //                    areaCode = item.areaCode,
            //                    areaName = item.areaName,
            //                    collecttime = time,
            //                    paravalue = item.paravalue
            //                });
            //            }
            //        }
            //    }
            if (hourDataList.Count() > 0)//.Count() > 0
            {
                var maxResult = hourDataList.OrderByDescending(r => r.paravalue).FirstOrDefault();
                return new CDataResult<CWmtRainDetailListDto>()
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Data = maxResult
                };
            }
            else
            {
                return new CDataResult<CWmtRainDetailListDto>()
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Data = null
                };
            }
        }
        public CDataResult<CWmtRainDetailListDto> GetMaxWmtRainHourTotalFromMobile(CWmtRainInput input)
        {
            //var query = from allData in (from r in _wmtRainFiveMinutesRepository.GetAll()
            //                join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
            //                where (input.fromTime == null || r.tm > input.fromTime)
            //                select new
            //                {
            //                    areaCode = s.areaCode,
            //                    areaName = s.areaName,
            //                    paravalue = r.drp,
            //                    time = r.tm.Date.AddHours(r.tm.Hour)
            //                })
            //            group allData by new { allData.areaCode, allData.areaName,allData.time } into lst
            //            select new
            //            {
            //                areaCode = lst.Key.areaCode,
            //                areaName = lst.Key.areaName,
            //                paravalue = lst.Sum(c=>c.paravalue),
            //                time = lst.Key.time
            //            };
            var query = from r in _wmtRainFiveMinutesRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        where (input.fromTime == null || r.tm > input.fromTime)
                        select new
                        {
                            areaCode = s.areaCode,
                            areaName = s.areaName,
                            paravalue = r.drp,
                            time = r.tm
                        };
            var dataList = query.ToList();
            List<CWmtRainDetailListDto> hourDataList = new List<CWmtRainDetailListDto>();

            var timelist = dataList.GroupBy(c => new { areaCode = c.areaCode, areaName = c.areaName, Date = c.time.Date.AddHours(c.time.Hour) }).ToList();
            foreach (var item in timelist)
            {
                CWmtRainDetailListDto dto = new CWmtRainDetailListDto()
                {
                    areaCode = item.Key.areaCode,
                    areaName = item.Key.areaName,
                    collecttime = item.Key.Date,
                    paravalue = item.Sum(c => c.paravalue)
                };
                hourDataList.Add(dto);
            }

            //if (query.Count() > 0)
            //if (dataList != null) {
            //    foreach (var item in dataList)
            //    {
            //        var time = item.time.Date.AddHours(item.time.Hour);
            //        if (hourDataList.Any(r => r.collecttime == time && r.areaCode == item.areaCode))
            //        {
            //            hourDataList.Find(r => r.collecttime == time).paravalue += item.paravalue;
            //        }
            //        else
            //        {
            //            hourDataList.Add(new CWmtRainDetailListDto()
            //            {
            //                areaCode = item.areaCode,
            //                areaName = item.areaName,
            //                collecttime = time,
            //                paravalue = item.paravalue
            //            });
            //        }
            //    }
            //}
            if (hourDataList.Count() > 0)//.Count() > 0
            {
                var maxResult = hourDataList.OrderByDescending(r => r.paravalue).FirstOrDefault();
                return new CDataResult<CWmtRainDetailListDto>()
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Data = maxResult
                };
            }
            else
            {
                return new CDataResult<CWmtRainDetailListDto>()
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Data = null
                };
            }


        }

        public CDataResults<CWmtRainDetailListDto> GetWmtRainDetailFromMobile(CWmtRainInput input)
        {
            //input.stcd = "00065156";
            //input.fromTime = new DateTime(2017, 9, 2, 12, 0, 0);
            //input.toTime = new DateTime(2017, 9, 3, 12, 0, 0);
            //Extract data from DB
            var query = from r in _wmtRainFiveMinutesRepository.GetAll()
                        join s in _stnInfoBRepository.GetAll() on r.stcd equals s.areaCode
                        //join res in _relationReposity.GetAll() on s.Id equals res.site_id
                        orderby r.tm
                        select new CWmtRainDetailListDto
                        {
                            areaCode = s.areaCode,
                            areaName = s.areaName,
                            stcd = r.stcd,
                            paravalue = r.drp,
                            collecttime = r.tm,
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
            return new CDataResults<CWmtRainDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = totla
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

            var query = (from regTotal in (from allData in (from region in _administrationBReposity.GetAll()
                                                            where region.parentcd.Equals("2102")
                                                            join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
                                                            from cr in temp.DefaultIfEmpty()
                                                            join rain in _wmtRainFiveMinutesRepository.GetAll().Where(c => c.tm >= input.fromTime && c.tm <= input.toTime) on cr.areaCode equals rain.stcd
                                                            into relation
                                                            from data in relation.DefaultIfEmpty()
                                                            select new
                                                            {
                                                                addvcd = region.Id,
                                                                addvName = region.addvname,
                                                                areaName = cr.areaName,
                                                                areaCode = cr.areaCode,
                                                                paraValue = data.drp
                                                            })
                                           group allData by new { allData.addvcd, allData.addvName, allData.areaName, allData.areaCode } into lst //, allData.areaName, allData.areaCode
                                           select new
                                           {
                                               addvcd = lst.Key.addvcd,
                                               addvName = lst.Key.addvName,
                                               areaName = lst.Key.areaName,
                                               areaCode = lst.Key.areaCode,
                                               total =  lst.Sum(c => c.paraValue)  
                                           })
                         group regTotal by new { regTotal.addvcd, regTotal.addvName } into regList
                         select new CWmtRainTotalDto
                         {
                             addvcd = regList.Key.addvcd,
                             addvName = regList.Key.addvName,
                             num = regList.Count(),
                             total = regList.Sum(c => c.total.ToString() == null ? 0 : c.total),
                         }).OrderBy(t => t.total);
            var result = query.ToList();
            result.ForEach(act =>
            {
                act.cal = Math.Round(Convert.ToDouble(act.total / act.num), 2);
            });

            var total = query.Count();
            var results = result.OrderBy(t => t.cal);
            //return null;
            return new CDataResults<CWmtRainTotalDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = results.ToList(),
                Total = total
            };
        }

        public CDataResults<CWmtRainTotalDto> GetWmtRainTotal1(CWmtRainInput input)
        {
            var query = (from regTotal in (from allData in (from region in _administrationBReposity.GetAll()
                                                            where region.parentcd.Equals("2102")
                                                            join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
                                                            from cr in temp.DefaultIfEmpty()
                                                            join rain in _wmtRainFiveMinutesRepository.GetAll().Where(c => c.tm >= input.fromTime && c.tm <= input.toTime) on cr.areaCode equals rain.stcd
                                                            into relation
                                                            from data in relation.DefaultIfEmpty()
                                                            select new
                                                            {
                                                                addvcd = region.Id,
                                                                addvName = region.addvname,
                                                                areaName = cr.areaName,
                                                                areaCode = cr.areaCode,
                                                                paraValue = data.drp
                                                            })
                                           group allData by new { allData.addvcd, allData.addvName, allData.areaName, allData.areaCode } into lst //
                                           select new
                                           {
                                               addvcd = lst.Key.addvcd,
                                               addvName = lst.Key.addvName,
                                               areaName = lst.Key.areaName,
                                               areaCode = lst.Key.areaCode,
                                               total = lst.Sum(c => c.paraValue)//(lst.Where(c => c.paraValue != null).Count() > 1) ? lst.Max(c => c.paraValue) - lst.Min(c => c.paraValue) : lst.Max(c => c.paraValue)//lst.Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue)
                                           })
                         group regTotal by new { regTotal.addvcd, regTotal.addvName } into regList//, regTotal.areaName, regTotal.areaCode
                         select new CWmtRainTotalDto
                         {
                             addvcd = regList.Key.addvcd,
                             addvName = regList.Key.addvName,
                             //areaName = regList.Key.areaName,
                             //areaCode = regList.Key.areaCode,
                             num = regList.Count(),
                             total = regList.Sum(c => c.total == null ? 0 : c.total),
                             //cal = Math.Round(Convert.ToDouble(regList.Sum(c => c.total == null ? 0 : c.total)) / regList.Count(), 2)
                         }).OrderBy(t => t.total);
            var result = query.ToList();

            if (input.addvcdArray != null && input.addvcdArray.Count > 0)
            {
                result = result.Where(c => input.addvcdArray.Contains(c.addvcd)).ToList();
            }
            result.ForEach(act =>
            {
                act.cal = Math.Round(Convert.ToDouble(act.total / act.num), 2);
            });

            var total = query.Count();
            var results = result.OrderBy(t => t.cal);
            //return null;
            return new CDataResults<CWmtRainTotalDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = results.ToList(),
                Total = total
            };
        }
        public CDataResults<CWmtRainTotalBySiteDto> GetWmtRainTotalBySite(CWmtRainInput input)
        {
            var query = (from allData in (from region in _administrationBReposity.GetAll()
                                          where region.parentcd.Equals("2102")
                                          join site in _stnInfoBRepository.GetAll() on region.Id equals site.addvcd into temp
                                          from cr in temp.DefaultIfEmpty()
                                          join rain in _wmtRainFiveMinutesRepository.GetAll().Where(c => c.tm >= input.fromTime && c.tm <= input.toTime) on cr.areaCode equals rain.stcd
                                           into relation
                                          from data in relation.DefaultIfEmpty()
                                          select new
                                          {
                                              addvName = region.addvname,
                                              areaCode = cr.areaCode,
                                              areaName = cr.areaName,
                                              paraValue = data.drp
                                          })
                         group allData by new { allData.addvName, allData.areaName, allData.areaCode } into lst
                         select new CWmtRainTotalBySiteDto
                         {
                             addvName = lst.Key.addvName,
                             areaName = lst.Key.areaName,
                             areaCode = lst.Key.areaCode,
                             total = lst.Where(c => c.paraValue != null).Sum(c => c.paraValue)
                         }).OrderByDescending(t => t.total);
            var result = query.ToList();
            var totla = query.Count();
            return new CDataResults<CWmtRainTotalBySiteDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = totla
            };
        }
        public CDataResults<CWmtRainTotalByHoursDto> GetWmtRainTotalByHours(CWmtRainInput input)
        {
            var now = DateTime.Now;//new DateTime(2017, 8, 30);//
            var beforeYesterday = now.AddDays(-2);
            var oneHourAgo = now.AddHours(-1);
            var threeHoursAgo = now.AddHours(-3);
            var sixHoursAgo = now.AddHours(-6);
            var twelveHoursAgo = now.AddHours(-12);
            var twentyFourHoursAgo = now.AddHours(-24);
            var addvcdArray = (input.addvcdArray == null) ? new string[] { } : input.addvcdArray.ToArray();
            var addvcdArrayLength = addvcdArray.Length;
            var query = from allData in (from site in _stnInfoBRepository.GetAll().Where(s => addvcdArrayLength == 0 || addvcdArray.Any(a => a == s.addvcd.Substring(0, a.Length)))//.Contains(s.addvcd)
                                         join rain in _wmtRainFiveMinutesRepository.GetAll().Where(r => r.tm != null && r.tm >= beforeYesterday && r.tm < now) on site.areaCode equals rain.stcd into temp
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
                                             collecttime = cr.tm,
                                             paravalue = cr.drp
                                         })
                        group allData by new { allData.areaName, allData.areaCode, allData.addvname } into lst
                        select new CWmtRainTotalByHoursDto
                        {
                            areaName = lst.Key.areaName,
                            areaCode = lst.Key.areaCode,
                            addvname = lst.Key.addvname,
                            total_1 = lst.Where(t => t.collecttime > oneHourAgo && t.paravalue != null).Sum(c => c.paravalue),
                            total_3 = lst.Where(t => t.collecttime > threeHoursAgo && t.paravalue != null).Sum(c => c.paravalue),
                            total_6 = lst.Where(t => t.collecttime > sixHoursAgo && t.paravalue != null).Sum(c => c.paravalue),
                            total_12 = lst.Where(t => t.collecttime > twelveHoursAgo && t.paravalue != null).Sum(c => c.paravalue),
                            total_24 = lst.Where(t => t.collecttime > twentyFourHoursAgo && t.paravalue != null).Sum(c => c.paravalue),
                            total_48 = lst.Where(t => t.paravalue != null).Sum(c => c.paravalue),
                        };
            //var query = from allData in (from site in _stnInfoBRepository.GetAll().Where(s => addvcdArrayLength == 0 || addvcdArray.Any(a => a == s.addvcd.Substring(0, a.Length)))//.Contains(s.addvcd)
            //                             join rain in _wmtRainRepository.GetAll().Where(r => r.collecttime != null && r.collecttime >= beforeYesterday && r.collecttime < now) on site.areaCode equals rain.stcd into temp
            //                             from cr in temp.DefaultIfEmpty()
            //                             join admin in _administrationBReposity.GetAll() on site.addvcd equals admin.Id into relation
            //                             from data in relation.DefaultIfEmpty()
            //                                 //where cr.collecttime != null && cr.collecttime >= beforeYesterday && cr.collecttime < now &&
            //                                 // (addvcdArrayLength == 0 || addvcdArray.Contains(site.addvcd))//((addvcdArray == null)? ((input.addvcd ==null) ? true : site.addvcd.StartsWith(input.addvcd)) : addvcdArray.Contains(site.addvcd))
            //                             select new
            //                             {
            //                                 areaCode = site.areaCode,
            //                                 areaName = site.areaName,
            //                                 addvcd = site.addvcd,
            //                                 addvname = data.addvname,
            //                                 collecttime = cr.collecttime,
            //                                 paravalue = cr.paravalue
            //                             })
            //            group allData by new { allData.areaName, allData.areaCode, allData.addvname } into lst
            //            select new CWmtRainTotalByHoursDto
            //            {
            //                areaName = lst.Key.areaName,
            //                areaCode = lst.Key.areaCode,
            //                addvname = lst.Key.addvname,
            //                total_1 = (lst.Where(t => t.collecttime > oneHourAgo && t.paravalue != null).Count() > 1) ? lst.Where(t => t.collecttime > oneHourAgo).Max(c => c.paravalue) - lst.Where(t => t.collecttime > oneHourAgo).Min(c => c.paravalue) : lst.Where(t => t.collecttime > oneHourAgo).Max(c => c.paravalue),
            //                total_3 = (lst.Where(t => t.collecttime > threeHoursAgo && t.paravalue != null).Count() > 1) ? lst.Where(t => t.collecttime > threeHoursAgo).Max(c => c.paravalue) - lst.Where(t => t.collecttime > threeHoursAgo).Min(c => c.paravalue) : lst.Where(t => t.collecttime > threeHoursAgo).Max(c => c.paravalue),
            //                total_6 = (lst.Where(t => t.collecttime > sixHoursAgo && t.paravalue != null).Count() > 1) ? lst.Where(t => t.collecttime > sixHoursAgo).Max(c => c.paravalue) - lst.Where(t => t.collecttime > sixHoursAgo).Min(c => c.paravalue) : lst.Where(t => t.collecttime > sixHoursAgo).Max(c => c.paravalue),
            //                total_12 = (lst.Where(t => t.collecttime > twelveHoursAgo && t.paravalue != null).Count() > 1) ? lst.Where(t => t.collecttime > twelveHoursAgo).Max(c => c.paravalue) - lst.Where(t => t.collecttime > twelveHoursAgo).Min(c => c.paravalue) : lst.Where(t => t.collecttime > twelveHoursAgo).Max(c => c.paravalue),
            //                total_24 = (lst.Where(t => t.collecttime > twentyFourHoursAgo && t.paravalue != null).Count() > 1) ? lst.Where(t => t.collecttime > twentyFourHoursAgo).Max(c => c.paravalue) - lst.Where(t => t.collecttime > twentyFourHoursAgo).Min(c => c.paravalue) : lst.Where(t => t.collecttime > twentyFourHoursAgo).Max(c => c.paravalue),
            //                total_48 = (lst.Where(t => t.paravalue != null).Count() > 1) ? lst.Max(c => c.paravalue) - lst.Min(c => c.paravalue) : lst.Max(c => c.paravalue),
            //                //total_1 = lst.Where(t => t.collecttime > oneHourAgo).Max(c => c.paravalue),// - lst.Where(t => t.collecttime > oneHourAgo).Min(c => c.paravalue),
            //                //total_3 = lst.Where(t => t.collecttime > threeHoursAgo).Max(c => c.paravalue),// - lst.Where(t => t.collecttime > threeHoursAgo).Min(c => c.paravalue),//lst.Where(t => t.collecttime > threeHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
            //                //total_6 = lst.Where(t => t.collecttime > sixHoursAgo).Max(c => c.paravalue),// - lst.Where(t => t.collecttime > sixHoursAgo).Min(c => c.paravalue),//lst.Where(t => t.collecttime > sixHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
            //                //total_12 = lst.Where(t => t.collecttime > twelveHoursAgo).Max(c => c.paravalue),// - lst.Where(t => t.collecttime > twelveHoursAgo).Min(c => c.paravalue),//lst.Where(t => t.collecttime > twelveHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
            //                //total_24 = lst.Where(t => t.collecttime > twentyFourHoursAgo).Max(c => c.paravalue),// - lst.Where(t => t.collecttime > twentyFourHoursAgo).Min(c => c.paravalue),//lst.Where(t => t.collecttime > twentyFourHoursAgo).Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue),
            //                //total_48 = lst.Max(c => c.paravalue),// - lst.Min(c => c.paravalue),//lst.Sum(c => c.paravalue) == null ? 0 : lst.Sum(c => c.paravalue)
            //                //total_1 = lst.Min(c => c.paravalue),
            //                //total_3 = lst.Max(c => c.paravalue)
            //            };
            var totla = query.Count();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                if (input.sortHour.HasValue && input.sortHour.Value == 1)
                {
                    query = query.OrderByDescending(c => c.total_1).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else if (input.sortHour.HasValue && input.sortHour.Value == 3)
                {
                    query = query.OrderByDescending(c => c.total_3).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else if (input.sortHour.HasValue && input.sortHour.Value == 6)
                {
                    query = query.OrderByDescending(c => c.total_6).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else if (input.sortHour.HasValue && input.sortHour.Value == 12)
                {
                    query = query.OrderByDescending(c => c.total_12).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else if (input.sortHour.HasValue && input.sortHour.Value == 24)
                {
                    query = query.OrderByDescending(c => c.total_24).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else if (input.sortHour.HasValue && input.sortHour.Value == 48)
                {
                    query = query.OrderByDescending(c => c.total_48).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
                else
                {
                    query = query.OrderByDescending(c => c.total_1).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
                }
            }
            else
            {
                query = query.OrderByDescending(c => c.total_1);
            }
            var result = query.ToList();
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
