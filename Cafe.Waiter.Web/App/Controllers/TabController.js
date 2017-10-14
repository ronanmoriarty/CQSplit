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
                status: getStatusText(tab.status),
                id: tab.id
            };
        });
    }, function (errorResponse) {
        console.log(errorResponse);
        });

    function getStatusText(statusId) {
        switch (statusId) {
        case 0:
            return "Seated";
        case 1:
            return "Order placed";
        case 2:
            return "All drinks served";
        case 3:
            return "All food and drinks served";
        case 4:
            return "Dessert ordered";
        case 5:
            return "All desserts served";
        case 6:
            return "Bill requested";
        }
    }
}]);