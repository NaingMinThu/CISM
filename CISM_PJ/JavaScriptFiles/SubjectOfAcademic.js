//showData_Table();
var subjectList = [];
var check_list = [];
GetYearList("");
showData_Table();
function GetYearList(selected) {
    $.get(
        "/SetupMaster/GetYearList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#grade_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].grade_id}">${
                        response[i].year_name}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

function GetSubjecListByAcademic(grade_id, year) {
    const actionUrl = '/SetupMaster/GetSubjecListByAcademic';
    $.post(actionUrl,
        {
            grade_id: grade_id,
            year : year
        },
        function (data, status) {
            $("#divSubjectList").empty();
            SubjectListByGrade = data;
            showSubjectTable();             
        });
}

function SubjectDataBinding() {
    //data.WO[0].cust_id
    var prg_list = "";
    var ind = 0;
    var div = '<div class="form-group row mx-0">';
    var div_end = "</div>";
    var div_sm4_start = '<div class="col-sm-2 row mx-0">';
    var div_sm6_start = '<div class="col-sm-6 pr-0">';
    var data = subjectList;
    $.each(data, function (index) {

        if (index % 6 == 0 && index > 0) {
            $("#divSubjectList").append(div + prg_list + div_end);
            prg_list = "";

            console.log(data[index].isCheck);
            // checked="' + data[index].isCheck +'"
            prg_list += div_sm4_start +
                '<input type="checkbox" data-id="' + data[index].yearsubj_id + '" class="custom-checkbox align-self-center" name="chk_Subject" value="' + data[index].subject_id + '" id="chkpt_' + data[index].subject_id + '" onclick="Check_Subject(' + data[index].subject_id + ",'" + data[index].yearsubj_id + "',this)" + '"/>'
                + '&nbsp;<label for="chkpt_' + data[index].subject_id + '" id="lbl' + data[index].subject_id + '" class="col-form-label col-sm-10 px-0 align-self-center">' + data[index].subject_name + ' </label>' + div_end;

        }
        else {  ///checked="' + data[index].isCheck +'" 
            prg_list += div_sm4_start +
                '<input type="checkbox" data-id="' + data[index].yearsubj_id + '" class="custom-checkbox align-self-center" name="chk_Subject" value="' + data[index].subject_id + '" id="chkpt_' + data[index].subject_id + '" onclick="Check_Subject(' + data[index].subject_id + ",'" + data[index].yearsubj_id + "',this)" + '"/>'
                + '&nbsp;<label for="chkpt_' + data[index].subject_id + '" id="lbl' + data[index].subject_id + '" class="col-form-label col-sm-10 px-0 align-self-center">' + data[index].subject_name + ' </label>' + div_end;

        }
    });
    $("#divSubjectList").append(div + prg_list + div_end);
    $.each(check_list, function (index) {
        $('#chkpt_' + check_list[index].subject_id).attr('checked', true);
    });
}

function showData_Table() {
    var actionurl = '/SetupMaster/GetSubjectofAcademicList';

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
                targets: [0,1],
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
                "aTargets": [2],
                searchable: false,
                orderable: false,
                "mRender": function (data, type, full) {
                    var isEdit = $("#hid_isAut_Edit").val() == "True" ? 1 : 0;
                  //  edit = '<a class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic data-id="' + data + '" data-year="' + $("#hid_year").val() + '"><span class="fa fa-edit"></span>&nbsp;Edit</a>';
                    edit = '<button class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic" data-id="' + data + '" data-year="' + $("#hid_year").val() + '" onclick="editData(this)" value="Edit"><span class="fa fa-edit"></span>&nbsp;Edit</button>';
                    return edit; ///((isEdit || isDetail) ? edit : "") ;
                }
            }
        ],

        "order": [[0, "asc"]],
        columns: [
            { data: 'grade_name', name: 'grade_name', title: 'Grade', autoWidth: true, width: '40%' },
            { data: 'year', name: 'year', title: 'Year', autoWidth: true, width: '40%' },
            { data: 'grade_id', name: 'grade_id', title: '', autoWidth: true, width: '20%' },
        ],
    });
}

function BindEntity() {
    var a = {
        grade_id: $("#grade_id").val(),
        year: $("#year").val(),
        state : $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Setup/SetupMaster/SubjectByAcademicMerge",
        type: "POST",
        data: {
            model: a,
            subjectList: SubjectListByGrade
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

function editData(ctrl) {
    var grade_id = $(ctrl).data("id");
    var year = $(ctrl).data("year");
    $("#state").val("Modified");
    $("#grade_id").prop('disabled', true);
    $("#grade_id").val(grade_id);
    $("#year").val(year);

    let _isedit = $("#hid_isAut_Edit").val();
    var isEdit = _isedit === "True" ? 1 : 0;
    if (isEdit) {
        $("#btnSave").show();
    }
    else {
        $("#btnSave").hide();
    }
    $("#btnSave").val("Update");
    GetSubjecListByAcademic(grade_id, year);
}

$('#btnSave').click(function () {
    if (SubjectListByGrade.length > 0) {
        var a = BindEntity();
        Merge(a);
    }
});
