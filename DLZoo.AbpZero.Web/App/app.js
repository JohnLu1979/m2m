﻿/* 'app' MODULE DEFINITION */
var appModule = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    'ui.grid',
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ngSanitize",
    'angularFileUpload',
    'daterangepicker',
    'angularMoment',
    'frapontillo.bootstrap-switch',
    'abp'
]);

/* LAZY LOAD CONFIG */

/* This application does not define any lazy-load yet but you can use $ocLazyLoad to define and lazy-load js/css files.
 * This code configures $ocLazyLoad plug-in for this application.
 * See it's documents for more information: https://github.com/ocombe/ocLazyLoad
 */
appModule.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        cssFilesInsertBefore: 'ng_load_plugins_before', // load the css files before a LINK element with this ID.
        debug: false,
        events: true,
        modules: []
    });
}]);

/* THEME SETTINGS */
App.setAssetsPath(abp.appPath + 'metronic/assets/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        layoutImgPath: App.getAssetsPath() + 'admin/layout4/img/',
        layoutCssPath: App.getAssetsPath() + 'admin/layout4/css/',
        assetsPath: abp.appPath + 'metronic/assets',
        globalPath: abp.appPath + 'metronic/assets/global',
        layoutPath: abp.appPath + 'metronic/assets/layouts/layout4'
    };

    $rootScope.settings = settings;

    return settings;
}]);

/* ROUTE DEFINITIONS */

appModule.config([
    '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {

        // Default route (overrided below if user has permission)
        $urlRouterProvider.otherwise("/welcome");

        //Welcome page
        $stateProvider.state('welcome', {
            url: '/welcome',
            templateUrl: '~/App/common/views/welcome/index.cshtml'
        });

        //COMMON routes

        if (abp.auth.hasPermission('Pages.Administration.Roles')) {
            $stateProvider.state('roles', {
                url: '/roles',
                templateUrl: '~/App/common/views/roles/index.cshtml',
                menu: 'Administration.Roles'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Users')) {
            $stateProvider.state('users', {
                url: '/users?filterText',
                templateUrl: '~/App/common/views/users/index.cshtml',
                menu: 'Administration.Users'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalInfo')) {
            $stateProvider.state('AnimalInfo', {
                url: '/AnimalInfo',
                templateUrl: '~/App/common/views/AnimalInfo/Index.cshtml',
                menu: 'Administration.AnimalInfo'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalGenus')) {
            $stateProvider.state('animalgenus', {
                url: '/animalgenus',
                templateUrl: '~/App/common/views/AnimalGenus/Index.cshtml',
                menu: 'Administration.AnimalGenus'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.RepairCompany')) {
            $stateProvider.state('repaircompany', {
                url: '/repaircompany',
                templateUrl: '~/App/common/views/RepairCompany/Index.cshtml',
                menu: 'Administration.RepairCompany'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.PropertyManage')) {
            $stateProvider.state('propertyManage', {
                url: '/propertyManage',
                templateUrl: '~/App/common/views/PropertyManage/Index.cshtml',
                menu: 'Administration.PropertyManage'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.GenusInformation')) {
            $stateProvider.state('genusinformation', {
                url: '/genusinformation',
                templateUrl: '~/App/common/views/GenusInformation/Index.cshtml',
                menu: 'Administration.GenusInformation'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalBehav')) {
            $stateProvider.state('animalbehav', {
                url: '/animalbehav',
                templateUrl: '~/App/common/views/AnimalBehav/Index.cshtml',
                menu: 'Administration.AnimalBehav'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.GenuExamine')) {
            $stateProvider.state('GenuExamine', {
                url: '/GenuExamine',
                templateUrl: '~/App/common/views/GenuExamine/Index.cshtml',
                menu: 'Administration.GenuExamine'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalChange')) {
            $stateProvider.state('animalchange', {
                url: '/animalchange',
                templateUrl: '~/App/common/views/AnimalChange/Index.cshtml',
                menu: 'Administration.AnimalChange'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalChgeApr')) {
            $stateProvider.state('animalChgeApr', {
                url: '/animalChgeApr',
                templateUrl: '~/App/common/views/AnimalChgeApr/Index.cshtml',
                menu: 'Administration.AnimalChgeApr'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalFeed')) {
            $stateProvider.state('animalfeed', {
                url: '/animalfeed',
                templateUrl: '~/App/common/views/AnimalFeed/Index.cshtml',
                menu: 'Administration.AnimalFeed'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.AnimalInfo')) {
            $stateProvider.state('AnimalInfo', {
                url: '/AnimalInfo',
                templateUrl: '~/App/common/views/AnimalInfo/Index.cshtml',
                menu: 'Administration.AnimalInfo'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.RepairManage')) {
            $stateProvider.state('RepairManage', {
                url: '/RepairManage',
                templateUrl: '~/App/common/views/RepairManage/Index.cshtml',
                menu: 'Administration.RepairManage'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.RepairAppro')) {
            $stateProvider.state('RepairAppro', {
                url: '/RepairAppro',
                templateUrl: '~/App/common/views/RepairAppro/Index.cshtml',
                menu: 'Administration.RepairAppro'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.RepairHandle')) {
            $stateProvider.state('RepairHandle', {
                url: '/RepairHandle',
                templateUrl: '~/App/common/views/RepairHandle/Index.cshtml',
                menu: 'Administration.RepairHandle'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.Languages')) {
            $stateProvider.state('languages', {
                url: '/languages',
                templateUrl: '~/App/common/views/languages/index.cshtml',
                menu: 'Administration.Languages'
            });

            if (abp.auth.hasPermission('Pages.Administration.Languages.ChangeTexts')) {
                $stateProvider.state('languageTexts', {
                    url: '/languages/texts/:languageName?sourceName&baseLanguageName&targetValueFilter&filterText',
                    templateUrl: '~/App/common/views/languages/texts.cshtml',
                    menu: 'Administration.Languages'
                });
            }
        }

        if (abp.auth.hasPermission('Pages.Administration.AuditLogs')) {
            $stateProvider.state('auditLogs', {
                url: '/auditLogs',
                templateUrl: '~/App/common/views/auditLogs/index.cshtml',
                menu: 'Administration.AuditLogs'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.FoodClasss')) {
            $stateProvider.state('foodClasss', {
                url: '/foodClasss',
                templateUrl: '~/App/common/views/foodClasss/index.cshtml',
                menu: 'Administration.FoodClasss'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.FoodManager')) {
            $stateProvider.state('foodManager', {
                url: '/foodManager',
                templateUrl: '~/App/common/views/foodManager/index.cshtml',
                menu: 'Administration.FoodManager'
            });
        }
        if (abp.auth.hasPermission('Pages.Administration.OrganizationUnits')) {
            $stateProvider.state('organizationUnits', {
                url: '/organizationUnits',
                templateUrl: '~/App/common/views/organizationUnits/index.cshtml',
                menu: 'Administration.OrganizationUnits'
            });
        }

        $stateProvider.state('notifications', {
            url: '/notifications',
            templateUrl: '~/App/common/views/notifications/index.cshtml'
        });

        //HOST routes

        $stateProvider.state('host', {
            'abstract': true,
            url: '/host',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenants')) {
            $urlRouterProvider.otherwise("/host/tenants"); //Entrance page for the host
            $stateProvider.state('host.tenants', {
                url: '/tenants',
                templateUrl: '~/App/host/views/tenants/index.cshtml',
                menu: 'Tenants'
            });
        }

        if (abp.auth.hasPermission('Pages.Editions')) {
            $stateProvider.state('host.editions', {
                url: '/editions',
                templateUrl: '~/App/host/views/editions/index.cshtml',
                menu: 'Editions'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Host.Settings')) {
            $stateProvider.state('host.settings', {
                url: '/settings',
                templateUrl: '~/App/host/views/settings/index.cshtml',
                menu: 'Administration.Settings.Host'
            });
        }

        //TENANT routes

        $stateProvider.state('tenant', {
            'abstract': true,
            url: '/tenant',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenant.Dashboard')) {
            $urlRouterProvider.otherwise("/tenant/dashboard"); //Entrance page for a tenant
            $stateProvider.state('tenant.dashboard', {
                url: '/dashboard',
                templateUrl: '~/App/tenant/views/dashboard/index.cshtml',
                menu: 'Dashboard.Tenant'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Tenant.Settings')) {
            $stateProvider.state('tenant.settings', {
                url: '/settings',
                templateUrl: '~/App/tenant/views/settings/index.cshtml',
                menu: 'Administration.Settings.Tenant'
            });
        }
    }
]);

appModule.run(["$rootScope", "settings", "$state", function ($rootScope, settings, $state) {
    $rootScope.$state = $state;
    $rootScope.$settings = settings; 

    $rootScope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
}]);