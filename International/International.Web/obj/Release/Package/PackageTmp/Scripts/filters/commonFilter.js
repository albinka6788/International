rootApplication.filter('textCutFilter', function (applicationConstants) {
 
    return function (val) {
        if (val == null) return '';
        var maxLength = applicationConstants.appconstant.fixLength;
        if (val.length > maxLength) {
           
            return val.substr(0, maxLength) + '...';
        } else {
            return val;
        }
    };
});


rootApplication.filter("jsonDate", function ($filter) {
    var dateFilter = $filter('date');
    return function (item) {
        if (item != null) {
            if (item.indexOf('Date') != -1) {
                return dateFilter(parseInt(item.substr(6)), 'd-MMM-yy');
            }
            else {
                return dateFilter(item, 'd-MMM-yy');
            }
        }
        return "";
    };
});


rootApplication.filter('numberFixedLen', function () {
        return function (n, len) {
            var num = parseInt(n, 10);
            len = parseInt(len, 10);
            if (isNaN(num) || isNaN(len)) {
                return n;
            }
            num = '' + num;
            while (num.length < len) {
                num = '0' + num;
            }
            return num;
        };
    });

rootApplication.filter('NumFilter', function () {
    return function (num, NumDecimals) {
        if (!num) num = 0;
        return num.toFixed(NumDecimals)
    }
})