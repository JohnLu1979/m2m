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
using MyTempProject.VisitRecord.Dto;
using MyTempProject.Entities;

namespace MyTempProject.VisitRecord
{
    public class VisitRecordAppService : CBaseAppService, IVisitRecordAppService
    {
        private ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.CCustomer, int> customerRepository;
        public VisitRecordAppService(ISqlExecuter sqlExecuter,
            IRepository<Entities.CVisitRecord, int> visitRecordRepository,
            IRepository<Entities.CCustomer, int> customerRepository,
            IRepository<Entities.CIp, int> ipRepository
            ) :base(customerRepository, ipRepository, visitRecordRepository)
        {
            
            this._sqlExecuter = sqlExecuter;
        }

        public CDataResults<CVisitRecordListDto> GetVisitRecord(CVisitRecordInput input)
        {
            //Check Ip & visitRecord
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CVisitRecordListDto>() {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            var query = this._visitRecordRepository.GetAll();
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CVisitRecordListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CVisitRecordListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = query.Count()
            }; 
        }

        public CDataResults<CVisitRecordDetailListDto> GetVisitWhiteRecord(CVisitRecordInput input)
        {
            //Check Ip & visitRecord
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CVisitRecordDetailListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            //var query = this._visitRecordRepository.GetAll();
            var query = from r in _visitRecordRepository.GetAll()
                        join s in this._customerRepository.GetAll() on r.customerId equals s.Id into rs
                        from rst in rs.DefaultIfEmpty()
                        orderby r.visit_dateTime descending
                        where r.flag == (int)VisitRecordFlag.White
                        select new CVisitRecordDetailListDto
                        {
                            customerId = r.customerId,
                            customerName = rst.customerName,
                            visit_dateTime = r.visit_dateTime,
                            ip = r.ip,
                            flag = r.flag
                        };
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CVisitRecordDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }

        public CDataResults<CVisitRecordDetailListDto> GetVisitBlackRecord(CVisitRecordInput input)
        {
            //Check Ip & visitRecord
            if (!checkIPandCustomer(input.customerId))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CVisitRecordDetailListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            //Extract data from DB
            //var query = this._visitRecordRepository.GetAll();
            var query = from r in _visitRecordRepository.GetAll()
                        join s in this._customerRepository.GetAll() on r.customerId equals s.Id into rs
                        from rst in rs.DefaultIfEmpty()
                        orderby r.visit_dateTime descending
                        where r.flag == (int)VisitRecordFlag.Black
                        select new CVisitRecordDetailListDto
                        {
                            customerId = r.customerId,
                            customerName = rst.customerName,
                            visit_dateTime = r.visit_dateTime,
                            ip = r.ip,
                            flag = r.flag
                        };
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CVisitRecordDetailListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
    }
}
