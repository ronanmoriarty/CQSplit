describe('DetailsController', function() {
  var $scope, menuItems, tabId, menuNumber1, isDrink1, name1, notes1, menuNumber2, isDrink2, name2, notes2;

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
        }
      );

    tabId = 321;
    $routeParams = {
      tabId: tabId
    };

    waiter = 'John';
    tableNumber = 10;
    status = 'Seated';
    menuNumber1 = 12;
    isDrink1 = true;
    name1 = 'Coca cola';
    notes1 = 'no ice';
    menuNumber2 = 13;
    isDrink2 = false;
    name2 = 'Spaghetti carbonara';
    notes2 = '';
    items = [
      {
        menuNumber: menuNumber1,
        isDrink: isDrink1,
        name: name1,
        notes: notes1
      },
      {
        menuNumber: menuNumber2,
        isDrink: isDrink2,
        name: name2,
        notes: notes2
      }
    ];
    _$httpBackend_
      .when('GET', '/api/tab/' + tabId)
      .respond(200, {
        waiter: waiter,
        tableNumber: tableNumber,
        status: status,
        items: items
      });
    ctrl = $controller('DetailsController', {$scope: $scope, $routeParams: $routeParams});
    _$httpBackend_.flush();
  }));

  it('should get menu items from api', function(){
    assert.deepEqual($scope.menuItems, menuItems);
  });

  it('should set id from tabId querystring parameter', function(){
    assert.equal($scope.id, tabId);
  });

  it('should set the waiter name from the tab', function(){
    assert.equal($scope.waiter, waiter);
  });

  it('should set the table number from the tab', function(){
    assert.equal($scope.tableNumber, tableNumber);
  });

  it('should set the status from the tab', function(){
    assert.equal($scope.status, status);
  });

  it('should set the selected items from the tab', function(){
    assert.deepEqual($scope.selectedItems, items);
  });
});