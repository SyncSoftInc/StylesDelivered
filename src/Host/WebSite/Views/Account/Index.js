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

            $.ajax({
                url: '/api/user/profile',
                type: 'PATCH',
                data: {
                    User: self.user
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        bootbox.alert("Save successfully.");
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });

            return false;
        }
    }
});

$(function () {
    indexVM.loadProfile();
});