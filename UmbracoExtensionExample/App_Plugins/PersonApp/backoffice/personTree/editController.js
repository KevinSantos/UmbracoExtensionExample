angular.module("umbraco").controller("Person.editController", function ($scope, $routeParams, personResource) {
    $scope.loaded = false;

    if($routeParams.id == -1){
        $scope.person = {};
        $scope.loaded = true;
    }
    else{
        personResource.getById($routeParams.id).then(function(response){
            $scope.person = response.data;
            $scope.loaded = true;
        });
    }

    //$scope.publish = function (node) {
    //    personResource.publish(node.Id).then(function (response) {
    //        $scope.node = response.data;
    //        $scope.contentForm.$dirty = false;
    //        navigationService.syncTree({ tree: 'personTree', path: [-1, -1], forceReload: true });
    //        notificationsService.success("Success", node.Name + " has been published");
    //    });
    //};
})