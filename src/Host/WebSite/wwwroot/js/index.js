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

            return axios.get("/api/products?length=" + self.pageSize + "&start=" + cursor)
                .then(function (resp) {
                    var dt = resp.data;
                    self.totalPage = dt.totalPage;

                    for (var x in dt.data) {
                        x.imageUrl = $.pic(x.imageUrl);
                    }

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
                    self.loadData().then(function () {
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
        address: {
            shipping_Address1: '',
            shipping_Address2: '',
            shipping_City: '',
            shipping_State: '',
            shipping_ZipCode: '',
            shipping_Country: 'US',
        }
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
            self.address.shipping_Address1 = item.address1;
            self.address.shipping_Address2 = item.address2;
            self.address.shipping_City = item.city;
            self.address.shipping_State = item.state;
            self.address.shipping_ZipCode = item.zipCode;
            self.address.shipping_Country = item.country;
        },
        apply: function () {
            var self = this;
            var order = {
                items: [{
                    asin: self.asin,
                    sku: self.sku
                }],
                shipping_Address1: self.address.shipping_Address1,
                shipping_Address2: self.address.shipping_Address2,
                shipping_City: self.address.shipping_City,
                shipping_State: self.address.shipping_State,
                shipping_ZipCode: self.address.shipping_ZipCode,
                shipping_Country: self.address.shipping_Country,
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
        },
        clear: function () {
            var self = this;
            self.address.shipping_Address1 = '';
            self.address.shipping_Address2 = '';
            self.address.shipping_City = '';
            self.address.shipping_State = '';
            self.address.shipping_ZipCode = '';
            self.address.shipping_Country = 'US';
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadStates();
        self.loadAddresses();
    }
});