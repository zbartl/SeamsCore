var Page = Page || {};

Page.Load = {

    init: function () {
        Page.Load.slots = $("[data-editable='slot']");
        Page.Load.slots.click(Page.Load.setupWysiwyg);
        Page.Load.primary = window.primary;;
        Page.Load.secondary = window.secondary;
        Page.Load.tertiary = window.tertiary;
    },

    setupWysiwyg: function (e) {
        var Save = function (context) {
            var ui = $.summernote.ui;

            // create button
            var button = ui.button({
                contents: '<i class="fa fa-child"/> Save',
                tooltip: 'save content',
                click: function () {
                    var html = context.invoke("code");
                    var id = context.options.id;
                    Page.Load.saveContent(id, html);
                    context.invoke("destroy");
                }
            });
            return button.render();   // return button as jquery object 
        };

        var Image = function (context) {
            var ui = $.summernote.ui;

            // create button
            var button = ui.button({
                contents: '<i class="fa fa-child"/> Image',
                tooltip: 'insert image',
                click: function () {

                    function loadImages(subDirectory) {
                        $.ajax({
                            url: "/documents/images/" + (subDirectory ? subDirectory : ""),
                            type: "get",
                            success: function (view) {
                                if ($("#image_upload_modal").length > 0) {
                                    $("#image_upload_modal .modal-body").html(view);
                                }
                                else {
                                    var customModal =
                                        $('<div id="image_upload_modal" class="modal fade" tabindex="-1" role="dialog">' +
                                          '<div class="modal-dialog" role="document">' +
                                            '<div class="modal-content">' +
                                              '<div class="modal-body">' +
                                                view +
                                              '</div>' +
                                            '</div>' +
                                          '</div>' +
                                        '</div>'
                                        );
                                    $('body').append(customModal);
                                }

                                $('#image_upload_modal').modal();

                                $("#UploadTarget").on("load", function (e) {
                                    $("#upload_image_form").trigger("reset");
                                    var subDirectory = $(e.currentTarget).attr("data-dir");
                                    loadImages(subDirectory);
                                });
                            }
                        });
                    }

                    loadImages();

                    $(document).on("click", ".image_select", function (e) {
                        context.invoke('editor.insertImage', $(e.currentTarget).attr("data-img"));
                        $('#image_upload_modal').modal("toggle");
                    });

                    $(document).on("click", ".image_delete", function (e) {
                        var subDirectory = $(e.currentTarget).attr("data-dir");
                        var imageName = $(e.currentTarget).attr("data-img");

                        if (confirm("Are you sure you want to delete this image?")) {
                            $.post(
                                "/documents/images/delete",
                                {
                                    subDirectory: subDirectory,
                                    imageName: imageName
                                }
                            ).success(function() {
                                loadImages(subDirectory);
                            });
                        }
                    });

                    $(document).on("click", ".add_sub_directory", function (e) {
                        var subDirectory = $("#subDirectoryName").val();
                        $.post(
                            "/documents/images/create-directory",
                            {
                                subDirectory: subDirectory
                            }
                        ).success(function() {
                            loadImages(subDirectory);
                        });
                    });

                    $(document).on("click", ".directory_select", function (e) {
                        loadImages($(e.currentTarget).attr("data-dir"));
                    });

                    $(document).on("click", ".return_select", function (e) {
                        loadImages();
                    });
                }
            });
            return button.render();   // return button as jquery object 
        };

        $(e.currentTarget).summernote({
            minHeight: 300,
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['fontname', ['fontname']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'imageUpload']],
                ['view', ['fullscreen', 'codeview', 'help']],
                ['cms', ['save']]
            ],
            buttons: {
                save: Save,
                imageUpload: Image
            },
            id: $(e.currentTarget).attr("data-id")
        });
    },

    saveContent: function (id, html) {
        var action = "/page/save";
        var data = {
            ModifiedSlots: []
        };
        var modifiedSlot = {
            Primary: Page.Load.primary,
            Secondary: Page.Load.secondary,
            Tertiary: Page.Load.tertiary,
            SeaId: id,
            Html: html
        };
        data.ModifiedSlots.push(modifiedSlot);
        $.ajax({
            type: "POST",
            url: action,
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (d) {
                console.log(d);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        });
    }

}