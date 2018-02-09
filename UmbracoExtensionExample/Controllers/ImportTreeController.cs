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

namespace UmbracoExtensionExample.Controllers
{
    [Tree("import", "importTree", "Content for Approval")]
    [PluginController("Import")]
    public class ImportTreeController : TreeController {
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings) {

            var menu = new MenuItemCollection();



            //submenus of root
            if (id == Constants.System.Root.ToInvariantString()) {
                //menu.Items.Add<CreateChildEntity, ActionNew>(ApplicationContext.Services.TextService.Localize(ActionNew.Instance.Alias));
                //menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions1", ActionRefresh.Instance.Alias), true);
                //menu.Items.Add<RefreshNode, ActionRefresh>(ApplicationContext.Services.TextService.Localize(ActionRefresh.Instance.Alias));
            }
            //submenus of roots children
            else {
                var item = new MenuItem("editAthlete", "Edit");
                item.Icon = "edit";
                item.AdditionalData.Add("ParentCategoryId", id);
                item.NavigateToRoute("import/importTree/editAthlete/"+id);
                menu.Items.Add(item);
                //menu.Items.Add(new MenuItem("athlete/editAthlete2", "Edit2"));

                item = new MenuItem("deleteAthlete", "Delete");
                item.Icon = "delete";
                item.AdditionalData.Add("ParentCategoryId", id);
                //item.NavigateToRoute("import/importTree/deleteAthlete/" + id);
                menu.Items.Add(item);
            }

            return menu;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings) {
            var ctrl = new ImportApiController();
            var nodes = new TreeNodeCollection();

            if (id == Constants.System.Root.ToInvariantString())
                foreach (var athlete in ctrl.GetAll()) {
                    var node = CreateTreeNode(athlete.Id.ToString(), "-1", queryStrings, athlete.ToString(), "icon-document", false);

                    nodes.Add(node);
                }

            return nodes;
        }
    }
}