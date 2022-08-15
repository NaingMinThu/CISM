shownCoutryData_Table();
var validator = $("#addform").validate({
    rules: {
        //country_code: {
        //    required: true,
        //    remote: {
        //        url: "/SetupMaster/CheckCode",
        //        type: "post",
        //        data: {
        //            checkData: function () {
        //                return $("#country_code").val();
        //            },
        //            checkDataID: function () {
        //                return $("#country_id").val();
        //            }
        //        }
        //    }
        //},
        
        format_no: { required: true },
        doc_control: { required: true },
        doc_prefix: { required: true },
        doc_format: { required: true },
        next_no: { required: true },
        end_no: { required: true },
        restart_no: { required: true },
      
    },
    messages: {
        //country_code: { required: "Country Code is required!", remote: jQuery.validator.format("{0} is already in use") },
        format_no: { required: "Format No is required!" },
        doc_control: { required: "Document Control is required!" },
        doc_prefix: { required: "Prefix is required!" },
        doc_format: { required: "Format is required!" },
        next_no: { required: "Next No is required!" },
        end_no: { required: "End No is required!" },
        restart_no: { required: "Restart No is required!" }
    },
    errorElement: "em",
    errorPlacement: function (b, c) {
        b.addClass("invalid-feedback"),
            "checkbox" === c.prop("type") ?
                b.insertAfter(c.parent("label")) : b.insertAfter(c)
    },
    highlight: function (b) {
        $(b).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (b) {
        $(b).addClass("is-valid").removeClass("is-invalid");
    },
    submitHandler: function (form, event) {
        event.preventDefault();
        let a = BindEntity();
        Merge(a, event, true);
    }
});

function BindEntity() {
    var a = {
        num_id: $("#num_id").val(),
        format_no: $("#format_no").val(),
        doc_control: $("#doc_control").val(),
        doc_prefix: $("#doc_prefix").val(),
        doc_format: $("#doc_format").val(),
        next_no: $("#next_no").val(),
        end_no: $("#end_no").val(),
        restart_no: $("#restart_no").val(),
        state: $("#state").val()
    };
    return a;
}

function BindToControl(data) {
    let _isedit = $("#hid_isAut_Edit").val();
    var isEdit = _isedit == "True" ? 1 : 0;
    if (isEdit) {
        $("#btnAdd").show();
    }
    else {
        $("#btnAdd").hide();
    }
    $("#btnAdd").text('Update');
    $("#num_id").val(data[0].num_id)
    $("#format_no").val(data[0].format_no);
    $("#doc_control").val(data[0].doc_control);
    $("#doc_prefix").val(data[0].doc_prefix);
    $("#doc_format").val(data[0].doc_format);
    $("#next_no").val(data[0].next_no);
    $("#end_no").val(data[0].end_no);
    $("#restart_no").val(data[0].restart_no);
    $("#state").val("Modified");
}

function clear() {
    $("#format_no").val('');
    $("#doc_control").val('');
    $("#doc_prefix").val('');
    $("#doc_format").val('');
    $("#next_no").val('');
    $("#end_no").val('');
    $("#restart_no").val('');
    $("#num_id").val('');
    $("#state").val('Added');
    $("#btnAdd").text('Save');
}

$('#dialogDelete').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var recipient = button.attr("data-id"); // Extract info from data-* attributes
    var modal = $(this);
    modal.find('#txtID').val(recipient);
});

$("#btnDeletePopup").on("click", function (e) {
    let id = $("#txtID").val();
    e.preventDefault();
    Delete(id);
});

$('#dialogAdd').on('show.bs.modal', function (event) {
    clear();
});

function editing(ctrl) {
    var num_id = $(ctrl).data("id");
    console.log(num_id);
    GetNumMasterByID(num_id);
}

function GetNumMasterByID(id) {
    $.ajax({
        url: "/SetupMaster/GetNumMasterByID",
        type: "POST",
        data: { num_id: id },
        async: !0,
        dataType: "json",
        success: function (data) {
            BindToControl(data);
        }, error: function () { }
    });
}

function Delete(num_id) {
    $.ajax({
        url: "/SetupMaster/DeleteNumMaster",
        type: "POST",
        data: { num_id: num_id },
        async: !0,
        dataType: "json",
        success: function (d) {
            if (d.errorcode === -1) {
                toastr.error(d.message);
                setTimeout(function () { location.reload(); }, 2500), e.preventDefault();
            }
            else if (d.errorcode == 1) {
                toastr.error(d.message);
                setTimeout(function () { location.reload(); }, 2500), e.preventDefault();
            }
            else {
                toastr.error(d.message);
                setTimeout(function () { location.reload(); }, 2500), e.preventDefault();
            }
        }, error: function () { }
    });
}

function shownCoutryData_Table() {
    console.log('shownCoutryData_Table');
    var actionurl = '/SetupMaster/GetNumMasterAsPage';

    var tablecustomer = $('#myTable').DataTable({
        responsive: true,
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true, // this is for disable filter (search box)
        orderMulti: false, // for disable multiple column at once
        pageLength: 10,
        searching: true,

        ajax: {
            url: actionurl,
            type: 'POST',
            datatype: 'json'
        },

        columnDefs: [
            {
                targets: [0, 1, 2, 3],
                searchable: true,
                orderable: true
            },
            {
                "aTargets": [4],
                searchable: false,
                orderable: false,
                "mRender": function (data, type, full) {
                    var isEdit = $("#hid_isAut_Edit").val() == "True" ? 1 : 0;
                    var isDetail = $("#hid_isAut_ViewDetail").val() == "True" ? 1 : 0;
                    var isDelete = $("#hid_isAut_Delete").val() == "True" ? 1 : 0;

                    edit = '<a class="btn btn-primary btn-sm" data-toggle="modal" data-target="#dialogAdd" data-id="' + data + '" onclick="editing(this)"><span class="fa fa-edit"></span>&nbsp;Edit</a>';
                    del = '<a class="btn btn-danger btn-sm" id="btnDelete" name="btnDeleteLink" data-toggle="modal" data-target="#dialogDelete" data-id="' + data + '"><span class="fa fa-trash"></span>&nbsp;Delete</a>';
                    return ((isEdit || isDetail) ? edit : "") + (isDelete ? del : "");
                }
            }
        ],

        columns: [
            { data: 'doc_prefix', name: 'doc_prefix', title: 'Prefix', autoWidth: true },
            { data: 'doc_format', name: 'doc_format', title: 'Format', autoWidth: true, },
            { data: 'next_no', name: 'next_no', title: 'Next No', autoWidth: true, },
            { data: 'end_no', name: 'end_no', title: 'End No', autoWidth: true, },
            { data: 'num_id', name: 'num_id', title: '', autoWidth: true, width: '15%' },
        ],
    });
};

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/NumMasterMerge",
        type: "POST",
        data: { model: a },
        async: true,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () { window.location.reload(); }, 2500), e.preventDefault()) :
                (toastr.error("An error occured in the solution!", "Please contact with system administrator."),
                    setTimeout(function () { }, 1e4));
        },
        error: function () { }
    });
};

function redirectPage(url) {
    var dept_id = urlParam('');
    window.location.href = url + dept_id;
}

// for Update
function urlParam(name) {
    //var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href); -- ?parameter
    var results = new RegExp('/Entry/([^&#]*)').exec(window.location.href);
    if (results === null) {
        return "";
    }
    else {
        return results[1];
    }
}

// number check in keypress event
function isNumeric(event) {
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}
