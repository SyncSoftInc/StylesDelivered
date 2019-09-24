var itemsTable;

var orderVM = new Vue({
    el: "#app",
    data: {
        review: {
            id: idParam
        }
    },
    methods: {
        loadData: function () {
            var self = this;

            axios.get("/api/admin/review/" + self.review.id)
                .then(function (resp) {
                    var rs = resp.data;
                    self.review = rs;
                    self.review.status = $.enumToName(OrderStatusEnum, self.review.status);
                    self.review.createdOnUtc = $.timeFormat(self.review.createdOnUtc);
                });
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    }
});