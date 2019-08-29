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
            $.get("/api/user/addresses", function (rs) {
                self.addresses = rs;
            });
            $.get("/api/states/us", function (rs) {
                self.states = rs;
            });
        },
        loadStates: function () {
            var self = this;
            $.get("/api/states/us", function (rs) {
                self.states = rs;
            });
        },
        save: function () {
            var self = this;
            $.post("/api/user/address", { Address: self.address }, function (rs) {

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
                    $.ajax({
                        url: '/api/user/address',
                        type: 'DELETE',
                        data: {
                            Address: { "Hash": hash }
                        },
                        success: function (rs) {
                            if ($.isSuccess(rs)) {
                                self.loadAddresses();
                            }
                            else {
                                bootbox.alert(rs);
                            }
                        }
                    });
                }
            });
        }
    }
});

$(function () {
    addressVM.loadStates();
    addressVM.loadAddresses();

    $('#editor').on('show.bs.modal', function (e) {
        // 编辑器打开时清空数据
        addressVM.address = { "State": "" };
    });
});