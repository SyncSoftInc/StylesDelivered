var itemsTable;

var orderVM = new Vue({
    el: "#app",
    data: {
        order: {
            isAddress2: false,
            orderNo: orderNoParam
        }
    },
    methods: {
        loadData: function () {
            var self = this;

            axios.get("/api/admin/order/" + self.order.orderNo)
                .then(function (resp) {
                    var rs = resp.data;
                    self.order = rs;
                    self.order.status = $.enumToName(OrderStatusEnum, self.order.status);
                    self.order.createdOnUtc = $.timeFormat(self.order.createdOnUtc);
                    self.order.isAddress2 = !$.isNW(self.order.shipping_Address2);
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
            orderable: false,
            render: function (data, type, item) {
                if (!$.isNW(item.url)) {
                    return `<a target="_blank" href="${item.url}">${item.url}</a>`;
                }
            }
        },
        {
            data: "qty", width: 50, orderable: false
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