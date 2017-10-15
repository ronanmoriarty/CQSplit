waiterModule.controller("TabController", ["$scope", "$http", function ($scope, $http) {
    var tabListUrl = "/api/tab/index";
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
}]);