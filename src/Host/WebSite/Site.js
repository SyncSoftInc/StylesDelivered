// 判断对象是否为空
$.isNW = function (obj) {
    return obj === undefined || obj === null || obj.toString().trim().length === 0;
};

// 格式化成时间
$.timeFormat = function (value, timeformat = "MM/DD/YYYY hh:mm:ss A") {
    if (!$.isNW(value)) {
        var date = new moment.utc(value).tz('America/Los_Angeles');
        return date.format(timeformat);
    }
};

// 格式化成日期
$.dateFormat = function (value, timeformat = "MM/DD/YYYY") {
    if (!$.isNW(value)) {
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
    scrollTop = bodyScrollTop - documentScrollTop > 0 ? bodyScrollTop : documentScrollTop;
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
    scrollHeight = bodyScrollHeight - documentScrollHeight > 0 ? bodyScrollHeight : documentScrollHeight;
    return scrollHeight;
};
// 获取浏览器窗口高度
$.getWindowHeight = function () {
    var windowHeight = 0;
    if (document.compatMode === "CSS1Compat") {
        windowHeight = document.documentElement.clientHeight;
    } else {
        windowHeight = document.body.clientHeight;
    }
    return windowHeight;
};

$.pic = function (url, width = 350, height = 350) {
    if (!$.isNW(url)) {
        return url + "?x-oss-process=image/resize,m_lfit,h_" + height + ",w_" + width;
    }
    else {
        return "https://eec.oss-us-west-1.aliyuncs.com/r/nopic.png?x-oss-process=image/resize,m_lfit,h_" + height + ",w_" + width;
    }
};

Vue.component("itembox", {
    props: ["item"],
    template: '#item-box',
    data: function () {
        return {
            json: null,
            selectedSize: "-Select Size-",
            selectedColor: "-Select Color-",
            sizeList: ["-Select Size-"],
            colorList: ["-Select Color-"],
            allSize: ["-Select Size-"],
            allColor: ["-Select Color-"]
        }
    },
    methods: {
        //addToCart: function (item) {
        //    $.post("/api/shoppingcart/item", {
        //        Item: {
        //            ItemNo: item.ItemNo,
        //            Qty: item.Qty
        //        }
        //    }, function (rs) {

        //    });
        //},
        createdTime: function (item) {
            return $.timeFormat(item.createdOnUtc);
        },
        decorateData: function () {
            var self = this;

            if (!$.isNW(self.item.itemsJson)) {
                self.json = JSON.parse(self.item.itemsJson);

                $.each(self.json, function (idx, item) {
                    if (!$.isNW(item.Size) && !self.sizeList.includes(item.Size)) {
                        self.sizeList.push(item.Size);
                        self.allSize.push(item.Size);
                    }
                    if (!$.isNW(item.Color) && !self.colorList.includes(item.Color)) {
                        self.colorList.push(item.Color);
                        self.allColor.push(item.Color);
                    }
                });
            }
        },
        onChangeList: function (type, selectedVal) {
            var self = this;

            if ((type === 'size' || type === 'color') && !$.isNW(self.json)) {
                if (selectedVal === '-Select Size-' || selectedVal === '-Select Color-') {
                    // return all
                    self.colorList = self.allColor;
                    self.sizeList = self.allSize;
                }
                else {
                    // clear list
                    if (type === 'size') self.colorList = ["-Select Color-"];
                    if (type === 'color') self.sizeList = ["-Select Size-"];

                    $.each(self.json, function (idx, item) {
                        if (type === 'size' && item.Size === selectedVal) {
                            self.colorList.push(item.Color);
                        }
                        if (type === 'color' && item.Color === selectedVal) {
                            self.sizeList.push(item.Size);
                        }
                    });
                }
            }
        }
    },
    beforeMount: function () {
        var self = this;
        self.decorateData();
    }
});