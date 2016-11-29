var Page = Page || {};

Page.Load = {

    init: function () {
        Page.Load.slots = $("[data-editable='slot']");
        Page.Load.slots.click(Page.Load.setupWysiwyg);
    },

    setupWysiwyg: function (e) {
        var HelloButton = function (context) {
            var ui = $.summernote.ui;

            // create button
            var button = ui.button({
                contents: '<i class="fa fa-child"/> Hello',
                tooltip: 'hello',
                click: function () {
                    var html = context.invoke("code");
                    console.log(html);
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
                ['mybutton', ['hello']]
            ],
            buttons: {
                hello: HelloButton
            }
        });
    }

}