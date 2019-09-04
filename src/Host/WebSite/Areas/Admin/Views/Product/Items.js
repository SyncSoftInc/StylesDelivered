var mainTable;

$(function () {
    mainTable = $('#mainTable').DataTable({
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [5, 10, 25],
        ajax: {
            url: '/api/product/items'
        },
        columns: [
            {
                width: 100,
                orderable: false,
                render: function (id, display, item) {
                    return '<img src="' + item.imageUrl + '" style="width:50px;height:50px;" />';
                }
            },
            {
                data: "itemNo"
            },
            {
                data: "createdOnUtc",
                width: 150,
                render: function (data, type, full, meta) {
                    return $.timeFormat(data);
                }
            },
            {
                width: 20,
                orderable: false,
                render: function (id, display, item) {
                    return '<a class="btn btn-sm btn-primary" href="/admin/product/Save/' + item['itemNo'] + '">Edit</a>';
                }
            }
        ],
        order: [[2, "DESC"]]
    });
});