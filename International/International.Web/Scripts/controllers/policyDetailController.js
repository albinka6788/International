
rootApplication.PolicyDetailController = function ($scope, utilities, $compile, $timeout, $filter) {

    $scope.IsRenewable = false;
    $scope.OnRenewableChange = function(val) {
        $scope.IsRenewable = true;

    }
   
    $scope.ExistPolicyNumber = null;
    $scope.alredyExistPolicyNumber = false;
    $scope.checkPolicyNumberExist = function () {      
        if ($scope.submissionModel.NewRenewalTypeCode == $scope.enums.Newrenewal.New && $scope.submissionModel.PolicyDetail.PolicyNumber.length > 5) {
            if ($scope.submissionModel.PolicyDetail.PolicyNumber != null && ($scope.ExistPolicyNumber==null || $scope.ExistPolicyNumber!=$scope.submissionModel.PolicyDetail.PolicyNumber) ) {
                utilities.ajax({
                    url: '/Submission/checkPolicyNumberExist',
                    method: 'GET',
                    params: { PolicyNumber: $scope.submissionModel.PolicyDetail.PolicyNumber },
                    throbber: true,
                    throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                    disableScreen: true,                   
                    success: function (mvcResponse) {
                        if (mvcResponse == true) {
                            $scope.alredyExistPolicyNumber = true;
                        }
                        else
                            $scope.alredyExistPolicyNumber = false;
                    }
                });
            }
        }
    };

    $scope.setPolicySymbolList = function () {
        if ($scope.submissionModel.ProductLineTypeId) {
            utilities.ajax({
                url: '/Master/GetPolicySybolList',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { ProductLineTypeId: $scope.submissionModel.ProductLineTypeId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    $scope.submissionListModel.PolicySymbolList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.PolicySymbolList = null;
    };
        
    $scope.PolicyDetailInit = function () {
        if ($scope.submissionModel.CurrentStatusId == Status.Bound) {
            if ($scope.submissionModel.PolicyDetail.AdmittedTypeCode == null) {
                $scope.submissionModel.PolicyDetail.AdmittedTypeCode = 'AN_ADMT';
            }

            var productLinesubTypeList =
                [
                    ProductSubType.BuilderRisk,
                    ProductSubType.Construction 
                ]
            if (productLinesubTypeList.any(function(x){return x== $scope.submissionModel.ProductLineSubTypeId}) && !$scope.submissionModel.PolicyDetail.Renewable) {
                $scope.submissionModel.PolicyDetail.Renewable = "RENEW_N";
            }
        }

        $scope.buttonText_PD="Next";
        if ($scope.submissionModel.PolicyDetail.PolicyNumber != null) {
            $scope.ExistPolicyNumber = $scope.submissionModel.PolicyDetail.PolicyNumber;
        }

        if ($scope.currentprocess == $scope.enums.SubmissionProcess.SubmissionQC || $scope.currentprocess == $scope.enums.SubmissionProcess.AmendmentQC )
        { $scope.buttonText_PD = "Next"; }
        else if ($scope.currentprocess == $scope.enums.SubmissionProcess.ViewSubmission)
        {
            $scope.buttonText_PD = "";
        }
        else {
            $scope.buttonText_PD = "Submit";
        }

        $scope.AmendmentRenewalDate();
        
        console.log('Init Policy');
        $.validator.unobtrusive.parse('#PolicyDetailForm');
        $('#PolicyDetailForm').validate();             
    }

    $scope.OnRenewableChange = function () {
        
        if ($scope.submissionModel.PolicyDetail.Renewable == "RENEW_Y" && $scope.submissionModel.PolicyDetail.RenewalDate == null) {

            $scope.submissionModel.PolicyDetail.RenewalDate = $scope.submissionModel.ExpiryDate;
            if ($scope.submissionModel.CurrentStatusId == Status.Cancellation) $scope.submissionModel.PolicyDetail.RenewalDate = $scope.submissionModel.EffectiveDate;
            console.log($scope.submissionModel.CurrentStatusId + ' - ' + Status.Cancellation + ' - ' + $scope.submissionModel.EffectiveDate + ' - ' + $scope.submissionModel.ExpiryDate);

        }
        else if ($scope.submissionModel.PolicyDetail.Renewable != "RENEW_Y")
        { $scope.submissionModel.PolicyDetail.RenewalDate = null;}
    }
   
    $scope.AmendmentRenewalDate = function () {

        if ($scope.submissionModel.PolicyDetail.Renewable == "RENEW_Y" ) {

            $scope.submissionModel.PolicyDetail.RenewalDate = $scope.submissionModel.ExpiryDate;
            if ($scope.submissionModel.CurrentStatusId == Status.Cancellation) $scope.submissionModel.PolicyDetail.RenewalDate = $scope.submissionModel.EffectiveDate;
            console.log($scope.submissionModel.CurrentStatusId + ' - ' + Status.Cancellation + ' - ' + $scope.submissionModel.EffectiveDate + ' - ' + $scope.submissionModel.ExpiryDate);

        }
    }

    $scope.GetMasterPolicyNumber = function () {

        var CompanyPaperNumberTypeCode = ($scope.submissionModel.PolicyDetail.CompanyPaperNumberTypeCode == 'NA') ? 'NA' : $scope.submissionListModel.CompanyPaperNumberList.getValue($scope.submissionModel.PolicyDetail.CompanyPaperNumberTypeCode);
        var PolicySymbol = $scope.submissionModel.PolicyDetail.PolicySymbol;
        var PolicyNumber = $scope.submissionModel.PolicyDetail.PolicyNumber;
        var SuffixCode = $scope.submissionModel.PolicyDetail.SuffixCode;
        var MasterPolicyNumber = null;       
        if (CompanyPaperNumberTypeCode == null || CompanyPaperNumberTypeCode.length<1 || PolicySymbol == null || PolicyNumber == null || PolicyNumber.length<1 || SuffixCode == null)
        { MasterPolicyNumber = null; }
        else {
            MasterPolicyNumber =
                CompanyPaperNumberTypeCode
                + '-' + $scope.submissionListModel.PolicySymbolList.getValue(PolicySymbol)
                + '-' + PolicyNumber
                + '-' + $scope.submissionListModel.SuffixCodeList.getValue(SuffixCode);
        }
        $scope.submissionModel.PolicyDetail.MasterPolicyNumber = MasterPolicyNumber;
    }

    $scope.CHKPolicyNumberChange = function() {
        $scope.submissionModel.PolicyDetail.PolicyNumber = ($scope.submissionModel.PolicyDetail.IsPolicyNumber == true) ? 'NT/APP' : '';
        $timeout(function () {
            $('#PolicyNumber').valid();
        }, 200);
       
        $scope.GetMasterPolicyNumber();
    }
    $scope.CHKCompanyPaperNumberChange = function () {
        $scope.submissionModel.PolicyDetail.CompanyPaperNumberTypeCode = ($scope.submissionModel.PolicyDetail.IsCompanyPaperNumber == true) ? 'NA' : null;
        $scope.GetMasterPolicyNumber();
    }

    $scope.PolicyDetailSubmit = function () {
        var form = angular.element('#PolicyDetailForm');      
        if (!$(form).valid()) {
            return false;
        }
        else {
            if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment) { $scope.layerEndorse(); }
            $scope.SubmitSubmission();
        }
       
    }

    $scope.PD_SubmitOrNextClick = function () {
        var form = angular.element('#PolicyDetailForm');
        if (!$(form).valid()) {

            return false;
        }
        else {
            if ($scope.currentprocess == $scope.enums.SubmissionProcess.SubmissionQC || $scope.currentprocess == $scope.enums.SubmissionProcess.AmendmentQC)
            { $scope.incrementStep();}
            else {
            
             
                $scope.SubmitSubmission();
            }
        }
    }

}