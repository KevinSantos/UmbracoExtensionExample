angular.module("umbraco").controller("Import.ImportEditAthleteController", function ($scope, $routeParams, $timeout, importResource, notificationsService, navigationService) {

    $scope.loaded = true;

    importResource.getById($routeParams.id).then(function (response) {
        $scope.athlete = response.data;
    });
    

    $scope.edit = function () {

        importResource.edit($routeParams.id, $scope.athlete).then(function (response) {
            if (parseInt(response.data) < 0) //error
                notificationsService.error("Edit Failed", "booooh");
            else {
                notificationsService.success("Edit Successfull", "hooraaaay for you!");

                navigationService.syncTree({ tree: "importTree", path: ["-1", "import", $scope.contentId], forceReload: true }).then(function (syncArgs) {
                    $scope.busy = false;
                    $scope.currentNode = syncArgs.node;
                    if ($routeParams.id == $scope.currentNode.id) {
                        $route.reload();
                    }
                });
            }
        });
    };
});



