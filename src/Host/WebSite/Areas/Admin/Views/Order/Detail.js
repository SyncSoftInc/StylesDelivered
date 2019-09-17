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

                if (!$.isNW(rs.shipping_Address1)) {
                    self.order.isAddress = true;
                    self.order.fullAddress = $.isNW(rs.shipping_Address2) ?
                        `${rs.shipping_Address1}, ${rs.shipping_City}, ${rs.shipping_State}, ${rs.shipping_ZipCode}, ${rs.shipping_Country}`
                        : `${rs.shipping_Address1}, ${rs.shipping_Address2}, ${rs.shipping_City}, ${rs.shipping_State}, ${rs.shipping_ZipCode}, ${rs.shipping_Country}`
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
        {
            width: 50,
            orderable: false,
            render: function (id, display, item) {
                return '<img src="' + $.pic(item.imageUrl) + '" class="pic_s" />';
            }
        },
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
        { "className": "text-center align-middle", "targets": [-1, 0] },
        { "className": "align-middle", "targets": '_all' }
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