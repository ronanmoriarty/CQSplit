waiterModule.controller('DetailsController', ['$scope', '$http', function ($scope, $http) {
        var menuUrl = '/menu/index';
        $http({
            method: 'GET',
            url: menuUrl
        }).then(function (successResponse) {
            $scope.options = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        var id = '82ebc82f-72ee-42d8-9565-49b0e1844c86';
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