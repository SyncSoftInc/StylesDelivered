var addressVM = new Vue({
    el: "#app",
    data: {
        addresses: [],
        states: [],
        address: {}
    },
    methods: {
        loadAddresses: function () {
            var self = this;
            axios.get("/api/user/addresses")
                .then(function (resp) {
                    var rs = resp.data;
                    self.addresses = rs;
                });
        },
        loadStates: function () {
            var self = this;
            axios.get("/api/states/us")
                .then(function (resp) {
                    var rs = resp.data;
                    self.states = rs;
                });
        },
        save: function () {
            var self = this;
            axios.post("/api/user/address", {  Address: self.address })
                .then(function (resp) {
                    var rs = resp.data;

                    if ($.isSuccess(rs)) {
                        $('#editor').modal('hide');
                        self.loadAddresses();
                    }
                    else {
                        bootbox.alert(rs);
                    }
                });
        },
        remove: function (hash) {
            var self = this;
            bootbox.confirm("Delete this address?", function (flag) {
                if (flag) {
                    axios.delete('/api/user/address', { data: { Address: { "Hash": hash } } })
                        .then(function (resp) {
                            var rs = resp.data;
                            if ($.isSuccess(rs)) {
                                self.loadAddresses();
                            }
                            else {
                                bootbox.alert(rs);
                            }
                        });
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

$(function () {
    $('#editor').on('show.bs.modal', function (e) {
        // 编辑器打开时清空数据
        addressVM.address = { "State": "" };
    });
});