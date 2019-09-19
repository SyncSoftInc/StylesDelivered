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


// 根据给定flags取得一系列枚举名
$.enumToName = function (EnumType, value) {
    for (var key in EnumType) {
        if (value === EnumType[key]) {
            return key;
        }
    }
    return '';
};

$.pic = function (url, width = 350, height = 350) {
    if (!$.isNW(url)) {
        return url + "?x-oss-process=image/resize,m_lfit,h_" + height + ",w_" + width;
    }
    else {
        return "https://eec.oss-us-west-1.aliyuncs.com/r/nopic.png?x-oss-process=image/resize,m_lfit,h_" + height + ",w_" + width;
    }
};

var itemVM = Vue.component("itembox", {
    props: ["item"],
    template: '#item-box',
    data: function () {
        return {
            items: null,
            selectedSize: null,
            selectedColor: null,
            sizeList: [],
            colorList: []
        };
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
        //createdTime: function (item) {
        //    return $.timeFormat(item.createdOnUtc);
        //},
        decorateData: function () {
            var self = this;

            if (!$.isNW(self.item.itemsJson)) {
                self.items = JSON.parse(self.item.itemsJson);

                for (var item in self.items) {
                    if (!$.isNW(item.Size) && !self.sizeList.includes(item.Size)) {
                        self.sizeList.push(item.Size);
                    }
                    if (!$.isNW(item.Color) && !self.colorList.includes(item.Color)) {
                        self.colorList.push(item.Color);
                    }
                }

                if (!$.isNW(self.sizeList)) {
                    self.selectedSize = self.sizeList[0];
                    this.onSizeChange(self.selectedSize);
                }
                if (!$.isNW(self.colorList)) self.selectedColor = self.colorList[0];
            }
        },
        onSizeChange: function (selectedVal) {
            var self = this;

            if (!$.isNW(self.items)) {
                // clear color list
                self.colorList = [];

                for (var item in self.items) {
                    if (!$.isNW(item.Color) && item.Size === selectedVal && !self.colorList.includes(item.Color)) {
                        self.colorList.push(item.Color);
                    }
                }

                if (!$.isNW(self.colorList)) self.selectedColor = self.colorList[0];
            }
        },
        applyItem: function () {
            var self = this;
            var selectedItem = self.items.find(x => x.Size === self.selectedSize && x.Color === self.selectedColor);

            if (!$.isNW(selectedItem)) {
                applyVM.asin = self.item.asin;
                applyVM.sku = selectedItem.SKU;
                $("#addressModal").modal("show");
            }
        }
    },
    beforeMount: function () {
        var self = this;
        self.decorateData();
    }
});