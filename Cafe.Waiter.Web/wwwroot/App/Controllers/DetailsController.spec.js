describe('DetailsController', function() {
  var $scope, menuItems, tabId, menuNumber1, isDrink1, name1, notes1, menuNumber2, isDrink2, name2, notes2;

  beforeEach(function(){
    module('waiter');
  });

  beforeEach(inject(function($rootScope, _$httpBackend_, $controller){
    $scope = $rootScope.$new();
    menuItems = [
      {
        id: 123,
        isDrink: true,
        name: 'Lemonade'
      },{
        id: 234,
        isDrink: false,
        name: 'Lasagne'
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

  describe('when loading details page', function() {
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
      var selectedItem;

      items.forEach(function(item, index){
        selectedItem = $scope.selectedItems[index];
        assert.equal(selectedItem.menuNumber, item.menuNumber);
        assert.equal(selectedItem.isDrink, item.isDrink);
        assert.equal(selectedItem.name, item.name);
        assert.equal(selectedItem.notes, item.notes);
      });
    });

    it('should set the tabDetailsIndex for each item', function() {
      $scope.selectedItems.forEach(function(item, index) {
        assert.equal(item.tabDetailsIndex, index);
      });
    });

  });

  describe('when adding menu item', function() {
    it('should add to selected items list', function() {
      var lastItem;
      $scope.formData = {
        newMenuItem: {
          id: 123
        },
        notes: 'some notes'
      };
      $scope.selectedItems = [];

      $scope.addMenuItem();

      assert.equal($scope.selectedItems.length, 1);
      lastItem = $scope.selectedItems[$scope.selectedItems.length - 1];
      assert.equal(lastItem.menuNumber, 123);
      assert.equal(lastItem.isDrink, true);
      assert.equal(lastItem.name, 'Lemonade');
      assert.equal(lastItem.notes, 'some notes');
      assert.equal(lastItem.tabDetailsIndex, 0);
    });

    it('should add item to the bottom of the selected items list', function() {
      var lastItem;
      $scope.formData = {
        newMenuItem: {
          id: 123
        }
      };
      $scope.selectedItems = [
        {tabDetailsIndex: 0},
        {tabDetailsIndex: 1}
      ];

      $scope.addMenuItem();

      lastItem = $scope.selectedItems[$scope.selectedItems.length - 1];
      assert.equal(lastItem.tabDetailsIndex, 2);
    });
  });
});