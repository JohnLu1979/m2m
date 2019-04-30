using Abp.Domain.Repositories;
using MyTempProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyTempProject.Base
{
    public class CBaseAppService
    {

        private readonly IRepository<Entities.CCustomer, int> _wmtCustomerRepository;
        private readonly IRepository<Entities.CIp, int> _wmtIpRepository;
        private readonly IRepository<Entities.CVisitRecord, int> _wmtVisitRecordRepository;
        public CBaseAppService(
            IRepository<Entities.CCustomer, int> wmtCustomerRepository,
            IRepository<Entities.CIp, int> wmtIpRepository,
            IRepository<Entities.CVisitRecord, int> wmtVisitRecordRepository)
        {
            this._wmtCustomerRepository = wmtCustomerRepository;
            this._wmtIpRepository = wmtIpRepository;
            this._wmtVisitRecordRepository = wmtVisitRecordRepository;
        }
        protected void AddVisitRecord(int customerId, VisitRecordFlag flag) {
            CVisitRecord visitRecord = new CVisitRecord() {
                customerId = customerId,
                visit_dateTime = DateTime.Now,
                ip = RequestIP,
                flag = (int)flag
            };
            this._wmtVisitRecordRepository.InsertAndGetId(visitRecord);
        }
        protected string RequestIP{
            get {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                           HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                // HttpContext.Current.Request.UserHostAddress
            }
        }

        protected bool checkIPandCustomer(int customerId) {
            return CheckIP() && CheckCustomer(customerId);
        }

        protected bool CheckIP() {
            var ip = this._wmtIpRepository.FirstOrDefault(c => c.IP == RequestIP);
            return ip != null;
        }
        protected bool CheckCustomer(int customerid)
        {
            var customer = this._wmtCustomerRepository.FirstOrDefault(c => c.Id == customerid);
            return customer != null;
        }
    }
}
