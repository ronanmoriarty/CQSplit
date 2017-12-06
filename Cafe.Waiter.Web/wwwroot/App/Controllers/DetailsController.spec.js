describe('DetailsController', function() {
  var $scope, menuItems, menuItem1, menuItem2, tabId, menuNumber1, isDrink1, name1, notes1, menuNumber2, isDrink2, name2, notes2, $httpBackend;

  beforeEach(function(){
    module('waiter');
  });

  beforeEach(inject(function($rootScope, _$httpBackend_, $controller){
    $scope = $rootScope.$new();
    menuItem1 = {
      id: 123,
      isDrink: true,
      name: 'Lemonade'
    };
    menuItem2 = {
      id: 234,
      isDrink: false,
      name: 'Lasagne'
    };
    menuItems = [
      menuItem1,
      menuItem2
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
    $httpBackend = _$httpBackend_;
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

    it('should be ready to add form data', function() {
      assert.isDefined($scope.formData);
    });
  });

  describe('when tab loaded', function() {
    var tabDetailsId;

    beforeEach(function() {
      $scope.selectedItems = items;
      $scope.selectedItems.forEach(function(item, index){
        item.tabDetailsIndex = index;
      });
      tabDetailsId = 543;
      $scope.id = tabDetailsId;
    });

    describe('when adding menu item', function() {
      var notes;

      beforeEach(function() {
        notes = 'some notes';
        $scope.formData = {
          newMenuItem: {
            id: 123
          },
          notes: notes
        };
      });

      it('should add to selected items list', function() {
        var lastItem,
          initialNumberOfSelectedItems,
          expectedNewTabDetailsIndex;

        initialNumberOfSelectedItems = $scope.selectedItems.length;
        expectedNewTabDetailsIndex = initialNumberOfSelectedItems;

        $scope.addMenuItem();

        assert.equal($scope.selectedItems.length, initialNumberOfSelectedItems + 1);
        lastItem = $scope.selectedItems[$scope.selectedItems.length - 1];
        assert.equal(lastItem.menuNumber, menuItem1.id);
        assert.equal(lastItem.isDrink, menuItem1.isDrink);
        assert.equal(lastItem.name, menuItem1.name);
        assert.equal(lastItem.notes, notes);
        assert.equal(lastItem.tabDetailsIndex, expectedNewTabDetailsIndex);
      });

      it('should add item to the bottom of the selected items list', function() {
        var lastItem;
        $scope.addMenuItem();

        lastItem = $scope.selectedItems[$scope.selectedItems.length - 1];
        assert.equal(lastItem.tabDetailsIndex, 2);
      });
    });

    describe('when removing selected item', function(){
      it('selected item should not be in the list of selected items any more', function() {
        var itemThatShouldHaveBeenRemoved;

        $scope.selectedItems = items;
        $scope.selectedItems.forEach(function(item, index){
          item.tabDetailsIndex = index;
        });

        $scope.removeMenuItem(0);

        assert.equal($scope.selectedItems.length, 1);
        itemThatShouldHaveBeenRemoved = $scope.selectedItems.find(function(item) {
          return item.menuNumber === menuNumber1
            & item.isDrink === isDrink1
            & item.name === name1
            & item.notes === notes1;
        });

        assert.isUndefined(itemThatShouldHaveBeenRemoved);
      });
    });

    describe('when placing order', function() {
      it('should post tab details to api', function() {
        var tabDetails = {
              id: tabDetailsId,
              waiter: waiter,
              tableNumber: tableNumber,
              status: status,
              items: items
          };
        $httpBackend.expectPOST('/api/tab/placeOrder', tabDetails).respond(200, '');

        $scope.placeOrder();
        $httpBackend.flush();
      });
    });
  });
});