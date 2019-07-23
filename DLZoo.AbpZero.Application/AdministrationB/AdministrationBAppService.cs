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
using MyTempProject.AdministrationB.Dto;

namespace MyTempProject.AdministrationB
{
    public class AdministrationBAppService : CBaseAppService, IAdministrationBAppService
    {
        private readonly IRepository<Entities.CAdministrationB, string> _administrationBRepository;
        public AdministrationBAppService(
            IRepository<Entities.CAdministrationB, string> administrationBRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository
            ) : base(CustomerRepository, IpRepository, VisitRecordRepository)
        {
            this._administrationBRepository = administrationBRepository;
        }

        public CDataResults<CAdministrationBListDto> GetAdministrationB(CAdministrationBInput input)
        {
            //Check Ip & customer
            //if (!checkIPandCustomer(input.customerId))
            //{
                //AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                //return new CDataResults<CAdministrationBListDto>()
                //{
                //    IsSuccess = false,
                //    ErrorMessage = "Validation failed.",
                //    Data = null,
                //    Total = 0
                //};
            //}

            //Extract data from DB
            var query = this._administrationBRepository.GetAll();
            if (!string.IsNullOrEmpty(input.addvcd)) {
                query = query.Where(r => r.Id.StartsWith(input.addvcd));
            }
            if (input.pageNumber.HasValue && input.pageNumber.Value > 0 && input.pageSize.HasValue)
            {
                query = query.OrderBy(r => r.Id).Take(input.pageSize.Value * input.pageNumber.Value).Skip(input.pageSize.Value * (input.pageNumber.Value - 1));
            }

            var result = query.ToList().MapTo<List<CAdministrationBListDto>>();
            //Add visit record
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CAdministrationBListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result,
                Total = query.Count()
            };
        }
    }
}
