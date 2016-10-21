var ChildRowsColumnGroups = [];

$(document).ready(function () {
    $.validator.setDefaults({ ignore: [] });
});

rootApplication.service('utilities', function ($http, $log, $document, $compile, $filter, applicationConstants) {

    var throbberImage = angular.element('#throbber-image-1');
    var throbberCurtain = angular.element('#throbber-curtain');

    this.ajax = function (options) {

        var self = this;
        var nullFunction = function () { };
        var params = angular.extend({
            success: nullFunction,
            error: nullFunction,
            complete: nullFunction,
            validate: false,
            throbber: false,
            disableScreen: false
        }, options);

        var isValid = true;
      
        if (params.validate === true) {

            var form = angular.element(params.form);
            if (form.length === 0) {

                $log.error('form to validate missing');
                isValid = false;
            }
            else isValid = form.valid();
        }

        var success = function (angularResponse) {
            params.success(angularResponse.data, angularResponse);
        };

        var error = function (angularResponse) {
            toastr.error(angularResponse.statusText)
            params.error(angularResponse);
        };

        var complete = function () {
            self.hideThrobber(params.disableControl);
            params.complete();
        };

   
        if (isValid === true) {
           
            if (params.throbber === true)
            { self.showThrobber(params.throbberPosition, params.disableScreen, params.disableControl); }

            $http(params).then(success, error).then(complete);
        }

    };

    this.showThrobber = function (position, disableScreen, disableControl) {

        if (disableScreen === true) this.disableScreen();
        if (disableControl != undefined) {

            var control = angular.element(disableControl);
            if (control.length > 0) control.prop("disabled", true);
        }

        throbberImage.show().position(position)
    };

    this.hideThrobber = function (disableControl) {

        if (disableControl != undefined) {

            var control = angular.element(disableControl);
            if (control.length > 0) control.prop("disabled", false);
        }

        throbberImage.hide();
        this.enableScreen();
    };

    this.disableScreen = function () {

        throbberCurtain.show()
            .position({ my: 'left top', at: 'left top', of: $document })
            .width($document.width())
            .height($document.height());
    };

    this.enableScreen = function () {
        throbberCurtain.hide();
    };

    this.dataTable = function (options) {

        var getTargetIndexes = function () {

            var targets = [];
            if (options.columnGroup)
                $(options.columnGroup).each(function () { targets = targets.concat(this.targets); });

            return targets;
        };

        var oTable = null;
        var params = angular.extend({
            "aLengthMenu": [10],
            "bLengthChange": false,
            "sPaginationType": "full_numbers",
            "iDisplayLength": 10,
            "oLanguage": { "sLengthMenu": "Show<span class='lenghtMenu'> _MENU_</span>Entries", "sInfo": "Showing _END_ of _TOTAL_" },
            "sDom": '<"top"i>r<"dt-wrapper"t><"bottom"lp><"clear">',
            "excelFilter": false,
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                $compile(nRow)(params.scope);
            },
            columnDefs: [{
                "targets": getTargetIndexes(),
                "visible": false,
                "className": "grp-body"
            }],
            initComplete: function (settings, json) {

                if (options.columnGroup) {

                    $.each(options.columnGroup, function (i, grp) {

                        var handler = $('<span>').addClass('expand-header').addClass('close').html('+');
                        $(oTable.column(grp.handler).header()).addClass('expand-space').append(handler);
                        handler.click(function (e) {

                            var self = $(this);
                            if (self.hasClass('close')) {

                                $(grp.targets).each(function () { oTable.column(this).visible(true); });
                                self.removeClass('close').addClass('open').html('-');
                                ChildRowsColumnGroups.push(grp.handler);
                            }
                            else {

                                $(grp.targets).each(function () { oTable.column(this).visible(false); });
                                self.removeClass('open').addClass('close').html('+');
                                var indx = $.inArray(grp.handler, ChildRowsColumnGroups);
                                ChildRowsColumnGroups.splice(indx, 1);
                            }
                            showChildRows();

                            //    oTable.draw();
                            e.preventDefault();
                            return false;
                        });
                    });
                }
            }
        }, options);

        params.aoColumns.forEach(function (o) {
            o.sKey = String.newIdentifier();
        });

        oTable = params.element.DataTable(params);

        if (params.excelFilter === true) {
            if (params.bServerSide === true) oTable.createAjaxExcelSearchFooter(params.sFilterSource);
            else oTable.createExcelSearchFooter();
        }

        if (options.columnGroup) {

            oTable.collapseAll = function () {

                $.each(options.columnGroup, function (i, grp) {

                    var self = $(oTable.column(grp.handler).header()).find('span.expand-header');
                    $(grp.targets).each(function () { oTable.column(this).visible(false); });
                    self.removeClass('open').addClass('close').html('+');
                    ChildRowsColumnGroups = [];
                });
                showChildRows();
            };

            oTable.expandAll = function () {
                ChildRowsColumnGroups = [];
                $.each(options.columnGroup, function (i, grp) {

                    var self = $(oTable.column(grp.handler).header()).find('span.expand-header');
                    $(grp.targets).each(function () { oTable.column(this).visible(true); });
                    self.removeClass('close').addClass('open').html('-');
                    ChildRowsColumnGroups.push(grp.handler);
                });
                showChildRows();
            };
        }

        return oTable;
    };

    showChildRows = function () {
        hideChildRows();
        $.each(ChildRowsColumnGroups, function (i, grp) {
            $(".handler-" + grp).show();
        });
    }

    hideChildRows = function (i) {
        $('[class^="handler"]').hide();

    }

    this.unixToDate = function (unixDateStr) {
        if (unixDateStr == null) return null;
        return unixDateStr.unixToDate();
    };

    this.unixToDateString = function (unixDateStr) {
        if (unixDateStr == null) return null;
        return unixDateStr.unixToDateString(applicationConstants.dateFormat);
    };

    this.unixToDateTimeString = function (unixDateStr) {
        if (unixDateStr == null) return null;
        return unixDateStr.unixToDateString(applicationConstants.dateTimeFormat);
    };

    this.getlistTitle = function (list, id) {
        var res = list.where(function (val) {
            return (val.Value == id)
        });
        if (res.length > 0) { return res[0].Text; }
        else return "";

    }

});
