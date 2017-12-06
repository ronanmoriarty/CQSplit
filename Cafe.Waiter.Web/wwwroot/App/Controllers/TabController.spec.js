describe('TabController', function() {
    var $scope,
        ctrl,
        tabs,
        tab1,
        tab2,
        $httpBackend,
        waiter,
        tableNumber,
        notificationService,
        $location,
        detailsPath;

    beforeEach(function() {
        module('waiter');
    });

    beforeEach(module(function($provide) {
        notificationService = {
            success: sinon.spy(),
            error: sinon.spy()
        };
        $provide.value('notificationService', notificationService);
        $location = {
            path: sinon.stub()
        };

        detailsPath = {
            search: sinon.spy()
        };
        $location.path.withArgs('/details').returns(detailsPath);
        $provide.value('$location', $location);
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
        waiter = 'Bill';
        $scope = $rootScope.$new();
        _$httpBackend_
            .when('GET', '/api/tab')
            .respond(200, tabs);
        ctrl = $controller('TabController', {$scope: $scope, notificationService: notificationService, $location: $location});
        _$httpBackend_.flush();

        $httpBackend = _$httpBackend_;
    }));

    describe('when loading tabs', function() {
        it('should load tabs from api', function() {
            assert.deepEqual($scope.tabs, tabs);
        });
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

        describe('when tab creation fails', function() {
            beforeEach(function() {
                $httpBackend
                    .when('POST', '/api/tab/create', {
                        waiter: waiter,
                        tableNumber: tableNumber
                    })
                    .respond(500, {});
            });

            it('should notify user when tab creation failed', function() {
                $scope.createNewTab();
                $httpBackend.flush();

                assert.equal(notificationService.error.withArgs('An error occurred while creating new tab.').callCount, 1);
            });
        });
    });

    describe('when viewing tab details', function() {
        it('should load details view for selected tab', function() {
            var tabId;

            tabId = 456;

            $scope.viewDetails(tabId);

            assert.equal(detailsPath.search.withArgs({tabId: tabId}).callCount, 1);
        });
    });
});
