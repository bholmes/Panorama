﻿/// <reference path="jquery-1.6.4.js" />
/// <reference path="jquery-ui-1.10.2.js" />
/// <reference path="jquery.signalR-1.1.1.js" />
/// <reference path="signalr/hubs" />


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
        refreshProjectView();
    });

    $("#collapse-all-action").disableSelection ()

    document.title = "Project Name";

    refreshProjectView();

    hub = $.connection.projecthub;
    hub.client.refresh_project = function (data) {
        refreshProjectView();
    };

    $.connection.hub.start();
});

function storeCollapseList() {
    var cardCollapseList = [];
    $(".portlet-header .ui-icon").each(function () {
        var cardId = this.parentElement.parentElement.id;
        if ($(this).hasClass('ui-icon-minusthick')) {
            cardCollapseList[cardId] = false;
        }
        else {
            cardCollapseList[cardId] = true;
        }
    });

    return cardCollapseList;
}

function refreshProjectView() {
    var collapseList = storeCollapseList();
    //var project = {
    //    "lists": [
    //        {
    //            "id": 1, "title": "Ideas", cards: [
    //                { "id": 3, "title": "Drink" },
    //                { "id": 5, "title": "Play" },
    //            ]
    //        },
    //        {
    //            "id": 2, "title": "Todo", cards: [
    //                { "id": 1, "title": "Eat" },
    //                { "id": 2, "title": "Sleep" },
    //                { "id": 4, "title": "Work" },
    //            ]
    //        },
    //        {
    //            "id": 3, "title": "In Porgress", cards: [
    //            ]
    //        },
    //        {
    //            "id": 4, "title": "Completed", cards: [
    //            ]
    //        }]
    //};

    $.getJSON('servicestack/projectcontents/1?format=json', function (project) {

        $("#column-list").empty();

        $(".project-name-header").first().text(project.Title);

        project.Lists.forEach(function (list) {
            var newColumn = $('<div class="column ui-widget ui-helper-clearfix ui-corner-all">');
            newColumn.attr('id', 'column-' + list.Id);
            newColumn.disableSelection();

            var newColumnHeader = $('<div class="column-header">');
            newColumnHeader.append(list.Title);

            var newColumnDropArea = $('<div class="column-drop-area">');

            list.Cards.forEach(function (card) {
                addCardToColumnDropArea(card, newColumnDropArea, collapseList);
            });

            newColumnDropArea.sortable({
                connectWith: ".column-drop-area",
                update: function (event, ui) {
                    if (this === ui.item.parent()[0]) {
                        var columnId = parseInt($(this).parents(".column:first").attr('id').substr (7));
                        var cardId = parseInt(ui.item.attr('id').substr (8));

                        //alert('Move ' + cardId + ' to position ' + ui.item.index() + ' in list ' + columnId);
                        var postData = { Project:1 , List: columnId, Card: cardId, Index: ui.item.index() };
                        jQuery.ajax({
                            type: 'PUT',
                            url: 'servicestack/movecard',
                            data: postData,
                            success: null,
                            dataType: 'json'
                        });
                    }
                }
            });

            newColumn.append(newColumnHeader);
            newColumn.append(newColumnDropArea);
            $("#column-list").append(newColumn);
            $("#column-list").sortable({
                update: function (event, ui) {
                    if (this === ui.item.parent()[0]) {
                        var columnId = parseInt(ui.item.attr('id').substr(7));
                        var postData = { Project: 1, List: columnId, Index: ui.item.index() };
                        //alert('drop list ' + postData.List + ' at pos ' + postData.Index);
                        jQuery.ajax({
                            type: 'PUT',
                            url: 'servicestack/movelist',
                            data: postData,
                            success: null,
                            dataType: 'json'
                        });

                    }
                }
            });
        });
    });
}

function addCardToColumnDropArea(card, columnDropArea, collapseList) {
    var newPortlet = $('<div class="portlet ui-widget ui-widget-content ui-helper-clearfix ui-corner-all">');
    var cardId = 'portlet-' + card.Id;
    newPortlet.attr('id', cardId);

    var newPortletHeader = $('<div class="portlet-header ui-widget-header ui-corner-all">');

    var newPortletHeaderSpan = $("<span class='ui-icon'></span>");

    newPortletHeaderSpan.click(function () {
        $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
        $(this).parents(".portlet:first").find(".portlet-content").toggle();
    });

    newPortletHeader.append(newPortletHeaderSpan);
    newPortletHeader.append(card.Title);

    var newPortlerContent = $('<div class="portlet-content">');
    newPortlerContent.append('Lorem ipsum dolor sit amet, consectetuer adipiscing elit');

    newPortlet.append(newPortletHeader);
    newPortlet.append(newPortlerContent);

    columnDropArea.append(newPortlet);

    if (collapseList[cardId]) {
        newPortletHeaderSpan.addClass("ui-icon-plusthick");
        newPortlerContent.hide();
    }
    else {
        newPortletHeaderSpan.addClass("ui-icon-minusthick");
    }
}

