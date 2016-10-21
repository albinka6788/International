rootApplication.controller('submissionController', function ($scope, utilities, $compile, $timeout, $filter, $location, applicationConstants) {

    rootApplication.BasicDetailController($scope, utilities, $compile, $timeout, $filter, applicationConstants);
    rootApplication.ProjectDetailController($scope, utilities, $compile, $timeout);  
    rootApplication.InsuredController($scope, utilities, $compile, $timeout, $filter, applicationConstants);
    rootApplication.BrokerController($scope, utilities, $compile, $timeout);
    rootApplication.PremiumController($scope, utilities, $compile, $timeout);
    rootApplication.PolicyDetailController($scope, utilities, $compile, $timeout);
    rootApplication.OtherDetailController($scope, utilities, $compile, $timeout);
    rootApplication.QCController($scope, utilities, $compile, $timeout);
    rootApplication.CoverageLevelPremiumController($scope, utilities, $compile, $timeout);

    $scope.inProgress = { Insured: false, Broker: false };

  
    //ng - switch code -----------------------*-----------------------------------
    $scope.steps = [        
          'Basic Details',  
          'Project Details',         
          'Insured Details',
          'Broker Details',
          'Other Details',
          'Status Dependent Detail',         
          'Policy Detail',
          'QC Remarks'
    ];

    $scope.selection = $scope.steps[-1];
    
    $scope.getCurrentStepIndex = function () {
        // Get the index of the current step given selection
        return _.indexOf($scope.steps, $scope.selection);
    };

    // Go to a defined step index
    $scope.goToStep = function (index) {
        if (!_.isUndefined($scope.steps[index])) {
            $scope.selection = $scope.steps[index];
        }
    };

    $scope.hasNextStep = function () {
        var stepIndex = $scope.getCurrentStepIndex();
        var nextStep = stepIndex + 1;        
        // Return true if there is a next step, false if not
        return !_.isUndefined($scope.steps[nextStep]);
    };

    $scope.hasPreviousStep = function () {
        var stepIndex = $scope.getCurrentStepIndex();
        var previousStep = stepIndex - 1;
        // Return true if there is a next step, false if not
        return !_.isUndefined($scope.steps[previousStep]);
    };

    $scope.incrementStep = function () {
        if ($scope.hasNextStep()) {
            var stepIndex = $scope.getCurrentStepIndex();
            var nextStep = stepIndex + 1;          
            $scope.selection = $scope.steps[nextStep];          
        }
    };

    $scope.decrementStep = function () {
        if ($scope.hasPreviousStep()) {
            var stepIndex = $scope.getCurrentStepIndex();
            var previousStep = stepIndex - 1;
            $scope.selection = $scope.steps[previousStep];
        }
    };

   

    //------------------------------------------*----------------------------------------

    $scope.StatusDepNext = "Next";  
   
    $scope.getSelectionList = function () {
        utilities.ajax({
            url: '/Submission/GetSelectionList',
            params: { submissionId: $scope.submissionId, currentProcess: $scope.currentprocess },
            method: 'GET',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('p') },
            disableScreen: true,
            disableControl: angular.element('button'),
            success: function (mvcResponse) {              
                $scope.submissionListModel = mvcResponse;               
                $timeout(function () {
                    $scope.selection = $scope.steps[0];
                }, 500);
               
            }
        });
    };

    $scope.submissionModel = null;  
    $scope.getSubmissionDetails = function () {        
        $scope.getSelectionList();
        utilities.ajax({
            url: '/Submission/GetSubmissionModel',
            method: 'GET',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('p') },
            disableScreen: true,
            disableControl: angular.element('button'),
            params: { submissionId: $scope.submissionId, currentProcess: $scope.currentprocess },
            success: function (mvcResponse) {              
                $scope.submissionModel = mvcResponse;            
                $scope.AddPremiumDetailsPage();               
                
                if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateSubmission) {
                    $scope.submissionModel.EffectiveDate = null;
                    $scope.submissionModel.ExpiryDate = null;
                    $scope.submissionModel.ProfitCodeId = "";
                    if ($scope.currentprocess == 0) {
                        $scope.StatusDepNext = "Submit";
                    }
                }
                else {
                    
                    $scope.submissionModel.EffectiveDate = utilities.unixToDateString($scope.submissionModel.EffectiveDate);                   
                    $scope.submissionModel.ExpiryDate = utilities.unixToDateString($scope.submissionModel.ExpiryDate);                   
                   
                    $scope.submissionModel.ParentEffectiveDate = utilities.unixToDateString($scope.submissionModel.ParentEffectiveDate);
                    $scope.Broker = $scope.submissionModel.BrokerDetail;
                    $scope.InsuredDetail = $scope.submissionModel.InsuredDetail;
                    $scope.BrokerContact = $scope.submissionModel.BrokerContact;                   
                    if ($scope.submissionModel.By_Berk_SI_FROM_Broker != null) {
                        console.log("Ts=" + $scope.submissionModel.By_Berk_SI_FROM_Broker)
                        $scope.submissionModel.By_Berk_SI_FROM_Broker = utilities.unixToDateTimeString($scope.submissionModel.By_Berk_SI_FROM_Broker);
                        console.log("F=" + $scope.submissionModel.By_Berk_SI_FROM_Broker)
                        $scope.OnBy_Berk_SI_FROM_BrokerChange();                       
                    }
                    if ($scope.submissionModel.By_India_FROM_Berk_SI != null) {
                        $scope.submissionModel.By_India_FROM_Berk_SI = utilities.unixToDateTimeString($scope.submissionModel.By_India_FROM_Berk_SI);
                        $scope.OnBy_India_FROM_Berk_SIChange();
                    }
                    if ($scope.submissionModel.ProcessDate != null) {
                        $scope.submissionModel.ProcessDate = utilities.unixToDateString($scope.submissionModel.ProcessDate);
                    }
                    if ($scope.submissionModel.PolicyDetail != null) {
                        $scope.submissionModel.PolicyDetail.BindDate = utilities.unixToDateString($scope.submissionModel.PolicyDetail.BindDate);
                        $scope.submissionModel.PolicyDetail.RenewalDate = utilities.unixToDateString($scope.submissionModel.PolicyDetail.RenewalDate);
                    }
                   
                    $scope.InitPremiumDetailCalc()
                    $scope.IsLayerExistInSubmission = false;
                    angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                        angular.forEach(layer.CoverageDetails, function (coverage, ckey) {
                            if (coverage.PremiumDetail.PremiumId != 0) {
                                $scope.IsLayerExistInSubmission = true;
                                if (coverage.PremiumDetail.ExchangeRateDate != null)
                                    coverage.PremiumDetail.ExchangeRateDate = utilities.unixToDateString(coverage.PremiumDetail.ExchangeRateDate, "MMM-DD-YYYY");
                            }

                        })
                    })

                  

                }
            }

        });
      
    };  

  

    $scope.AddPremiumDetailsPage = function () {
        if ($scope.submissionModel.LayerDetails.length > 0) {
            $scope.steps.splice(6, 0, "Premium Details LayerLevel");
        }
        else {
            $scope.steps.splice(6, 0, "Premium Details");
        }
    }

    //Status Dependent Details....
    $scope.StatusDepInit = function () {        
        $.validator.unobtrusive.parse('#StatusDependentDetailForm');
        $('#StatusDependentDetailForm').validate();
    }


    $scope.NextStatusDependent = function () {
        var form = angular.element('#StatusDependentDetailForm');
        if (!$(form).valid()) {
            return false;
        }
        else {           
            $scope.incrementStep();
          
        }
    }

    $scope.ProcessDateChange = function () {
        $("#ProcessDate").valid();
    }


    $scope.BackToList = function () {


        if ($scope.currentprocess == $scope.enums.SubmissionProcess.SubmissionQC)
            window.location.href = "QCSubmission"

        else if ($scope.currentprocess == $scope.enums.SubmissionProcess.AmendmentQC)
            window.location.href = "QCAmendment"
        else
            window.location.href = "Submissions"
    }


    $scope.getExport = function () {
       // alert('ho gya')
        //utilities.ajax({
        //    url: '/Submission/Export',//.format($scope.brokerContactPersonId),
        //    method: 'GET',
        //    throbber: true,
        //    throbberPosition: { my: 'center center', at: 'center center', of: angular.element('p') },
        //    disableScreen: true,
        //    disableControl: angular.element('button'),
        //    success: function () {
            
        //        alert('ho gya');

        //    }
        //});
    }

   

});


