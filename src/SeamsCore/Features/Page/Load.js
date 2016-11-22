var Page = Page || {};

Page.Load = {

    init: function () {
        Page.Load.slots = $("[data-editable='slot']");

        Page.Load.slots.on("click", Page.Load.setupWysiwyg(this));
        Page.Load.setupWysiwyg();
    },

    setupWysiwyg: function (el) {
        var quill = new Quill(el, {
            theme: 'snow'
        });
    }

}