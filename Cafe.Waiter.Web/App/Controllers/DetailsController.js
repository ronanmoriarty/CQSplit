waiterModule.controller('DetailsController', ['$scope', function($scope) {
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

        $scope.selectedItems = [{
                menuNumber: 301,
                class: 'food',
                name: 'Lasagne',
                notes: ''
            }, {
                menuNumber: 302,
                class: 'food',
                name: 'Spaghetti Carbonara',
                notes: ''
            }, {
                menuNumber: 303,
                class: 'drink',
                name: 'Peroni',
                notes: ''
            }
        ];
    }
]);