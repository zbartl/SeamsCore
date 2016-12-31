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
        }).on('drop', function (el, container) {
            var data = [];
            $(container).find(".mvseams_manage_folder[data-container-id]").each(function () {
                data.push(
                    { id: $(this).data("container-id"), type: $(this).data("container-type") }
                );
            });
            $.ajax({
                type: "POST",
                url: "/admin/changeprimarypriority",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                success: function (d) {
                    console.log(d);
                },
                error: function (xhr, textStatus, errorThrown) {
                    console.log(xhr.responseText);
                }
            });
        });

        $(".secondary_draggable").each(function () {
            dragula([this], {
                moves: function (el, container, handle) {
                    if (!handle.parentElement.classList.contains("dragula-handle-secondary")) {
                        return false;
                    }
                    return handle.parentElement.classList.contains("dragula-handle");
                }
            }).on('drop', function (el, container, source) {
                var data = [];
                $(container).find(".mvseams_manage_page[data-container-id]").each(function () {
                    data.push(
                        { id: $(this).data("container-id"), type: $(this).data("container-type") }
                    );
                });
                $.ajax({
                    type: "POST",
                    url: "/admin/changesecondarypriority",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    success: function (d) {
                        console.log(d);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        console.log(xhr.responseText);
                    }
                });
            });;
        });

        $(".tertiary_draggable").each(function () {
            dragula([this], {
                moves: function (el, container, handle) {
                    return handle.parentElement.classList.contains("dragula-handle");
                }
            }).on('drop', function (el, container, source) {
                var data = [];
                $(container).find(".mvseams_manage_page[data-container-id]").each(function () {
                    data.push(
                        { id: $(this).data("container-id"), type: $(this).data("container-type") }
                    );
                });
                $.ajax({
                    type: "POST",
                    url: "/admin/changesecondarypriority",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    success: function (d) {
                        console.log(d);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        console.log(xhr.responseText);
                    }
                });
            });;
        });
    }
}