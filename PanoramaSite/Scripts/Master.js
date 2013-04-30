/// <reference path="Scripts/jquery-1.6.4.js" />
/// <reference path="Scripts/jquery-ui-1.10.2.js" />

$(function () {
    $(".column-list").sortable();
    $(".column-drop-area").sortable({
        connectWith: ".column-drop-area"
    });

    $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
              .find(".portlet-header")
                .addClass("ui-widget-header ui-corner-all")
                .prepend("<span class='ui-icon ui-icon-minusthick'></span>")
                .end()
              .find(".portlet-content");

    $(".portlet-header .ui-icon").click(function () {
        $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
        $(this).parents(".portlet:first").find(".portlet-content").toggle();
    });

    $("#collapse-all-action").click(function () {
        $(".portlet-header .ui-icon").each(function () {
            if ($(this).hasClass('ui-icon-minusthick')) {
                $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
                $(this).parents(".portlet:first").find(".portlet-content").toggle();
            }
        });
    });

    $("#collapse-all-action").disableSelection ()

    $(".column").disableSelection();
    document.title = "Project Name";
});

