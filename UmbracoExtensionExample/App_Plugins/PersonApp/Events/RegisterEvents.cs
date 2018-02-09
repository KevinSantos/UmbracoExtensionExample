using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using UmbracoExtensionExample.App_Plugins.PersonApp.Objects;

namespace UmbracoExtensionExample.App_Plugins.PersonApp.Events {
    public class RegisterEvents : ApplicationEventHandler{

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            var db = applicationContext.DatabaseContext.Database;

            if (!db.TableExist("People"))
                db.CreateTable<Person>(false);
        }
    }
}