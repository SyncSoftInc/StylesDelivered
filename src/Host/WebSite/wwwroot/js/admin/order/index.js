var mainTable;

function DeleteProduct(orderNo) {
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

//function UpdateProductStatus(asinIn, statusIn) {
//    $.ajax({
//        url: '/api/admin/product',
//        data: { asin: asinIn, status: statusIn },
//        type: 'PATCH',
//        success: function (rs) {
//            if ($.isSuccess(rs)) {
//                mainTable.ajax.reload();
//            }
//            else {
//                bootbox.alert(rs);
//            }
//        }
//    });
//}


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
            { data: "user_ID" },
            {
                data: "status",
                width: 30,
                orderable: false
            },
            {
                width: 120,
                orderable: false,
                render: function (id, display, item) {
                    var btn = '<a class="btn btn-sm btn-primary mr-2" href="/admin/order/Detail/' + item['orderNo'] + '">Detail</a>' +
                        '<button class="delBtn btn btn-sm btn-danger" type="button" onclick="DeleteOrder(\'' + item['orderNo'] + '\')" disabled>Delete</button>';

                    return btn;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 2] },
        ],
        order: [[0, "DESC"]]
    });
});