waiterModule.controller("TabController", ["$scope", "$http", "$location", "notificationService", function ($scope, $http, $location, notificationService) {
    $scope.waiter = "Jim"; // hard-coded for now. TODO get this from api later
    $http({
        method: "GET",
        url: getUrlToListTabs()
    }).then(function (successResponse) {
        $scope.tabs = successResponse.data;
    }, function (errorResponse) {
        console.log(errorResponse);
    });

    function getUrlToListTabs(){
        return '/api/tab';
    }

    $scope.createNewTab = function() {
        $http({
            method: "POST",
            url: getUrlToCreateTab(),
            data: getDataToCreateNewTab()
        }).then(function (successResponse) {
            notificationService.success('Tab created.');
        }, function (errorResponse) {
            notificationService.error('An error occurred while creating new tab.');
        });
    };

    function getUrlToCreateTab(){
        return "/api/tab/create";
    }

    function getDataToCreateNewTab(){
        return {
            waiter: $scope.waiter,
            tableNumber: $scope.formData.tableNumber
        };
    }

    $scope.viewDetails = function(id) {
        $location.path("/details").search({tabId: id});
    };
}]);