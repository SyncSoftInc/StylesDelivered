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
            { data: "orderNo" },
            { data: "user" },
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
            { "className": "text-center", "targets": [-1] },
        ],
        order: [[3, "DESC"]]
    });
});