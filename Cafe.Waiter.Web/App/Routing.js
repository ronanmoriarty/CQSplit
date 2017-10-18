waiterModule.config(["$routeProvider", function ($routeProvider) {
    $routeProvider
        .when("/tabs",
            {
                templateUrl: "Views/Tabs.html",
                controller: "TabController"
            }
        )
        .when("/details",
            {
                templateUrl: "Views/Details.html",
                controller: "DetailsController"
            }
        );
}]);