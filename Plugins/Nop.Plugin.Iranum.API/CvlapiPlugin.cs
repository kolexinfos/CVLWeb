using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.Iranum.API
{
    class CvlapiPlugin: BasePlugin, IPlugin, IMiscPlugin
    {

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        public CvlapiPlugin(ISettingService settingService,ILocalizationService localizationService, ILogger logger)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _logger = logger;
        }


        public void GetConfigurationRoute( out string actionName, out string controllerName,out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Cvlapi";
            routeValues = new RouteValueDictionary {
            { "Namespaces", "Nop.Plugin.Iranum.API.Controllers" },
            { "area", null } };
        }

        public override void Install()
        {
            var settings = new CvlapiSettings()
            {
                config = ""
            };

            base.Install();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            throw new NotImplementedException();
        }

        public override void Uninstall()
       {
            

            base.Uninstall();
       }

    
    }
}
