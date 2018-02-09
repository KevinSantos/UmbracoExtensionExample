# Fazer extensões em Umbraco 101

### Nota: A minha extensão de teste chama-se _Import_. A maioria dos nomes dos ficheiros segue uma nomenclatura definida pelo Umbraco.

### Links Uteis
* [Custom Sections - A Whole New Umbraco](https://www.youtube.com/watch?v=NY0OXGaCWVU&list=PLLYO0Qmbv5pvGjpI6Cyg3mVBoYtG_LK63&index=19)
* [Database & Api - Custom Data Needs A Home](https://www.youtube.com/watch?v=PXuIXptLZ5U&list=PLLYO0Qmbv5pvGjpI6Cyg3mVBoYtG_LK63&index=20)
* [Custom Tree In Umbraco - Structuring Your Content](https://www.youtube.com/watch?v=DbYNzEozj2U&index=21&list=PLLYO0Qmbv5pvGjpI6Cyg3mVBoYtG_LK63)
* [Finishing Up - Finishing the project](https://www.youtube.com/watch?v=BCTcXPZFZns&list=PLLYO0Qmbv5pvGjpI6Cyg3mVBoYtG_LK63&index=22)
* [BUILDING AN UMBRACO 7 BACKOFFICE EXTENSION – PART I](https://blogit.create.pt/andresantos/2015/11/16/building-an-umbraco-7-backoffice-extension-part-i/)
* [Tutorial de AngularJS](https://docs.angularjs.org/tutorial/step_00)
* [API de AngularJS do Umbraco](http://umbraco.github.io/Belle/#/api)
* [API de C# do Umbraco](https://our.umbraco.org/apidocs/csharp/api/Umbraco.Core.html)

## Estrutura

1. Criar pasta para extensão na directoria ~/App_Plugins: 

![](https://snag.gy/6F9W7J.jpg)

2. Criar ficheiros para o nosso WebAPI:

![](https://snag.gy/lqrSLJ.jpg)

3. Dentro da pasta criada, criar a seguinte estrutura:
   1. **backoffice** - Pasta que contém views e controllers em AngularJS.
   1. **Models** - Pasta com classes C#
   1. **import.resource.js** - Ficheiro escrito em AngularJS que permite fazer as chamadas AJAX para o nosso WebAPI.
   1. **ImportSection.cs** - Classe que permite criar o icon da extensão no Umbraco.
   1. **package.manifest** - Ficheiro que descrimina os ficheiros JS, CSS e outros, usados no projecto.
   1. **RegisterEvent.cs** - Cria tabela na BD caso ainda não exista.
   
![](https://snag.gy/Vj7YdL.jpg)

## Código

### importSection.cs
* Esta classe deve herdar de IApplication e receber o atributo Application que recebe como argumentos:
   * _alias_ da extensão
   * _titulo_
   * _icon_ (podem ser encontrados aqui: [Clica-me!](https://nicbell.github.io/ucreate/icons.html))
   * _nivel_ que deve ocupar na ordem das extensões. As 8 primeiras estão reservadas ao Umbraco (Content, Media, Settings, ...) e é boa prática deixar algumas posições para possíveis alterações do próprio Umbraco, daí se começar no 15.
```c#
[Application("import", "Import", "icon-smiley", 15)]
public class ImportSection : IApplication{}
```
**Nota:** O titulo da extensão que aparece no Umbraco não é este que acabamos de definir. O Umbraco vai à directoria ~/Umbraco/Config/Lang/<?>.xml à procura da tradução apropriada para o título. No meu caso, como uso EN-US tive de alterar o ficheiro _en_us.xml_ e adicionar o seguinte código à área das secções:
```xml
<area alias="sections">
  .
  .
  .
  <key alias="import">Import</key>
</area>
```

## Web API
### ImportApiController.cs
* Classe com os nossos métodos. De notar que o Umbraco usa o ORM [Petapoco](https://github.com/CollaboratingPlatypus/PetaPoco/wiki). É feita a ligação à nossa extensão usando o atributo _PluginController_.
```c#
[PluginController("Import")]
public class ImportApiController : UmbracoAuthorizedJsonController {
   public Athlete GetById(int id){...}
   ...
}
```

### ImportTreeController.cs
* Classe que cria a árvore que se pode ver no Umbraco:

![](https://snag.gy/jwYXch.jpg)
* Novamente, é feita a ligação com a extensão através do atributo _PluginController_. São também definidas as propriedades da árvore:
   * _alias_ da extensão
   * _alias_ da árvore
   * _titulo_ da root node
* Esta classe possui 2 métodos:
   * __GetMenuForNode__ - Define as acções possíveis para cada tipo de node (criar, editar, remover, regar...)
   * __GetTreeNodes__ - Cria a estrutura da árvore
```c#
[Tree("import", "importTree", "Content for Approval")]
[PluginController("Import")]
public class ImportTreeController : TreeController {
   protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings) {
      var menu = new MenuItemCollection();
      //submenus da root
      if (id == Constants.System.Root.ToInvariantString()) {
         // FORMA 1 DE CRIAR MENUS:
         // ao criar um menuItem ActionNew, o umbraco espera que haja um ficheiro edit.html na pasta backoffice.
         // ao criar um menuItem ActionDelete, é esperado um delete.html
         menu.Items.Add<CreateChildEntity, ActionNew>(ApplicationContext.Services.TextService.Localize(ActionNew.Instance.Alias));
         
         //FORMA 2 DE CRIAR MENUS:
         // fica a espera de um editAthlete.html
         var item = new MenuItem("editAthlete", "Edit");
         item.Icon = "edit";
         item.AdditionalData.Add("ParentCategoryId", id);
         item.NavigateToRoute("import/importTree/editAthlete/"+id);
         menu.Items.Add(item);
      }
      return menu
   }
   
   protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings) {...}
}
```
## Fim Web API

## backoffice
* Existem dois tipos de views no backoffice:
   * As que estão associados à árvore. Estas devem estar numa pasta com o mesmo _alias_ dado à árvore na class **ImportTreeController**.
   * Dashboards

![](https://snag.gy/xWGm29.jpg)
### importTree
* Vou exemplificar com a view **editAthlete**:
#### editAthlete.html
* É criado um form associado ao controller **Import.ImportEditAthleteController**.
```html
<form ng-controller="Import.ImportEditAthleteController"
     ng-show="loaded"
     ng-submit="edit()"
     val-form-manager>
   ...
```

#### editAthlete.controller.js
* Controller recebe importResource, (criado em import.resource.js, já falo dele de seguida) e chama um dos seus métodos para obter informação acerca do atleta que se quer editar.
```js
angular.module("umbraco").controller("Import.ImportEditAthleteController", function ($scope, $routeParams, $timeout, importResource, notificationsService, navigationService) {
   importResource.getById($routeParams.id).then(function (response) {
        $scope.athlete = response.data;
    });
    ...
 });
```
### Fim importTree

### Dashboards
* View que aparece quando se clica na extensão. Não é necessário estar numa pasta chamada Dashboards. Para avisar o Umbraco da nossa Dashboard é preciso acrescentar o seguinte código a /config/Dashboard.config:
```xml
<section alias="StartupImportDashboardSection">
   <areas>
      <area>import</area>
   </areas>
   <tab caption="Dashboard">
      <control>/app_plugins/import/backoffice/dashboard/importdashboard.html</control>
   </tab>
</section>
```

* O _alias_ da secção é irrelevante (até agora...)
* O valor da _area_ é o alias da nossa extensao
* A _caption_ é a frase que aparece na tab da nossa dashboard. Uma dashboard pode ter várias tabs:

![](https://snag.gy/kGXu8K.jpg)
* _control_ é a absolute path até à nossa view.
### Fim Dashboards

### import.resource.js
* Ficheiro que com os métodos chamados pelos nossos controllers e que, por sua vez, chamam os métodos da nossa Web API.
```js
angular.module("umbraco.resources")
    .factory("importResource", function ($http) {
        return {
            getById: function (id) {
                return $http.get("backoffice/Import/ImportApi/GetById?id=" + id);
            },
            ...
        }
    });
```

### package.manifest
* Ficheiro onde se avisa o Umbraco quais os ficheiros que devem pertencer à extensão. As views não são necessárias, mas o CSS pode ser posto aqui, ou importado directamente pela view.
```json
{
  javascript: [
    "~/App_Plugins/Import/backoffice/dashboard/import.controller.js",
    "~/App_Plugins/Import/backoffice/importTree/editAthlete.controller.js",
    "~/App_Plugins/Import/backoffice/importTree/deleteAthlete.controller.js",

    "~/App_Plugins/Import/import.resource.js"
  ]
}
```
