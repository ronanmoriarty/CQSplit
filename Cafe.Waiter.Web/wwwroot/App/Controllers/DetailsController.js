waiterModule.controller("DetailsController", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {

        $http({
            method: "GET",
            url: getMenuUrl()
        }).then(function (successResponse) {
            $scope.menuItems = successResponse.data.items;
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        function getMenuUrl(){
            return "/api/menu";
        }

        $scope.id = $routeParams.tabId;
        $http({
            method: "GET",
            url: getTabDetailsUrl($scope.id)
        }).then(function(successResponse) {
            loadTabFromDetails(successResponse);
        }, function (errorResponse) {
            console.log(errorResponse);
        });

        function getTabDetailsUrl(tabDetailsId){
            return "/api/tab/" + tabDetailsId;
        }

        function loadTabFromDetails(response){
            $scope.waiter = response.data.waiter;
            $scope.tableNumber = response.data.tableNumber;
            $scope.status = response.data.status;
            loadItems(response.data.items);
        }

        function loadItems(items){
            if(items){
                $scope.selectedItems = items;
                var tabDetailsIndex = 0;
                $scope.selectedItems.forEach(function (item) {
                    item.tabDetailsIndex = tabDetailsIndex;
                    tabDetailsIndex++;
                });
            } else {
                $scope.selectedItems = [];
            }
        }

        $scope.formData = {};

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

        $scope.removeMenuItem = function(index) {
            $scope.selectedItems = $scope.selectedItems.filter(function(item) { return item.tabDetailsIndex !== index; });
        };

        $scope.placeOrder = function() {
            var tabDetails = {
                id: $scope.id,
                waiter: $scope.waiter,
                tableNumber: $scope.tableNumber,
                status: $scope.status,
                items: $scope.selectedItems
            };

            $http({
                method: "POST",
                url: "/api/tab/placeOrder",
                data: tabDetails
            });
        };
    }
]);