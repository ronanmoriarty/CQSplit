describe('DetailsController', function() {
  var $scope, menuItems, tabId;

  beforeEach(function(){
    module('waiter');
  });

  beforeEach(inject(function($rootScope, _$httpBackend_, $controller){
    $scope = $rootScope.$new();
    menuItems = [
      {
        id: 123
      },{
        id: 234
      }
    ];
    _$httpBackend_
      .when('GET', '/api/menu')
      .respond(200,
        {
          items: menuItems
        });
    tabId = 321;
    $routeParams = {
      tabId: tabId
    };
    ctrl = $controller('DetailsController', {$scope: $scope, $routeParams: $routeParams});
    _$httpBackend_.flush();
  }));

  it('should get menu items from api', function(){
    assert.deepEqual($scope.menuItems, menuItems);
  });

  it('should set id from tabId querystring parameter', function(){
    assert.equal($scope.id, tabId);
  });
});