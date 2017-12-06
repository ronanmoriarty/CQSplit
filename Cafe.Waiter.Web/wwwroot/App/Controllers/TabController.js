waiterModule.controller("TabController", ["$scope", "$http", "$location", function ($scope, $http, $location) {
    var tabListUrl = "/api/tab";
    $scope.waiter = "Jim"; // hard-coded for now. TODO get this from api later
    $http({
        method: "GET",
        url: tabListUrl
    }).then(function (successResponse) {
        $scope.tabs = successResponse.data.map(function(tab) {
            return {
                tableNumber: tab.tableNumber,
                waiter: tab.waiter,
                status: tab.status,
                id: tab.id
            };
        });
    }, function (errorResponse) {
        console.log(errorResponse);
    });

    var createTabUrl = "/api/tab/create";
    $scope.createNewTab = function() {
        $http({
            method: "POST",
            url: createTabUrl,
            data: {
                waiter: $scope.waiter,
                tableNumber: $scope.formData.tableNumber
            }
        }).then(function (successResponse) {
            // toastr.success("Tab created.");
        }, function (errorResponse) {
            console.log(errorResponse);
            // toastr.error('An error occurred creating a new tab.');
        });
    };

    // $scope.viewDetails = function(id) {
    //     $location.path("/details").search({tabId: id});
    // };
}]);