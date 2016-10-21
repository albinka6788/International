
function parseDate(s) {
    var months = {
        jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5,
        jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11
    };
    if (s) {
        console.log(s);
        var p = s.split('-');
        return new Date(p[2], months[p[0].toLowerCase()], p[1]);
    }

    return new Date();

}

$(document).ready(function () {

    //$('#btnCancel').click(function () {
    //if ($('#chkRemove').is(':checked')) {
    $.fn.MessageBox = function (msg, callBack) {
        //var msg = 'Confirmation Msg.';
        var div = $("<div id='msgBox'>" + msg + "</div>");
        div.dialog({
            title: "Confirmation",
            buttons: [
                {
                    text: "Yes",
                    click:callBack
                },
                {
                    text: "No",
                    click: function () {
                        div.dialog("close");
                    }
                }
            ]
        });
        //}
        //});
    }

})