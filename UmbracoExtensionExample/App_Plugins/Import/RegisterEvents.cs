using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using UmbracoExtensionExample.App_Plugins.Import.Models;

namespace UmbracoExtensionExample.App_Plugins.Import {
    public class RegisterEvents : ApplicationEventHandler {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            var db = applicationContext.DatabaseContext.Database;

            if (!db.TableExist(""))
                db.CreateTable<Athlete>(false);
        }
    }
}