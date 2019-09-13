var itemsTable;

var orderVM = new Vue({
    el: "#app",
    data: {
        order: {
            orderNo: orderNoParam
        }
    },
    methods: {
        loadData: function () {
            var self = this;

            $.get("/api/admin/order/" + self.order.orderNo, function (rs) {
                self.order = rs;
            });
        }
    },
    beforeMount: function () {
        var self = this;
        self.loadData();
    }
});

//var itemVM = new Vue({
//    el: "#itemModal",
//    data: {
//        isNew: true,
//        item: {
//            sku: null,
//            invQty: 0
//        }
//    },
//    methods: {
//        loadData: function () {
//            var self = this;
//            self.isNew = $.isNW(self.item.sku);

//            if (!self.isNew) {
//                $.get("/api/product/item", { asin: productVM.product.asin, sku: self.item.sku }, function (rs) {
//                    self.item = rs;
//                });
//            }
//            else {
//                self.item = { asin: productVM.product.asin, invQty: 0 };
//            }
//        },
//        save: function () {
//            var self = this;
//            var actionType = self.isNew ? 'POST' : 'PUT';

//            $.ajax({
//                url: '/api/product/item',
//                type: actionType,
//                data: {
//                    ProductItem: self.item
//                },
//                success: function (rs) {
//                    if ($.isSuccess(rs)) {
//                        $("#itemModal").modal("toggle");
//                        itemsTable.ajax.reload();
//                    }
//                    else {
//                        bootbox.alert(rs);
//                    }
//                }
//            });

//            return false;
//        }
//    }
//});

//// Items Table
//function createTable() {
//    itemsTable = $('#itemsTable').DataTable({
//        responsive: true,
//        serverSide: true,
//        searchDelay: 500,
//        lengthMenu: [10, 15, 20],
//        ajax: {
//            url: '/api/product/items',
//            data: {
//                "asin": productVM.product.asin
//            }
//        },
//        columns: [
//            { data: "sku" },
//            { data: "alias" },
//            { data: "color", orderable: false },
//            { data: "size", orderable: false },
//            {
//                data: "invQty",
//                width: 50,
//                orderable: false
//            },
//            {
//                width: 120,
//                orderable: false,
//                render: function (id, display, item) {
//                    return '<button class="editBtn btn btn-sm btn-primary mr-2" type="button" data-toggle="modal" data-target="#itemModal"  data-id="' + item['sku'] + '">Edit</a>' +
//                        '<button class="delBtn btn btn-sm btn-danger" type="button" onclick="DeleteItem(\'' + item['sku'] + '\')">Delete</button>';
//                }
//            }
//        ],
//        columnDefs: [
//            { "className": "text-center", "targets": [-1] }
//        ],
//        order: [[0, "DESC"]]
//    });
//}