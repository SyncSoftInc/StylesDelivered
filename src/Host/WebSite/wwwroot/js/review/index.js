var reviewVM = new Vue({
    el: "#app",
    data: {
        review: {
            orderNo: reviewModel.orderNo,
            sku: reviewModel.sku
        }
    },
    methods: {
        save: function () {
            var self = this;

            axios.post('/api/review', { Review: self.review })
                .then(function (resp) {
                    var rs = resp.data;
                    if ($.isSuccess(rs)) {
                        bootbox.alert("Save successfully.");
                    }
                    else {
                        bootbox.alert(rs);
                    }
                });

            return false;
        }
    }
});