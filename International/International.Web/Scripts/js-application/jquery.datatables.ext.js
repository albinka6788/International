
//$.fn.dataTableExt.aTypes.push(
//    function (dateStr) {

//        var date = dateStr.split('-')[0];
//        var month = dateStr.split('-')[1];
//        var year = dateStr.split('-')[2];

//        if (isNaN(date)) return null;
//        if (parseInt(date) < 1 || parseInt(date) > 31) return null;

//        if (isNaN(year)) return null;
//        if (parseInt(year) < 0 || parseInt(year) > 99) return null;

//        if ((['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']).indexOf(month) < 0) return null;

//        return 'custom-date';
//    }
//);

$.fn.dataTableExt.oSort['custom-date-asc'] = function (x, y) {
    if (x.length == 8) x = ('0' + x);
    if (y.length == 8) y = ('0' + y);

    var x = x.substring(7, 9) + getMonIndex(x.substring(3, 6)) + x.substring(0, 2);
    var y = y.substring(7, 9) + getMonIndex(y.substring(3, 6)) + y.substring(0, 2);

    return ((x < y) ? -1 : ((x > y) ? 1 : 0));
};

$.fn.dataTableExt.oSort['custom-date-desc'] = function (x, y) {
    if (x.length == 8) x = ('0' + x);
    if (y.length == 8) y = ('0' + y);

    var x = x.substring(7, 9) + getMonIndex(x.substring(3, 6)) + x.substring(0, 2);
    var y = y.substring(7, 9) + getMonIndex(y.substring(3, 6)) + y.substring(0, 2);

    return ((x < y) ? 1 : ((x > y) ? -1 : 0));
};

$.fn.dataTableExt.oApi.fnResetAllFilters = function (oSettings, bDraw) {
    for (iCol = 0; iCol < oSettings.aoPreSearchCols.length; iCol++) {
        oSettings.aoPreSearchCols[iCol].sSearch = '';
    }
    oSettings.oPreviousSearch.sSearch = '';

    if (typeof bDraw === 'undefined') bDraw = true;
    if (bDraw) this.fnDraw();
};

var createExcelSearchFooterFlag = false;
$.fn.createExcelSearchFooter = function () {

    if (createExcelSearchFooterFlag == false) {
        createExcelSearchFooterFlag = true;

        $.fn.dataTableExt.afnFiltering.push(
            function (oSettings, aData, iDataIndex) {

                var searchResult = true;
                $.each(oSettings.aoData[iDataIndex].anCells, function (i, cell) {

                    var selectedKeys = $('input#' + oSettings.aoColumns[i].sKey);
                    if (selectedKeys.length > 0) {

                        selectedKeys = selectedKeys.val();
                        if (selectedKeys != '') {
                            var cellText = $.trim(cell.textContent).toLowerCase();

                            if (selectedKeys != '' && cellText == '')
                                searchResult = false;

                            if (selectedKeys.indexOf(cellText) < 0)
                                searchResult = false;
                        }
                    }
                });

                return searchResult;
            }
        );
    }

    var oTable = this;
    var columns = oTable.fnSettings().aoColumns;

    var footer = $('<tfoot>').addClass('dataTables_footer_excel');
    var tr = $('<tr>');

    sort = function (ul, order) {
        return $('li', ul).sort(function (a, b) {
            debugger
            var aText = $(a).text();
            var bText = $(b).text();

            if (aText === '(Select All)') return -1;
            else if (aText > bText) return (order === 'asc' ? 1 : -1);
            else if (aText < bText) return (order === 'asc' ? -1 : 1);
            else return 0;
        });
    }

    var initUl = function (ul, column, key, order, handlerID) {

        key = $.trim(key || '').toLowerCase();

        var rows = oTable.fnGetData();
        var pushedLi = [];
        var liList = '';
        var i = 0;
        var row = rows[i];

        var initUlInner = function (breakOn) {

            while (row != undefined) {

                var value = row[column.mData];
                if (column.mRender != null) value = column.mRender(value, undefined, row).replace(/<(?:.|\n)*?>/gm, '');
                value = $.trim(value);

                var valueKey = value.toLowerCase();

                if (value != '' && valueKey.indexOf(key) >= 0) {
                    if (pushedLi.indexOf(valueKey) === -1) {
                        liList += '<li secondary title="' + value + '"><input type="checkbox" /><span>' + value + '</span></li>'
                        pushedLi.push(valueKey);
                    }
                }

                if (pushedLi.length >= breakOn)
                    break;

                row = rows[++i];
            }
        };

        initUlInner(50);
        ul.html('<li primary><input type="checkbox" /><span>(Select All)</span></li>');
        ul.append(liList);
        liList = '';
        ul.html(sort(ul, order));

        $('li[secondary] input[type=checkbox]', ul).click(function () {
            $('li[primary] input[type=checkbox]', ul).prop('checked', false);
        });

        setTimeout(function (event) {

            initUlInner(1000);
            ul.append(liList);
            ul.html(sort(ul, order));

            $('li[primary] input[type=checkbox]', ul).click(function () {

                var selectedKeys = $('#' + column.sKey);
                var selectedKeysVal = '';
                var checked = $(this).is(':checked');

                if (checked == true) {

                    $('li[secondary] input[type=checkbox]', ul).prop('checked', true);
                    $('li[secondary] input[type=checkbox]', ul).each(function () {
                        var actionedKey = $(this).parents('li:first').find('span').html().toLowerCase();
                        selectedKeysVal = selectedKeysVal + actionedKey + ';';
                    });
                    selectedKeys.val(selectedKeysVal);
                }
                else if (checked == false) {

                    $('li[secondary] input[type=checkbox]', ul).prop('checked', false);
                    selectedKeys.val(selectedKeysVal);
                }
            });

        }, 0);

    };

    $.each(columns, function (i, column) {

        var th = $('<th>');

        if (column.filter != false) {

            var filterID = column.sKey;

            //var button = $('<input>').attr({ type: 'button', 'handler-id': filterID });
            var button = $('<button>').attr({ 'handler-id': filterID }).addClass('exl-f-btn btn btn-primary').html('Filter');
            button.append('<i class="fa fa-filter fa-lg"></i>')

            var sortDirection = 'desc';
            th.append(button);

            button.click(function () {

                var filterContainer = $('div[handler-id=' + button.attr('handler-id') + ']');
                var filterInput = filterContainer.find('input[type=text]');
                var filterInitialized = false;
                var filterContainerClose;

                if (filterContainer.length == 0) {

                    filterContainer = $('<div>')
                        .attr({ 'handler-id': filterID })
                        .addClass('excel-filter-box')

                    filterContainer.html(
                        '<table>\
                            <tr>\
                                <td><i class="fa fa-sort-alpha-asc selected">&nbsp;Sort A to Z</i>\
                                <i class="fa fa-sort-alpha-desc">&nbsp;Sort Z to A</i></td>\
                            </tr>\
                            <tr><td><input type="text" /></td></tr>\
                            <tr><td><ul></ul></td></tr>\
                            <tr><td><button cancel>Cancel</button><button clear>Clear</button><button ok>Ok</button></td></tr>\
                        </table>\
                        <input type="hidden" id="' + filterID + '" />'
                    );

                    $('body').append(filterContainer);

                    filterInput = $('input', filterContainer);
                    var ul = $('ul', filterContainer);
                    var sortAsc = $('.fa-sort-alpha-asc', filterContainer);
                    var sortDesc = $('.fa-sort-alpha-desc', filterContainer);
                    var okButton = $('button[ok]', filterContainer);
                    var cancelButton = $('button[cancel]', filterContainer);
                    var clearButton = $('button[clear]', filterContainer);
                    var selectedKeys = $('input#' + filterID, filterContainer);

                    var filterInputKeyupTimer;
                    filterInput.keyup(function () {

                        clearTimeout(filterInputKeyupTimer);
                        filterInputKeyupTimer = setTimeout(function (event) {
                            initUl(ul, column, filterInput.val(), sortDirection);
                        }, 300);
                    });

                    sortAsc.click(function () {
                        $('.fa', filterContainer).removeClass('selected');
                        sortAsc.addClass('selected');
                        sortDirection = 'asc';
                        initUl(ul, column, filterInput.val(), sortDirection);
                    });

                    sortDesc.click(function () {
                        $('.fa', filterContainer).removeClass('selected');
                        sortDesc.addClass('selected');
                        sortDirection = 'desc';
                        initUl(ul, column, filterInput.val(), sortDirection);
                    });

                    clearButton.click(function () {
                        selectedKeys.val('');
                        filterInput.val('');
                        //oTable.fnFilter('', i);

                        $('.fa', filterContainer).removeClass('selected');
                        sortAsc.addClass('selected');
                        sortDirection = 'asc';

                        initUl(ul, column, filterInput.val(), sortDirection);
                        filterContainer.find('input[type=checkbox]:checked').removeAttr('checked');

                        filterContainerClose();
                        oTable.fnDraw();

                        $('i.fa', button).removeClass('selected');
                    });

                    cancelButton.click(function () {
                        filterContainerClose();
                    });

                    okButton.click(function () {
                        filterContainerClose();
                        oTable.fnDraw();

                        if (selectedKeys.val() == '') $('i.fa', button).removeClass('selected');
                        else $('i.fa', button).addClass('selected');
                    });

                    filterContainer.on('click', 'input[type=checkbox]', function () {

                        var actionedKey = $(this).parents('li:first').find('span').html().toLowerCase();
                        if ($(this).is(':checked')) selectedKeys.val(selectedKeys.val() + actionedKey + ';');
                        else selectedKeys.val(selectedKeys.val().replace(actionedKey + ';', ''));
                    });
                }
                else filterInitialized = true;

                filterContainer.show();
                filterContainer.popup({
                    preserveContent: true,
                    position: {
                        my: 'center bottom',
                        at: 'center top',
                        of: this,
                        collision: 'fit'
                    },
                    close: function () {
                        filterContainer.hide();
                    },
                    init: function (detachHandlers) {
                        filterContainerClose = detachHandlers;
                    }
                });

                if (filterInitialized == false) {
                    setTimeout(function (event) {
                        initUl(ul, column, filterInput.val(), sortDirection);
                    }, 0);
                }

            });
        }

        tr.append(th);
    });

    footer.append(tr);
    oTable.append(footer);

    return oTable;
};

$.fn.createAjaxExcelSearchFooter = function (sFilterSource) {

    var oTable = this;
    var columns = oTable.fnSettings().aoColumns;

    var footer = $('<tfoot>').addClass('dt-xl-fltr-ajx');
    var tr = $('<tr>');

    var initUl = function (ul, column, key, orderBy) {

        $.ajax({
            url: sFilterSource,
            type: oTable.fnSettings().sServerMethod,
            data: { key: key, fieldName: column.mData, orderBy: orderBy },
            success: function (response) {

                var liList = '';
                $(response).each(function () {
                    liList += '<li secondary title="' + this.FilterValue + '"><input type="checkbox" /><span>' + this.FilterValue + '</span></li>';
                });

                ul.html('<li primary><input type="checkbox" /><span>(Select All)</span></li>');
                ul.append(liList);

                $('li[primary] input[type=checkbox]', ul).click(function () {

                    var selectedKeys = $('#' + column.sKey);
                    var selectedKeysVal = '';
                    var checked = $(this).is(':checked');

                    if (checked == true) {

                        $('li[secondary] input[type=checkbox]', ul).prop('checked', true);
                        $('li[secondary] input[type=checkbox]', ul).each(function () {
                            var actionedKey = $(this).parents('li:first').find('span').html().toLowerCase();
                            selectedKeysVal = selectedKeysVal + actionedKey + ';';
                        });
                        selectedKeys.val(selectedKeysVal);
                    }
                    else if (checked == false) {

                        $('li[secondary] input[type=checkbox]', ul).prop('checked', false);
                        selectedKeys.val(selectedKeysVal);
                    }
                });

                $('li[secondary] input[type=checkbox]', ul).click(function () {
                    $('li[primary] input[type=checkbox]', ul).prop('checked', false);
                });
            }
        });
    };

    $.each(columns, function (i, column) {

        var th = $('<th>');

        if (column.filter != false) {

            var filterID = column.sKey;

            //var button = $('<input>').attr({ value: 'Filter', type: 'button', 'handler-id': filterID, index: i });
            var button = $('<span>').attr({ 'handler-id': filterID, index: i }).addClass('exl-f-btn btn btn-primary').html('Filter');
            button.append('<i class="fa fa-filter fa-lg"></i>')

            var sortDirection = 'asc';
            th.append(button);


            button.click(function () {

                var filterContainer = $('div[handler-id=' + button.attr('handler-id') + ']');
                var filterInput = filterContainer.find('input[type=text]');
                var filterInitialized = false;

                if (filterContainer.length == 0) {

                    filterContainer = $('<div>')
                        .attr({ 'handler-id': filterID })
                        .addClass('excel-filter-box')

                    filterContainer.html(
                        '<table>\
                            <tr>\
                                <td><i class="fa fa-sort-alpha-asc selected">&nbsp;Sort A to Z</i>\
                                <i class="fa fa-sort-alpha-desc">&nbsp;Sort Z to A</i></td>\
                            </tr>\
                            <tr><td><input type="text" /></td></tr>\
                            <tr><td><ul></ul></td></tr>\
                            <tr><td><button cancel>Cancel</button><button clear>Clear</button><button ok>Ok</button></td></tr>\
                        </table>\
                        <input type="hidden" id="' + filterID + '" />'
                    );

                    $('body').append(filterContainer);

                    filterInput = $('input', filterContainer);
                    var ul = $('ul', filterContainer);
                    var sortAsc = $('.fa-sort-alpha-asc', filterContainer);
                    var sortDesc = $('.fa-sort-alpha-desc', filterContainer);
                    var okButton = $('button[ok]', filterContainer);
                    var cancelButton = $('button[cancel]', filterContainer);
                    var clearButton = $('button[clear]', filterContainer);
                    var selectedKeys = $('input#' + filterID, filterContainer);

                    var filterInputKeyupTimer;
                    filterInput.keyup(function () {

                        clearTimeout(filterInputKeyupTimer);
                        filterInputKeyupTimer = setTimeout(function (event) {
                            initUl(ul, column, filterInput.val(), sortDirection);
                        }, 300);
                    });

                    sortAsc.click(function () {
                        $('.fa', filterContainer).removeClass('selected');
                        sortAsc.addClass('selected');
                        sortDirection = 'asc';
                        initUl(ul, column, filterInput.val(), sortDirection);
                    });

                    sortDesc.click(function () {
                        $('.fa', filterContainer).removeClass('selected');
                        sortDesc.addClass('selected');
                        sortDirection = 'desc';
                        initUl(ul, column, filterInput.val(), sortDirection);
                    });

                    okButton.click(function () {
                        oTable.fnFilter(selectedKeys.val(), i);
                        filterContainer.hide();

                        if (selectedKeys.val() == '') $('i.fa', button).removeClass('selected');
                        else $('i.fa', button).addClass('selected');
                    });

                    clearButton.click(function () {
                        selectedKeys.val('');
                        filterInput.val('');
                        oTable.fnFilter('', i);

                        $('.fa', filterContainer).removeClass('selected');
                        sortAsc.addClass('selected');
                        sortDirection = 'asc';

                        initUl(ul, column, filterInput.val(), sortDirection);
                        filterContainer.find('input[type=checkbox]:checked').removeAttr('checked');

                        filterContainer.hide();
                        $('i.fa', button).removeClass('selected');
                    });

                    cancelButton.click(function () {
                        filterContainer.hide();
                    });

                    filterContainer.on('click', 'input[type=checkbox]', function () {

                        var actionedKey = $(this).parents('li:first').find('span').html().toLowerCase();
                        if ($(this).is(':checked')) selectedKeys.val(selectedKeys.val() + actionedKey + ';');
                        else selectedKeys.val(selectedKeys.val().replace(actionedKey + ';', ''));
                    });
                }
                else filterInitialized = true;

                filterContainer.show();
                filterContainer.popup({
                    position: {
                        my: 'center bottom',
                        at: 'center top',
                        of: this,
                        collision: 'fit'
                    },
                    preserveContent: true,
                    close: function () {
                        filterContainer.hide();
                    }
                });

                if (filterInitialized == false) {
                    setTimeout(function (event) {
                        initUl(ul, column, filterInput.val(), sortDirection);
                    }, 0);
                }

            });
        }

        tr.append(th);
    });

    footer.append(tr);
    oTable.append(footer);

    return oTable;
};

$.popupPlaceHolder = $('div.popup-view-placeholder');
$.fn.popup = function (extender) {
    var self = $(this);

    self.css({ 'position': 'absolute', 'z-index': '1000' });
    self.position(extender.position);

    detachHandlers = function (clearContent) {
        $(document).off({
            'mousedown': mouseDownDocHandler,
            'keydown': keyDownDocHandler
        });
        self.off('mousedown', mouseDownSelfHandler)

        if (clearContent == true && extender.preserveContent != true)
            self.html('');

        if (extender.close != undefined)
            extender.close();
    };

    attachHandlers = function () {
        $(document).on({
            'mousedown': mouseDownDocHandler,
            'keydown': keyDownDocHandler
        });
        self.on('mousedown', mouseDownSelfHandler);
    };

    mouseDownDocHandler = function () {
        detachHandlers(true);
    };

    keyDownDocHandler = function (e) {
        if (e.keyCode == 27) detachHandlers(true);
    };

    mouseDownSelfHandler = function (e) {
        if (!e) e = window.event;

        if (e.stopPropagation) e.stopPropagation();
        else e.cancelBubble = true;
    };

    attachHandlers();

    if (extender.init)
        extender.init(detachHandlers);

    return self;
};

$.dataTableMapper = function (postData) {

    var result = [];
    postData.map(function (key) {
        if (key.name.indexOf('_') > -1) {

            var keyAttrs = key.name.split('_');
            if (keyAttrs.length == 2) {
                if (['iSortCol', 'sSortDir'].indexOf(keyAttrs[1]) > 0)
                    result.push({ name: keyAttrs[1], value: key.value });
                else result.push({ name: 'Columns' + '[' + keyAttrs[1] + '].' + keyAttrs[1], value: key.value });
            }
        }
    });

    result.map(function (item) { postData.push(item); })
    return postData;
};

$.dataTableMapperV1 = function (aoData, customData) {
    if (aoData.order[0].column == 0) {
        aoData.sDir = 'desc';
        aoData.sCol = aoData.columns[aoData.order[0].column + 32].data;
    }
    else {
        aoData.sDir = aoData.order[0].dir;
        aoData.sCol = aoData.columns[aoData.order[0].column].data;
    }

    $.extend(customData, aoData);
    $.extend(aoData, customData);

    return aoData;
};



