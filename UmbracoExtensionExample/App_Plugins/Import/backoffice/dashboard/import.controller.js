angular.module("umbraco").controller("Import.ImportController", function ($scope, $routeParams, $timeout, importResource,notificationsService) {

    $scope.node = {};
    $scope.loaded = true;
    $scope.canSave = false;

    $scope.htmlTable = "";

    $scope.preview = function () {
        if ($('#myfile').get(0).files.length)
            $scope.canSave = true;
    };

    $scope.save = function save() {

        //transform html into json;
        var rowsChecked = $('input:checked').parent().parent();

        if (!rowsChecked.length)
            return;

        var columnsName = $('.umb-table-head .umb-table-cell');
        var result = [];

        for (var i = 0; i < rowsChecked.length; i++) {
            var columnsValue = rowsChecked.eq(i).children();
            var obj = new Object();
            for (var j = 0; j < columnsValue.length - 1; j++) { // -1 no need to read check column
                obj[columnsName.eq(j).text()] = columnsValue.eq(j).text();
                if (!j)
                    obj[columnsName.eq(j).text()] = parseInt(columnsValue.eq(j).text());
            }
            result.push(obj);
        }
        //console.log(result);

        importResource.save(result).then(function (response) {
            if (parseInt(response.data) < 0) //error
                notificationsService.error("Import Failed", "booooh");
            else
                notificationsService.success("Import Successfull", "hooraaaay for you!");
            

        });
    };
});



//angular.module("umbraco").controller("Import.ImportEditAthleteController", function ($scope, $routeParams, $timeout, importResource) {


//    console.log("bazinga");

//});