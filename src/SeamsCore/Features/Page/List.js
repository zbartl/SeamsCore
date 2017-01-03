var Page = Page || {};

Page.List = {
    init: function () {
        this.initDragulas();
    },

    initDragulas: function () {
        dragula([document.getElementById("primary_draggable")], {
            moves: function (el, container, handle) {
                if (!handle.parentElement.classList.contains("dragula-handle-primary")) {
                    return false;
                }
                return handle.parentElement.classList.contains("dragula-handle");
            }
        }).on('drop', Page.List.updatePriority);

        $(".secondary_draggable").each(function () {
            dragula([this], {
                moves: function (el, container, handle) {
                    if (!handle.parentElement.classList.contains("dragula-handle-secondary")) {
                        return false;
                    }
                    return handle.parentElement.classList.contains("dragula-handle");
                }
            }).on('drop', Page.List.updatePriority);
        });

        $(".tertiary_draggable").each(function () {
            dragula([this], {
                moves: function (el, container, handle) {
                    return handle.parentElement.classList.contains("dragula-handle");
                }
            }).on('drop', Page.List.updatePriority);
        });
    },

    updatePriority: function (el, container, source) {
        var data = { Ids: [] };
        $(container).find(".manage_page[data-page-id]").each(function () {
            data.Ids.push($(this).attr("data-page-id"));
        });
        $.ajax({
            type: "POST",
            url: "/page/update-priority",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            success: function (d) {
                console.log(d);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        });
    }
}