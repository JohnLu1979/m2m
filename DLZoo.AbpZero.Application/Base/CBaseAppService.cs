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

        protected readonly IRepository<Entities.CCustomer, int> _customerRepository;
        protected readonly IRepository<Entities.CIp, int> _ipRepository;
        protected readonly IRepository<Entities.CVisitRecord, int> _visitRecordRepository;
        public CBaseAppService(
            IRepository<Entities.CCustomer, int> customerRepository,
            IRepository<Entities.CIp, int> ipRepository,
            IRepository<Entities.CVisitRecord, int> visitRecordRepository)
        {
            this._customerRepository = customerRepository;
            this._ipRepository = ipRepository;
            this._visitRecordRepository = visitRecordRepository;
        }
        protected void AddVisitRecord(int customerId, VisitRecordFlag flag) {
            CVisitRecord visitRecord = new CVisitRecord() {
                customerId = customerId,
                visit_dateTime = DateTime.Now,
                ip = RequestIP,
                flag = (int)flag
            };
            this._visitRecordRepository.InsertAndGetId(visitRecord);
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
            var ip = this._ipRepository.FirstOrDefault(c => c.IP == RequestIP);
            return ip != null;
        }
        protected bool CheckCustomer(int customerid)
        {
            var customer = this._customerRepository.FirstOrDefault(c => c.Id == customerid);
            return customer != null;
        }
    }
}
