var itemsTable;
Dropzone.autoDiscover = false;

$(function () {
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
            if (last != "Save") self.product.asin = last;

            self.loadData();
        }
    });

    var itemVM = new Vue({
        el: "#itemsModal",
        data: {
            item: {
                sku: null
            }
        },
        methods: {
            loadData: function () {
                var self = this;

                if (!$.isNW(self.item.sku)) {
                    $.get("/api/product/item", { asin: saveVM.product.asin, sku: self.item.sku }, function (rs) {
                        self.item = rs;
                    });
                }
            },
            save: function () {
                //var self = this;
                //var actionType = self.isNew ? 'POST' : 'PUT';

                //$.ajax({
                //    url: '/api/product',
                //    type: actionType,
                //    data: {
                //        Product: self.product
                //    },
                //    success: function (rs) {
                //        if ($.isSuccess(rs)) {
                //            bootbox.alert("Save successfully.", function () {
                //                window.location = "/admin/product/Save/" + self.product.asin;
                //            });
                //        }
                //        else {
                //            bootbox.alert(rs);
                //        }
                //    }
                //});

                //return false;
            }
        },
        beforeMount: function () {
            var self = this;
            self.item.asin = saveVM.product.asin;
            self.loadData();
        }
    });

    // Items Table
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
                width: 120,
                orderable: false,
                render: function (id, display, item) {
                    return '<button class="btn btn-sm btn-primary mr-2" type="button" data-toggle="modal" data-target="#itemsModal">Edit</a>' +
                        '<button class="delBtn btn btn-sm btn-danger" type="button" data-id="' + item['asin'] + '">Delete</button>';
                }
            }
        ],
        columnDefs: [
            { "className": "text-center", "targets": [-1] },
        ],
        order: [[1, "DESC"]]
    });

    $('#itemsTable').on('click', '.delBtn', function () {
        var btn = $(this);
        bootbox.confirm("Delete product?", function (confirm) {
            if (confirm) {
                var asin = btn.data('id');

                $.ajax({
                    url: '/api/product/' + asin,
                    type: 'DELETE',
                    success: function (rs) {
                        if ($.isSuccess(rs)) {
                            window.location = "/admin/product";
                        }
                        else {
                            bootbox.alert(rs);
                        }
                    }
                });
            }
        });
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
                if (myDropzone.files[1] != null) {
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