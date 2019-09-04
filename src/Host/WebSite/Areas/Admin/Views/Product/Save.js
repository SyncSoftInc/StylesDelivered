var saveVM = new Vue({
    el: "#app",
    data: {
        isNew: true,
        itemNo: "",
        title: "New",
        productItem: {
            invQty: 0
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.isNew = self.itemNo == "";
            self.title = self.isNew ? "Edit" : "New";

            if (self.itemNo != "") {
                $.get("/api/product/item/" + self.itemNo, function (rs) {
                    rs.imageUrl = $.pic(rs.imageUrl);
                    self.productItem = rs;
                });
            }
        },
        save: function () {
            var self = this;
            var actionType = self.itemNo != "" ? 'PUT' : 'POST';

            $.ajax({
                url: '/api/product/item',
                type: actionType,
                data: {
                    ProductItem: self.productItem
                },
                success: function (rs) {
                    if ($.isSuccess(rs)) {
                        self.itemNo = self.productItem.itemNo;
                        bootbox.alert("Save successfully.", function () {
                            window.location = "/admin/product/Save/" + self.itemNo;
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
        if (last != "Save") self.itemNo = last;

        self.loadData();
    }
});

// Image Upload
Dropzone.autoDiscover = false;
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
            formData.append('PostData[ItemNo]', saveVM.productItem.itemNo);
        });
        myDropzone.on("success", function (files, response) {
            if (response.isSuccess) {
                saveVM.productItem.imageUrl = response.result.imageUrl;
            }
            else {
                bootbox.alert(respones.msgCode);
            }

            $("#uploadModal").modal('toggle');
            myDropzone.removeAllFiles();
        });
    }
});