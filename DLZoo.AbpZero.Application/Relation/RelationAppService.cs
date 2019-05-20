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
using MyTempProject.Relation.Dto;
using MyTempProject.Base;
using MyTempProject.Base.Dto;

namespace MyTempProject.Relation
{
    public class RelationAppService : CBaseAppService, IRelationAppService
    {
        private readonly IRepository<Entities.CRelation, int> _relationRepository;
        private readonly IRepository<Entities.CStnInfoB, int> _cstnInfoRepository;
        public RelationAppService(
            IRepository<Entities.CRelation, int> relationRepository,
            IRepository<Entities.CStnInfoB, int> stnInfoBRepository,
            IRepository<Entities.CCustomer, int> CustomerRepository,
            IRepository<Entities.CIp, int> IpRepository,
            IRepository<Entities.CVisitRecord, int> VisitRecordRepository) : base(CustomerRepository, IpRepository, VisitRecordRepository)
        {
            this._relationRepository = relationRepository;
            this._cstnInfoRepository = stnInfoBRepository;


        }
        public CDataResults<CRelationListDto> AddRelations(CRelationInput input)
        {
            if (!checkIPandCustomer(input.customer_id))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CRelationListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }

            if (input.siteIdArr.Count > 0)
            {
                _relationRepository.Delete(d => d.customer_id == input.customer_id);

                foreach (var item in input.siteIdArr)
                {
                    Entities.CRelation entity = new Entities.CRelation()
                    {
                        customer_id = input.customer_id,
                        site_id = item
                    };
                    _relationRepository.Insert(entity);
                };
            }
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CRelationListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = null
            };

        }

        public CDataResults<CRelationListDto> GetRelations(CRelationInput input)
        {
            if (!checkIPandCustomer(input.customer_id))
            {
                AddVisitRecord(input.customerId, Entities.VisitRecordFlag.Black);
                return new CDataResults<CRelationListDto>()
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed.",
                    Data = null
                };
            }
            //var siteList = this._cstnInfoRepository.GetAll().OrderBy(order => order.areaName);
            var relationList = this._relationRepository.GetAll().Where(p => p.customer_id == (input.customer_id));
            var result = relationList.ToList().MapTo<List<CRelationListDto>>();
            AddVisitRecord(input.customerId, Entities.VisitRecordFlag.White);
            return new CDataResults<CRelationListDto>()
            {
                IsSuccess = true,
                ErrorMessage = null,
                Data = result
            };
        }
    }
}
