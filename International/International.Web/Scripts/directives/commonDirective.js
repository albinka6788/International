var comDir = angular.module('comDirectiveApp', []);


comDir.directive('validationRules', function (applicationConstants) {
    return {
        transclusion: true,
        restrict: 'A',
        scope: {
            status: '@',
            identifier: '@',
            product: '@',
            producttype: '@',
            currentprocess: '@'
        },
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            configureElement = function () {
                var rules = applicationConstants.validationRules
                    .where(function (o) { return o.controlIdentifier == scope.identifier });

                var applicableRules = [];
                if (scope.status) rules.where(function (o) { return (o.status || []).any(function (x) { return x == scope.status }) }).forEach(function (o) { applicableRules.push(o); });
                if (scope.currentprocess) rules.where(function (o) { return (o.currentprocess || []).any(function (x) { return x == scope.currentprocess }) }).forEach(function (o) { applicableRules.push(o); });
                if (scope.product) rules.where(function (o) { return (o.product || []).any(function (x) { return x == scope.product }) }).forEach(function (o) { applicableRules.push(o); });
                if (scope.producttype) rules.where(function (o) { return (o.producttype || []).any(function (x) { return x == scope.producttype }) }).forEach(function (o) { applicableRules.push(o); });
                
             
              
                var rule = {};
                applicableRules.forEach(function (o) { $.extend(rule, o); });
                if (rule.disabled == true) {
                    element.prop("disabled", true);
                    element.data('js-disabled', true)
                }
                else if (element.data('js-disabled') == true) {
                    element.prop("disabled", false);
                    element.data('js-disabled', false);
                }
                
                if (rule.mandatory == true) {
                    $(element).rules("add", { required: true, messages: { required: rule.mandatoryMsg } });
                    element.data('js-mandatory', true);
                }
                else if (element.data('js-mandatory') == true) {

                    $(element).rules("remove", "required");
                    element.data('js-mandatory', false).removeClass('input-validation-error');
                    angular.element('[data-valmsg-for={0}]'.format(attrs.name)).html('');
                }

                if (rule.min) {
                    $(element).rules("add", { min: rule.min, messages: { min: rule.minMsg } });
                    element.data('js-min', true);
                }
                else if (element.data('js-min') == true) {
                    $(element).rules("remove", "min");
                    element.data('js-min', false).removeClass('input-validation-error');
                    angular.element('[data-valmsg-for={0}]'.format(attrs.name)).html('');
                }
            };

            if (scope.status) scope.$watch("status", configureElement);
            if (scope.product) scope.$watch("product", configureElement);
            if (scope.producttype) scope.$watch("producttype", configureElement);
        }
    };
});


comDir.directive('validDecimalNumber2', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }

                var clean = val.replace(/[^0-9\.]/g, '');
                var negativeCheck = clean.split('-');
                var decimalCheck = clean.split('.');
                if (!angular.isUndefined(negativeCheck[1])) {
                    negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                    clean = negativeCheck[0] + '-' + negativeCheck[1];
                    if (negativeCheck[0].length > 0) {
                        clean = negativeCheck[0];
                    }

                }

                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 2);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});




comDir.directive('validDecimalNumber5', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }

                var clean = val.replace(/[^0-9\.]/g, '');
                var negativeCheck = clean.split('-');
                var decimalCheck = clean.split('.');
                if (!angular.isUndefined(negativeCheck[1])) {
                    negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                    clean = negativeCheck[0] + '-' + negativeCheck[1];
                    if (negativeCheck[0].length > 0) {
                        clean = negativeCheck[0];
                    }

                }

                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 5);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });


            ngModelCtrl.$formatters.push(function (text) {
                //console.log('5 decimal - ' + text);

                if (text == '0' || text == null) return text;
                else return parseFloat(text).toFixed(5);
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            })
             .on('blur', function () {

                 if (element.val() == '' || parseFloat(element.val()) == 0.0 || element.val() == '-') {
                     //ngModelCtrl.$setViewValue('0.00');
                 }
                 else {
                     var fixedValue = parseFloat(element.val()).toFixed(5);
                     ngModelCtrl.$setViewValue(fixedValue);
                 }
                 ngModelCtrl.$render();
                 scope.$apply();
            });

        }
    };
});

comDir.directive('validDecimalNumber7', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }

                var clean = val.replace(/[^-+0-9\.]/g, '');
                var negativeCheck = clean.split('-');
                var decimalCheck = clean.split('.');
                if (!angular.isUndefined(negativeCheck[1])) {
                    negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                    clean = negativeCheck[0] + '-' + negativeCheck[1];
                    if (negativeCheck[0].length > 0) {
                        clean = negativeCheck[0];
                    }

                }

                if (!angular.isUndefined(decimalCheck[1])) {
                    decimalCheck[1] = decimalCheck[1].slice(0, 7);
                    clean = decimalCheck[0] + '.' + decimalCheck[1];
                }

                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});

comDir.directive('validNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            element.on('keydown', function (event) {

                var minus = navigator.userAgent.toUpperCase().indexOf('FIREFOX') > -1 ? 173 : 189;
                var keyCode = [8, 9, 17, 36, 35, 37, 39, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 86, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 109, 110, minus, 190];

                if ($.inArray(event.which, keyCode) == -1) event.preventDefault();
                else {
                    var oVal = ngModelCtrl.$modelValue || '';
                    if (oVal.length > 14 && $.inArray(event.which, [17, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 86, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 109, minus]) > -1) event.preventDefault();
                    else if ($.inArray(event.which, [109, 173]) > -1 && oVal.indexOf('-') > -1) event.preventDefault();
                    else if ($.inArray(event.which, [110, 190]) > -1 && oVal.indexOf('.') > -1) event.preventDefault();
                }
            })
            .on('blur', function () {

                if (element.val() == '' || parseFloat(element.val()) == 0.0 || element.val() == '-') {
                    ngModelCtrl.$setViewValue('');
                }
                else {
                    var fixedValue = parseFloat(element.val()).toFixed(2);
                    ngModelCtrl.$setViewValue(fixedValue);
                }

                ngModelCtrl.$render();
                scope.$apply();
            });

            ngModelCtrl.$parsers.push(function (text) {
                var oVal = ngModelCtrl.$modelValue;
                var nVal = ngModelCtrl.$viewValue;

                if (parseFloat(nVal) != nVal) {

                    if (nVal == null || nVal == undefined || nVal == '' || nVal == '-') oVal = nVal;

                    ngModelCtrl.$setViewValue(oVal);
                    ngModelCtrl.$render();
                    return oVal;
                }
                else {
                    var decimalCheck = nVal.split('.');
                    if (!angular.isUndefined(decimalCheck[1])) {
                        decimalCheck[1] = decimalCheck[1].slice(0, 2);
                        nVal = decimalCheck[0] + '.' + decimalCheck[1];
                    }

                    ngModelCtrl.$setViewValue(nVal);
                    ngModelCtrl.$render();
                    return nVal;
                }
            });

            ngModelCtrl.$formatters.push(function (text) {
                if (text == '0' || text == null) return parseFloat(0).toFixed(2);
                else return parseFloat(text).toFixed(2);
            });
        }
    };
});


comDir.directive('autoComplete', function () {
    return {
        restrict: "A",
        scope: {
            indexNo: '@',
            getdata: '&',
            selectaction: '&',
            minLength: '@',
            isautofocus: '@',
            width: '@',
        },
        link: function (scope, elem, attr) {
            var index = scope.$index;
            elem.autocomplete({
                source: function (key, response) {
                    var searchKey = {
                        term: key.term, index: scope.indexNo
                    };

                    scope.getdata({
                        key: searchKey, callback: function (result) {
                            if (result.length > 0) {
                                response($.map(result, function (item) {
                                            return $.extend({},
                                                {
                                        label: item.Key,
                                        value: item.Key,
                                        realvalue: item.Value,
                                        index: scope.indexNo
                                    }, item)
                                })
                       )
                            }
                            else {
                                response($.map(result, function (item) {
                                    return $.extend({},
                                        {
                                            label: '',
                                            value: '',
                                            realvalue: '',
                                        }, item)
                                })
                                        )
                            }
                            elem.removeClass('ui-autocomplete-loading');
                        }
                    });
                },
                minLength: scope.minLength,
                open: function (event, ui) {
                    $('ul.ui-autocomplete').width(scope.width);
                },
                select: function (event, selectedItem) {
                    elem.removeClass('ui-autocomplete-loading');
                    scope.selectaction({
                        result: selectedItem
                    });
                }
            }).focus(function () {
                if (scope.isautofocus) {
                    if ($(this).autocomplete("widget").is(":visible")) {
                        return;
                    }
                    if ($(this).val() == 0) elem.autocomplete("search", "");
                }
            });
        }
    };
});



comDir.directive('convertNan', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {         
            ngModel.$formatters.push(function (val) {               
                if (isNaN(val)) { return ''; }
                else { return val; }
            });
        }
    };
});



comDir.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function (val) {
                return '' + val;
            });
        }
    };
});


comDir.directive('renewalPolicyFormat', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {        
            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    return val = '';
                }               
                val = val.replace(/([a-zA-Z0-9]{2})([a-zA-Z0-9]{3})([a-zA-Z0-9]{6})([a-zA-Z0-9]{2})/, '$1-$2-$3-$4');
                console.log(val);
                ngModelCtrl.$setViewValue(val);
                ngModelCtrl.$render();
            });
        }
    };
});




comDir.directive('allowPattern', [allowPatternDirective]);

function allowPatternDirective() {
    return {
        restrict: "A",
        compile: function (tElement, tAttrs) {
            return function (scope, element, attrs) {
                // I handle key events
                element.bind("keypress", function (event) {
                    if (event.keyCode == 37 || event.keyCode == 35) return;
                    var keyCode = event.which || event.keyCode; // I safely get the keyCode pressed from the event.
                    if (keyCode == 8 || keyCode == 0  || keyCode == 39 ) {
                        return;
                    }

                   

                    var keyCodeChar = String.fromCharCode(keyCode); // I determine the char from the keyCode.               

                    // If the keyCode char does not match the allowed Regex Pattern, then don't allow the input into the field.
                    if (!keyCodeChar.match(new RegExp(attrs.allowPattern, "i"))) {
                        event.preventDefault();
                        return false;
                    }

                });
            };
        }
    };
}

 

comDir.directive('someSpecialChar', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                if (inputValue == undefined)
                    return ''
                cleanInputValue = inputValue.replace(/[^A-Za-z0-9 #$%&()*+\-.\/:;<=>?@\[\]^_`{|}~\s]/gi, '');
                if (cleanInputValue != inputValue) {
                    modelCtrl.$setViewValue(cleanInputValue);
                    modelCtrl.$render();
                }
                return cleanInputValue;
            });
        }
    }
});



comDir.directive('multiselectDropdown', [function () {
    return function (scope, element, attributes) {

        element = $(element[0]); // Get the element as a jQuery element

        // Below setup the dropdown:

        element.multiselect({
            buttonClass: 'btn btn-small',
            buttonWidth: '200px',
            buttonContainer: '<div class="btn-group" />',
            maxHeight: 200,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            buttonText: function (options) {
                if (options.length == 0) {
                    return element.data()['placeholder'];// + ' <b class="caret"></b>';
                } else if (options.length > 1) {
                    return _.first(options).text
                    + ' + ' + (options.length - 1)
                    + ' more selected <b class="caret"></b>';
                } else {
                    return _.first(options).text
                    + ' <b class="caret"></b>';
                }
            },
            // Replicate the native functionality on the elements so
            // that angular can handle the changes for us.
            onChange: function (optionElement, checked) {
                optionElement.removeAttr('selected');
                if (checked) {
                    optionElement.attr('selected', 'selected');
                }
                element.change();
            }

        });
        // Watch for any changes to the length of our select element
        scope.$watch(function () {
            return element[0].length;
        }, function () {
            element.multiselect('rebuild');
        });

        // Watch for any changes from outside the directive and refresh
        scope.$watch(attributes.ngModel, function () {
            element.multiselect('refresh');
        });

        // Below maybe some additional setup
    }
}]);



comDir.directive('dateTimePicker', function (utilities, applicationConstants) {   
    return {        
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
           
            var disableText = function (e) {
                
                var keyCode = e.keyCode || e.which
                if (keyCode == 8) {
                    $(this).val('').trigger('change');                    
                        e.preventDefault();                    
                }
                else return false;
            };
         
            var options = {
                dateFormat: "M-dd-yy",
                changeMonth: true,
                changeYear: true,                
                controlType: 'select',
                oneLine: true,
                maxDate: attrs.maxdate,
                minDate: attrs.mindate
            };

            $(element).prop('readonly', true).keydown(disableText);

            if (attrs.time == 'True') $(element).datetimepicker(options);
            else $(element).datepicker(options);
           
            attrs.$observe("maxdate", function (newVal) { if (newVal) $(element).datepicker("option", { maxDate: newVal }); });
            attrs.$observe("mindate", function (newVal) { if (newVal) $(element).datepicker("option", { minDate: newVal }); });
            $(element).next('span.input-group-addon').click(function () {
                if (element[0].disabled == false) {                   
                    $(element).datepicker('show');
                }
            });

        }
    }
});


comDir.directive('extraspace', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            var isValid = true;
            element.on('blur', function () {
                console.log('fianl val - ' + element.val().replace(/\s+/g, ' ').trim());
                ngModelCtrl.$setViewValue(element.val().replace(/\s+/g, ' ').trim());
                ngModelCtrl.$render();
                scope.$apply();
            });

            ngModelCtrl.$parsers.push(function (text) {              
                var nVal = (ngModelCtrl.$viewValue).replace(/\s+/g, ' ').trim();
                ngModelCtrl.$setViewValue(nVal);
                ngModelCtrl.$render();
                return nVal;
            });

        }
    };
});

comDir.directive('tooltip', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).hover(function () {
                // on mouseenter
                $(element).tooltip('show');
            }, function () {
                // on mouseleave
                $(element).tooltip('hide');
            });
        }
    };
});


comDir.directive('bootstrapTooltip', function () {
    return function (scope, element, attrs) {
        attrs.$observe('title', function (title) {
            // Destroy any existing tooltips (otherwise new ones won't get initialized)
            element.tooltip('destroy');
            // Only initialize the tooltip if there's text (prevents empty tooltips)
            if (jQuery.trim(title)) element.tooltip();
})
        element.on('$destroy', function () {
            element.tooltip('destroy');
            delete attrs.$$observers['title'];
        });
    }
});