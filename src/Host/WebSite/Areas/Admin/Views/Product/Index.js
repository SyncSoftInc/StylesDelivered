var mainTable;

function DeleteProduct(asin) {
    bootbox.confirm("Delete product?", function (confirm) {
        if (confirm) {
            $.ajax({
                url: '/api/admin/product/' + asin,
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

function UpdateProductStatus(asinIn, statusIn) {
    $.ajax({
        url: '/api/admin/product',
        data: { asin: asinIn, status: statusIn },
        type: 'PATCH',
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

$(function () {
    mainTable = $('#mainTable').DataTable({
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [5, 10, 25],
        ajax: {
            url: '/api/admin/products'
        },
        columns: [
            {
                width: 50,
                orderable: false,
                render: function (id, display, item) {
                    return '<img src="' + $.pic(item.imageUrl) + '" class="pic_s" />';
                }
            },
            { data: "asin" },
            { data: "productName" },
            {
                data: "status",
                width: 30,
                orderable: false
            },
            {
                data: "createdOnUtc",
                width: 150,
                render: function (data, type, full, meta) {
                    return $.timeFormat(data);
                }
            },
            {
                width: 50,
                orderable: false,
                render: function (id, display, item) {
                    if (item['status'] === 1) {
                        return '<button type="button" class="btn btn-sm btn-primary" onclick="UpdateProductStatus(\'' + item['asin'] + '\', 0)">Deactivate</button>';
                    }
                    else {
                        return '<button type="button" class="btn btn-sm btn-primary" onclick="UpdateProductStatus(\'' + item['asin'] + '\', 1)">Activate</button>';
                    }
                }
            },
            {
                width: 120,
                orderable: false,
                render: function (id, display, item) {
                    return '<a class="btn btn-sm btn-primary mr-2" href="/admin/product/Save/' + item['asin'] + '">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-danger" type="button" onclick="DeleteProduct(\'' + item['asin'] + '\')">Delete</button>';
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 0, 3, 5] },
            { "className": "align-middle", "targets": '_all' }
        ],
        order: [[2, "DESC"]]
    });
});