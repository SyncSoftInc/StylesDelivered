var saveVM = new Vue({
    el: "#app",
    data: {
        isNew: true,
        title: "New",
        user: {
            id: null
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.isNew = $.isNW(self.user.id);
            self.title = !self.isNew ? "Edit" : "New";

            if (!self.isNew) {
                $.get("/api/admin/user/" + self.user.id, function (rs) {
                    self.user = rs;
                });
            }
        },
        save: function () {
            var self = this;
            var actionType = self.isNew ? 'POST' : 'PUT';
            if (self.isNew) self.user.id = "00000000-0000-0000-0000-000000000000";

            $.ajax({
                url: '/api/admin/user',
                type: actionType,
                data: {
                    User: self.user
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        var returnUrl = self.isNew ? "/admin/user" : "/admin/user/Save/" + self.user.id;
                        bootbox.alert("Save successfully.", function () {
                            window.location = returnUrl;
                        });
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });

            return false;
        }
    },
    beforeMount: function () {
        var self = this;
        var url = window.location.href;
        var last = url.substring(url.lastIndexOf('/') + 1);
        if (last != "Save") self.user.id = last;

        self.loadData();
    }
});