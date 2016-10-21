
rootApplication.QCController = function ($scope, utilities, $compile, $timeout, $filter, $location) {


    //$scope.QC_SubmitOrNextClick = function () {
    //    var form = angular.element('#OtherDetailForm');
    //    if (!$(form).valid()) {

    //        return false;
    //    }
    //    else {
    //        if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateSubmission)
    //        { $scope.SubmitSubmission(); }
    //        else {
    //            $scope.incrementStep();
    //        }
    //    }
    //}

    $scope.UpdateQCStatus = function () {
        utilities.ajax({
            url: '/Submission/UpdateQCStatus',
            method: 'POST',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
          //  data: { submissionModel: $scope.submissionModel },
            data: { SubmissionId: $scope.submissionModel.SubmissionId, CurrentStatusId: $scope.submissionModel.CurrentStatusId, QCStatusId: $scope.submissionModel.QCStatusId, QCRemark: $scope.submissionModel.QCRemark },
            success: function (mvcResponse) {
                if (mvcResponse == "success") {
                    if ($scope.currentprocess == $scope.enums.SubmissionProcess.SubmissionQC)
                        window.location.href = "QCSubmission"

                    else if ($scope.currentprocess == $scope.enums.SubmissionProcess.AmendmentQC)
                        window.location.href = "QCAmendment"
                    else
                        window.location.href = "Submissions"

                   // window.location.href = '/Submission/Submissions';

                }
            }
        });
    }





}