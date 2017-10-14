waiterModule.controller("DetailsController", ["$scope", "$http", "$location", function ($scope, $http, $location) {
        var menuUrl = "/api/menu/index";
        $http({
            method: "GET",
            url: menuUrl
        }).then(function (successResponse) {
            $scope.menuItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        console.log("tabId is:");
        $scope.id = getQueryVariable("tabId");
        console.log($scope.id);
        var tabDetailsUrl = "/api/tab/tabdetails?tabId=" + $scope.id;
        $http({
            method: "GET",
            url: tabDetailsUrl
        }).then(function(successResponse) {
            $scope.waiter = successResponse.data.waiter;
            $scope.tableNumber = successResponse.data.tableNumber;
            $scope.statusId = successResponse.data.status;
            $scope.status = getStatus($scope.statusId);
            $scope.selectedItems = successResponse.data.items;
            var tabDetailsIndex = 0;
            $scope.selectedItems.forEach(function (item) {
                item.tabDetailsIndex = tabDetailsIndex;
                tabDetailsIndex++;
            });
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        $scope.formData = {};

        $scope.addMenuItem = function () {
            console.log("Add item to selected items...");
            var selectedMenuItem = $scope.menuItems.find(function(item) { return item.id === $scope.formData.newMenuItem.id });
            $scope.selectedItems.push({
                menuNumber: selectedMenuItem.id,
                isDrink: selectedMenuItem.isDrink,
                name: selectedMenuItem.name,
                notes: $scope.formData.notes,
                tabDetailsIndex: Math.max.apply(null, $scope.selectedItems.map(function(item) { return item.tabDetailsIndex })) + 1
            });
        };

        $scope.removeMenuItem = function(index) {
            console.log("Remove menu item from selected items...");
            $scope.selectedItems = $scope.selectedItems.filter(function(item) { return item.tabDetailsIndex !== index });
        };

        $scope.placeOrder = function() {
            var selectedItems = $scope.selectedItems.map(
                function (item) {
                    return {
                        menuNumber: item.menuNumber,
                        isDrink: item.isDrink,
                        name: item.name,
                        notes: item.notes
                    };
                }
            );

            var tabDetails = {
                id: $scope.id,
                waiter: $scope.waiter,
                tableNumber: $scope.tableNumber,
                status: $scope.statusId,
                items: selectedItems
            };

            $http({
                method: "POST",
                url: "/api/tab/details",
                data: tabDetails
            }).then(function (successResponse) {
                console.log("Success placing order");
                console.log(successResponse);
            }, function (errorResponse) {
                console.log("Error placing order");
                console.log(errorResponse);
            });
        }

        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (decodeURIComponent(pair[0]) === variable) {
                    return decodeURIComponent(pair[1]);
                }
            }
            console.log("Query variable %s not found", variable);
        }

        function getStatus(statusId) {
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
    }
]);