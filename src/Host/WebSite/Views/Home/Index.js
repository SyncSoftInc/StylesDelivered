var homeVM = new Vue({
    el: "#app",
    data: {
        pageSize: 9,
        pageIndex: 1,
        totalPage: 1,
        items: []
    },
    methods: {
        loadData: function () {
            var self = this;

            var cursor = (self.pageIndex - 1) * self.pageSize;

            return $.get("/api/products?length=" + self.pageSize + "&start=" + cursor, function (dt) {
                self.totalPage = dt.totalPage;
                $.each(dt.data, function (i, x) {
                    x.imageUrl = $.pic(x.imageUrl);
                });
                self.items = self.items.concat(dt.data);
            });
        },
        scroll: function () {
            var self = this;
            let isLoading = false;

            window.onscroll = () => {
                let bottomOfWindow = $.getScrollTop() + $.getWindowHeight() === $.getScrollHeight();
                if (bottomOfWindow && !isLoading && self.pageIndex < self.totalPage) {
                    isLoading = true;
                    self.pageIndex++;
                    self.loadData().done(function () {
                        isLoading = false;
                    });
                }
            };
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    },
    mounted: function () {
        var self = this;
        self.scroll();
    }
});

var applyVM = new Vue({
    el: "#addressModal",
    data: {
        asin: "",
        sku: "",
        addresses: [],
        states: [],
        address: {}
    },
    methods: {
        loadAddresses: function () {
            var self = this;
            $.get("/api/user/addresses", function (rs) {
                self.addresses = rs;
            });
        },
        loadStates: function () {
            var self = this;
            $.get("/api/states/us", function (rs) {
                self.states = rs;
            });
        },
        select: function (item) {
            var self = this;
            self.address = item;
        },
        apply: function () {
            var self = this;
            var order = {
                items: [{
                    asin: self.asin,
                    sku: self.sku
                }],
                shipping_Address1: self.address.address1,
                shipping_Address2: self.address.address2,
                shipping_City: self.address.city,
                shipping_State: self.address.state,
                shipping_ZipCode: self.address.zipCode,
            };

            $.ajax({
                url: '/api/order',
                type: "PUT",
                data: {
                    Order: order
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        bootbox.alert("Success");
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadStates();
        self.loadAddresses();
    }
});

var ClearAddress = function () {
    applyVM.address = { "state": "" };
};

$(function () {
    $('#addressModal').on('show.bs.modal', function (e) {
        // 编辑器打开时清空数据
        ClearAddress();
    });

    $("#clearBtn").on('click', function () {
        ClearAddress();
    });
});