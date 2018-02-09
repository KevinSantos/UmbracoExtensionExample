using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoExtensionExample.App_Plugins.Import.Models {
    public class EditAthlete {
        public EditAthlete() {

        }
        public int oldId { get; set; }
        public Athlete athlete { get; set; }
    }
}