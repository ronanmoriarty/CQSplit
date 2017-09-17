waiterModule.controller('DetailsController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        var menuUrl = '/menu/index';
        $http({
            method: 'GET',
            url: menuUrl
        }).then(function (successResponse) {
            $scope.menuItems = successResponse.data.items;
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

        $scope.formData = {};

        $scope.addMenuItem = function () {
            console.log('Add item to selected items...');
            $scope.selectedItems.push({
                menuNumber: $scope.formData.newMenuItem.id,
                isDrink: true, // TODO fix this - hardcoded to drink for now.
                name: $scope.formData.newMenuItem.name,
                notes: $scope.formData.notes
            });
        };
    }
]);