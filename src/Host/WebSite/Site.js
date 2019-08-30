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
// 获取滚动条顶部位置
$.getScrollTop = function () {
    var scrollTop = 0, bodyScrollTop = 0, documentScrollTop = 0;
    if (document.body) {
        bodyScrollTop = document.body.scrollTop;
    }
    if (document.documentElement) {
        documentScrollTop = document.documentElement.scrollTop;
    }
    scrollTop = (bodyScrollTop - documentScrollTop > 0) ? bodyScrollTop : documentScrollTop;
    return scrollTop;
};
// 获取文档的总高度
$.getScrollHeight = function () {
    var scrollHeight = 0, bodyScrollHeight = 0, documentScrollHeight = 0;
    if (document.body) {
        bodyScrollHeight = document.body.scrollHeight;
    }
    if (document.documentElement) {
        documentScrollHeight = document.documentElement.scrollHeight;
    }
    scrollHeight = (bodyScrollHeight - documentScrollHeight > 0) ? bodyScrollHeight : documentScrollHeight;
    return scrollHeight;
};
// 获取浏览器窗口高度
$.getWindowHeight = function () {
    var windowHeight = 0;
    if (document.compatMode == "CSS1Compat") {
        windowHeight = document.documentElement.clientHeight;
    } else {
        windowHeight = document.body.clientHeight;
    }
    return windowHeight;
};