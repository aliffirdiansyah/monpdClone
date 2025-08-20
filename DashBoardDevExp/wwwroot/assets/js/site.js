// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var token = $.cookie("XSRF-TOKEN");


$(document).ready(function () {
    $(window).keydown(function (event) {
        if ((event.keyCode == 13)) {
            event.preventDefault();
            return false;
        }
    });
});

$.ajaxSetup({
    headers: {
        RequestVerificationToken: token
    }
});


function fngoUrlData(url, data) {
    var uri = encodeURI(url);
    //alert(uri);
    var form = $('#frmNavigator');
    form.attr('action', uri);
    form.attr('target', '_self');

    $.each(data, function (key, value) {
        // fd.append(key, value);
        var input = $("<input>")
            .attr("type", "hidden")
            .attr("name", key).val(value);
        form.append($(input));
    })
    form.submit();
}

function fngoUrlNewTabData(url, data) {
    var uri = encodeURI(url);

    $('#frmNavigator input').remove();
    var form = $('#frmNavigator');
    form.attr('action', uri);
    form.attr('target', '_blank');

    //var fd = new FormData();
    $.each(data, function (key, value) {
        // fd.append(key, value);
        var input = $("<input>")
            .attr("type", "hidden")
            .attr("name", key).val(value);
        form.append($(input));
    });

    form.submit();
}

function fnGridAutoNumber(cellElement, cellInfo) {
    cellElement.text(1 + cellInfo.row.rowIndex + (cellInfo.component.pageIndex() * cellInfo.component.pageSize()))
}