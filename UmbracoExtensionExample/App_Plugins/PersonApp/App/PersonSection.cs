using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.businesslogic;
using umbraco.interfaces;

namespace UmbracoExtensionExample.App_Plugins.PersonApp.App {
    [Application("person", "Person", "icon-people", 24)]
    public class PersonSection : IApplication{
    }
}