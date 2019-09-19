var mainTable;

function DeleteOrder(orderNo) {
    bootbox.confirm("Delete product?", function (confirm) {
        if (confirm) {
            axios.delete(`/api/admin/order/${orderNo}`, { data: { OrderNo: orderNo } })
                .then(function (resp) {
                    var rs = resp.data;
                    if ($.isSuccess(rs)) {
                        mainTable.ajax.reload();
                    }
                    else {
                        bootbox.alert(rs);
                    }
                });
        }
    });
}

function ChangeOrder(action, orderNo) {
    var actionType;
    if (action === "Approve") actionType = "put";
    else if (action === "Ship") actionType = "patch";
    else return;

    axios({
        method: actionType,
        url: '/api/admin/order/' + orderNo
    })
        .then(function (resp) {
            var rs = resp.data;
            if ($.isSuccess(rs)) {
                bootbox.alert("Success.", function () {
                    mainTable.ajax.reload();
                });
            }
            else {
                bootbox.alert(rs);
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
                render: function (data, type, item) {
                    if (!$.isNW(data)) {
                        return $.enumToName(OrderStatusEnum, data);
                    }
                }
            },
            {
                data: "createdOnUtc",
                width: 150,
                render: function (data, type, full, meta) {
                    return $.timeFormat(data);
                }
            },
            {
                width: 210,
                orderable: false,
                render: function (id, display, item) {
                    var disabled = "", action = "";

                    if (item.status === OrderStatusEnum.Pending) {
                        action = "Approve";
                    }
                    else if (item.status === OrderStatusEnum.Approved) {
                        action = "Ship";
                    }
                    else if (item.status === OrderStatusEnum.Shipped) {
                        disabled = "disabled";
                        action = "Shipped";
                    }

                    var btns =
                        `<button class="btn btn-sm btn-primary" type="button" style="width:70px;" onclick="ChangeOrder('${action}', '${item.orderNo}')" ${disabled}>${action}</button>` +
                        `<a class="btn btn-sm btn-info mr-2 ml-2" href="/admin/order/Detail/${item.orderNo}">Detail</a>` +
                        `<button class="btn btn-sm btn-danger" type="button" onclick="DeleteOrder('${item.orderNo}')" ${disabled}>Delete</button>`;

                    return btns;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 0] },
            { "className": "align-middle", "targets": '_all' }
        ],
        order: [[6, "DESC"]]
    });
});