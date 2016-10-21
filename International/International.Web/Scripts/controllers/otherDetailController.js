
rootApplication.OtherDetailController = function ($scope, utilities, $compile, $timeout, $filter, $location) {

    $scope.DisableBy_Berk_SI_FROM_Broker = false;
    $scope.DisableBy_India_FROM_Berk_SI = false;

    $scope.OtherDetailsInit = function () {
        $.validator.unobtrusive.parse('#OtherDetailForm');
        $('#OtherDetailForm').validate();
    }

    $scope.SetIssuingOffice = function () {     
        $scope.submissionModel.IssuingOffice = $scope.submissionModel.ProfitCentreOffice;
        $timeout(function () {
            $('#IssuingOffice').valid();
        }, 200);
        //if ($scope.submissionModel.IssuingOffice)
        //    $scope.SetRequired(false, 'IssuingOffice');
        //else
        //    $scope.SetRequired(true, 'IssuingOffice', 'Please select Domicile Country');
    }

    $scope.CHKBy_Berk_SI_FROM_BrokerChange = function (val) {
        if (val == true) {
            $scope.submissionModel.By_Berk_SI_FROM_Broker = null;
            $scope.By_Berk_SI_FROM_Broker_GMT = null;
            $scope.DisableBy_Berk_SI_FROM_Broker = val;
        }
    }

    $scope.CHKBy_India_FROM_Berk_SIChange = function (val) {
        if (val == true) {
            $scope.submissionModel.By_India_FROM_Berk_SI = null;
            $scope.By_India_FROM_Berk_SI_GMT = null;
            $scope.DisableBy_India_FROM_Berk_SI = val;

        }

    }

    $scope.By_Berk_SI_FROM_Broker_GMT = null;
    $scope.OnBy_Berk_SI_FROM_BrokerChange = function () {
        var UtcDate = null;
        if ($scope.submissionModel.By_Berk_SI_FROM_Broker != null && $scope.submissionModel.By_Berk_SI_FROM_Broker != '') {
            UtcDate = moment.utc(moment($scope.submissionModel.By_Berk_SI_FROM_Broker, 'MMM-DD-YYYY HH:mm')).format("MMM-DD-YYYY HH:mm");
            $scope.By_Berk_SI_FROM_Broker_GMT = UtcDate; 
        }
        else {
            $scope.By_Berk_SI_FROM_Broker_GMT = UtcDate;
        }
    }


    $scope.By_India_FROM_Berk_SI_GMT = null;
    $scope.OnBy_India_FROM_Berk_SIChange = function () {
        if ($scope.submissionModel.By_India_FROM_Berk_SI != null && $scope.submissionModel.By_India_FROM_Berk_SI != '') {

            var UtcDate = moment.utc(moment($scope.submissionModel.By_India_FROM_Berk_SI, 'MMM-DD-YYYY HH:mm')).format("MMM-DD-YYYY HH:mm");// moment.utc(DateIST, 'MMM-DD-YYYY HH:mm').format("MMM-DD-YYYY") + moment.utc(DateIST, 'MMM-DD-YYYY HH:mm').format("HH:mm");

            $scope.By_India_FROM_Berk_SI_GMT = UtcDate;
        }
        else {
            $scope.By_India_FROM_Berk_SI_GMT = null;
        }
    }

    $scope.OD_SubmitOrNextClick = function () {
        var form = angular.element('#OtherDetailForm');
        if (!$(form).valid()) {

            return false;
        }
        else {
            if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateSubmission)
            { $scope.SubmitSubmission(); }
            else {
                $scope.incrementStep();
            }
        }
    }

    $scope.SubmitSubmission = function () {
        $scope.submissionModel.BrokerDetail = null;
        $scope.submissionModel.InsuredDetail = null;
        $scope.submissionModel.BrokerContact = null;
        console.log($scope.submissionModel.RenewalofPolicyNumber);
        utilities.ajax({
            url: '/Submission/SaveSubmission',
            method: 'POST',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            data: { submissionModel: $scope.submissionModel },
            success: function (mvcResponse) {
                if (mvcResponse == "success") {
                    window.location.href = '/Submission/Submissions';

                }
            }
        });
    }





}