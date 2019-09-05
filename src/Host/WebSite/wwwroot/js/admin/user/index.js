var mainTable;

$(function () {
    mainTable = $('#mainTable').DataTable({
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [5, 10, 25],
        ajax: {
            url: '/api/users'
        },
        columns: [
            {
                width: 50,
                orderable: false,
                render: function (id, display, item) {
                    return '<img src="' + $.pic(item.imageUrl) + '" class="pic_s" />';
                }
            },
            {
                data: "itemNo"
            },
            {
                data: "productName"
            },
            {
                data: "createdOnUtc",
                width: 150,
                render: function (data, type, full, meta) {
                    return $.timeFormat(data);
                }
            },
            {
                width: 120,
                orderable: false,
                render: function (id, display, item) {
                    return '<a class="btn btn-sm btn-primary mr-2" href="/admin/product/Save/' + item['itemNo'] + '">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-secondary" type="button" data-id="' + item['itemNo'] + '">Delete</button>';
                }
            }
        ],
        columnDefs: [
            { "className": "text-center align-middle", "targets": [-1, 0] },
            { "className": "align-middle", "targets": '_all' }
        ],
        order: [[2, "DESC"]]
    });

    $('#mainTable').on('click', '.delBtn', function () {
        var btn = $(this);
        bootbox.confirm("Delete item?", function (confirm) {
            if (confirm) {
                var itemNo = btn.data('id');

                $.ajax({
                    url: '/api/product/item/' + itemNo,
                    type: 'DELETE',
                    success: function (rs) {
                        if ($.isSuccess(rs)) {
                            window.location = "/admin/product/items";
                        }
                        else {
                            bootbox.alert(rs);
                        }
                    }
                });
            }
        });
    });
});