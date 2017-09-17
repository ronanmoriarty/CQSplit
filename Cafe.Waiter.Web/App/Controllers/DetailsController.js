waiterModule.controller('DetailsController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        var menuUrl = '/menu/index';
        $http({
            method: 'GET',
            url: menuUrl
        }).then(function (successResponse) {
            $scope.options = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        console.log('tabId is:');
        var id = $location.search().tabId;
        console.log(id);
        var tabDetailsUrl = '/tab/tabdetails?tabId=' + id;
        $http({
            method: 'GET',
            url: tabDetailsUrl
        }).then(function(successResponse) {
            $scope.selectedItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });
    }
]);