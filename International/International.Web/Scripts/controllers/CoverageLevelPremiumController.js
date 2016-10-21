
rootApplication.CoverageLevelPremiumController = function ($scope, utilities, $compile, $timeout, $filter) {



    //Premium Detail Coverage Level..........
    $scope.PremiumDetailCoverageLevelInit = function () {
        $.validator.unobtrusive.parse('#PremiumDetailCLForm');
        $('#PremiumDetailCLForm').validate();
        if ($scope.IsAddLayerClick == false) {
            angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                if (layer.Selected == true)
                    layer.Selected = false;
                angular.forEach(layer.CoverageDetails, function (coverage) {
                    if (coverage.Selected == true)
                        coverage.Selected = false;

                    coverage.PremiumDetail.ValidExchangeRateDate = true;
                })
            })

            $scope.submissionModel.LayerDetails[0].Selected = true;
            $scope.submissionModel.LayerDetails[0].CoverageDetails[0].Selected = true;

        }
        else {
            $scope.OnAddLayerClick();
            //$scope.IsAddLayerClick = false;        
            //$scope.AddLayerAndCoverage($scope.SelectedCoverageIdPremium);
        }


        angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
            angular.forEach(layer.CoverageDetails, function (coverage, ckey) {
                coverage.OriginalLimitCL = 0;
                coverage.TransactionalLayerLimitCL = 0;
                coverage.TransactionalLimitCL = 0;
                coverage.TransactionalAttachmentPointCL = 0;
                coverage.TransactionalPolicyCommissionCL = 0;
                coverage.TransactionalNetPremiumCL = 0;
                coverage.TransactionalNetPremiumFromBrokerCL = 0;
                coverage.uniqueControlKey = key + "_" + ckey;
                if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment) {
                    coverage.IsEndorse = !coverage.IsEndorse ? false : coverage.IsEndorse;
                    coverage.PremiumDetail.PremiumType = coverage.PremiumDetail.TransactionalGrossPremium >= 0 ? $scope.enums.PremimumType.AdditionalPremium : $scope.enums.PremimumType.ReturnPremium;
                    console.log('PremiumType - ' + coverage.PremiumDetail.PremiumType + ' - gross - ' + coverage.PremiumDetail.TransactionalGrossPremium);
                }
                else
                    coverage.IsEndorse = true;
            })
        })

        $scope.InitCoverageLevelPremiumDetailCalc();
    }


    $scope.OnNext_PremiumDetailCoverageLevel = function () {
        var form = angular.element('#PremiumDetailCLForm');
        $scope.ExchangeRateDateValid();
       
        if (!$(form).valid()) {
            console.log('Premium Detail bug');
            return false;
        }
        else {
            if (!$scope.IsExchangeDateValid) return false;
            var GrossPremiumInUSDCoverageLevel = $scope.CompareGrossPremiumInUSD();
            var GrossPremiumInUSDPreBound = $scope.submissionModel.PremiumDetail.TransactionalGrossPremium * $scope.submissionModel.PremiumDetail.USDExchangeRate;
            if (GrossPremiumInUSDPreBound > 0 && (GrossPremiumInUSDCoverageLevel != GrossPremiumInUSDPreBound) && $scope.IsLayerExistInSubmission == false) {
                var res = false;
                bootbox.confirm({
                    title: 'Gross Premium Confirmation',
                    message: "Gross premium in USD is changed from " + GrossPremiumInUSDPreBound + " to " + GrossPremiumInUSDCoverageLevel + ", where " + GrossPremiumInUSDPreBound + " is pre bound Gross premium (in USD) and " + GrossPremiumInUSDCoverageLevel + " is the Final Gross premium (in USD). Do you want to proceed ?",
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
                            var stepIndex = $scope.getCurrentStepIndex();
                            var nextStep = stepIndex + 1;
                            console.log(nextStep);
                            console.log($scope.steps);
                        }
                    }
                });

            }
            else {

                $scope.incrementStep();
            }
        }

    }

    $scope.layerEndorse = function () {
        if ($scope.currentprocess == $scope.enums.SubmissionProcess.CreateAmendment) {
            angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                layer.CoverageDetails.removeAll(function (x) {
                    return x.IsEndorse == false
                });
            });

            $scope.submissionModel.LayerDetails.removeAll(function (x) {
                return x.CoverageDetails.length < 1
            });
        }
    }

    $scope.CompareGrossPremiumInUSD = function () {
        var TotalGrossPremiumUSD = 0;
        angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
            angular.forEach(layer.CoverageDetails, function (coverage, ckey) {
                if ((coverage.PremiumDetail.TransactionalGrossPremium * coverage.PremiumDetail.USDExchangeRate) > 0) {
                    TotalGrossPremiumUSD += coverage.PremiumDetail.TransactionalGrossPremium * coverage.PremiumDetail.USDExchangeRate;
                }
            })
        })
        return TotalGrossPremiumUSD;
    }

    $scope.CheckExchangeRate = function () {
        angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
            if (layer.Selected == true)
                LayerIndex = key;
            angular.forEach(layer.CoverageDetails, function (coverage,key) {


            })
        })

    }
   
    $scope.IsAddLayer = false;
    $scope.OnAddLayerClick = function () {
        $scope.ExchangeRateDateValid();
        var form = angular.element('#PremiumDetailCLForm');
        if (!$(form).valid()) {
            return false;
        }
        if (!$scope.IsExchangeDateValid) return false;
        $scope.IsCoverageIdExists = false;
        $("#SelectedCoverageM").prop("selectedIndex", 0);
        $scope.IsAddLayer = true;
        $('#add-coverageModel').modal({
            show: 'true'
        });
    }


    $scope.OnAddCoverageClick = function () {
        $scope.ExchangeRateDateValid();
        var form = angular.element('#PremiumDetailCLForm');
        if (!$(form).valid()) {
            return false;
        }
        if (!$scope.IsExchangeDateValid) return false;
        $scope.IsCoverageIdExists = false;
        $("#SelectedCoverageM").prop("selectedIndex", 0);
        $scope.IsAddLayer = false;
        $('#add-coverageModel').modal({
            show: 'true'
        });
    }

    $scope.CancelAddCoverage = function () {
        $('#add-coverageModel').modal('hide');
        $timeout(function () {
            $scope.switchPremiumPage();
        }, 500);
     

    }

    $scope.switchPremiumPage = function () {
        if ($scope.submissionModel.LayerDetails.length < 1) {
            $scope.steps.splice(6, 1);
            $scope.steps.splice(6, 0, "Premium Details");
            $scope.selection = $scope.steps[6];
        }
    }

    $scope.ChkIsCoverageExist = function (SelectedCoverageId) {
        $scope.IsCoverageIdExists = false;
        if ($scope.IsAddLayer == false) {
            var LayerIndex = 0;
            if (SelectedCoverageId > 0) {
                angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                    if (layer.Selected == true)
                        LayerIndex = key;
                })
            }
            angular.forEach($scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails, function (coverage) {
                if (coverage.CoverageId == SelectedCoverageId) {
                    $scope.IsCoverageIdExists = true;
                }
            })
        }
    }

    $scope.CancelAddCoverage

    $scope.IsCoverageIdExists = false;
    $scope.AddLayerAndCoverage = function (SelectedCoverageId) {
        $scope.IsCoverageIdExists = false;
        var LayerIndex = 0;
        if ($scope.IsAddLayer == true) {
            if (SelectedCoverageId > 0) {
                angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                    if (layer.Selected == true)
                        layer.Selected = false;
                    angular.forEach(layer.CoverageDetails, function (coverage) {
                        if (coverage.Selected == true)
                            coverage.Selected = false;
                    })
                })

                LayerIndex = $scope.submissionModel.LayerDetails.length;
                var newLayer = { LayerId: 0, SubmissionId: 0, LayerName: "Layer " + LayerIndex + 1, CreatedOnDate: null, CreatedByUserId: null, LastModifedOnDate: null, LastModifiedByUserId: null, CoverageDetails: [] };
                $scope.submissionModel.LayerDetails.push(newLayer)
                $scope.AddCoverage(SelectedCoverageId, LayerIndex);
            }
        }
        else {

            if (SelectedCoverageId > 0) {
                angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
                    if (layer.Selected == true)
                        LayerIndex = key;
                    angular.forEach(layer.CoverageDetails, function (coverage) {
                        if (coverage.CoverageId == SelectedCoverageId) {
                            $scope.IsCoverageIdExists = true;
                        }
                        if (coverage.Selected == true)
                            coverage.Selected = false;
                    })
                })

                if ($scope.IsCoverageIdExists == false) {
                    $scope.AddCoverage(SelectedCoverageId, LayerIndex);
                }


            }

        }
    }

    $scope.DeleteCoverage = function (LayerIndex, CoverageIndex) {      
        var totalcoverage = $scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails.length;       
        if (totalcoverage > 1) {
            $scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails.splice(CoverageIndex, 1);
            $scope.isActivetabLayer(LayerIndex);
        }
        else {
            $scope.submissionModel.LayerDetails.splice(LayerIndex, 1);
            if ($scope.submissionModel.LayerDetails.length > 0)
                $scope.isActivetabLayer(0);
        }

        $scope.switchPremiumPage();
    }


    $scope.SelectedCoverageId = 0;
    $scope.AddCoverage = function (SelectedCoverageId, LayerIndex) {
        var d = new Date();
        var uDate = d.toUTCString();
        console.log(uDate);
        var PremiumDetail = {
            PremiumId: 0, ExchangeRateDate: null, LayerPercent: null, PolicyCommissionPercent: null, OriginalCurrencyCode: null
                             , OriginalLayerLimit: null, IsOriginalAttachmentPoint: null, OriginalAttachmentPoint: null, TransactionalCurrencyCode: null
                             , ConversionRateToTransactional: null, TransactionalSIR: null, TransactionalDeductible: null, TransactionalGrossPremium: null
                             , TransactionalCollections: null, TransactionalDeductions: null, TransactionalGSTIN: null, TransactionalGSTOUT: null, IsTransactionalTIV: null
                             , TransactionalTIV: null, JurCurrencyCode: null, JurExchangeRate: null, USDExchangeRate: null, ProcessIdentifier: null
                             , CreatedOnDate: uDate, CreatedByUserId: null, LastModifedOnDate: null, LastModifiedByUserId: null,ValidExchangeRateDate : true
        }
        var lastindex = $scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails.length;

        var isEndorse = false;
        if ($scope.currentprocess != $scope.enums.SubmissionProcess.CreateAmendment)
            isEndorse = true;

        var newCoverage = { CoverageDetailId: 0, LayerId: 0, CoverageId: SelectedCoverageId, PremiumId: 0, PremiumDetail: PremiumDetail, uniqueControlKey: LayerIndex + "_" + lastindex, Selected: true, IsEndorse: isEndorse };



        $scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails.push(newCoverage);
        console.log($scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails);
        $scope.submissionModel.LayerDetails[LayerIndex].Selected = true;
        $scope.submissionModel.LayerDetails[LayerIndex].CoverageDetails[lastindex].Selected = true;
        $scope.isActivetabCoverage(LayerIndex, lastindex - 1);

    }

    $scope.isActivetabLayer = function (id) {
        var totalCount = $scope.submissionModel.LayerDetails.length;
        for (var i = 0; i < totalCount; i++) {
            var CtotalCount = $scope.submissionModel.LayerDetails[i].length;
            for (var j = 0; j < CtotalCount; j++) {
                $("#coverageTab" + i + "_" + j).removeClass('in active');
                $("#coverageTab" + i + "_" + j).removeClass('active');
                $("#CliSelected" + i + "_" + j).removeClass('active');
                $("#CliSelected" + i + "_" + j).removeClass('in active');
            }
        }
        for (var i = 0; i < totalCount; i++) {
            $("#tab" + i).removeClass('in active');
            $("#tab" + i).removeClass('active');
            $("#liSelected" + i).removeClass('active');
            $("#liSelected" + i).removeClass('in active');
        }
        $("#tab" + id).addClass('tab-pane fade in active');
        $("#liSelected" + id).addClass('active');
        $("#coverageTab" + id + "_" + 0).addClass('tab-pane fade in active');
        $("#CliSelected" + id + "_" + 0).addClass('active');


        angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
            if (layer.Selected == true)
                $scope.submissionModel.LayerDetails[key].Selected = false;
            angular.forEach(layer.CoverageDetails, function (coverage) {
                if (coverage.Selected == true)
                    coverage.Selected = false;
            })
        })
        $scope.submissionModel.LayerDetails[id].Selected = true;
        $scope.submissionModel.LayerDetails[id].CoverageDetails[0].Selected = true;
        $scope.isActivetabCoverage(id, 0);
    }

    $scope.isActivetabCoverage = function (pid, id) {
        var totalCount = $scope.submissionModel.LayerDetails[pid].CoverageDetails.length;
        for (var i = 0; i < totalCount; i++) {
            $("#coverageTab" + pid + "_" + i).removeClass('in active');
            $("#coverageTab" + pid + "_" + i).removeClass('active');
            $("#CliSelected" + pid + "_" + i).removeClass('active');
            $("#CliSelected" + pid + "_" + i).removeClass('in active');
        }
        $("#coverageTab" + pid + "_" + id).addClass('tab-pane fade in active');
        $("#CliSelected" + pid + "_" + id).addClass('active');

    }





    $scope.InitCoverageLevelPremiumDetailCalc = function () {
        angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {
            angular.forEach(layer.CoverageDetails, function (coverage, ckey) {
                if (coverage.PremiumDetail.PremiumId != 0) {
                    //if (coverage.PremiumDetail.ExchangeRateDate != null)                                          
                    //coverage.PremiumDetail.ExchangeRateDate = utilities.unixToDateString(coverage.PremiumDetail.ExchangeRateDate);                   
                    $scope.OnConversionRateToTransactionalChangeCL(key, ckey);
                    $scope.OnOriginalLayerLimitChangeCL(key, ckey);
                    $scope.OnTransactionalGrossPremiumChangeCL(key, ckey);
                }

            })
        })

    }

    //events..
    $scope.OnConversionRateToTransactionalChangeCL = function (Pid, id) {
        $scope.CalcTransactionalLayerLimitCL(Pid, id);
        $scope.CalcTransactionalLimitCL(Pid, id);
        $scope.CalcTransactionalAttachmentPointCL(Pid, id);


    }
    $scope.OnOriginalLayerLimitChangeCL = function (Pid, id) {
        $scope.CalcOriginalLimitCL(Pid, id);
        $scope.CalcTransactionalLayerLimitCL(Pid, id);
    }
    $scope.OnTransactionalGrossPremiumChangeCL = function (Pid, id) {
        $scope.CalcTransactionalPolicyCommissionCL(Pid, id);
        $scope.CalcTransactionalNetPremiumCL(Pid, id);
    }


    //Calc Functions for Transactional Currency...
    $scope.CalcOriginalLimitCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].OriginalLimitCL = (($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.OriginalLayerLimit * $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.LayerPercent)/100);
        $scope.CalcTransactionalLimitCL(Pid, id);
    }

    $scope.CalcTransactionalLayerLimitCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalLayerLimitCL = ($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.OriginalLayerLimit * $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.CalcTransactionalLimitCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalLimitCL = ($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].OriginalLimitCL * $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.CalcTransactionalAttachmentPointCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalAttachmentPointCL = ($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.OriginalAttachmentPoint * $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.ConversionRateToTransactional).toFixed(2);
    }

    $scope.CalcTransactionalPolicyCommissionCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalPolicyCommissionCL = (($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium * $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.PolicyCommissionPercent) / 100).toFixed(2);
        $scope.CalcTransactionalNetPremiumCL(Pid, id);
       
    }

    $scope.CalcTransactionalNetPremiumCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalNetPremiumCL = (((Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium) - Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalPolicyCommissionCL)) + Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalCollections)) - Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalDeductions)).toFixed(2);
        $scope.CalcTransactionalNetPremiumFromBrokerCL(Pid, id);
    }

    $scope.CalcTransactionalNetPremiumFromBrokerCL = function (Pid, id) {
        $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalNetPremiumFromBrokerCL = ((Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalNetPremiumCL) + Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGSTIN)) - Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGSTOUT)).toFixed(2);

        console.log(Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].TransactionalNetPremiumCL))
        console.log(Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGSTIN))
        console.log(Number($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGSTOUT))
    }

    $scope.setLayerPremimumType = function (Pid, id) {
        if ($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium) {
            if ($scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.PremiumType == $scope.enums.PremimumType.AdditionalPremium
                && $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium < 0
                || $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.PremiumType == $scope.enums.PremimumType.ReturnPremium
                && $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium >= 0)
                $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium = $scope.submissionModel.LayerDetails[Pid].CoverageDetails[id].PremiumDetail.TransactionalGrossPremium * -1;
            $scope.OnTransactionalGrossPremiumChangeCL(Pid, id);
        }

    };

    $scope.ExchangeRateDateValid = function () {
        $scope.IsExchangeDateValid = true;

        if ($scope.submissionModel.LayerDetails.length > 0) {
            angular.forEach($scope.submissionModel.LayerDetails, function (layer, key) {

                angular.forEach(layer.CoverageDetails, function (coverage) {
                    if (!coverage.PremiumDetail.ExchangeRateDate)
                    {
                        coverage.PremiumDetail.ValidExchangeRateDate = false;
                        $scope.IsExchangeDateValid = false;
                    }
                    else
                        coverage.PremiumDetail.ValidExchangeRateDate = true;
                });
            });
        }


    };

}