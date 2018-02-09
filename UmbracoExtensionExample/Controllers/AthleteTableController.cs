using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using umbraco.NodeFactory;
using Umbraco.Web.Mvc;

namespace UmbracoExtensionExample.Controllers
{
    public class AthleteTableController : SurfaceController {
        // GET: Home
        public ActionResult Index()
        {
            Node currentNode = Node.GetCurrent();

            var athletes = new ImportApiController().GetAll();
            ViewBag.Athletes = athletes;

            //get properties for table
            Type myType = athletes.FirstOrDefault().GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            ViewBag.Props = props;

            return PartialView("AthleteTable");
        }
    }
}