using Umbraco.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using umbraco.BusinessLogic.Actions;
using umbraco;
using Umbraco.Core.Services;

namespace UmbracoExtensionExample.Controllers {
    [Tree("person", "personTree", "Person")]
    [PluginController("Person")]
    public class PersonTreeController : TreeController {
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings) {

            var menu = new MenuItemCollection();

            //submenus of root
            if (id == Constants.System.Root.ToInvariantString()) {
                menu.Items.Add<CreateChildEntity, ActionNew>(ApplicationContext.Services.TextService.Localize(ActionNew.Instance.Alias));
                //menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
                menu.Items.Add<RefreshNode, ActionRefresh>(ApplicationContext.Services.TextService.Localize(ActionRefresh.Instance.Alias));
            }
            //submenus of roots children
            else {
                menu.Items.Add<CreateChildEntity, ActionNew>(ApplicationContext.Services.TextService.Localize(ActionNew.Instance.Alias));
                menu.Items.Add<ActionDelete>(ApplicationContext.Services.TextService.Localize(ActionDelete.Instance.Alias));
            }

            return menu;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings) {
            var ctrl = new PersonApiController();
            var nodes = new TreeNodeCollection();

            if (id == Constants.System.Root.ToInvariantString())
                foreach(var person in ctrl.GetAll()) {
                    var node = CreateTreeNode(person.Id.ToString(), "-1", queryStrings, person.ToString(), "icon-document", false);

                    nodes.Add(node);
                }

            return nodes;
        }
    }
}