// 判断对象是否为空
$.isNW = function (obj) {
    return obj === undefined || obj === null || obj.toString().trim().length === 0;
};

// 格式化成时间
$.timeFormat = function (value, timeformat) {
    if (!$.isNW(value)) {
        timeformat = timeformat || "MM/DD/YYYY hh:mm:ss A";

        var date = new moment.utc(value).tz('America/Los_Angeles');
        return date.format(timeformat);
    }
};

// 格式化成日期
$.dateFormat = function (value, timeformat) {
    if (!$.isNW(value)) {
        timeformat = timeformat || "MM/DD/YYYY";
        var date = new moment.utc(value);
        return date.format(timeformat);
    }
};

// 判断消息码是否成功
$.isSuccess = function (msgCode) {
    return msgCode === "";
};