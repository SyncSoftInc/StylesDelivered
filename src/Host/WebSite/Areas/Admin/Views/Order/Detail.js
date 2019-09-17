var itemsTable;

var orderVM = new Vue({
    el: "#app",
    data: {
        order: {
            orderNo: orderNoParam,
            isAddress: false,
            fullAddress: ''
        }
    },
    methods: {
        loadData: function () {
            var self = this;

            $.get("/api/admin/order/" + self.order.orderNo, function (rs) {
                self.order = rs;
                self.order.status = $.enumToName(OrderStatusEnum, self.order.status);
                self.order.createdOnUtc = $.timeFormat(self.order.createdOnUtc);

                if (!$.isNW(rs.shipping_Address1)) {
                    self.order.isAddress = true;
                    self.order.fullAddress = $.isNW(rs.shipping_Address2) ?
                        `${rs.shipping_Address1}, ${rs.shipping_City}, ${rs.shipping_State}, ${rs.shipping_ZipCode}`
                        : `${rs.shipping_Address1}, ${rs.shipping_Address2}, ${rs.shipping_City}, ${rs.shipping_State}, ${rs.shipping_ZipCode}`
                }
            });
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    }
});

// Items Table
itemsTable = $('#itemsTable').DataTable({
    responsive: true,
    serverSide: true,
    searchDelay: 500,
    lengthMenu: [10, 15, 20],
    ajax: {
        url: '/api/admin/order/items',
        data: {
            "orderNo": orderVM.order.orderNo
        }
    },
    columns: [
        { data: "sku" },
        { data: "asin" },
        { data: "alias", orderable: false },
        { data: "color", orderable: false },
        { data: "size", orderable: false },
        {
            data: "qty",
            width: 50,
            orderable: false
        }
    ],
    columnDefs: [
        { "className": "text-center", "targets": [-1] },
    ],
    order: [[0, "DESC"]]
});

$(function () {
    $('#items-tab').on('shown.bs.tab', function () {
        if ($.isNW(itemsTable)) {
            createTable();
        }
    });
});