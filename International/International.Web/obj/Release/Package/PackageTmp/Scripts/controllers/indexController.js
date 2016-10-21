
rootApplication.controller('indexController', function ($scope, utilities) {

    utilities.ajax({
        url: '/home/get',
        method: 'GET',
        throbber: true,
        throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h2') },
        disableScreen: false,
        disableControl: angular.element('h2'),
        success: function (mvcResponse, angularResponse) {
            $scope.users = mvcResponse;
        }
    });

    $scope.BrokerName = '';
    $scope.BrokerDescription=null;
    /*$scope.login = function (sender) {

        utilities.ajax({
            url: '/api/values/login',
            method: 'POST',
            data: $scope.user,
            validate: true,
            form: 'form',
            success: function (mvcResponse, angularResponse) {

                if (mvcResponse.status == true) {}
            }
        });
    };*/

});