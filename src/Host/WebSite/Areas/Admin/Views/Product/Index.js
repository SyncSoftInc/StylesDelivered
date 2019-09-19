var mainTable;

function DeleteProduct(asin) {
    bootbox.confirm("Delete product?", function (confirm) {
        if (confirm) {
            axios.delete(`/api/admin/product/${asin}`, { data: { ASIN: asin } })
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

function UpdateProductStatus(asinIn, statusIn) {
    axios.patch('/api/admin/product', { ASIN: asinIn, Status: statusIn })
        .then(function (resp) {
            var rs = resp.data;
            if ($.isSuccess(rs)) {
                mainTable.ajax.reload();
            }
            else {
                bootbox.alert(rs);
            }
        })
        .catch(function (error) {
            console.log(error);
        });
}


$(function () {
    mainTable = $('#mainTable').DataTable({
        responsive: true,
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
                orderable: false,
                render: function (data, type, full, meta) {
                    return $.enumToName(ProductStatusEnum, data);
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
                    var btn;
                    if (item['status'] === 1) {
                        btn = '<button style="width:80px;" type="button" class="btn btn-sm btn-primary" onclick="UpdateProductStatus(\'' + item['asin'] + '\', ProductStatusEnum.Inactive)">Deactivate</button>';
                    }
                    else {
                        btn = '<button style="width:80px;" type="button" class="btn btn-sm btn-primary" onclick="UpdateProductStatus(\'' + item['asin'] + '\', ProductStatusEnum.Active)">Activate</button>';
                    }
                    btn += '<a class="btn btn-sm btn-info ml-2 mr-2" href="/admin/product/Save/' + item['asin'] + '">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-danger" type="button" onclick="DeleteProduct(\'' + item['asin'] + '\')">Delete</button>';

                    return btn;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 0] },
            { "className": "align-middle", "targets": '_all' }
        ],
        order: [[2, "DESC"]]
    });
});