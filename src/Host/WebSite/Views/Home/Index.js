var homeVM = new Vue({
    el: "#app",
    data: {
        pageSize: 10,
        pageIndex: 1,
        items: []
    },
    methods: {
        loadData: function () {
            var self = this;
            $.get("/api/product/items?pagesize=" + this.pageSize + "&pageindex=" + this.pageIndex, function (dt) {
                self.items = dt.data;
            });
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    }
});