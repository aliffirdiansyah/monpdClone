if (pagename == 'approvalnpwpd') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor() {
        $(document).ready(function () {
            var data = {};

            AddDefaultParam(data);

            var cols = [
                {
                    "data": "SeqPel", 
                    render: function (data, type, row, meta) {
                        var npwpd = row.ThnPel.toString().padStart(4, '0') + row.BlnPel.toString().padStart(2, '0') + row.SeqPel.toString().padStart(5, '0');
                        return npwpd;
                    }
                },
                { "data": "TglPermohonan" },
                { "data": "JenisWp", render: fnCreateEJenisWP },
                { "data": "IdIdentity" },
                { "data": "NamaWp" },
                { "data": "AlamatWp" },
                { "data": "Email" },
                { "data": "NikPJwb" },
                { "data": "Sumber", render: fnCreateSumberInputan },
                { "data": "InsDate" },
                {
                    "data": "EncryptedKey",
                    render: function (data, type, row, meta) {
                        return '<div class="hstack gap-3 flex-wrap"><a href="javascript:void(0);" class="link-success fs-15 linkeditsetapprovenpwpd"><i class="ri-edit-2-line" title="Approve NPWPD"></i></a></div>';
                    }
                }
            ];
            // contoh ada kolom edit 
            //var edit = { callback: Ubah, column: { "data": "EncryptedKey", } };
            // contoh tidak ada kolom edit 
            var edit = null;

            t = fnCreateTable('tbl', strUrlGetList, data, cols, null, edit);
        });
    }

    function Ubah(id) {
        fnOpenModalEditBlazor('staticBackdropLarge', id, strUrlAdd);
    }

    function SetApproveNpwpd(id) {
        var idmodal = 'staticBackdropLarge';
        var url = strUrlSetApproveNpwpd;
        var data = { id: id };

        //console.log(url);

        AddDefaultParam(data);

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {
                $("#" + idmodal + " .modal-body").html(data);
                $("#" + idmodal + " .modal-title").text('Approve Npwpd');
                $("#" + idmodal).modal('show');
            }
        });
    }

    function fnApprove() {
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});

        AddDefaultParam(data);

        return fnCallAjaxPost(strUrlApprove, data, fnSearch, 'Data Tersimpan');
    }
    function fnSave() {
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});

        AddDefaultParam(data);

        return fnCallAjaxPost(strUrlSaveDraft, data, fnSearch, 'Data Tersimpan');
    }

    function fnSearch() {
        t.clearPipeline();
        t.draw();
    }

    function fnCreateItem(obj) {
        obj.id = obj.id || obj.Id;
        obj.text = obj.text || obj.Nama;

        return obj;
    }

    function fnAddInit(url) {
        fnCreateSelect2('IdDokumen', url, fnCreateItem);
    }
}