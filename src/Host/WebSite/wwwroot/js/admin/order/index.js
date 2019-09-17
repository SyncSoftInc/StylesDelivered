var mainTable;

function DeleteOrder(orderNo) {
    bootbox.confirm("Delete product?", function (confirm) {
        if (confirm) {
            $.ajax({
                url: '/api/admin/order/' + orderNo,
                type: 'DELETE',
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        mainTable.ajax.reload();
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });
        }
    });
}

function ApproveOrder(orderNo) {
    $.ajax({
        url: '/api/admin/order/' + orderNo,
        type: 'PATCH',
        success: function (rs) {
            if ($.isSuccess(rs)) {
                bootbox.alert("Order Approved.", function () {
                    mainTable.ajax.reload();
                });
            }
            else {
                bootbox.alert(rs);
            }
        }
    });
}

$(function () {
    mainTable = $('#mainTable').DataTable({
        responsive: true,
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [10, 15, 20],
        ajax: {
            url: '/api/admin/orders'
        },
        columns: [
            {
                width: 50,
                orderable: false,
                render: function (id, display, item) {
                    return '<img src="' + $.pic(item.imageUrl) + '" class="pic_s" />';
                }
            },
            { data: "user" },
            { data: "asin" },
            { data: "sku" },
            { data: "alias" },
            {
                data: "status",
                width: 30,
                orderable: false,
                render: function (data, type, item) {
                    if (!$.isNW(data)) {
                        return $.enumToName(OrderStatusEnum, data);
                    }
                }
            },
            { data: "createdOnUtc" },
            {
                width: 210,
                orderable: false,
                render: function (id, display, item) {
                    var btn =
                        '<button class="approveBtn btn btn-sm btn-secondary" type="button" onclick="ApproveOrder(\'' + item.orderNo + '\')">Approve</button>' +
                        '<a class="btn btn-sm btn-primary mr-2 ml-2" href="/admin/order/Detail/' + item.orderNo + '">Detail</a>' +
                        '<button class="btn btn-sm btn-danger" type="button" onclick="DeleteOrder(\'' + item.orderNo + '\')">Delete</button>';

                    return btn;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 0] },
            { "className": "align-middle", "targets": '_all' }
        ],
        order: [[3, "DESC"]]
    });
});