var waiterModule = angular.module('waiter', ['ngRoute'], function($locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
});