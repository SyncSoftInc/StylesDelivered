var profileVM = new Vue({
    el: "#app",
    data: {
        user: {}
    },
    methods: {
        loadData: function () {
            var self = this;
            axios.get("/api/user")
                .then(function (resp) {
                    self.user = resp.data;
                });
        },
        save: function () {
            var self = this;
            axios.patch("/api/user/profile", { User: self.user })
                .then(function (resp) {
                    var rs = resp.data;

                    if ($.isSuccess(rs)) {
                        bootbox.alert("Success.");
                    }
                    else {
                        bootbox.alert(rs);
                    }
                });

            return false;
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    }
});