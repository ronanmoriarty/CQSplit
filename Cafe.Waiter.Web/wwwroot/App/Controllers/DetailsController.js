waiterModule.controller("DetailsController", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        var menuUrl = "/api/menu";
        $http({
            method: "GET",
            url: menuUrl
        }).then(function (successResponse) {
            $scope.menuItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        $scope.id = $routeParams.tabId;
        var tabDetailsUrl = "/api/tab/" + $scope.id;
        $http({
            method: "GET",
            url: tabDetailsUrl
        }).then(function(successResponse) {
            $scope.waiter = successResponse.data.waiter;
            $scope.tableNumber = successResponse.data.tableNumber;
            $scope.status = successResponse.data.status;
            $scope.selectedItems = successResponse.data.items;
            var tabDetailsIndex = 0;
            $scope.selectedItems.forEach(function (item) {
                item.tabDetailsIndex = tabDetailsIndex;
                tabDetailsIndex++;
            });
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        // $scope.formData = {};

        $scope.addMenuItem = function () {
            var selectedMenuItem;

            selectedMenuItem = getSelectedMenuItem();
            $scope.selectedItems.push({
                menuNumber: selectedMenuItem.id,
                isDrink: selectedMenuItem.isDrink,
                name: selectedMenuItem.name,
                notes: $scope.formData.notes,
                tabDetailsIndex: getNewTabDetailsIndex()
            });
        };

        function getSelectedMenuItem(){
            return $scope.menuItems.find(function(item) { return item.id === $scope.formData.newMenuItem.id; });
        }

        function getNewTabDetailsIndex(){
            var maxTabDetailsIndex = Math.max.apply(null, $scope.selectedItems.map(function(item) {
                return item.tabDetailsIndex;
            }));
            var newTabDetailsIndex = maxTabDetailsIndex + 1;
            if(newTabDetailsIndex < 0) {
                newTabDetailsIndex = 0;
            }

            return newTabDetailsIndex;
        }

        // $scope.removeMenuItem = function(index) {
        //     console.log("Remove menu item from selected items...");
        //     $scope.selectedItems = $scope.selectedItems.filter(function(item) { return item.tabDetailsIndex !== index });
        // };

        // $scope.placeOrder = function() {
        //     var selectedItems = $scope.selectedItems.map(
        //         function (item) {
        //             return {
        //                 menuNumber: item.menuNumber,
        //                 isDrink: item.isDrink,
        //                 name: item.name,
        //                 notes: item.notes
        //             };
        //         }
        //     );

        //     var tabDetails = {
        //         id: $scope.id,
        //         waiter: $scope.waiter,
        //         tableNumber: $scope.tableNumber,
        //         status: $scope.status,
        //         items: selectedItems
        //     };

        //     $http({
        //         method: "POST",
        //         url: "/api/tab/placeOrder",
        //         data: tabDetails
        //     }).then(function (successResponse) {
        //         console.log("Success placing order");
        //         console.log(successResponse);
        //     }, function (errorResponse) {
        //         console.log("Error placing order");
        //         console.log(errorResponse);
        //     });
        // }
    }
]);