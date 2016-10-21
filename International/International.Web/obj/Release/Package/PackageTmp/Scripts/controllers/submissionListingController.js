rootApplication.controller('submissionListingController', function ($scope, utilities) {

    var colGrp;
    var finalFilters = {};
    var oTable;
    var expandedSubmissions = [];

    $scope.initSubmissionListing = function (val) {
        configureSubmissions(val);
        configureFilters(val);
    };

    $scope.applyFilters = function (clear) {
        if (clear == true) {
            finalFilters = {};
            $scope.filterVals = {};
        }
        else $.extend(finalFilters, $scope.filterVals);
        oTable.draw();
    };

     configureFilters = function (val) {       
            utilities.ajax({
                url: '/Submission/GetSubmissionFilters',
                method: 'GET',
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                //  disableControl: angular.element('button'),
                success: function (mvcResponse) {
                    if (val == 2) {
                        mvcResponse.FilterLists.CurrentStatus.removeAll(function (x) {
                            return x.Value == $scope.enums.CurrentStatusEnum.Endorsement + ''
                        });
                    }

                    $scope.filterList = mvcResponse.FilterLists;
                    $scope.filterVal = mvcResponse.Filters;
                }
            });
        
    };

    $scope.setFilters = function () {
       
        if ($scope.filterLists == undefined && $scope.filterVals == undefined) {
            $scope.filterLists = $scope.filterList;
            $scope.filterVals = $scope.filterVal;
        }
    }



    configureSubmissions = function (flag) {
   
        if (flag == 1)
            var url = "/Submission/Submissions";
        if (flag == 2)
            var url = "/Submission/QCSubmission";
        if (flag == 3)
            var url = "/Submission/QCAmendment";

        var formatDate = function (val) {
            return val == null ? '' : utilities.unixToDateString(val);
        };

        var formatTimeDate = function (val) {
            return val == null ? '' : utilities.unixToDateTimeString(val);
        };
        oTable = utilities.dataTable({
            element: angular.element('#clist'),
            scope: $scope,
            bServerSide: true,
            sServerMethod: "POST",
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            ajax: {
                url: url,
                type: "POST",
                data: function (aoData) {
                    aoData = $.dataTableMapperV1(aoData, finalFilters);
                }
            },
            columnGroup: [
                  { handler: 6, targets: [7, 8, 9, 10], isActive: false },
                  { handler: 12, targets: [13, 14, 15, 16, 17] },
                  { handler: 22, targets: [23, 24, 25] }
            ],

            rowCallback: function (node, data, index) {

                var childList = data["AmendmentList"];
                //=========================================
                //====== DO NOT REMOVE THIS (Sudeep) ======
                //=========================================
                if (childList != undefined) {
                    var row = oTable.row(node);
                    if (childList.length > 0) {

                        var cancellationFlag = false;

                        amandmentFlag = true;
                        angular.forEach(childList, function (value, key) {
                            if ("Cancellation" == value.StatusName) {
                                cancellationFlag = true;
                            }
                        });

                        var childHtml = '';
                        angular.forEach(childList, function (value, key) {
                            var EditProcess = $scope.enums.SubmissionProcess.EditSubmission;
                            var ViewProcess = $scope.enums.SubmissionProcess.ViewSubmission;

                            switch (value.StatusName) {
                                case 'Cancellation':
                                    {

                                        ViewProcess = $scope.enums.SubmissionProcess.ViewAmendment;
                                        break;
                                    }
                                case 'Re-Entry':
                                    {
                                        EditProcess = $scope.enums.SubmissionProcess.EditReEntry;

                                        ViewProcess = $scope.enums.SubmissionProcess.ViewReEntry;
                                        break;
                                    }
                                case 'Endorsement':
                                    {
                                        EditProcess = $scope.enums.SubmissionProcess.EditAmendment;
                                        ViewProcess = $scope.enums.SubmissionProcess.ViewAmendment;
                                        break;
                                    }
                                case 'Reversal':
                                    {

                                        ViewProcess = $scope.enums.SubmissionProcess.ViewReversal;
                                        break;
                                    }
                            }


                            childHtml += '<tr style="background-color:rgba(5,160,154,0.1); " class="ng-scope actions" role="row"><td> <a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}&IsView=True" rowid={0} class="eye-link"><i class="fa fa-eye"></i></a>'.format(value.SubmissionId, ViewProcess);
                            if (!(cancellationFlag || value.StatusName == 'Reversal'))
                                childHtml += '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}" rowid="{0}" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(value.SubmissionId, EditProcess) + '</td>';

                            childHtml += '<td> ' + value.SubmissionNumber + '</td> <td>' + value.NewRenewal + ' </td><td>' + formatDate(value.RenewalDate) + ' </td><td>' + value.Renewal + ' </td><td> ' + value.MasterPolicyNumber + '</td><td> ' + value.InsuredName + '</td><td class="handler-6" >' + value.Addressline1
                              + ' </td><td class="handler-6"> ' + value.CityName + '</td><td class="handler-6">' + value.StateName + ' </td><td class="handler-6"> ' + value.CountryName + '</td><td> ' + value.ProfitCenterUW + '</td><td> ' + value.ProductLineName + '</td><td class="handler-12">' + value.ProductLineSubTypeName + ' </td>'
                              + '<td class="handler-12">' + value.SectionCodeName + ' </td><td class="handler-12">' + value.ProfitCodeName + ' </td><td class="handler-12">' + value.PolicyType + ' </td><td class="handler-12">' + value.CoverageName + ' </td><td> ' + value.StatusName + '</td>'
                              + '<td>' + formatDate(value.EffectiveDate) + ' </td><td> ' + formatDate(value.ExpiryDate) + '</td><td> ' + formatDate(value.ProcessDate) + '</td><td> ' + value.BrokerName + '</td><td  class="handler-22"> ' + value.BCityName + '</td>'
                              + '<td class="handler-22"> ' + value.BStatename + '</td><td class="handler-22"> ' + value.BCountryName + '</td><td> ' + value.ProfitCenterOffice + '</td>'
                                + '<td>' + value.IssuingOffice + ' </td><td>' + value.GrossPremiumUSD + ' </td><td>' + value.TotalInsuredValueUSD + ' </td><td> ' + formatTimeDate(value.CreatedOnDate) + '</td><td>' + formatTimeDate(value.Date1) + '</td>'
                                 + '<td>' + value.QCStatusName + ' </td>'
                                + '</tr>';


                        })
                        row.child([
                          $(childHtml)

                        ]);

                        var childSpan = $(node).find("td:first span");

                        if (expandedSubmissions.indexOf(data.SubmissionNumber) > -1)
                            row.child.show();

                        $(childSpan).click(function () {

                            if (row.child.isShown()) {

                                row.child.hide();
                                $(childSpan).find("i").removeClass('fa-minus').addClass('fa-plus');
                                $(childSpan).find("i").attr("title", "Expand")
                                expandedSubmissions.pop(row.data().SubmissionNumber);
                            }
                            else {

                                row.child.show();
                                $('[class^="handler"]').hide();
                                $.each(ChildRowsColumnGroups, function (i, grp) {
                                    $(".handler-" + grp).show();
                                });

                                $(childSpan).find("i").removeClass('fa-plus').addClass('fa-minus');
                                $(childSpan).find("i").attr("title", "Collapse")
                                expandedSubmissions.push(row.data().SubmissionNumber);
                            }
                        });
                    }
                }
            },




            aoColumns: [
                {
                    "sTitle": "Actions", "bSortable": false, "sClass": 'actions', "sWidth": "90", "mRender": function (val, type, row) {
                        var cancellationFlag = false;
                        var amandmentFlag = false;
                        var qcApproval = false;
                        var html = "";

                        if ("Approved" == row.QCStatusName) {
                            qcApproval = true;
                        }
                        if (flag == 1) {
                            if (row.AmendmentList.length > 0) {
                                amandmentFlag = true;
                                for (var i = 0; i < row.AmendmentList.length; i++) {

                                    if ("Cancellation" == row.AmendmentList[i].StatusName) {
                                        cancellationFlag = true;
                                    }
                                }


                            }
                            html = html + '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}&IsView=True" rowid={0}  title="View Submission" class="eye-link"><i class="fa fa-eye"></i></a>'.format(row.SubmissionId, $scope.enums.SubmissionProcess.ViewSubmission)

                            if (!amandmentFlag && !cancellationFlag) {
                                html = html + '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}" rowid="{0}" title="EditSubmission" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(row.SubmissionId, $scope.enums.SubmissionProcess.EditSubmission);
                            }
                            if (row.StatusName == "Bound" && !cancellationFlag && qcApproval) {
                                var as = row.SubmissionId;
                                html = html + '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}" title="Amendment" rowid="{0}" class="amendment"><i class="fa fa-pencil-square" aria-hidden="true"></i></a>'.format(row.SubmissionId, $scope.enums.SubmissionProcess.CreateAmendment);
                                html = html + '<a ng-click="CreateReversal(\'{0}\')" title="Reversal"  class="reversal" href=""><i class="fa fa-exchange"></i></a>'.format(as);
                            }

                            if (amandmentFlag) {
                                html = html + '<span title="Expand"> <i class="fa fa-plus" aria-hidden="true"></i></span>';
                            }
                        }
                        else if (flag == 2) {
                            html = html + '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}&IsView=True"  title="Submission QC"  rowid={0} class="eye-link"> <i class="fa fa-check-square-o" aria-hidden="true"></i></a>'.format(row.SubmissionId, $scope.enums.SubmissionProcess.SubmissionQC);
                        }
                        else if (flag == 3) {
                            html = html + '<a href="/Submission/CreateSubmission?submissionId={0}&currentProcess={1}&IsView=True"  title="Amendment QC"  rowid={0} class="eye-link"> <i class="fa fa-check-square-o" aria-hidden="true"></i></a>'.format(row.SubmissionId, $scope.enums.SubmissionProcess.AmendmentQC);
                        }


                        return html;
                    }
                },
                { "mData": "SubmissionNumber", "sTitle": "Work item /submission no." },
                { "mData": "NewRenewal", "sTitle": "New/Renewal" },
                { "mData": "RenewalDate", "sTitle": "Date of Renewal", "mRender": formatDate },
                { "mData": "Renewal", "sTitle": "Renewal (Yes/No)" },
                { "mData": "MasterPolicyNumber", "sTitle": "Master Policy Number" },
                { "mData": "InsuredName", "sTitle": "Insured Name" },
                { "mData": "Addressline1", "sTitle": "Address Line 1" },
                { "mData": "CityName", "sTitle": "Insured City", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "StateName", "sTitle": "Insured State", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "CountryName", "sTitle": "Insured Country", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "ProfitCenterUW", "sTitle": "Profit Centre Underwriter", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "ProductLineName", "sTitle": "Product Line", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "ProductLineSubTypeName", "sTitle": "Product Line Subtype", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "SectionCodeName", "sTitle": "Section Code" },
                { "mData": "ProfitCodeName", "sTitle": "Profit Code" },
                { "mData": "PolicyType", "sTitle": "Policy Type" },
                { "mData": "CoverageName", "sTitle": "Coverage Code" },
                { "mData": "StatusName", "sTitle": "Current Status" },
                { "mData": "EffectiveDate", "sTitle": "Effective Date", "mRender": formatDate },
                { "mData": "ExpiryDate", "sTitle": "Expiry Date", "mRender": formatDate },
                { "mData": "ProcessDate", "sTitle": "Process Date", "mRender": formatDate },
                { "mData": "BrokerName", "sTitle": "Broker Name", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "BCityName", "sTitle": "Broker City", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "BStatename", "sTitle": "Broker State", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "BCountryName", "sTitle": "Broker Country", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                { "mData": "ProfitCenterOffice", "sTitle": "Profit Center Office" },
                { "mData": "IssuingOffice", "sTitle": "Issuing Office" },
                { "mData": "GrossPremiumUSD", "sTitle": "Gross Premium in USD" },
                { "mData": "TotalInsuredValueUSD", "sTitle": "Total Insured Value (TIV) in USD" },
                { "mData": "CreatedOnDate", "sTitle": "Date of Creation", "mRender": formatTimeDate },
                { "mData": "Date1",  "sTitle": "Date1" , "mRender": formatTimeDate   },
                { "mData": "LastModifedOnDate", "visible": false, "sTitle": "LastModifedOnDate" },
                { "mData": "QCStatusName", "sTitle": "QC Status" },
                 //{ "mData": "MarketSegment", "sTitle": "Market Segment" },
                 // { "mData": "Affiliations", "sTitle": "Affiliations" },
                 //    { "mData": "Offshore_Onshore", "sTitle": "Offshore/Onshore" },
                 //       { "mData": "Formtype", "sTitle": "Form Type" },
            ]
        });

        $scope.expandAll = oTable.expandAll;
        $scope.collapseAll = oTable.collapseAll;

    };


    //$scope.getExport = function () {

    //    utilities.ajax({
    //        url: '/Submission/Export',//.format($scope.brokerContactPersonId),
    //        method: 'GET',
    //        throbber: true,
    //        throbberPosition: { my: 'center center', at: 'center center', of: angular.element('p') },
    //        disableScreen: true,
    //        disableControl: angular.element('button'),
    //        success: function () {

    //        }
    //    });
    //}

    $scope.getRegionList = function () {
        utilities.ajax({
            url: '/Submission/GetRegionList',//.format($scope.brokerContactPersonId),
            method: 'GET',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            success: function (mvcResponse) {
                $scope.UserRegionModel = mvcResponse;
            }
        });
    };


    $scope.SetRegion = function () {
        if ($scope.UserRegionModel.RegionId != undefined)
            utilities.ajax({
                url: '/Submission/SetSubmissionRegion',
                method: 'POST',
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('select'),
                params: { RegionId: $scope.UserRegionModel.RegionId },
                success: function (mvcResponse) {
                    location.href = 'submissions';

                }
            });
    };

    $scope.CreateReversal = function (submissionId) {
        if (submissionId)
            utilities.ajax({
                url: '/Submission/CreateReversal',
                method: 'POST',
                throbber: true,
                throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
                disableScreen: true,
                disableControl: angular.element('select'),
                params: { submissionId: submissionId },
                success: function (mvcResponse) {
                    location.href = 'submissions';

                }
            });
    }
});