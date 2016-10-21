//var rootApplication = angular.module("rootApplication", []);

//rootApplication.constant('applicationConstants', {

//    validationRules: [
//        { controlIdentifier: 'Name', status: [2, 4], disabled: true },
//        { controlIdentifier: 'Premium', status: [2, 3, 4], mandatory: true, mandatoryMsg: 'Premium Required' },
//        { controlIdentifier: 'Premium', status: [3], min: '0', minMsg: 'Cannot Be < 0' },
//        { controlIdentifier: 'Premium', status: [4], min: '1', minMsg: 'Cannot Be < 1' },
//        { controlIdentifier: 'PolicyType', status: [4], product: [1], mandatory: true, mandatoryMsg: 'Policy Type Required' },
//        { controlIdentifier: 'CoverageType', producttype: [1], mandatory: true, mandatoryMsg: 'Coverage Type Required' },
//    ]
//});


//rootApplication.directive('validationRules', function (applicationConstants) {
//    return {
//        restrict: 'A',
//        scope: {
//            status: '@',
//            identifier: '@',
//            product: '@',
//            producttype: '@'
//        },
//        require: 'ngModel',
//        link: function (scope, element, attrs, ngModelCtrl) {

//            configureElement = function () {

//                var rules = applicationConstants.validationRules
//                    .where(function (o) { return o.controlIdentifier == scope.identifier });

//                if (scope.status) rules = rules.where(function (o) { return o.status.any(function (x) { return x == scope.status }) })
//                if (scope.product) rules = rules.where(function (o) { return o.product.any(function (x) { return x == scope.product }) })
//                if (scope.producttype) rules = rules.where(function (o) { return o.producttype.any(function (x) { return x == scope.producttype }) })

//                var rule = {};
//                rules.forEach(function (o) { $.extend(rule, o); });

//                if (rule.disabled == true) {
//                    element.prop("disabled", true);
//                    element.data('js-disabled', true)
//                }
//                else if (element.data('js-disabled') == true) {
//                    element.prop("disabled", false);
//                    element.data('js-disabled', false);
//                }


//                if (rule.mandatory == true) {
//                    $(element).rules("add", { required: true, messages: { required: rule.mandatoryMsg } });
//                    element.data('js-mandatory', true);
//                }
//                else if (element.data('js-mandatory') == true) {
//                    $(element).rules("remove", "required");
//                    element.data('js-mandatory', false).removeClass('input-validation-error');
//                    angular.element('[data-valmsg-for={0}]'.format(attrs.name)).html('');
//                }


//                if (rule.min) {
//                    $(element).rules("add", { min: rule.min, messages: { min: rule.minMsg } });
//                    element.data('js-min', true);
//                }
//                else if (element.data('js-min') == true) {
//                    $(element).rules("remove", "min");
//                    element.data('js-min', false).removeClass('input-validation-error');
//                    angular.element('[data-valmsg-for={0}]'.format(attrs.name)).html('');
//                }
//            };

//            if (scope.status) scope.$watch("status", configureElement);
//            if (scope.product) scope.$watch("product", configureElement);
//            if (scope.producttype) scope.$watch("producttype", configureElement);
//        }
//    };
//});

//rootApplication.controller('indexController', function ($scope) {

//    $('#form').validate();

//    $scope.userModel = { Name: '', Premium: 1000.25, PolicyType: '', CoverageType: '' };
//    $scope.status = '1';
//    $scope.product = '1';
//    $scope.productSubtype = '1';
//    $scope.validate = function () { $('#form').valid(); };
//});