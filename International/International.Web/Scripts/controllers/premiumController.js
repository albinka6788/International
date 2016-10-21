
rootApplication.PremiumController = function ($scope, utilities, $compile, $timeout, $filter) {

       
    //Premium Details ...............

    $scope.GetPremiumDetail = function () {
        utilities.ajax({
            url: '/Submission/GetPremiumDetail',//.format($scope.brokerContactPersonId),
            method: 'GET',
            params: { submissionId: $scope.submissionId },
            success: function (mvcResponse) {
                $scope.PremiumDetailModel = mvcResponse;
            }
        });
    }


    $scope.PremiumDetailInit = function () {
        $.validator.unobtrusive.parse('#PremiumDetailForm');
        $('#PremiumDetailForm').validate();
    }

    $scope.SavePremiumDetail = function () {
        var form = angular.element('#PremiumDetailForm');
        if (!$(form).valid()) {          
            return false;
        }

        if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment) {
            utilities.ajax({
                url: '/Submission/compareEndorsePremium',//.format($scope.brokerContactPersonId),
                method: 'POST',
                data: { premiumModel: $scope.submissionModel.PremiumDetail, ParentSubmissionId: $scope.submissionId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    if (mvcResponse == false) {
                        bootbox.confirm({
                            title: 'Gross Premium Confirmation',
                            message: 'Do you want to proceed without making any changes in premium',
                            buttons: {
                                'cancel': {
                                    label: 'No',
                                    className: 'btn-default pull-right'
                                },
                                'confirm': {
                                    label: 'Yes',
                                    className: 'btn-danger pull-right'
                                }
                            },
                            callback: function (result) {
                                if (result) {
                                    $scope.incrementStep();
                                    $scope.$apply();
                                }
                            }
                        });

                    }
                    else $scope.incrementStep();
                }
            });
        }
        else {           
            $scope.incrementStep();
        }



    }

    function ConfirmPremium() {
     
        $scope.incrementStep();
        $scope.$apply();
    }

    $scope.CHKOriginalAttachmentPointChange = function () {
        $scope.submissionModel.PremiumDetail.OriginalAttachmentPoint = "0.00";
        $scope.CalcTransactionalAttachmentPoint();
    }

    $scope.ExchangeRateDateChange = function () {
        $("#ExchangeRateDate").valid();
    }

    $scope.SelectedCoverageIdPremium;
    $scope.IsAddLayerClick = false;
   

    //Premium Details Formulas....
    $scope.OnAddLayerClickPremium = function () {      
        $scope.IsAddLayerClick = true;
        var stepIndex = $scope.getCurrentStepIndex();
        $scope.steps.splice(stepIndex, 1);
        $scope.steps.splice(stepIndex, 0, "Premium Details LayerLevel");
        $scope.selection = $scope.steps[stepIndex];  
          
    }

    $scope.InitPremiumDetailCalc = function () {
        if ($scope.submissionModel.PremiumDetail.PremiumId != 0) {
            if ($scope.submissionModel.PremiumDetail.ExchangeRateDate != null)
                $scope.submissionModel.PremiumDetail.ExchangeRateDate = utilities.unixToDateString($scope.submissionModel.PremiumDetail.ExchangeRateDate);
            $scope.OnConversionRateToTransactionalChange();
            $scope.OnOriginalLayerLimitChange();
            $scope.OnTransactionalGrossPremiumChange();
        }

    }

    //events..
    $scope.OnConversionRateToTransactionalChange = function (TransactionalConversionRate) {
        $scope.CalcTransactionalLayerLimit();
        $scope.CalcTransactionalLimit();
        $scope.CalcTransactionalAttachmentPoint();

    }
    $scope.OnOriginalLayerLimitChange = function () {
        $scope.CalcOriginalLimit();
        $scope.CalcTransactionalLayerLimit();
    }
    $scope.OnTransactionalGrossPremiumChange = function () {
        $scope.CalcTransactionalPolicyCommission();
        $scope.CalcTransactionalNetPremium();
    }
    

    //Calc Functions for Transactional Currency...
    $scope.OriginalLimit = null;
    $scope.CalcOriginalLimit = function () {
        $scope.OriginalLimit = (($scope.submissionModel.PremiumDetail.OriginalLayerLimit * $scope.submissionModel.PremiumDetail.LayerPercent)/100);
        $scope.CalcTransactionalLimit();
    }

    $scope.TransactionalLayerLimit = null;
    $scope.CalcTransactionalLayerLimit = function () {
        $scope.TransactionalLayerLimit = ($scope.submissionModel.PremiumDetail.OriginalLayerLimit * $scope.submissionModel.PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.TransactionalLimit = null;
    $scope.CalcTransactionalLimit = function () {
        $scope.TransactionalLimit = ($scope.OriginalLimit * $scope.submissionModel.PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.TransactionalAttachmentPoint = null;
    $scope.CalcTransactionalAttachmentPoint = function () {
        $scope.TransactionalAttachmentPoint = ($scope.submissionModel.PremiumDetail.OriginalAttachmentPoint * $scope.submissionModel.PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.TransactionalPolicyCommission = null;
    $scope.CalcTransactionalPolicyCommission = function () {

        $scope.TransactionalPolicyCommission = (($scope.submissionModel.PremiumDetail.TransactionalGrossPremium * $scope.submissionModel.PremiumDetail.PolicyCommissionPercent)/100).toFixed(2);
        $scope.CalcTransactionalNetPremium();
    }

    $scope.TransactionalNetPremium = null;
    $scope.CalcTransactionalNetPremium = function () {
        $scope.TransactionalNetPremium = (((Number($scope.submissionModel.PremiumDetail.TransactionalGrossPremium) - Number($scope.TransactionalPolicyCommission)) + Number($scope.submissionModel.PremiumDetail.TransactionalCollections)) - Number($scope.submissionModel.PremiumDetail.TransactionalDeductions)).toFixed(2);
        $scope.CalcTransactionalNetPremiumFromBroker();
    }

    $scope.TransactionalNetPremiumFromBroker = null;
    $scope.CalcTransactionalNetPremiumFromBroker = function () {
        $scope.TransactionalNetPremiumFromBroker = (((Number($scope.TransactionalNetPremium) + Number($scope.submissionModel.PremiumDetail.TransactionalGSTIN)) - Number($scope.submissionModel.PremiumDetail.TransactionalGSTOUT))).toFixed(2);

    }

    $scope.setPremimumType = function () {
        if ($scope.submissionModel.PremiumDetail.PremiumType == $scope.enums.PremimumType.AdditionalPremium && $scope.submissionModel.PremiumDetail.TransactionalGrossPremium < 0
            || $scope.submissionModel.PremiumDetail.PremiumType == $scope.enums.PremimumType.ReturnPremium && $scope.submissionModel.PremiumDetail.TransactionalGrossPremium >= 0)
            $scope.submissionModel.PremiumDetail.TransactionalGrossPremium = $scope.submissionModel.PremiumDetail.TransactionalGrossPremium * -1;
        $scope.OnTransactionalGrossPremiumChange();
    };




}