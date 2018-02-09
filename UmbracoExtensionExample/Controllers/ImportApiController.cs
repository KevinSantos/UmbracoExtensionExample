using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using UmbracoExtensionExample.App_Plugins.Import.Models;

namespace UmbracoExtensionExample.Controllers
{
    [PluginController("Import")]
    public class ImportApiController : UmbracoAuthorizedJsonController {
        public IEnumerable<Athlete> GetAll() {
            var query = new Sql().Select("*").From("Athlete");
            return DatabaseContext.Database.Fetch<Athlete>(query);
        }

        public Athlete GetById(int id) {
            var query = new Sql().Select("*").From("Athlete").Where<Athlete>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Athlete>(query).FirstOrDefault();
        }

        [HttpPost()]
        public int Edit(EditAthlete edit) {


            //Debug.WriteLine("\n\n\n\t\t" + oldId + "\t" + athlete.Name + "\n\n\n");

            //if Id changed, remove old one
            if (edit.oldId != edit.athlete.Id)
                DeleteById(edit.oldId);

            var list = new List<Athlete>();
            list.Add(edit.athlete);
            return Save(list);
        }

        public int Save(List<Athlete> athletesList) {
            //DatabaseContext.Database.Save(new Athlete {Name="Joel", Nickname="Deadmau5" });
            //DatabaseContext.Database.Save(new Athlete { Id = 1, Name = "Joel", Nickname = "Deadmau5" });
            //DatabaseContext.Database.Save("Athlete", "Id", new Athlete { Id = 1, Name = "Joel", Nickname = "Deadmau5" });

            var count = 0;

            foreach (Athlete athlete in athletesList) {
                
                //TODO: sanitize user inputs
                if (GetById(athlete.Id) != null)
                    count += DatabaseContext.Database.Update(athlete);
                else {
                    var query = String.Format("SET IDENTITY_INSERT Athlete ON;INSERT INTO Athlete (Id, Name,Nickname) VALUES ({0}, '{1}', '{2}');SET IDENTITY_INSERT Athlete OFF;", athlete.Id, athlete.Name, athlete.Nickname);
                    count += DatabaseContext.Database.Execute(query);
                }
            }

            return count;
        }

        
        public int DeleteById(int id) {
            return DatabaseContext.Database.Delete<Athlete>(id);
        }
    }
}