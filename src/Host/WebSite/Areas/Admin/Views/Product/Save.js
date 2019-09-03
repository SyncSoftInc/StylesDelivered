var saveVM = new Vue({
    el: "#app",
    data: {
        itemNo: "",
        title: "New",
        productItem: {
            invQty: 0
        }
    },
    methods: {
        loadData: function () {
            var self = this;
            self.title = self.itemNo != "" ? "Edit" : "New";

            if (self.itemNo != "") {
                $.get("/api/product/item/" + self.itemNo, function (rs) {
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
                        bootbox.alert("Save successfully.");
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
var dropzone = new Dropzone('#mydropzone', {
    parallelUploads: 1,
    //autoProcessQueue: false,
    addRemoveLinks: true,
    dictResponseError: 'Error uploading file!',
    //previewsContainer: false,
    maxFilesize: 1,
    dictDefaultMessage: '<span class="text-center"><span class="font-lg visible-xs-block visible-sm-block visible-lg-block"><span class="font-lg">Drop files to upload</span> <span>&nbsp&nbsp<h4 class="display-inline"> (Or Click)</h4></span>',
    dictResponseError: 'Error uploading file!',
    acceptedFiles: "image/*",
    init: function () {
        var myDropzone = this;

        myDropzone.on("addedfile", function (file) {
            //var image = $("#dz-image").src();
            //$("#productItemImg").src(file);
        });

        //myDropzone.on("sending", function (file, xhr, formData) {
        //    // post extra data
        //    var postData = {
        //        width: !$.isNW(file.width) ? file.width : 0,
        //        height: !$.isNW(file.height) ? file.height : 0
        //    }
        //    formData.append('PostData[' + file.name + ']', JSON.stringify(postData));
        //});

        //myDropzone.on("queuecomplete", function (progress) {
        //    var image = $(".dz-image").find("img")[0].src;
        //    $("#productItemImg").src = image;
        //});
    }
});