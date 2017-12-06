describe('TabController', function() {
    var $scope,
        ctrl,
        tabs,
        tab1,
        tab2,
        $httpBackend,
        waiter,
        tableNumber,
        notificationService;

    beforeEach(function() {
        module('waiter');
    });

    beforeEach(module(function($provide) {
        $provide.value('notificationService', notificationService);
        notificationService = {
            success: sinon.spy()
        };
    }));

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
        tableNumber = 11;
        waiter = 'Jim';
        $scope = $rootScope.$new();
        _$httpBackend_
            .when('GET', '/api/tab')
            .respond(200, tabs);
        ctrl = $controller('TabController', {$scope: $scope, notificationService: notificationService});
        _$httpBackend_.flush();

        $httpBackend = _$httpBackend_;
    }));

    it('should load tabs from api', function() {
        assert.equal($scope.tabs.length, 2);
    });

    describe('when tabs loaded', function() {
        beforeEach(function() {
            $scope.waiter = waiter;
            $scope.formData = {
                tableNumber: tableNumber
            };
        });

        describe('when tab created successfully', function() {
            beforeEach(function() {
                $httpBackend
                    .when('POST', '/api/tab/create', {
                        waiter: waiter,
                        tableNumber: tableNumber
                    })
                    .respond(200, {});
            });

            it('should notify user when tab created successfully', function() {
                $scope.createNewTab();
                $httpBackend.flush();

                assert.equal(notificationService.success.withArgs('Tab created.').callCount, 1);
            });
        });
    });
});
