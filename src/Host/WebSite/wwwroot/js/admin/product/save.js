var itemsTable;
Dropzone.autoDiscover = false;

var saveVM = new Vue({
    el: "#app",
    data: {
        isNew: true,
        title: "New",
        product: {
            asin: null
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.isNew = $.isNW(self.product.asin);
            self.title = self.isNew ? "New" : "Edit";

            if (!self.isNew) {
                $.get("/api/product/" + self.product.asin, function (rs) {
                    rs.imageUrl = $.isNW(rs.imageUrl) ? $.pic(null, 100, 100) : $.pic(rs.imageUrl);
                    self.product = rs;
                });
            }
        },
        save: function () {
            var self = this;
            var actionType = self.isNew ? 'POST' : 'PUT';

            $.ajax({
                url: '/api/product',
                type: actionType,
                data: {
                    Product: self.product
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        bootbox.alert("Save successfully.", function () {
                            window.location = "/admin/product/Save/" + self.product.asin;
                        });
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });

            return false;
        }
    },
    beforeMount: function () {
        var self = this;
        var url = window.location.href;
        var last = url.substring(url.lastIndexOf('/') + 1);
        if (last !== "Save") self.product.asin = last;

        self.loadData();
    }
});

var itemVM = new Vue({
    el: "#itemModal",
    data: {
        isNew: true,
        item: {
            sku: null,
            invQty: 0
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.isNew = $.isNW(self.item.sku);

            if (!self.isNew) {
                $.get("/api/product/item", { asin: saveVM.product.asin, sku: self.item.sku }, function (rs) {
                    self.item = rs;
                });
            }
            else {
                self.item = { asin: saveVM.product.asin, invQty: 0 };
            }
        },
        save: function () {
            var self = this;
            var actionType = self.isNew ? 'POST' : 'PUT';

            $.ajax({
                url: '/api/product/item',
                type: actionType,
                data: {
                    ProductItem: self.item
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        $("#itemModal").modal("toggle");
                        itemsTable.ajax.reload();
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });

            return false;
        },
        show: function () {
            bootbox.alert("aaa");
        }
    }
});

// Item Delete
function DeleteItem(sku) {
    bootbox.confirm("Delete product?", function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: '/api/product/item',
                data: { asin: saveVM.product.asin, sku: sku },
                type: 'DELETE',
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        itemsTable.ajax.reload();
                    }
                    else {
                        bootbox.alert(rs);
                    }
                }
            });
        }
    });
}

// Items Table
function createTable() {
    itemsTable = $('#itemsTable').DataTable({
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [5, 10, 25],
        ajax: {
            url: '/api/product/items',
            data: {
                "asin": saveVM.product.asin
            }
        },
        columns: [
            { data: "sku" },
            { data: "alias", orderable: false },
            { data: "color", orderable: false },
            { data: "size", orderable: false },
            {
                data: "invQty",
                width: 50,
                orderable: false
            },
            {
                width: 150,
                orderable: false,
                render: function (id, display, item) {
                    return '<button class="editBtn btn btn-sm btn-primary mr-2" type="button" data-toggle="modal" data-target="#itemModal"  data-id="' + item['sku'] + '">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-danger" type="button" onclick="DeleteItem(\'' + item['sku'] + '\')">Delete</button>';
                }
            }
        ],
        columnDefs: [
            { "className": "text-center", "targets": [-1] }
        ],
        order: [[1, "DESC"]]
    });
}

$(function () {
    $('#items-tab').on('shown.bs.tab', function () {
        if ($.isNW(itemsTable)) {
            createTable();
        }
    });

    // ItemModal events
    $('#itemModal').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        itemVM.item.sku = btn.data('id');
        itemVM.loadData();
    });

    $('#itemModal').on('click', '#itemSaveBtn', function () {
        var btn = $(this);
        itemVM.save();
    });

    // Image Upload
    $("#mydropzone").dropzone({
        parallelUploads: 1,
        maxFilesize: 1,
        autoProcessQueue: false,
        addRemoveLinks: true,
        dictDefaultMessage: '<span class="text-center"><span class="font-lg visible-xs-block visible-sm-block visible-lg-block"><span class="font-lg">Drop files to upload</span> <span>&nbsp&nbsp<h4 class="display-inline"> (Or Click)</h4></span>',
        dictResponseError: 'Error uploading file!',
        acceptedFiles: "image/*",
        init: function () {
            var myDropzone = this;

            $("#uploadBtn").on("click", function (e) {
                e.preventDefault();
                e.stopPropagation();

                if (myDropzone.getQueuedFiles().length > 0) {
                    myDropzone.processQueue();
                }
                else {
                    bootbox.alert("Please add files to upload.");
                }
            });
            myDropzone.on("removedfile", function (file) {
                if (myDropzone.files.length <= 0) {
                    $('#uploadBtn').attr('disabled', true);
                }
            });
            myDropzone.on("addedfile", function (file) {
                if (myDropzone.files.length > 0) {
                    $('#uploadBtn').attr('disabled', false);
                }
                if (myDropzone.files[1] !== null) {
                    myDropzone.removeFile(myDropzone.files[0]);
                }
            });
            myDropzone.on("sending", function (file, xhr, formData) {
                // post extra data
                formData.append('PostData[ASIN]', saveVM.product.asin);
            });
            myDropzone.on("success", function (files, response) {
                if (response.isSuccess) {
                    saveVM.product.imageUrl = response.result.imageUrl;
                }
                else {
                    bootbox.alert(respones.msgCode);
                }

                $("#uploadModal").modal('toggle');
                myDropzone.removeAllFiles();
            });
        }
    });
});