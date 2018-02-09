angular.module("umbraco").controller("Import.DeleteAthleteController", function ($scope, $routeParams, importResource, notificationsService, navigationService) {
    $scope.loaded = true;
    $scope.athlete = {};


    //angular.element(targetNode).scope(document.querySelector())
    importResource.getById($scope.currentNode.id).then(function (response) {
        $scope.athlete = response.data;
    });

    $scope.delete = function () {
        importResource.deleteById($scope.currentNode.id).then(function (response) {
            if (response < 0)
                notificationsService.error("Delete Failed", "booooh");
            else {
                notificationsService.success("Delete Successful", "yeahhhhhh");

                navigationService.syncTree({ tree: "importTree", path: ["-1", "import", $scope.contentId], forceReload: true }).then(function (syncArgs) {
                    $scope.busy = false;
                    $scope.currentNode = syncArgs.node;
                    if ($routeParams.id == $scope.currentNode.id) {
                        $route.reload();
                    }

                });

            }
        });
    }
});