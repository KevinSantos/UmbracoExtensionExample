using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web.Mvc;
using UmbracoExtensionExample.App_Plugins.PersonApp.Objects;

namespace UmbracoExtensionExample.Controllers {
    [PluginController("Person")]
    public class PersonApiController : UmbracoAuthorizedController {
        public IEnumerable<Person> GetAll() {
            var query = new Sql().Select("*").From("Person");
            return DatabaseContext.Database.Fetch<Person>(query);
        }

        public Person GetById(int id) {
            var query = new Sql().Select("*").From("Person").Where<Person>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Person>(query).FirstOrDefault();
        }

        public Person PostSave(Person person) {
            if (person.Id > 0)
                DatabaseContext.Database.Update(person);
            else
                DatabaseContext.Database.Save(person);

            return person;
        }

        public int DeleteById(int id) {
            return DatabaseContext.Database.Delete<Person>(id);
        }
    }
}