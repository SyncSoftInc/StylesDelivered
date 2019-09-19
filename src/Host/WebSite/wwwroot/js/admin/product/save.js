var itemsTable;
Dropzone.autoDiscover = false;

var productVM = new Vue({
    el: "#app",
    data: {
        isNew: true,
        title: "New",
        product: {
            asin: asinPara
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.isNew = $.isNW(self.product.asin);
            self.title = self.isNew ? "New" : "Edit";

            if (!self.isNew) {
                $.get("/api/admin/product/" + self.product.asin, function (rs) {
                    rs.imageUrl = $.isNW(rs.imageUrl) ? $.pic(null, 150, 150) : $.pic(rs.imageUrl);
                    self.product = rs;
                });
            }
        },
        save: function () {
            var self = this;
            var actionType = self.isNew ? 'POST' : 'PUT';

            $.ajax({
                url: '/api/admin/product',
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
                $.get("/api/product/item", { asin: productVM.product.asin, sku: self.item.sku }, function (rs) {
                    self.item = rs;
                });
            }
            else {
                self.item = { asin: productVM.product.asin, invQty: 0 };
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
        }
    }
});

// Items Table
function createTable() {
    itemsTable = $('#itemsTable').DataTable({
        responsive: true,
        serverSide: true,
        searchDelay: 500,
        lengthMenu: [10, 15, 20],
        ajax: {
            url: '/api/product/items',
            data: {
                "asin": productVM.product.asin
            }
        },
        columns: [
            { data: "sku" },
            { data: "alias" },
            { data: "color", orderable: false },
            { data: "size", orderable: false },
            {
                data: "invQty",
                width: 50,
                orderable: false
            },
            {
                data: "holdInvQty",
                width: 80,
                orderable: false
            },
            {
                width: 120,
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
        order: [[0, "DESC"]]
    });
}

// Item Delete
function DeleteItem(skuIn) {
    bootbox.confirm("Delete product?", function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: '/api/product/item',
                data: { asin: productVM.product.asin, sku: skuIn },
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
                if (!$.isNW(myDropzone.files[1])) {
                    myDropzone.removeFile(myDropzone.files[0]);
                }
            });
            myDropzone.on("sending", function (file, xhr, formData) {
                // post extra data
                formData.append('PostData[ASIN]', productVM.product.asin);
            });
            myDropzone.on("success", function (files, response) {
                if (response.isSuccess) {
                    productVM.product.imageUrl = response.result.imageUrl;
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