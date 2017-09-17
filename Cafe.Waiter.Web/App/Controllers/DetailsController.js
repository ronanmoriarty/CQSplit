waiterModule.controller('DetailsController', ['$scope', '$http', function ($scope, $http) {
        $scope.options = [{
                id: 123,
                text: 'Bacon & Cheese Burger'
            }, {
                id: 124,
                text: 'Steak & Chips'
            }, {
                id: 234,
                text: 'Coca Cola'
            }, {
                id: 235,
                text: 'Fanta'
            }, {
                id: 236,
                text: 'Lemonade'
            }
        ];

        var id = '82ebc82f-72ee-42d8-9565-49b0e1844c86';
        var url = '/tab/tabdetails?tabId=' + id;
        $http({
            method: 'GET',
            url: url
        }).then(function(successResponse) {
            $scope.selectedItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });
    }
]);