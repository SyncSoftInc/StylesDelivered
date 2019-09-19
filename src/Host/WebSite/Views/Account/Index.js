var indexVM = new Vue({
    el: "#app",
    data: {
        user: {}
    },
    methods: {
        loadProfile: function () {
            var self = this;
            $.get("/api/user", function (rs) {
                self.user = rs;
            });
        },
        save: function () {
            var self = this;

            axios.patch('/api/user/profile', { User: self.user })
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
    },
    beforeMount: function () {
        var self = this;
        self.loadProfile();
    }
});