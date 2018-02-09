angular.module("umbraco.resources")
.factory("personResource", function ($http) {
    return {
        getById: function (id) {
            return $http.get("backoffice/PersonApp/PersonApi/GetById?id="+id);
        },
        save: function (person) {
            return $http.post("backoffice/PersonApp/PersonApi/PostSave", angular.toJson(person));
        },
        deleteById: function (id) {
            return $http.delete("backoffice/PersonApp/PersonApi/DeleteById?id=" + id);
        }
    }
});