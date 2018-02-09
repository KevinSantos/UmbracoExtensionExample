angular.module("umbraco.resources")
    .factory("importResource", function ($http) {
        return {
            getById: function (id) {
                return $http.get("backoffice/Import/ImportApi/GetById?id=" + id);
            },
            edit: function (oldId, athlete) {
                return $http.post("backoffice/Import/ImportApi/Edit", {oldId: oldId, athlete: athlete} , {
                    headers: {
                        'Content-Type':  'application/json',
                    }});
                
            },
            save: function (athletesList) {
                return $http.post("backoffice/Import/ImportApi/Save", athletesList);
            },
            deleteById: function (id) {
                return $http.delete("backoffice/Import/ImportApi/DeleteById?id=" + id);
            }
        };
    });