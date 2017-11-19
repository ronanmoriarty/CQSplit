describe('DetailsController', function() {
  var ctrl, $scope, $httpBackend, $controller, menuItems;

  beforeEach(function(){
    module('waiter');
  });

  beforeEach(inject(function($rootScope, $injector, _$httpBackend_, $controller){
    $scope = $rootScope.$new();
    $httpBackend = _$httpBackend_;
    menuItems = [{
      id: 123
    },{
      id: 234
    }];
    $httpBackend
      .when('GET', '/api/menu')
      .respond(200,
        {
          items: menuItems
        });
    ctrl = $controller('DetailsController', {$scope: $scope});
    $httpBackend.flush();
  }));

  it('should get menu items from api', function(){
    assert.deepEqual($scope.menuItems, menuItems);
  });
});