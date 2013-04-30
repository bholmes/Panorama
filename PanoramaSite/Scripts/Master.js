/// <reference path="jquery-1.6.4.js" />
/// <reference path="jquery-ui-1.10.2.js" />


$(function () {
    $("#collapse-all-action").click(function () {
        $(".portlet-header .ui-icon").each(function () {
            if ($(this).hasClass('ui-icon-minusthick')) {
                $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
                $(this).parents(".portlet:first").find(".portlet-content").toggle();
            }
        });
    });

    $("#refresh-lists-action").click(function () {
        $("#column-list").empty();
        refreshProjectView();
    });

    $("#collapse-all-action").disableSelection ()

    document.title = "Project Name";

    refreshProjectView();
});

function refreshProjectView() {
    var project = {
        "lists": [
            {
                "id": 1, "title": "Ideas", cards: [
                    { "id": 3, "title": "Drink" },
                    { "id": 5, "title": "Play" },
                ]
            },
            {
                "id": 2, "title": "Todo", cards: [
                    { "id": 1, "title": "Eat" },
                    { "id": 2, "title": "Sleep" },
                    { "id": 4, "title": "Work" },
                ]
            },
            {
                "id": 3, "title": "In Porgress", cards: [
                ]
            },
            {
                "id": 4, "title": "Completed", cards: [
                ]
            }]
    };

    project.lists.forEach(function (list) {
        var newColumn = $('<div class="column ui-widget ui-helper-clearfix ui-corner-all">');
        newColumn.attr('id', 'column-' + list.id);
        newColumn.disableSelection();

        var newColumnHeader = $('<div class="column-header">');
        newColumnHeader.append(list.title);

        var newColumnDropArea = $('<div class="column-drop-area">');

        list.cards.forEach(function (card) {
            addCardToColumnDropArea(card, newColumnDropArea);
        });

        newColumnDropArea.sortable({
            connectWith: ".column-drop-area"
        });

        newColumn.append(newColumnHeader);
        newColumn.append(newColumnDropArea);
        $("#column-list").append(newColumn);
        $("#column-list").sortable();
    });
}

function addCardToColumnDropArea(card, columnDropArea) {
    var newPortlet = $('<div class="portlet ui-widget ui-widget-content ui-helper-clearfix ui-corner-all">');
    newPortlet.attr('id', 'portlet-' + card.id);

    var newPortletHeader = $('<div class="portlet-header ui-widget-header ui-corner-all">');

    var newPortletHeaderSpan = $("<span class='ui-icon ui-icon-minusthick'></span>");
    newPortletHeaderSpan.click(function () {
        $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
        $(this).parents(".portlet:first").find(".portlet-content").toggle();
    });

    newPortletHeader.append(newPortletHeaderSpan);
    newPortletHeader.append(card.title);

    var newPortlerContent = $('<div class="portlet-content">');
    newPortlerContent.append('Lorem ipsum dolor sit amet, consectetuer adipiscing elit');

    newPortlet.append(newPortletHeader);
    newPortlet.append(newPortlerContent);

    columnDropArea.append(newPortlet);
}

