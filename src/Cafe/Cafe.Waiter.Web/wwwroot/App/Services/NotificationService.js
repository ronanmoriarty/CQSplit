angular
    .module('waiter')
    .service('notificationService', [function() {
        return {
            success: success,
            error: error
        };

        function success(message){
            toastr.success(message);
        }

        function error(message){
            toastr.error(message);
        }
}]);