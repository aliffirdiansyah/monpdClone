// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var token = $.cookie("XSRF-TOKEN");
var headers = { __RequestVerificationToken: token };
var pagename = '';
var tblExport;
//headers['__RequestVerificationToken'] = token;

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
function fngoUrlNewTabData( url, data) {
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


var fnCreateDetailColumn = function (data, type, row, meta) {
    return '<div class="hstack gap-3 flex-wrap"><a href="javascript:void(0);" class="link-success fs-15 linkedit"><i class="ri-edit-2-line"></i></a></div>';
}

var fnCreateEJenisPegawaiColumn = function (data, type, row, meta) {
    if (data == 0) {
        return '<span class="badge bg-success">NON PNS</span>';
    }
    else {
        return '<span class="badge bg-danger">PNS</span>';
    }
}

var fnCreateEJenisWP = function (data, type, row, meta) {
    if (data == 0) {
        return '<span class="badge bg-info">Perorangan</span>';
    }
    else {
        return '<span class="badge bg-info">Badan</span>';
    }
}

var fnCreateSumberInputan = function (data, type, row, meta) {
    if (data == 0) {
        return '<span class="badge bg-info">Mobile</span>';
    }
    else if (data == 1) {
        return '<span class="badge bg-info">Web</span>';
    }
    else {
        return '<span class="badge bg-info">Kantor</span>';
    }
}
var fnCreateMandatoriColumn = function (data, type, row, meta) {
    if (data == 1) { return '<span class="badge bg-success">Ya</span>'; } else { return '<span class="badge bg-danger">Tidak</span>'; }
}
var fnCreateEDokumenSendNotifStateColumn = function (data, type, row, meta) {
    str = "";
    switch (data) {
        case 0:
            str = "Created";
            break;
        case 1:
            str = "Send";
            break;
        case 2:
            str = "Receive";
            break;
        case 3:
            str = "Reject";
            break;
        case 4:
            str = "Approved";
            break;
        default:
            break;
    }

    return '<span>' + str + '</span>';
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
        "processing": true, "serverSide": true, "paging": true, "lengthChange": true, "searching": true, "ordering": true, "info": true, "autoWidth": true, "responsive": true,
        dom: 'Bfrtip',
        "order": [[0, 'asc']],
        "columns": columns,
        "buttons": btn,
    });
    t.buttons().container().appendTo('#tbl_wrapper .col-md-6:eq(0)');

    $(".dataTables_filter input")
        .unbind()
        .bind('keyup change', function (e) {
            if (e.keyCode == 13 || this.value == "") {
                t
                    .search(this.value)
                    .draw();
            }
        });

    //$('#' + tblId).on('click', 'tr .linkedit', function (e) {
    //    let d = t.row(e.target.closest('tr')).data();
    //    console.log(d);
    //    edit.callback(d.EncryptedKey);
    //});

    $('#' + tblId).on('click', 'tr .linkedit', function (e) {
        // Find the closest parent 'tr' element
        let $row = $(this).closest('tr');

        // Check if it's a child row
        if ($row.hasClass('child')) {
            // Traverse up to the parent row
            $row = $row.prev('tr');
        }

        // Get the DataTable row object
        let table = $('#' + tblId).DataTable();
        let row = table.row($row);

        // Access the data from the parent row
        let rowData = row.data();

        if (rowData && rowData.EncryptedKey) {
            edit.callback(rowData.EncryptedKey);
        }

        // Prevent the event from continuing
        return false;
    });

    $('#' + tblId).on('click', 'tr .linkeditwf', function (e) {
        let z = t.row(e.target.closest('tr')).data();

        Ubah(z.EncryptedKey);
    });

    $('#' + tblId).on('click', 'tr .linkwfdokumen', function (e) {
        let zz = t.row(e.target.closest('tr')).data();

        SetWfDok(zz.EncryptedKey);
    });

    $('#' + tblId).on('click', 'tr .linkwfact', function (e) {
        let zzz = t.row(e.target.closest('tr')).data();

        SetWfAct(zzz.EncryptedKey);
    });

    $('#' + tblId).on('click', 'tr .linkeditsetapprovenpwpd', function (e) {
        let $row = $(this).closest('tr');

        // Check if it's a child row
        if ($row.hasClass('child')) {
            // Traverse up to the parent row
            $row = $row.prev('tr');
        }

        // Get the DataTable row object
        let table = $('#' + tblId).DataTable();
        let row = table.row($row);

        // Access the data from the parent row
        let rowData = row.data();

        if (rowData && rowData.EncryptedKey) {
            SetApproveNpwpd(rowData.EncryptedKey)
        }

        // Prevent the event from continuing
        return false;
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

function fnOpenModalBlazor(id, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: headers,
        success: function (data) {
            //fnOpenModal('Tambah Ancaman', data);
            $("#" + id + " .modal-body").html(data);
            $("#" + id).modal('show');
        }
    });
}
function fnOpenModalEditBlazor(idmodal, id, url) {
    var data = { id: id };
    AddDefaultParam(data);

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (data) {
            //fnOpenModal('Tambah Ancaman', data);
            $("#" + idmodal + " .modal-body").html(data);
            $("#" + idmodal).modal('show');
        }
    });
}
function fnCloseModalBlazor(id) {
    $("#" + id).modal('toggle');
}

function fnCloseModalById(id) {
    $("#" + id).modal('toggle');
}

function fnNotifyError(msg) {
    fnToast("Error ! " + msg,
        { position: "center", close: false, class: 'bg-danger text-white' });
}

function fnNotifySuccess(msg) {
    fnToast(msg,
        { position: "center", close: false, class: 'bg-success text-white' });
}

function fnToast(msg, toastData) {
    Toastify({
        newWindow: true,
        text: msg,
        gravity: toastData.gravity,
        position: toastData.position,
        className: toastData.class,
        stopOnFocus: true,
        //offset: {
        //    x: toastData.offset ? 50 : 0, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
        //    y: toastData.offset ? 10 : 0, // vertical axis - can be a number or a string indicating unity. eg: '2em'
        //},
        duration: toastData.duration,
        close: toastData.close,//== "close" ? true : false,
        style:
            toastData.style == "style"
                ? {
                    background: "linear-gradient(to right, #0AB39C, #405189)",
                }
                : "",
    }).showToast();
}

function fnCallAjaxPost(url, data, onSuccessCallback, successMsg) {
    var result = false;
    $.ajax({
        async: false,
        type: "POST",
        url: url,
        data: data,
        success: function (data) {
            
            if (data.Status == 1) {
                result = true;
                fnNotifySuccess(successMsg);
                onSuccessCallback();
                
            } else {
                result = false;
                fnNotifyError(data.Message);
                
            }
        }
    });
    return result;
}



function fnCreateSelect2forModal(id, idmodal, url, callbackItem) {
    $('#' + id).select2({
        dropdownParent: $('#' + idmodal),
        dropdownAutoWidth: false,
        dropdownCssClass: 'form-control',
        width: 'resolve',
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
                var data = $.map(datax, function (obj) {
                    obj = callbackItem(obj);
                    //obj.id = obj.Id;
                    //obj.text = obj.Nama;
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
    var idmodal = $('#' + id).closest('.modal,.fade,.show').attr('id');
    fnCreateSelect2forModal(id, idmodal, url, callbackItem)

}

