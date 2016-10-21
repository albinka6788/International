rootApplication.BrokerController = function ($scope, utilities, $compile, $timeout, $filter) {

    $scope.InitBrokerDetailPage = function () {
        var form = angular.element('#BrokerForm');
        $.validator.unobtrusive.parse(form);
    }

    $scope.getBroker = function (key, callback) {
        utilities.ajax({
            url: '/Master/SearchBroker',
            method: 'GET',
            params: { key: $scope.submissionModel.Broker, productLineId: $scope.submissionModel.ProductLineTypeId, productlineSubTypeId: $scope.submissionModel.ProductLineSubTypeId },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            success: function (mvcResponse) {
                var data = $.map(mvcResponse, function (item) {
                    return $.extend({
                    }, {
                        label: item.BrokerName,
                        value: item.BrokerName,
                        realvalue: item.BrokerId,
                    }, item)
                });

                callback(data);
            }
        });
    };



    $scope.selectBroker = function (result) {
        $scope.submissionModel.Broker = result.item.BrokerName;
        $scope.submissionModel.SelectedBroker = result.item.BrokerName;  
        $scope.submissionModel.BrokerId = result.item.BrokerId;

        $('#SelectedBroker').val(result.item.BrokerName);
        $('#SelectedBroker').valid();

        $scope.Broker = null;
        $scope.BrokerContact = null;
        $scope.submissionModel.BrokerEntity = '';
        $scope.submissionModel.SelectedBrokerEntity = '';
        $scope.submissionModel.SelectedBrokerContactPerson = '';
        $scope.submissionModel.BrokerEntityId = 0;
        $scope.BrokerContact = null;
        $scope.submissionModel.BrokerContactPerson = '';
        $scope.$apply();
    };

    $scope.getBrokerEntity = function (key, callback) {
        if ($scope.submissionModel.BrokerId) {
            utilities.ajax({
                url: '/Master/SearchBrokerEntity',
                method: 'GET',
                params: {
                    key: $scope.submissionModel.BrokerEntity ? '' : $scope.submissionModel.BrokerEntity,
                    productLineId: $scope.submissionModel.ProductLineTypeId,
                    productlineSubTypeId: $scope.submissionModel.ProductLineSubTypeId,
                    brokerId: $scope.submissionModel.BrokerId
                },
                //throbber: true,
                //throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                success: function (mvcResponse) {
                    var data = $.map(mvcResponse, function (item) {
                        return $.extend({
                        }, {
                            label: item.BrokerEntityName + '-' + item.CityName,
                            value: item.BrokerEntityName,
                            realvalue: item.BrokerPartyId,
                        }, item)
                    });
                    callback(data);
                }
            });
        }
    };

    $scope.selectBrokerEntity = function (result) {       
        $scope.Broker = result.item;
        $scope.submissionModel.BrokerEntityId = result.item.BrokerPartyId;
        $scope.submissionModel.BrokerEntity = result.item.BrokerEntityName;
        
        $scope.submissionModel.SelectedBrokerEntity = result.item.BrokerEntityName;
        $scope.submissionModel.Broker.Address = result.item.StreetAddress;
        
        $('#SelectedBrokerEntity').val(result.item.BrokerEntityName);
        $('#SelectedBrokerEntity').valid();

        $scope.BrokerContact = null;
      
        $scope.submissionModel.SelectedBrokerContactPerson = '';
        $scope.submissionModel.BrokerContactPerson = '';

        var form = angular.element('#BrokerForm');
        $.validator.unobtrusive.parse(form);
        $('#SelectedBrokerEntity').valid();
        $scope.$apply();
    };

    $scope.checkBroker = function()
    {
        if(!$scope.submissionModel.Broker)
        {
            $scope.Clearbroker();
        }
    }

    $scope.checkBrokerEntity = function () {
        if (!$scope.submissionModel.BrokerEntity) {
            $scope.ClearBrokerEntity();
        }
    }

    $scope.checkBrokerContact = function () {
        if (!$scope.submissionModel.BrokerContactPerson) {
            $scope.BrokerContact = null;
            $scope.submissionModel.BrokerContactPerson = '';
            $scope.submissionModel.BrokerContactPersonId = null;
            $scope.submissionModel.SelectedBrokerContactPerson = '';
        }
    }

    


    $scope.Clearbroker = function () {
        $scope.Broker = null;
        $scope.submissionModel.BrokerId = 0;
        $scope.submissionModel.BrokerEntity = '';
        $scope.submissionModel.SelectedBrokerEntity = '';
        $scope.submissionModel.SelectedBrokerContactPerson = '';
        $scope.submissionModel.SelectedBroker = '';
        $scope.submissionModel.BrokerEntityId = null;
        $scope.BrokerContact = null;
        $scope.submissionModel.BrokerContactPerson = '';
        $scope.submissionModel.BrokerContactPersonId = null;
    }

    $scope.ClearBrokerEntity = function () {
        $scope.Broker = null;      
        $scope.submissionModel.BrokerEntityId = null;
        $scope.BrokerContact = null;
        $scope.submissionModel.BrokerContactPerson = '';
        $scope.submissionModel.BrokerContactPersonId = null;
        $scope.submissionModel.SelectedBrokerContactPerson = '';
        $scope.submissionModel.SelectedBrokerEntity = '';
    }



    $scope.getBrokerContact = function (key, callback) {
        utilities.ajax({
            url: '/Master/SearchBrokerContact',
            method: 'GET',
            params: {
                key: $scope.submissionModel.BrokerContactPerson,
                brokerEntityId: $scope.submissionModel.BrokerEntityId,
            },
            //throbber: true,
            //throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            success: function (mvcResponse) {
                var data = $.map(mvcResponse, function (item) {
                    return $.extend({
                    }, {
                        label: item.FirstName + ' ' + item.LastName,
                        value: item.FirstName + ' ' + item.LastName,
                        realvalue: item.ContactPersonId,
                    }, item)
                });

                callback(data);
            }
        });
    };

    $scope.selectBrokerContact = function (result) {        
      
        $scope.BrokerContact = result.item;
        $scope.submissionModel.BrokerContactPersonId = result.item.ContactPersonId;
        $scope.submissionModel.SelectedBrokerContactPerson = result.item.FirstName + ' ' + result.item.LastName;
        $scope.submissionModel.BrokerContactPerson = result.item.FirstName + ' ' + result.item.LastName;
        
        $('#SelectedBrokerContactPerson').val(result.item.FirstName);
     
        $('#SelectedBrokerContactPerson').valid();
        $scope.$apply();
    };

    $scope.saveBroker = function () {
        var form = angular.element('#BrokerForm');
        

        if (!$(form).valid()) {
            console.log('Invalid broker'); return false;
        }
        else {
            $scope.incrementStep();
        }
    };
};