using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Abp.EntityFramework;
using MyTempProject.Entities.Temp;
using MyTempProject.Entities;
//using MyTempProject.Authorization.Roles;
//using MyTempProject.Authorization.Users;
//using MyTempProject.MultiTenancy;
//using MyTempProject.Storage;

namespace MyTempProject.EntityFramework
{
    public class AbpZeroTemplateDbContext : AbpDbContext//AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */

        //public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }
        public virtual IDbSet<CTableClass> CTableObjects { get; set; }
        public virtual IDbSet<CStnInfoB> CStnInfoBObjects { get; set; }
        public virtual IDbSet<CStnParaR> CStnParaRObjects { get; set; }
        public virtual IDbSet<CAdministrationB> CAdministrationBObjects { get; set; }
        public virtual IDbSet<CWmtRain> CWmtRainObjects { get; set; }
        public virtual IDbSet<CWmtRiver> CWmtRiverObjects { get; set; }
        public virtual IDbSet<CWmtRsvr> CWmtRsvrObjects { get; set; }
        public virtual IDbSet<CWmtSoilMoisture> CWmtSoilMoistrueObjects { get; set; }
        public virtual IDbSet<CCustomer> CCustomerObjects { get; set; }
        public virtual IDbSet<CIp> CIPObjects { get; set; }
        public virtual IDbSet<CVisitRecord> CVisitRecordObjects { get; set; }
        public virtual IDbSet<CRelation> CRelationObjects { get; set; }
        public virtual IDbSet<CUser> CUserObjects { get; set; }
        public virtual IDbSet<CAdministrationB> CAdministrationBs { get; set; }


        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public AbpZeroTemplateDbContext()
            : base("Default")
        {

        }

        /* This constructor is used by ABP to pass connection string defined in AbpZeroTemplateDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of AbpZeroTemplateDbContext since ABP automatically handles it.
         */
        public AbpZeroTemplateDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public AbpZeroTemplateDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
