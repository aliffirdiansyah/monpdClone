if (pagename == 'cont') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor()
    {
        $(document).ready(function () {
            var data = {};
            AddDefaultParam(data);
            var cols = [
                { "data": "Nama" },
                { "data": "IndexAction" },
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

    function fnSave() {
        var frm = $('#frmAdd');

        var tabel = $('#tbldetail').DataTable();
        var detact = tabel.rows().data();
        var detaildata = [];
        for (var i = 0; i < detact.length; i++) {            
            var tempElement = $("<div>").html(detact[i][3]);
            var spanValue = tempElement.find("span").text();

            var isEditElement = $("<div>").html(detact[i][2]);
            var isEditValue = isEditElement.find("span").text();

            if (isEditValue == 'isEdit') {
                var idValue = $(".actdet-input").eq(i).attr("id");
                var inputValue = $("#" + idValue).val();

                var idAuthValue = $(".actdet-checkbox").eq(i).attr("id");
                var isAuthValue = $("#" + idAuthValue).is(":checked");

                detaildata[i] = [inputValue, isAuthValue, spanValue]
            }
            else {
                detaildata[i] = [detact[i][0], detact[i][1], spanValue]
            }

        }

        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});

        data.detaction = JSON.stringify(detaildata); // Adding detaildata to the data object

        AddDefaultParam(data);
        return fnCallAjaxPost(strUrlSaveDraft, data, fnSearch, 'Data Tersimpan');
    }

    function fnSearch()
    {
        t.clearPipeline();
        t.draw();
    }

}