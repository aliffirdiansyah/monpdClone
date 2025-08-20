if (pagename == 'poli') {
    var t;
    $(document).ready(function () {
        var data = {};
        AddDefaultParam(data);
        var cols = [{ "data": "Id" }, { "data": "Nama" }, { "data": "Keterangan" }, { "data": "Aktif", render: fnCreateAktifColumn }, { "data": "InsDate" }, { "data": "InsBy" }];
        // contoh ada kolom edit 
        var edit = { callback: Ubah, column: { "data": "EncryptedKey", } }; 
        // contoh tidak ada kolom edit 
        //var edit = null;


        t = fnCreateTable('tbl', strUrlGetList, data, cols, null, edit);       
    });


    function Tambah() {
        $.ajax({
            type: "POST",
            url: strUrlAdd,
            data: headers,
            success: function (data) {
                fnOpenModal('Tambah Dokumen', data);
            }
        });
    }

    function Ubah(id) {
        var data = { id: id };
        AddDefaultParam(data);

        $.ajax({
            type: "POST",
            url: strUrlAdd,
            data: data,
            success: function (data) {
                fnOpenModal('Ubah Dokumen', data);
            }
        });
    }
    function fnSave(frm) {
        fnCloseModal();
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});
        AddDefaultParam(data);
        $.ajax({
            type: "POST",
            url: strUrlSaveDraft,
            data: data,
            success: function (data) {
                fnSearch();
            }
        });
    }
    function fnSearch() {
        t.clearPipeline();
        t.draw();
    }


}