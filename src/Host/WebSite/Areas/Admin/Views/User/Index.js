var mainTable;

$(function () {
    mainTable = $('#mainTable').DataTable({
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [5, 10, 25],
        ajax: {
            url: '/api/admin/users'
        },
        columns: [
            { data: "id" },
            { data: "username" },
            { data: "email" },
            {
                data: "status",
                orderable: false
            },
            {
                data: "roles",
                orderable: false
            },
            {
                width: 120,
                orderable: false,
                render: function (id, display, item) {
                    return '<a class="btn btn-sm btn-primary mr-2" href="/admin/user/Save/' + item['id'] + '">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-secondary" type="button" data-id="' + item['id'] + '">Delete</button>';
                }
            }
        ],
        order: [[0, "DESC"]]
    });

    $('#mainTable').on('click', '.delBtn', function () {
        var btn = $(this);
        bootbox.confirm("Delete user?", function (confirm) {
            if (confirm) {
                var id = btn.data('id');

                $.ajax({
                    url: '/api/admin/user/' + id,
                    type: 'DELETE',
                    success: function (rs) {
                        if ($.isSuccess(rs)) {
                            window.location = "/admin/user";
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