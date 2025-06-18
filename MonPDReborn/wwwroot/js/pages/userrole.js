if (pagename == 'userrole') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor() {
        $(document).ready(function () {
            var data = {};
            AddDefaultParam(data);
            var cols = [
                { "data": "Id" },
                { "data": "Nama" },
                { "data": "Ket" },
                { "data": "Aktif", render: fnCreateAktifColumn },
                { "data": "InsDate" },
                { "data": "InsBy" },
                {
                    "data": "EncryptedKey",
                    render: function (data, type, row, meta) {
                        return '<div class="hstack gap-3 flex-wrap">' +
                            '<a href="javascript:void(0);" class="link-success fs-15" onclick="Ubah(\'' + data + '\')"><i class="ri-edit-2-line"></i></a>' +
                            '<a href="javascript:void(0);" class="link-primary fs-15" onclick="Detail(\'' + data + '\')"><i class="ri-list-check"></i></a>' +
                            '</div>';
                    }
                }
            ];
            // contoh ada kolom edit 
            var edit = null;
            // contoh tidak ada kolom edit 
            //var edit = null;

            t = fnCreateTable('tbl', strUrlGetList, data, cols, null, edit);
        });
    }

    function Ubah(id) {
        fnOpenModalEditBlazor('btnModalAdd', id, strUrlAdd);
    }

    function Detail(id) {
        var data = { id: id };
        AddDefaultParam(data);
        fngoUrlData(strUrlDetail, data);        
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
        obj.id = obj.Nama;
        obj.text = obj.Nama;
        return obj;
    }


}