//showData_Table();
var subjectList = [];
var check_list = [];
GetGradeList("");
GetYearList("");
showData_Table();
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

function GetGradeList(selected) {
    $.get(
        "/SetupMaster/GetGradeList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#grade_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].grade_id}">${
                        response[i].grade_name}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

function GetYearList(selected) {
    $.get(
        "/SetupMaster/GetYearList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#year_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].year_id}">${
                        response[i].year}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

function GetSubjecListByGrade(grade_id, year) {
    const actionUrl = '/SetupMaster/GetSubjecListByGrade';
    $.post(actionUrl,
        {
            grade_id: grade_id,
            year_id : year
        },
        function (data, status) {
            $("#divSubjectList").empty();
            SubjectListByGrade = data;
            showSubjectTable();             
        });
}

function showData_Table() {
    var actionurl = '/SetupMaster/GetSubjectByGradeAsPage';

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
                targets: [0,2],
                searchable: true,
                orderable: true
                
            },
            {
                "aTargets": [1],
                "mRender": function (data, type, full) {
                    $("#hid_year").val(data);
                    return data;
                }
            },
            {
                "aTargets": [3],
                searchable: false,
                orderable: false,
                "mRender": function (data, type, full) {
                    var isEdit = $("#hid_isAut_Edit").val() == "True" ? 1 : 0;
                    var isDetail = $("#hid_isAut_ViewDetail").val() == "True" ? 1 : 0;
                   //  edit = '<a class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic data-id="' + data + '" data-year="' + $("#hid_year").val() + '"><span class="fa fa-edit"></span>&nbsp;Edit</a>';
                    edit = '<button class="btn btn-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic" data-id="' + data + '" data-year_id="' + $("#hid_year").val() + '" onclick="editMasterData(this)" value="Edit"><span class="fa fa-edit"></span>&nbsp;Edit</button>';
                    return ((isEdit || isDetail) ? edit : "") ;
                }
            }
        ],

        "order": [[0, "asc"]],
        columns: [
            { data: 'grade_name', name: 'grade_name', title: 'Grade', autoWidth: true, width: '40%' },
            { data: 'year_id', name: 'year_id', title: '', autoWidth: true, width: '10%',visible : false },
            { data: 'syear', name: 'syear', title: 'Year', autoWidth: true, width: '40%' },
            { data: 'grade_id', name: 'grade_id', title: '', autoWidth: true, width: '20%' },
        ],
    });
}

function BindEntity() {
    var a = {
        grade_id: $("#grade_id").val(),
        year_id : $("#year_id").val(),
        state : $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Setup/SetupMaster/SubjectByGradeMerge",
        type: "POST",
        data: {
            model: a,
            detailList: SubjectListByGrade
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
    var year_id = $(ctrl).data("year_id");
    $("#state").val("Modified");
    
   
    $("#year_id").val(year_id);
    let _isedit = $("#hid_isAut_Edit").val();
    var isEdit = _isedit === "True" ? 1 : 0;
    if (isEdit) {
        $("#btnSave").show();
    }
    else {
        $("#btnSave").hide();
    }
    $("#btnSave").val("Update");
    console.log("grade_id ==> " + grade_id);
    GetGradeList(grade_id);
    GetSubjecListByGrade(grade_id, year_id);
    $("#grade_id").prop('disabled', true);
}

$('#btnSave').click(function () {
    if (SubjectListByGrade.length > 0) {
        var a = BindEntity();
        console.log(a);
        Merge(a);
    }
});

$('#btnNew').click(function () {
    $("#grade_id").prop('disabled', false);
    SubjectListByGrade = [];
    $("#SubjectDataBody").empty();
    $("#index").val("");
    $("#subject_id").val("");
    $('#subject_name').val("");
    $('#description').val("");
    $("#btnAdd").val("Add");
    $("#btnSave").val("Save");
    
});
