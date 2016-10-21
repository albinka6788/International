
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();
});



rootApplication.InsuredController = function ($scope, utilities, $compile, $timeout, $filter) {




    $scope.SelectedInsured = 0;
    $scope.selectedAddress = 0;

    $scope.searchInsured = function () {
        if (!$scope.submissionModel.Insured || $scope.submissionModel.Insured.length < 3) {
            return false;
        }

        var insured = angular.element('#Insured');
        if ($(insured).prop('disabled')) return false;

        if ($scope.inProgress.Insured == false) {
            $scope.inProgress.Insured = true;
            $scope.selected = false;
            var serachType = ($scope.submissionModel.CurrentProcess == $scope.enums.SubmissionProcess.CreateAmendment || $scope.submissionModel.CurrentProcess == $scope.enums.SubmissionProcess.EditAmendment) ? $scope.enums.InsuredSearch.ChildInsured : $scope.enums.InsuredSearch.Insured;
            var insuredPartyId = ($scope.submissionModel.CurrentProcess == $scope.enums.SubmissionProcess.CreateAmendment || $scope.submissionModel.CurrentProcess == $scope.enums.SubmissionProcess.EditAmendment) ? $scope.submissionModel.InsuredId : null;
            utilities.ajax({
                url: '/Master/InsuredList',
                method: 'GET',
                params: { term: $scope.submissionModel.Insured, type: serachType, insuredPartyId: insuredPartyId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                success: function (mvcResponse) {
                    $scope.InsuredList = mvcResponse;
                    $scope.inProgress.Insured = false;
                    console.log(mvcResponse);
                    $('#search-insured').modal('show');
                }
            });

        }

    };

    $scope.getInsuredAddress = function (item, index) {

        $scope.InsuredAlias = item;

        $scope.selectedAddress = 0;

        $scope.InsuredList = $scope.InsuredList.splice(index, 1);

        utilities.ajax({
            url: '/Master/InsuredAddressList',
            method: 'GET',
            params: { insuredPartyId: item.PartyId },
            success: function (mvcResponse) {
                $scope.InsuredAddressList = mvcResponse;

            }
        });
    };

    $scope.selectInsuredItem = function (item, index) {
        $scope.selectedInsuredItem = item;
        $scope.IsSelectedInsured = true;
    }

    $scope.setInsured = function () {
   
        $scope.insuredSubmit = true;
        if (!$scope.IsSelectedInsured) { $scope.insuredErrorSubmit = true; return false; }
        var item = $scope.selectedInsuredItem;
        var selectedInsured = angular.element('#SelectedInsured');
        $.validator.unobtrusive.parse(selectedInsured);

        $(selectedInsured).valid();

        $scope.submissionModel.SelectedInsured = $scope.InsuredAlias.InsuredAliasName;
        $scope.InsuredDetail = item;
        $scope.submissionModel.Insured = $scope.InsuredAlias.InsuredAliasName;
        $scope.submissionModel.InsuredId = item.PartyId;
        $scope.submissionModel.InsuredAliasId = $scope.InsuredAlias.InsuredAliasId;

        $scope.submissionModel.InsuredAddressId = item.AddressId;

        $scope.submissionModel.DBAName =$scope.submissionModel.IsDifferentDBA?$scope.submissionModel.DBAName:$scope.InsuredAlias.InsuredAliasName;


        $timeout(function () {
            $('#DBAName').valid();
            $('#SelectedInsured').valid();
        }, 200);
       // $('#DBAName').val($scope.submissionModel.DBAName);
      
      
       
        $scope.insuredErrorSubmit = false;
        $scope.hidePopup();
    };

    $scope.checkInsured = function () {

        if (!$scope.submissionModel.Insured) {
            $scope.submissionModel.SelectedInsured = '';
            $scope.InsuredDetail = null;
            $scope.submissionModel.Insured = '';
            $scope.submissionModel.InsuredId = null;
            $scope.submissionModel.InsuredAddressId = null;
            $scope.submissionModel.DBAName = '';
        }
    }

    $scope.hidePopup = function () {
        $scope.InsuredList = null;
        $scope.InsuredAddressList = null;
        $scope.SelectedInsured = 0;
        $scope.selectedAddress = 0;
        $scope.IsSelectedInsured = false;
        delete $scope.SelectedInsured;
        delete $scope.selectedAddress;

        $scope.selectedAddress = null;
        $('#search-insured').modal('hide');
    };

    $scope.setDomicileStateList = function () {
        if ($scope.submissionModel.DomicileCountryId) {
            utilities.ajax({
                url: '/Master/stateByCountry',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { countryId: $scope.submissionModel.DomicileCountryId },
                success: function (mvcResponse) {
                    $scope.submissionListModel.DomacileStateList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.DomacileStateList = null;
    };



    $scope.saveInsured = function () {
        var form = angular.element('#InsuredForm');
        // $.validator.unobtrusive.parse(form);

     
        if (!$(form).valid()) {
        }
        else {
            $scope.incrementStep();
        }



    };

    $scope.InitInsuredPage = function () {
        var form = angular.element('#InsuredForm');
        $.validator.unobtrusive.parse(form);
    }


    $scope.setDba = function () {
        $scope.submissionModel.DBAName = $scope.submissionModel.IsDifferentDBA ? '' : $scope.submissionModel.Insured;

        $timeout(function () {
            $('#DBAName').valid();
        }, 200);
    }

    // Additional Insured

    $scope.AdditionalInsured = { InsuredName: '', id: 0, isNew: true, index: 0 };

    // $scope.AdditionalCedant = { InsuredCedantName: '', id: 0, isNew: true, index: 0 };

    $scope.addAdditionalInsured = function () {
        $scope.AdditionalInsured = { InsuredName: '', id: 0, isNew: true, index: 0 };
    }

    $scope.SaveAdditionalInsured = function (index) {
        var form = angular.element('#AdditionalInsured');
        $.validator.unobtrusive.parse(form);
        if (!$(form).valid()) {
            return false;
        }



        if ($scope.AdditionalInsured.isNew == true) {
            $scope.submissionModel.AdditionalInsureds.push({ InsuredName: $scope.AdditionalInsured.InsuredName, id: 0, isNew: false });
        }
        else {
            $scope.submissionModel.AdditionalInsureds[$scope.AdditionalInsured.index].InsuredName = $scope.AdditionalInsured.InsuredName;
        }

        $scope.AdditionalInsured = { InsuredName: '', id: 0, isNew: true };
        $('#add-insured').modal('hide');
    };

    $scope.getAdditionalInsured = function (index) {
        $scope.AdditionalInsured = $.extend(true, [], $scope.submissionModel.AdditionalInsureds[index]);// $scope.submissionModel.AdditionalInsureds[index];
        $scope.AdditionalInsured.index = index;
    };

    $scope.deleteAdditionalInsured = function (index) {
   
        $scope.submissionModel.AdditionalInsureds.splice(index, 1);
    };

    //End Additional Insured

    //Additional Cedant List

    $scope.AdditionalCedant1 = { InsuredCedantName: '', id: 0, DomicileCountryId: 0, AssumedEntityType: '', isNew: true, index: 0, DomicileStateId: 0 };


  

    $scope.addAdditionalCedant = function () {
        $scope.AdditionalCedant1.InsuredCedantName = '';
        $scope.AdditionalCedant1.DomicileCountryId = 0;
        $scope.AdditionalCedant1.AssumedEntityType = '';
        $scope.AdditionalCedant1.DomicileStateId = 0;
        $scope.submissionListModel.AdditionalCedantStateList = null;
        $scope.AdditionalCedant1.isNew = true;
    };




    $scope.SaveAdditionalCedanta = function () {
        var form = angular.element('#AdditionalCedant');
        $.validator.unobtrusive.parse(form);
        if (!$(form).valid()) {
            return false;
        }

        var index = $scope.AdditionalCedant1.index;
        if ($scope.AdditionalCedant1.isNew == true) {
            $scope.submissionModel.AdditionalCedants.push({
                CedantName: $scope.AdditionalCedant1.InsuredCedantName,
                id: 0,
                isNew: false,
                DomicileCountryId: $scope.AdditionalCedant1.DomicileCountryId,
                DomicileStateId: $scope.AdditionalCedant1.DomicileStateId,
                AssumedEntityType: $scope.AdditionalCedant1.AssumedEntityType,
                CountryName: $scope.submissionListModel.CountryList.getValue($scope.AdditionalCedant1.DomicileCountryId),
                Statename: $scope.submissionListModel.AdditionalCedantStateList != null ? $scope.submissionListModel.AdditionalCedantStateList.getValue($scope.AdditionalCedant1.DomicileStateId) : '',
                AssumedEntityTypeName: $scope.submissionListModel.AssumedEntityList.getValue($scope.AdditionalCedant1.AssumedEntityType)
            });
        }
        else {
            $scope.submissionModel.AdditionalCedants[index].CedantName = $scope.AdditionalCedant1.InsuredCedantName;
            $scope.submissionModel.AdditionalCedants[index].DomicileCountryId = $scope.AdditionalCedant1.DomicileCountryId;
            $scope.submissionModel.AdditionalCedants[index].AssumedEntityType = $scope.AdditionalCedant1.AssumedEntityType;
            $scope.submissionModel.AdditionalCedants[index].DomicileStateId = $scope.AdditionalCedant1.DomicileStateId;
            $scope.submissionModel.AdditionalCedants[index].CountryName = $scope.submissionListModel.CountryList.getValue($scope.AdditionalCedant1.DomicileCountryId);
            $scope.submissionModel.AdditionalCedants[index].Statename = $scope.submissionListModel.AdditionalCedantStateList != null ? $scope.submissionListModel.AdditionalCedantStateList.getValue($scope.AdditionalCedant1.DomicileStateId) : '';
            $scope.submissionModel.AdditionalCedants[index].AssumedEntityTypeName = $scope.submissionListModel.AssumedEntityList.getValue($scope.AdditionalCedant1.AssumedEntityType);
        }


        $scope.AdditionalCedant1.InsuredCedantName = '';
        $scope.AdditionalCedant1.DomicileCountryId = 0;
        $scope.AdditionalCedant1.AssumedEntityType = '';
        $scope.AdditionalCedant1.DomicileStateId = 0;
        $scope.submissionListModel.AdditionalCedantStateList = null;


        $('#add-cedant').modal('hide');

    };

    $scope.getAdditionalCedant = function (index) {
        $scope.AdditionalCedant1.InsuredCedantName = $scope.submissionModel.AdditionalCedants[index].CedantName;
        $scope.AdditionalCedant1.DomicileCountryId = $scope.submissionModel.AdditionalCedants[index].DomicileCountryId;
        $scope.AdditionalCedant1.AssumedEntityType = $scope.submissionModel.AdditionalCedants[index].AssumedEntityType;
        $scope.AdditionalCedant1.DomicileStateId = $scope.submissionModel.AdditionalCedants[index].DomicileStateId;
        if ($scope.AdditionalCedant1.DomicileCountryId) {
            $scope.setCedantDomicileStateList();
        }
        $scope.AdditionalCedant1.index = index;
        $scope.AdditionalCedant1.isNew = false;
    };

    $scope.deleteAdditionalCedant = function (index) {
        $scope.submissionModel.AdditionalCedants.splice(index, 1);
    };

    $scope.setCedantDomicileStateList = function () {
   
        var countryId = $scope.AdditionalCedant1.DomicileCountryId;
        if (countryId) {
            utilities.ajax({
                url: '/Master/stateByCountry',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { countryId: countryId },
                success: function (mvcResponse) {
                    $scope.submissionListModel.AdditionalCedantStateList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.AdditionalCedantStateList = null;
    };

    $scope.DomacileDisableRule = function () {
        var rs = false;
        var statusList = [
                           $scope.enums.CurrentStatusEnum.Blocked,
                           $scope.enums.CurrentStatusEnum.Blocked,
                           $scope.enums.CurrentStatusEnum.Cancellation,
                           $scope.enums.CurrentStatusEnum.Declined,
                           $scope.enums.CurrentStatusEnum.Indicated,
                           $scope.enums.CurrentStatusEnum.LostIndicated,
                           $scope.enums.CurrentStatusEnum.LostQuoted,
                           $scope.enums.CurrentStatusEnum.Quoted,
                           $scope.enums.CurrentStatusEnum.Reversal,
                           $scope.enums.CurrentStatusEnum.RenewalPending,
                           $scope.enums.CurrentStatusEnum.Working
        ];

        var mandatoryStatus = [
                                 Status.Bound,
                                 Status.Endorsement,                               
                                 Status.ReEntry
                             ];

        if (statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId }) || $scope.submissionModel.DirectAssumedTypeCode != $scope.enums.DirectAssumed.Assumed) {
        
            $scope.SetRequired(false, 'DomicileCountryId');
            $scope.SetRequired(false, 'DomicileStateId');
            rs = true;
        }
        if (mandatoryStatus.any(function (x) { return x == $scope.submissionModel.CurrentStatusId }) && $scope.submissionModel.DirectAssumedTypeCode == $scope.enums.DirectAssumed.Assumed)
        {
            $scope.SetRequired(true, 'DomicileCountryId', 'Please select Domicile Country');
            $scope.SetRequired(true, 'DomicileStateId', 'Please select Domicile State');
           // rs = false;
        }


        return rs;
    }



    // End Cedant List

    $scope.SetRequired = function(IsRequired, elementName, msg) {
        var elem = angular.element('#{0}'.format(elementName));
        if (!IsRequired) {
            $(elem).rules("remove", "required");
            elem.data('js-mandatory', false).removeClass('input-validation-error');
            angular.element('[data-valmsg-for={0}]'.format(elementName)).html('');
        }
        else {
            $(elem).rules("add", { required: true, messages: { required: msg } });
            elem.data('js-mandatory', true);
        }

    }
}