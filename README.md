# Fazer extensões em Umbraco 101

### Nota: A minha extensão de teste chama-se _Import_. A maioria dos nomes dos ficheiros segue uma nomenclatura definida pelo Umbraco.

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
   protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings) {...}
   protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings) {...}
}
```



