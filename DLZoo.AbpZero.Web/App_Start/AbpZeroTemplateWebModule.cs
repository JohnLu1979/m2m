using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Hangfire;
using Microsoft.Owin.Security;
using MyTempProject.Web.App.Startup;
using MyTempProject.Web.Areas.Mpa.Startup;
using MyTempProject.Web.Bundling;
using MyTempProject.Web.Navigation;
using MyTempProject.Web.Routing;
using MyTempProject.WebApi;

namespace MyTempProject.Web
{
    /// <summary>
    /// Web module of the application.
    /// This is the most top and entrance module that dependens on others.
    /// </summary>
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpZeroTemplateDataModule),
        typeof(AbpZeroTemplateApplicationModule),
        typeof(AbpZeroTemplateWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule))] //AbpHangfireModule dependency can be removed if not using Hangfire
    public class AbpZeroTemplateWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database as language management
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.MultiTenancy.IsEnabled = false;
            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
            Configuration.Navigation.Providers.Add<MpaNavigationProvider>();

            //Configure to use Hangfire as background job manager. Remove these lines to use default background job manager, instead of Hangfire.
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
                );

            //Areas
            AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
            BundleTable.Bundles.IgnoreList.Clear();
            CommonBundleConfig.RegisterBundles(BundleTable.Bundles);
            AppBundleConfig.RegisterBundles(BundleTable.Bundles);
            FrontEndBundleConfig.RegisterBundles(BundleTable.Bundles);
            MpaBundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
        }
    }
}
