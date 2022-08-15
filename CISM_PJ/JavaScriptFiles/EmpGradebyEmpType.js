var GradesList = [];
var GradesByEmpType = [];

showData_Table();

function GetGradeListByEmpType(emp_type_Id) {
    const actionUrl = '/SetupMaster/GetGradeListByEmpType';
    $.post(actionUrl,
        {
            emp_type_Id: emp_type_Id
        },
        function (data, status) {
            $("#divSubjectList").empty();
            GradesByEmpType = data;
            showTable();
        });
}

//function ClassDataBinding() {
//    //data.WO[0].cust_id
//    var prg_list = "";
//    var ind = 0;
//    var div = '<div class="form-group row mx-0">';
//    var div_end = "</div>";
//    var div_sm4_start = '<div class="col-sm-2 row mx-0">';
//    var div_sm6_start = '<div class="col-sm-6 pr-0">';
//    var data = subjectList;
//    $.each(data, function (index) {

//        if (index % 6 == 0 && index > 0) {
//            $("#divSubjectList").append(div + prg_list + div_end);
//            prg_list = "";

//            console.log(data[index].isCheck);
//            // checked="' + data[index].isCheck +'"
//            prg_list += div_sm4_start +
//                '<input type="checkbox" data-id="' + data[index].yearsubj_id + '" class="custom-checkbox align-self-center" name="chk_Subject" value="' + data[index].subject_id + '" id="chkpt_' + data[index].subject_id + '" onclick="Check_Subject(' + data[index].subject_id + ",'" + data[index].yearsubj_id + "',this)" + '"/>'
//                + '&nbsp;<label for="chkpt_' + data[index].subject_id + '" id="lbl' + data[index].subject_id + '" class="col-form-label col-sm-10 px-0 align-self-center">' + data[index].subject_name + ' </label>' + div_end;

//        }
//        else {  ///checked="' + data[index].isCheck +'" 
//            prg_list += div_sm4_start +
//                '<input type="checkbox" data-id="' + data[index].yearsubj_id + '" class="custom-checkbox align-self-center" name="chk_Subject" value="' + data[index].subject_id + '" id="chkpt_' + data[index].subject_id + '" onclick="Check_Subject(' + data[index].subject_id + ",'" + data[index].yearsubj_id + "',this)" + '"/>'
//                + '&nbsp;<label for="chkpt_' + data[index].subject_id + '" id="lbl' + data[index].subject_id + '" class="col-form-label col-sm-10 px-0 align-self-center">' + data[index].subject_name + ' </label>' + div_end;

//        }
//    });
//    $("#divSubjectList").append(div + prg_list + div_end);
//    $.each(check_list, function (index) {
//        $('#chkpt_' + check_list[index].subject_id).attr('checked', true);
//    });
//}

function showData_Table() {
    var actionurl = '/SetupMaster/GetGradeListByEmpTypeAsPage';

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
                  //  edit = '<a class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic data-id="' + data + '" data-year="' + $("#hid_year").val() + '"><span class="fa fa-edit"></span>&nbsp;Edit</a>';
                    edit = '<button class="btn btn-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic" data-id="' + data + '" onclick="editData(this)" value="Edit"><span class="fa fa-edit"></span>&nbsp;Edit</button>';
                    return edit; ///((isEdit || isDetail) ? edit : "") ;
                }
            }
        ],

        "order": [[0, "asc"]],
        columns: [
            { data: 'emp_type', name: 'emp_type', title: 'Employee Type', autoWidth: true, width: '40%' },
            //{ data: 'year', name: 'year', title: 'Year', autoWidth: true, width: '40%' },
            { data: 'emp_type_Id', name: 'emp_type_Id', title: '', autoWidth: true, width: '20%' },
        ],
    });
}

function BindEntity() {
    var a = {
        emp_type_Id: $("#emp_type_Id").val(),
        state : $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Setup/SetupMaster/GradeListByEmployeeTypeMerge",
        type: "POST",
        data: {
            model: a,
            detailList: GradesByEmpType
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
    var id = $(ctrl).data("id");
    $("#state").val("Modified");
    $("#emp_type_Id").prop('disabled', true);
    $("#emp_type_Id").val(id);

    if (id != "" && id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit === "True" ? 1 : 0;
        if (isEdit) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }
    $("#btnSave").val("Update");
    GetGradeListByEmpType(id);
}

$('#btnSave').click(function () {
    if (GradesByEmpType.length > 0) {
        var a = BindEntity();
        Merge(a);
    }
});
