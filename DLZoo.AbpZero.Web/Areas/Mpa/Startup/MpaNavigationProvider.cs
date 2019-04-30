using Abp.Application.Navigation;
using Abp.Localization;
//using MyTempProject.Authorization;
using MyTempProject.Web.Navigation;

namespace MyTempProject.Web.Areas.Mpa.Startup
{
    public class MpaNavigationProvider : NavigationProvider
    {
        public const string MenuName = "Mpa";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "Mpa/Tenants",
                    icon: "icon-globe"//,
                                      //requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}