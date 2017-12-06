describe('TabController', function() {
    var $scope,
        ctrl,
        tabs,
        tab1,
        tab2;

    beforeEach(function() {
        module('waiter');
    });

    beforeEach(inject(function($rootScope, _$httpBackend_, $controller){
        tab1 = {
            tableNumber: 5,
            waiter: 'Jim',
            status: 'Seated',
            id: 234
        };
        tab2 = {
            tableNumber: 6,
            waiter: 'Mary',
            status: 'Drinks Served',
            id: 345
        };
        tabs = [
            tab1,
            tab2
        ];
        $scope = $rootScope.$new();
        _$httpBackend_
            .when('GET', '/api/tab')
            .respond(200, tabs);
        ctrl = $controller('TabController', {$scope: $scope});
        _$httpBackend_.flush();
    }));

    it('should load tabs from api', function() {
        assert.equal($scope.tabs.length, 2);
    });
});
