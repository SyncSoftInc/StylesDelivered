var mainTable;

function DeleteReview(id) {
    bootbox.confirm("Delete Review?", function (confirm) {
        if (confirm) {
            axios.delete(`/api/admin/review/${id}`, { data: { ID: id } })
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

function ApproveReview(id) {
    axios.patch('/api/admin/review', { ID: id })
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
            url: '/api/admin/reviews'
        },
        columns: [
            { data: "orderNo" },
            { data: "sku" },
            { data: "user" },
            { data: "title" },
            {
                data: "status",
                width: 30,
                render: function (data, type, item) {
                    if (!$.isNW(data)) {
                        return $.enumToName(ReviewStatusEnum, data);
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
                    var disabled = item.status === ReviewStatusEnum.Approved ? "disabled" : "";

                    var btns =
                        `<button class="btn btn-sm btn-primary" type="button" style="width:70px;" onclick="ApproveReview('${item.id}')" ${disabled}>Approve</button>` +
                        `<a class="btn btn-sm btn-info mr-2 ml-2" href="/admin/review/Detail/${item.id}">Detail</a>` +
                        `<button class="btn btn-sm btn-danger" type="button" onclick="DeleteReview('${item.id}')" ${disabled}>Delete</button>`;

                    return btns;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center", "targets": [-1] }
        ],
        order: [[6, "DESC"]]
    });
});