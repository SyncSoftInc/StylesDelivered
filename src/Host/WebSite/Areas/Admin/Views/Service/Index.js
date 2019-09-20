var mainTable;

function TriggerJob(name, groupName) {
    axios.post('/api/service/job', { Name: name, GroupName: groupName });
};

$(function () {
    mainTable = $('#mainTable').DataTable({
        responsive: true,
        serverSide: false,
        searchDelay: 500,
        lengthMenu: [10, 15, 20],
        ajax: { url: '/api/service/jobs', dataSrc: "" },
        columns: [
            { data: "groupName" },
            { data: "name" },
            {
                width: 70,
                orderable: false,
                render: function (id, display, item) {
                    var btn = `<button type="button" class="btn btn-sm btn-info ml-2 mr-2" onclick="TriggerJob('${item.name}', '${item.groupName}')">Trigger</button>`
                    return btn;
                }
            }
        ],
        columnDefs: [
            { "className": "text-center ", "targets": [-1] },
        ],
        order: [[0, "DESC"]]
    });
});