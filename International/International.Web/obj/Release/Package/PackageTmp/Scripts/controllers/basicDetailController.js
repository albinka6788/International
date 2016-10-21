rootApplication.BasicDetailController = function ($scope, utilities, $compile, $timeout, $filter, applicationConstants) {
 
    // Basic Details Section

    

    $scope.InitBasicDetailPage = function () {       
        $.validator.unobtrusive.parse('#BasicForm');
        $('#BasicForm').validate();     
        $scope.Mindate = "Jan-01-2014";
        if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment || $scope.currentprocess == $scope.enums.SubmissionProcess.EditAmendment || $scope.currentprocess == $scope.enums.SubmissionProcess.EditReEntry)
        {
            $scope.Mindate = $scope.submissionModel.ParentEffectiveDate;
        }
        $scope.checkIssueUnderwriterRequire();
       
        if ($scope.submissionListModel.CoverageCodeList == null) {
            var options = {
                'id': 1,
                'label': 'Select'
            };
            $scope.submissionListModel.CoverageCodeList = [];            
            $scope.submissionListModel.CoverageCodeList.push(options);
            $scope.submissionModel.SelectedSubmissionCoverageList = [];           
        }
     
    }

    $scope.IsEffGreater = false;
    $scope.alredyExistSubmission = false;
    
    $scope.MultiselectDropdownSettings = {
        smartButtonMaxItems: 20,        
        smartButtonTextConverter: function (itemText, originalItem) {
            return itemText;
        }
    };


   
    $scope.CoverageNotSelected = false;
    $scope.SaveBasic = function () {
        $scope.CoverageNotSelected = false;        
        //if ($scope.CoverageCodeRequired())
        //{
        //    $scope.CoverageNotSelected = true;
        //    return false;
        //}
        var form = angular.element('#BasicForm');       
        if (!$(form).valid())
        {          
          return false;            
        }
        else {
            if (!$scope.alredyExistSubmission)  $scope.incrementStep();
        }       
    
    };

    $scope.CoverageCodeRequired = function () {

        var statusList = [
                             $scope.enums.CurrentStatusEnum.Cancellation,
                             $scope.enums.CurrentStatusEnum.Reversal,
                             $scope.enums.CurrentStatusEnum.Bound,
                             $scope.enums.CurrentStatusEnum.Endorsement,
                             $scope.enums.CurrentStatusEnum.ReEntry
        ];
        debugger;
        if (statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId }) && $scope.submissionModel.SelectedSubmissionCoverageList < 1 && $scope.currentprocess != enums.SubmissionProcess.ViewSubmission) {
            return true;
        }
        else {
            return false;
        }



    }

    $scope.checkIssueUnderwriterRequire = function () {

        if ($scope.submissionModel == null) return;

        var statusList = [
                        
                          $scope.enums.CurrentStatusEnum.Cancellation,
                          $scope.enums.CurrentStatusEnum.Reversal,
                          $scope.enums.CurrentStatusEnum.Bound,
                          $scope.enums.CurrentStatusEnum.Endorsement,
                          $scope.enums.CurrentStatusEnum.ReEntry
        ];

        if (statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId })) {
            console.log('call req');
            $scope.SetRequired(true, 'IssueUnderwriter', 'Please enter Issuing Underwriter');
        } else
        {
            $scope.SetRequired(false, 'IssueUnderwriter', '');
        }
    }

    $scope.checkSubmissionNumberExist = function () {
        if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateSubmission)
        {
            if ($scope.submissionModel.SixDigitNumber && $scope.submissionModel.SixDigitNumber.length > 5) {
                utilities.ajax({
                    url: '/Submission/checkSubmissionNumberExist',//.format($scope.brokerContactPersonId),
                    method: 'GET',
                    params: { sixDigitNo: $scope.submissionModel.SixDigitNumber },
                    success: function (mvcResponse) {
                        if (mvcResponse == true) {
                            $scope.alredyExistSubmission = true;
                         
                        }
                        else
                            $scope.alredyExistSubmission = false;
                    }
                });
            }
        }      
           
    };

    $scope.getPCUnderwriter = function (key, callback) {
        utilities.ajax({
            url: '/Master/GetPCUnderwriterList',//.format($scope.brokerContactPersonId),
            method: 'GET',
            params: { key: key.term },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            success: function (mvcResponse) {
                // console.log(mvcResponse);
                var data = $.map(mvcResponse, function (item) {
                    return $.extend({
                    }, {
                        label: item.UnderwriterName,
                        value: item.UnderwriterName,
                        realvalue: item.UnderwriterId,
                    }, item)
                });

                callback(data);
            }
        });
    };


    $scope.selectPCUnderWriter = function (result) {

        $scope.submissionModel.PCUnderwriter = result.item.UnderwriterName;
        $scope.submissionModel.SelectedPCUnderwriter = result.item.UnderwriterName;
        $scope.submissionModel.ProfitCenterUnderWriterId = result.item.UnderwriterId;
        $('#SelectedPCUnderwriter').val($scope.submissionModel.SelectedPCUnderwriter);
        $('#SelectedPCUnderwriter').valid();

      //  if ($scope.submissionModel.IssuingUnderWriterId == 0)
      //  {
        $scope.submissionModel.IssueUnderwriter = result.item.UnderwriterName;
        $scope.submissionModel.SelectedIssueUnderwriter = result.item.UnderwriterName;
        
        $scope.submissionModel.IssuingUnderWriterId = result.item.UnderwriterId;
     
        $scope.ClearBrokerDetails();
        $scope.setProductLine();
       // }

       // $scope.$apply();
    };

    $scope.checkPCUnderwriter = function()
    {
        if($scope.submissionModel.PCUnderwriter == "")
        {
            $scope.submissionModel.SelectedPCUnderwriter = "";
            $scope.submissionModel.ProfitCenterUnderWriterId = 0;
            $scope.ClearBrokerDetails();
            $scope.setProductLine();
        }
    }


    $scope.ClearBrokerDetails = function()
    {
        $scope.Broker = null;
        $scope.submissionModel.BrokerId = null;
        $scope.submissionModel.Broker = '';
        $scope.submissionModel.SelectedBroker = '';

        $scope.submissionModel.BrokerEntityId = null;
        $scope.submissionModel.BrokerEntity = '';
        $scope.submissionModel.SelectedBrokerEntity = '';

        $scope.BrokerContact = null;
        $scope.submissionModel.BrokerContactPersonId = null;
        $scope.submissionModel.SelectedBrokerContactPerson = '';
        $scope.submissionModel.BrokerContactPerson = '';
        
    }

    $scope.getUnderwriter = function (key, callback) {
        utilities.ajax({
            url: '/Master/GetUnderwriterList',//.format($scope.brokerContactPersonId),
            method: 'GET',
            params: { key: key.term },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            success: function (mvcResponse) {
                var data = $.map(mvcResponse, function (item) {
                    return $.extend({
                    }, {
                        label: item.UnderwriterName,
                        value: item.UnderwriterName,
                        realvalue: item.UnderwriterId,
                    }, item)
                });

                callback(data);
            }
        });
    };



    $scope.selectUnderWriter = function (result) {

        $scope.submissionModel.IssueUnderwriter = result.item.UnderwriterName;
        $scope.submissionModel.SelectedIssueUnderwriter = result.item.UnderwriterName;
        
        $scope.submissionModel.IssuingUnderWriterId = result.item.UnderwriterId;
    };



    $scope.setProductLine = function () {
        if ($scope.submissionModel.ProfitCenterUnderWriterId != 0) {
            utilities.ajax({
                url: '/Master/GetUWProductLine',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { UnderwriterId: $scope.submissionModel.ProfitCenterUnderWriterId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    if (mvcResponse.length > 0) $scope.submissionListModel.ProductLine = mvcResponse;
                    $scope.submissionListModel.ProductLineSubType = null;
                    $scope.submissionListModel.SectionCodeList = null;
                    $scope.submissionListModel.ProfitCodeList = null;
                    $scope.submissionListModel.PolicyTypeList = null;
                    $scope.submissionListModel.FormTypeList = null;
                    $scope.submissionModel.ProductLineTypeId = 0;
                    $scope.submissionModel.ProductLineSubTypeId = 0;
                    $scope.submissionModel.SectionCodeId = 0;
                    $scope.submissionModel.ProfitCodeId = "";
                    $scope.submissionModel.PolicyTypeID = 0;
                    if(mvcResponse.length == 1)
                    {
                        $scope.submissionModel.ProductLineTypeId = mvcResponse[0].Value;
                       
                        $scope.setProductSubLine();

                    }
                  
                }
            });

        } else {
            $scope.submissionListModel.ProductLine = null;
            $scope.submissionModel.PCUnderwriter = '';
            $scope.submissionListModel.ProductLineSubType = null;
            $scope.submissionListModel.SectionCodeList = null;
            $scope.submissionListModel.ProfitCodeList = null;
            $scope.submissionListModel.PolicyTypeList = null;
            $scope.submissionModel.ProductLineTypeId = 0;
            $scope.submissionModel.ProductLineSubTypeId = 0;
            $scope.submissionModel.SectionCodeId = 0;
            $scope.submissionModel.ProfitCodeId = "";
            $scope.submissionModel.PolicyTypeID = 0;
        } 

    };

    $scope.setProductSubLine = function () {
        $scope.submissionModel.Broker = '';
        $scope.ClearBrokerDetails();
        if ($scope.submissionModel.ProductLineTypeId) {

            utilities.ajax({
                url: '/Master/GetUWProductSubLine',
                method: 'GET',
                params: {
                            UnderwriterId: $scope.submissionModel.ProfitCenterUnderWriterId,
                            productLineId: $scope.submissionModel.ProductLineTypeId
                        },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    if (mvcResponse) {
                        $scope.submissionListModel.ProductLineSubType = mvcResponse.prductSubLine;
                        $scope.submissionListModel.FormTypeList = mvcResponse.formtype;
                    }
                    else $scope.submissionListModel.ProductLineSubType = null

                    $scope.submissionListModel.SectionCodeList = null;
                    $scope.submissionListModel.ProfitCodeList = null;
                    $scope.submissionListModel.PolicyTypeList = null;
                  
                    $scope.submissionModel.ProductLineSubTypeId = 0;
                    $scope.submissionModel.SectionCodeId = 0;
                    $scope.submissionModel.ProfitCodeId = "";
                    $scope.submissionModel.PolicyTypeID = 0;
                    $scope.setPolicyTypeList();
                }
            });
        }
        else {
            $scope.submissionListModel.ProductLineSubType = null;
            $scope.submissionListModel.SectionCodeList = null;
            $scope.submissionListModel.ProfitCodeList = null;
            $scope.submissionListModel.PolicyTypeList = null;
            $scope.submissionModel.ProductLineSubTypeId = 0;
            $scope.submissionModel.SectionCodeId = 0;
            $scope.submissionModel.ProfitCodeId = "";
            $scope.submissionModel.PolicyTypeID = 0;
        }
    };

    $scope.setSectionCodeList = function () {
        if ($scope.submissionModel.ProductLineSubTypeId) {
            $scope.ClearBrokerDetails();

            if ($scope.submissionModel.ProductLineTypeId != $scope.enums.Lob.Casualty && $scope.submissionModel.ProductLineTypeId != $scope.enums.Lob.Healthcare) {
                $scope.setPolicyTypeList();
            }


            utilities.ajax({
                url: '/Master/SectionCode',
                method: 'GET',
                params: { productLineSubTypeId: $scope.submissionModel.ProductLineSubTypeId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    $scope.submissionListModel.SectionCodeList = mvcResponse;

                   
                    $scope.submissionListModel.ProfitCodeList = null;
                   
                    $scope.submissionModel.SectionCodeId = 0;
                    $scope.submissionModel.ProfitCodeId = "";
                  
                }
            });
        } else {
            $scope.submissionListModel.SectionCodeList = null;
            $scope.submissionListModel.ProfitCodeList = null;
            $scope.submissionModel.SectionCodeId = 0;
            $scope.submissionModel.ProfitCodeId = "";
        }

    };

    $scope.setProfitCodeList = function () {
        if ($scope.submissionModel.SectionCodeId) {
            utilities.ajax({
                url: '/Master/ProfitCode',
                method: 'GET',
                params: { sectionCodeId: $scope.submissionModel.SectionCodeId },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    $scope.submissionListModel.ProfitCodeList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.ProfitCodeList = null;
       
    };

    $scope.setCoverageCodeList = function () {
       
        if ($scope.submissionModel.PolicyTypeID) {
            utilities.ajax({
                url: '/Master/CoverageCodeList',
                method: 'GET',
                params: { policyTypeId: $scope.submissionModel.PolicyTypeID },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    $scope.submissionListModel.CoverageCodeList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.CoverageCodeList = null;
        $scope.submissionModel.SelectedSubmissionCoverageList = [];       
    };


    $scope.setPolicyTypeList = function () {
        if (!$scope.submissionModel.AttachmentTypeCode) { $scope.submissionListModel.PolicyTypeList = null; return false; }
        var productLineSubTypeId = $scope.submissionModel.ProductLineSubTypeId;
        if ($scope.submissionModel.ProductLineTypeId == $scope.enums.Lob.Casualty || $scope.submissionModel.ProductLineTypeId == $scope.enums.Lob.Healthcare) {
          
            productLineSubTypeId = 0;

        }
        else if (!$scope.submissionModel.ProductLineSubTypeId) return false;


    
        utilities.ajax({
            url: '/Master/PolicyTypeList',
            method: 'GET',
            params: { attachmentType: $scope.submissionModel.AttachmentTypeCode, productLineId: $scope.submissionModel.ProductLineTypeId, productLineSubTypeId: productLineSubTypeId },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            success: function (mvcResponse) {
                $scope.submissionListModel.PolicyTypeList = mvcResponse;
            }
        });
    };


 


  
    //watches
    $scope.$watch('submissionModel.EffectiveDate', function (value) {
        if (value)
        {            
            if ($scope.submissionModel.CurrentStatusId == $scope.enums.CurrentStatusEnum.ReEntry) {
                    return;
            };
                      
            if ($scope.submissionModel.ExpiryDate == null)
                $scope.submissionModel.ExpiryDate = moment(value, applicationConstants.dateFormat).add(1, 'y').format('MMM-DD-YYYY');
           
            $('#ExpiryDate').val(moment(value, applicationConstants.dateFormat).add(1, 'y').format('MMM-DD-YYYY'));
            $('#EffectiveDate').val(value);
            if ($("#EffectiveDate").length > 0) $("#EffectiveDate").valid();
            if ($("#ExpiryDate").length > 0) $("#ExpiryDate").valid();       
        }               
    });


    $scope.renewalPolicyFormat = function () {

        if (angular.isUndefined($scope.submissionModel.RenewalofPolicyNumber)) {
             $scope.submissionModel.RenewalofPolicyNumber =  '';
        }
        if ($scope.submissionModel.RenewalofPolicyNumber.length >= 16)
            $scope.submissionModel.RenewalofPolicyNumber = $scope.submissionModel.RenewalofPolicyNumber.substring(0, 16);
         $scope.submissionModel.RenewalofPolicyNumber =  $scope.submissionModel.RenewalofPolicyNumber.replace(/([a-zA-Z0-9]{2})([a-zA-Z0-9]{3})([a-zA-Z0-9]{6})([a-zA-Z0-9]{2})/, '$1-$2-$3-$4');
    };

    $scope.NewRenewalChange = function () {
     
      
        if ($scope.submissionModel.NewRenewalTypeCode)
        {
            utilities.ajax({
                url: '/Master/CurrentStatusList',
                method: 'GET',
                params: {
                    currentStatusId: $scope.submissionModel.CurrentStatusId,
                    newRenewalCode: $scope.submissionModel.NewRenewalTypeCode,
                    currentProcess: $scope.currentprocess
                },
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    $scope.submissionListModel.StatusList = mvcResponse;
                    $scope.submissionModel.CurrentStatusId = 0;
                }
            });
        }
        else $scope.submissionListModel.StatusList = null;
      
    }

    $scope.LostDisableRule = function()
    {
        var statusList = [
                    $scope.enums.CurrentStatusEnum.Blocked,
                    $scope.enums.CurrentStatusEnum.Cancellation,
                    $scope.enums.CurrentStatusEnum.Declined,
                    $scope.enums.CurrentStatusEnum.Closed,
                    $scope.enums.CurrentStatusEnum.LostIndicated,
                    $scope.enums.CurrentStatusEnum.LostQuoted

        ];

        if(statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId }))
             return true;
        else return false;
    }

    $scope.BrokerDisableRule = function () {
        if (!$scope.submissionModel) return;
        var statusList = [
                       $scope.enums.CurrentStatusEnum.Blocked,
                       $scope.enums.CurrentStatusEnum.Cancellation,
                       $scope.enums.CurrentStatusEnum.Declined,
                      // $scope.enums.CurrentStatusEnum.Endorsement,
                       $scope.enums.CurrentStatusEnum.Closed

        ];
        //if ((statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId })) || ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment)) {
        if ((statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId }))) {
            return true;
        }
        return false;
    }

    $scope.UnderwriterDisableRule = function () {      
        if (!$scope.submissionModel) return;
        var statusList = [
                       $scope.enums.CurrentStatusEnum.Blocked,
                       $scope.enums.CurrentStatusEnum.Cancellation,
                       $scope.enums.CurrentStatusEnum.Declined,
                       $scope.enums.CurrentStatusEnum.Endorsement,
                       $scope.enums.CurrentStatusEnum.Closed
                   
        ];               
       if ((statusList.any(function (x) { return x == $scope.submissionModel.CurrentStatusId })) || ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment)) {       
            return true;
        }
        return false;
    }

    $scope.OnCurrentStatusChange = function () {       
        $scope.submissionModel.ReasonID = null;
        $scope.checkIssueUnderwriterRequire();
    }

    //function SetRequired(IsRequired, elementName, msg) {
    //    var elem = angular.element('#{0}'.format(elementName));
    //    if (!IsRequired) {
    //        $(elem).rules("remove", "required");
    //        elem.data('js-mandatory', false).removeClass('input-validation-error');
    //        angular.element('[data-valmsg-for={0}]'.format(elementName)).html('');
    //    }
    //    else {
    //        $(elem).rules("add", { required: true, messages: { required: msg } });
    //        elem.data('js-mandatory', true);
    //    }

    //}
    // End Basic Details

}