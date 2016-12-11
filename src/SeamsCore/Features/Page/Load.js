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
        }

        $(e.currentTarget).summernote({
            minHeight: 300,
            toolbar: [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['fontname', ['fontname']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture', 'video']],
                ['view', ['fullscreen', 'codeview', 'help']],
                ['cms', ['save']]
            ],
            buttons: {
                save: Save
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