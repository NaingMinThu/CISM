//showData_Table();
var ClassList = [];
var ClassListByGrade = [];
showData_Table();
function GetClassListByGrade(grade_id) {
    const actionUrl = '/SetupMaster/GetClassListByGrade';
    $.post(actionUrl,
        {
            grade_id: grade_id
        },
        function (data, status) {
            $("#divSubjectList").empty();
            ClassListByGrade = data;
            showClassTable();             
        });
}
$('#btnNew').click(function () {
    let _isedit = $("#hid_isAut_Create").val();
    var isEdit = _isedit === "True" ? 1 : 0;
    if (isEdit) {
        $("#btnSave").show();
    }
    else {
        $("#btnSave").hide();
    }
});

function showData_Table() {
    var actionurl = '/SetupMaster/GetClassofGradeAsPage';

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
                targets: [0],
                searchable: true,
                orderable: true
                
            },
           
            {
                "aTargets": [1],
                searchable: false,
                orderable: false,
                "mRender": function (data, type, full) {
                    var isEdit = $("#hid_isAut_Edit").val() == "True" ? 1 : 0;
                    var isDetail = $("#hid_isAut_ViewDetail").val() == "True" ? 1 : 0;
                    edit = '<button class="btn btn-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic" data-id="' + data + '" onclick="editMasterData(this)" value="Edit"><span class="fa fa-edit"></span>&nbsp;Edit</button>';
                    return ((isEdit || isDetail) ? edit : "") ;
                }
            }
        ],

        "order": [[0, "asc"]],
        columns: [
            { data: 'grade_name', name: 'grade_name', title: 'Grade', autoWidth: true, width: '40%' },
            //{ data: 'year', name: 'year', title: 'Year', autoWidth: true, width: '40%' },
            { data: 'grade_id', name: 'grade_id', title: '', autoWidth: true, width: '20%' },
        ],
    });
}

function BindEntity() {
    var a = {
        grade_id: $("#grade_id").val(),
        state : $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Setup/SetupMaster/ClassByGradeMerge",
        type: "POST",
        data: {
            model: a,
            detailList: ClassListByGrade
        },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () { location.reload() }, 2500)) :
                (toastr.error(d.message));
        },
        error: function () { }
    });
}

function editMasterData(ctrl) {
    var grade_id = $(ctrl).data("id");
    console.log("grade_id ==> " + grade_id);
    $("#state").val("Modified");
    $("#grade_id").prop('disabled', true);
    GetGradeList(grade_id);
    $("#btnSave").val("Update");

    if (grade_id != "" && grade_id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit === "True" ? 1 : 0;
        if (isEdit) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }
    GetClassListByGrade(grade_id);
}

$('#btnSave').click(function () {
    if (ClassListByGrade.length > 0) {
        var a = BindEntity();
        Merge(a);
    }
});
