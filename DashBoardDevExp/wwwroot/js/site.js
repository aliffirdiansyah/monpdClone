// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var token = $.cookie("XSRF-TOKEN");
var headers = { __RequestVerificationToken: token };
var reqHeaders = { RequestVerificationToken: token };
var pagename = '';
var tblExport;
var t = $("input[name='__RequestVerificationToken']").val();
//headers['__RequestVerificationToken'] = token;

$.ajaxSetup({
    headers: {
        RequestVerificationToken: token
    }
});

$(document).ready(function () {
    $(window).keydown(function (event) {
        if ((event.keyCode == 13)) {
            event.preventDefault();
            return false;
        }
    });
});

var exportOptions = {
    modifier: {
        page: 'all', search: 'none'
    },
    customizeData: function (d) {
        var exportBody = GetDataToExport();
        d.body.length = 0;
        d.body.push.apply(d.body, exportBody);
    },
};

var tombols = [
    {
        extend: 'copy',
        exportOptions: exportOptions,
    },
    {
        extend: 'csv',
        exportOptions: exportOptions,
    },
    {
        extend: 'excel',
        exportOptions: exportOptions,
    },
    {
        extend: 'pdf',
        exportOptions: exportOptions,
    },
    {
        extend: 'print',
        exportOptions: exportOptions,
    }, "colvis",
];


function GetDataToExport() {
    var data = tblExport.ajax.params();
    data.start = 0;
    data.length = -1;
    AddDefaultParam(data);

    var jsonResult = $.ajax({
        method: 'POST',
        url: strUrlGetList,
        data: data,
        success: function (result) { return result; },
        async: false
    });
    var exportBody = jsonResult.responseJSON.data;
    return exportBody.map(function (el) {
        return Object.keys(el).map(function (key) { return el[key] });
    });
}


var fnCreateEditColumn = function (data, type, row, meta) {
    return '<div class="hstack gap-3 flex-wrap"><a href="javascript:void(0);" class="link-success fs-15 linkedit"><i class="ri-edit-2-line"></i></a></div>';
}
var fnCreateAktifColumn = function (data, type, row, meta) {
    if (data == 1) { return '<span class="badge bg-success">Aktif</span>'; } else { return '<span class="badge bg-danger">Tidak Aktif</span>'; }
}


function fnCreateTable(tblId, urlGet, data, columns, btn, edit) {
    if (btn === null) {
        btn = tombols
    }
    if (edit != null) {
        edit.column['render'] = fnCreateEditColumn;
        columns.push(edit.column);
    }

    var t = $('#' + tblId).DataTable({
        ajax: DataTable.pipeline({
            url: urlGet, pages: 5, // number of pages to cache            
            method: 'POST', data: data
        }),
        "processing": true, "serverSide": true, "paging": true, "lengthChange": false, "searching": true, "ordering": true, "info": true, "autoWidth": true, "responsive": true,
        dom: 'Bfrtip',
        "order": [[0, 'asc']],
        "columns": columns,
        "buttons": btn,
    });
    t.buttons().container().appendTo('#tbl_wrapper .col-md-6:eq(0)');

    $('#' + tblId).on('click', 'tr .linkedit', function (e) {
        let d = t.row(e.target.closest('tr')).data();

        edit.callback(d.EncryptedKey);
    });
    tblExport = t;
    return t;

}


function AddDefaultParam(param) {
    Object.assign(param, headers);
    //return param;
}

function fnOpenModal(title, body) {
    $("#staticBackdrop .modal-title").html(title);
    $("#staticBackdrop .modal-body").html(body);
    $("#staticBackdrop").modal('show');
}
function fnCloseModal() {
    $("#staticBackdrop .modal-title").html('');
    $("#staticBackdrop .modal-body").html('');
    $("#staticBackdrop").modal('toggle');

}

function fnCreateSelect2forModal(id, idmodal, url, callbackItem) {
    $('#' + id).select2({
        dropdownParent: $('#' + idmodal),
        allowClear: true,
        placeholder: 'Pilih',
        //dropdownAutoWidth: false,
        //dropdownCssClass: 'form-control',
        //width: 'resolve',
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term, // search term
                };
            },
            processResults: function (datax, params) {
                var data = $.map(datax.Data, function (obj) {
                    obj = callbackItem(obj);
                    return obj;
                });

                params.page = params.page || 1;

                return {
                    results: data,
                    pagination: {
                        more: false// (params.page * 30) < data.total_count
                    }
                };

            },

        }
    });
}

function fnCreateSelect2(id, url, callbackItem) {
    $('#' + id).select2({
        allowClear: true,
        placeholder: 'Pilih',
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term, // search term
                };
            },
            processResults: function (datax, params) {
                var data = $.map(datax.Data, function (obj) {
                    obj = callbackItem(obj);
                    return obj;
                });

                params.page = params.page || 1;

                return {
                    results: data,
                    pagination: {
                        more: false// (params.page * 30) < data.total_count
                    }
                };

            },

        }
    });
}

function fnCreateSelect2Selector(id, url, callbackItem) {
    $(id).select2({
        allowClear: true,
        placeholder: 'Pilih',
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term, // search term
                };
            },
            processResults: function (datax, params) {
                var data = $.map(datax.Data, function (obj) {
                    obj = callbackItem(obj);
                    return obj;
                });

                params.page = params.page || 1;

                return {
                    results: data,
                    pagination: {
                        more: false// (params.page * 30) < data.total_count
                    }
                };

            },

        }
    });
}

//function fnCreateSelect2forModal(id, idmodal, url, callbackItem,callbackParam) {
//    $('#' + id).select2({
//        dropdownParent: $('#' + idmodal),
//        allowClear: true,
//        placeholder: 'Pilih',
//        //dropdownAutoWidth: false,
//        //dropdownCssClass: 'form-control',
//        //width: 'resolve',
//        ajax: {
//            url: url,
//            dataType: 'json',
//            delay: 250,
//            data: function (params) {

//                data = {
//                    term: params.term, // search term
//                };
//                callbackParam(data);
//                return data;
//            },
//            processResults: function (datax, params) {
//                var data = $.map(datax.Data, function (obj) {
//                    obj = callbackItem(obj);
//                    return obj;
//                });

//                params.page = params.page || 1;

//                return {
//                    results: data,
//                    pagination: {
//                        more: false// (params.page * 30) < data.total_count
//                    }
//                };

//            },

//        }
//    });
//}

function fnInitflatpickr() {
    /**
     * flatpickr
     */

    var flatpickrExamples = document.querySelectorAll("[data-provider]");

    flatpickrExamples.forEach(function (item) {

        if (item.getAttribute("data-provider") == "flatpickr") {
            var dateData = {};
            var isFlatpickerVal = item.attributes;

            if (isFlatpickerVal["data-date-format"]) {
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-enable-time"]) {
                (dateData.enableTime = true),
                    (dateData.dateFormat =
                        isFlatpickerVal["data-date-format"].value.toString() + " H:i");
            }
            if (isFlatpickerVal["data-altFormat"]) {
                (dateData.altInput = true),
                    (dateData.altFormat =
                        isFlatpickerVal["data-altFormat"].value.toString());
            }
            if (isFlatpickerVal["data-minDate"]) {
                dateData.minDate = isFlatpickerVal["data-minDate"].value.toString();
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-maxDate"]) {
                dateData.maxDate = isFlatpickerVal["data-maxDate"].value.toString();
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-deafult-date"]) {
                dateData.defaultDate =
                    isFlatpickerVal["data-deafult-date"].value.toString();
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-multiple-date"]) {
                dateData.mode = "multiple";
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-range-date"]) {
                dateData.mode = "range";
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-inline-date"]) {
                (dateData.inline = true),
                    (dateData.defaultDate =
                        isFlatpickerVal["data-deafult-date"].value.toString());
                dateData.dateFormat =
                    isFlatpickerVal["data-date-format"].value.toString();
            }
            if (isFlatpickerVal["data-disable-date"]) {
                var dates = [];
                dates.push(isFlatpickerVal["data-disable-date"].value);
                dateData.disable = dates.toString().split(",");
            }
            if (isFlatpickerVal["data-allow-input"]) { (dateData.allowInput = false), (dateData.allowInput = isFlatpickerVal["data-allow-input"].value) }
            flatpickr(item, dateData);
        } else if (item.getAttribute("data-provider") == "timepickr") {
            var timeData = {};
            var isTimepickerVal = item.attributes;
            if (isTimepickerVal["data-time-basic"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.dateFormat = "H:i");
            }
            if (isTimepickerVal["data-time-hrs"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.dateFormat = "H:i"),
                    (timeData.time_24hr = true);
            }
            if (isTimepickerVal["data-min-time"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.dateFormat = "H:i"),
                    (timeData.minTime =
                        isTimepickerVal["data-min-time"].value.toString());
            }
            if (isTimepickerVal["data-max-time"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.dateFormat = "H:i"),
                    (timeData.minTime =
                        isTimepickerVal["data-max-time"].value.toString());
            }
            if (isTimepickerVal["data-default-time"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.dateFormat = "H:i"),
                    (timeData.defaultDate =
                        isTimepickerVal["data-default-time"].value.toString());
            }
            if (isTimepickerVal["data-time-inline"]) {
                (timeData.enableTime = true),
                    (timeData.noCalendar = true),
                    (timeData.defaultDate =
                        isTimepickerVal["data-time-inline"].value.toString());
                timeData.inline = true;
            }
            if (isTimepickerVal["data-allow-input"]) { (timeData.allowInput = false), (timeData.allowInput = isTimepickerVal["data-allow-input"].value) }

            flatpickr(item, timeData);
        }
    });

}

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

    //$('#frmNavigator input').remove();
    var form = $('#frmNavigator');
    form.attr('action', uri);
    form.attr('target', '_blank');

    //var fd = new FormData();
    $.each(data, function (key, value) {
        $('#frmNavigator input[name="' + key + '"]').remove();
    });
    form = $('#frmNavigator');
    $.each(data, function (key, value) {
        // fd.append(key, value);
        //$('td[name="tcol1"]')
        $('#frmNavigator input[name="' + key + '"]').remove();
        var input = $("<input>")
            .attr("type", "hidden")
            .attr("name", key).val(value);
        form.append($(input));
    });

    form.submit();
}




function fnGridDevExtremeExporting(e) {
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet('Employees');

    DevExpress.excelExporter.exportDataGrid({
        component: e.component,
        worksheet: worksheet,
        autoFilterEnabled: true
    }).then(function () {
        workbook.xlsx.writeBuffer().then(function (buffer) {
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Employees.xlsx');
        });
    });
}

function fnGridAutoNumber(cellElement, cellInfo) {
    cellElement.text(1 + cellInfo.row.rowIndex + (cellInfo.component.pageIndex() * cellInfo.component.pageSize()))
}

function fnShowDevExtremePopUp(title, content) {
    var popupDevExtreme = $('#popupDevExtreme').dxPopup('instance');
    popupDevExtreme.option('title', title);
    popupDevExtreme.show();
    $('#popupDevExtremeContent').html(content);
}

function fnToast(pesan, notif) {
    let direction = 'up-push';
    let position = 'bottom right';
    DevExpress.ui.notify({
        message: pesan,
        height: 60,
        width: 400,
        minWidth: 150,
        type: notif,
        displayTime: 5000,
        animation: {
            show: { type: "fade", duration: 400, from: 0, to: 1 },
            hide: { type: "fade", duration: 40, to: 0 }
        }
    },
        {
            position,
            direction
        });
}

function ToUpperFilterColumns(columns) {
    $.each(columns, function (index, column) {
        var defaultCalculateFilterExpression = column.calculateFilterExpression;
        column.calculateFilterExpression = function (value, selectedFilterOperation) {
            if (this.dataType === 'string' && !this.lookup && value) {
                return [this.dataField,
                selectedFilterOperation || 'contains',
                value.toUpperCase()]
            }
            else {
                return defaultCalculateFilterExpression.apply(this, arguments);
            }
        }
    });
}  