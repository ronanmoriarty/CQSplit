waiterModule.controller("DetailsController", ["$scope", "$http", "$location", function ($scope, $http, $location) {
        var menuUrl = "/menu/index";
        $http({
            method: "GET",
            url: menuUrl
        }).then(function (successResponse) {
            $scope.menuItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        console.log("tabId is:");
        var id = getQueryVariable("tabId");
        console.log(id);
        var tabDetailsUrl = "/tab/tabdetails?tabId=" + id;
        $http({
            method: "GET",
            url: tabDetailsUrl
        }).then(function(successResponse) {
            $scope.waiter = successResponse.data.waiter;
            $scope.tableNumber = successResponse.data.tableNumber;
            $scope.status = getStatus(successResponse.data.status);
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

        function getQueryVariable(variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (decodeURIComponent(pair[0]) == variable) {
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