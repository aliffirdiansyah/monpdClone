if (pagename == 'menuheader') {
    var t;
    fnCreatableBlazor();

    function fnCreatableBlazor() {
        $(document).ready(function () {
            var data = {};
            AddDefaultParam(data);
            var cols = [
                { "data": "Id" },
                { "data": "Nama" },
                { "data": "ObjController.Nama" },
                { "data": "Icon" },
                { "data": "Ket" },
                { "data": "Aktif", render: fnCreateAktifColumn },
                { "data": "Seq" },
                { "data": "InsDate" },
                { "data": "InsBy" },
                {
                    "data": "EncryptedKey",
                    render: function (data, type, row, meta) {
                        return '<div class="hstack gap-3 flex-wrap">' +
                            '<a href="javascript:void(0);" class="link-success fs-15" onclick="editClicked(\'' + data + '\')"><i class="ri-edit-2-line"></i></a>' +
                            '<a href="javascript:void(0);" class="link-primary fs-15" onclick="listCheckClicked(\'' + data + '\')"><i class="ri-list-check"></i></a>' +
                            '</div>';
                    }
                }
            ];
            // contoh ada kolom edit 
            /*var edit = { callback: Ubah, column: { "data": "EncryptedKey", } };*/
            // contoh tidak ada kolom edit 
            var edit = null;
            var detail = null;

            t = fnCreateTable('tbl', strUrlGetList, data, cols, null, edit, detail);
        });
    }
   
    function Ubah(id) {
        fnOpenModalEditBlazor('btnModalAdd',id, strUrlAdd);    
    }
    function Detail(id) {
        fnOpenModalEditBlazor('btnModalAdd', id, strUrlDetail);
    }
    function editClicked(encryptedKey) {
        Ubah(encryptedKey)
    }
    function listCheckClicked(encryptedKey) {
        Detail(encryptedKey)
    }
    function fnSave() {        
        var frm = $('#frmAdd');
        var data = frm.serializeArray().reduce(function (obj, item) {
            obj[item.name] = item.value;
            return obj;
        }, {});
        AddDefaultParam(data);
        return   fnCallAjaxPost(strUrlSaveDraft, data, fnSearch, 'Data Tersimpan');       
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
    function fnAddInitController(id, url) {
        fnCreateSelect2(id, url, fnCreateItem);
    }
    function fnAddInitSubMenu(id, url) {
        fnCreateSelect2(id, url, fnCreateItem);
    }
    function ShowIconList() {
        //Modalscrollable
        $.ajax({
            type: "GET",
            url: "Shared/ListIcon",
            success: function (data) {
                //alert(data)
                $("#Modalscrollable .modal-title").html("List Icon");
                $("#Modalscrollable .modal-body").html(data);
                $("#Modalscrollable").css('z-index', 1060);
                $("#Modalscrollable").modal('show');
            }
        });
    }

    function Simpan() {
        var formData = new FormData($("#form-ubah")[0]);
        var t = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: $("#form-ubah").attr('action'),
            type: "POST",
            data: formData,
            dataType: "json",
            contentType: false,
            headers:
            {
                "RequestVerificationToken": t
            },
            processData: false,
            beforeSend: function () {
                $(".loading").show()
            },
            complete: function () { $(".loading").hide(); },
            success: function (data) {
                if (data.status == 1) {
                    $('#btnModalAdd').modal('hide');
                    fnNotifySuccess("Data Tersimpan");
                    fnSearch();
                } else {
                    fnNotifyError(data.message);
                }
            }
        })
    }
}