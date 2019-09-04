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

            return $.get("/api/product/items?length=" + self.pageSize + "&start=" + cursor, function (dt) {
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