using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace UmbracoExtensionExample.App_Plugins.Import.Models {
    [TableName("Athlete")]
    public class Athlete {
        public Athlete() {

        }

        [PrimaryKeyColumn(AutoIncrement = false)]
        public int Id { get; set; }

        //public int Number { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public override string ToString() {
            return Id+ " " + Name + " " + Nickname;
        }
    }
}