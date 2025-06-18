if (pagename == 'controllingpelaporansptpd') {
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
}