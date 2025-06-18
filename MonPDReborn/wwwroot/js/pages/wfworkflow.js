if (pagename == 'wfworkflow') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor() {
        $(document).ready(function () {
            var data = {};

            AddDefaultParam(data);

            var cols = [
                
                { "data": "Nama" },
                { "data": "Ket" },
                {
                    "data": "Aktif",
                    render: fnCreateAktifColumn
                },
                { "data": "InsDate" },
                { "data": "InsBy" },
                {
                    "data": "EncryptedKey",
                    render: function (data, type, row, meta) {
                        return '<div class="hstack gap-3 flex-wrap"><a href="javascript:void(0);" class="link-success fs-15 linkeditwf"><i class="ri-edit-2-line" title="Edit WF"></i></a><a href="javascript:void(0);" class="link-secondary fs-15 linkwfdokumen"><i class="ri-file-list-3-line" title="Set WF Dokumen"></i></a><a href="javascript:void(0);" class="link-warning fs-15 linkwfact"><i class="ri-footprint-line" title="Set WF Activity"></i></a></div>';
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
        fnOpenModalEditBlazor('btnModalAdd', id, strUrlAdd);
    }

    function SetWfDok(id) {
        //fnOpenModalEditBlazor('btnModalAdd', id, strUrlAdd);

        //var idmodal = 'btnModalAdd';
        var idmodal = 'setWfModal';
        var url = strUrlSetWfDok;
        var data = { id: id };
        AddDefaultParam(data);               

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {
                $("#" + idmodal + " .modal-body").html(data);
                $("#" + idmodal + " .modal-title").text('Set Work Flow Dokumen');
                $("#" + idmodal).modal('show');
            }
        });
    }

    function SetWfAct(id) {
        //fnOpenModalEditBlazor('btnModalAdd', id, strUrlAdd);

        //var idmodal = 'btnModalAdd';
        var idmodal = 'setWfModal';
        var url = strUrlSetWfAct;
        var data = { id: id };

        //console.log(url);

        AddDefaultParam(data);

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {
                $("#" + idmodal + " .modal-body").html(data);
                $("#" + idmodal + " .modal-title").text('Set Work Flow Activity');
                $("#" + idmodal).modal('show');
            }
        });
    }

    function fnSave() {
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});

        

        var d = { WFWorkflow: data };

        AddDefaultParam(d);
        console.log(d);


        return fnCallAjaxPost(strUrlSaveDraft, d, fnSearch, 'Data Tersimpan');
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

    function fnAddInitDokumen(id, url) {
        fnCreateSelect2(id, url, fnCreateItem);
    }

    function fnAddInitActivity(id, url) {
        fnCreateSelect2(id, url, fnCreateItem);
    }
}