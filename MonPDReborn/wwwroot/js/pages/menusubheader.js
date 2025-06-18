if (pagename == 'menusubheader') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor()
    {
        $(document).ready(function () {
            var data = {};
            AddDefaultParam(data);
            var cols = [
                { "data": "Id" },
                { "data": "Nama" },
                { "data": "ControllerAction.Nama" },
                { "data": "Icon" },
                { "data": "Ket" },
                { "data": "Aktif", render: fnCreateAktifColumn },
                { "data": "InsDate" },
                { "data": "InsBy" }
            ];
            // contoh ada kolom edit 
            var edit = {
                callback: Ubah,
                column: { "data": "EncryptedKey",}
            };
            // contoh tidak ada kolom edit 
            //var edit = null;


            t = fnCreateTable('tbl', strUrlGetList, data, cols, null, edit);
        });
    }

    function Ubah(id)
    {
        fnOpenModalEditBlazor('btnModalAdd', id, strUrlAdd);
    }
    function fnSave()
    {
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});
        AddDefaultParam(data);
        return   fnCallAjaxPost(strUrlSaveDraft, data, fnSearch, 'Data Tersimpan');
    }

    function fnSearch()
    {
        t.clearPipeline();
        t.draw();
    }

    function fnCreateItem(obj) {
        obj.id = obj.Nama;
        obj.text = obj.Nama;
        return obj;
    }

    function fnAddInitController(id, url) {
        fnCreateSelect2(id, url, fnCreateItem);
    }
}