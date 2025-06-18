if (pagename == 'provinsi') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor() {
        $(document).ready(function () {
            var data = {};

            AddDefaultParam(data);

            var cols = [
                { "data": "Id" },
                { "data": "Nama" },
                { "data": "Aktif", render: fnCreateAktifColumn },
                { "data": "InsDate" },
                { "data": "InsBy" }
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

    function fnSave() {
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});

        //console.log(data);
        var d = { Propinsi: data };
        //console.log(d)

        AddDefaultParam(d);

        return fnCallAjaxPost(strUrlSave, d, fnSearch, 'Data Tersimpan');
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
        fnCreateSelect2('IdProvinsi', url, fnCreateItem);
    }
}