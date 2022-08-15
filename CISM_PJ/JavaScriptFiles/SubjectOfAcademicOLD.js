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
                const $select = $('#year_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].year_id}">${
                        response[i].year_name}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

function GetSubjecListByAcademic(year_id) {
    const actionUrl = '/SetupMaster/GetSubjecListByAcademic';
    $.post(actionUrl,
        {
            year_id: year_id
        },
        function (data, status) {
            $("#divSubjectList").empty();
            subjectList = data;
            check_list = data.filter(x => x.yearsubj_id > 0);
            console.log("check_list");
            console.log(check_list);
            SubjectDataBinding();
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
                    edit = '<a class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogSubjectAcademic" data-id="' + data + '"><span class="fa fa-edit"></span>&nbsp;Edit</a>';
                    //edit = '<button class="btn btn-outline-primary btn-sm" data-toggle="modal" data-target="#dialogEntry" data-id="' + data + '" onclick="editData(this)" value="Edit"><span class="fa fa-edit"></span>&nbsp;Edit</button>';
                    return edit; ///((isEdit || isDetail) ? edit : "") ;
                }
            }
        ],

        "order": [[0, "asc"]],
        columns: [
            { data: 'year_name', name: 'year_name', title: 'Year', autoWidth: true, width: '80%' },
            { data: 'year_id', name: 'year_id', title: '', autoWidth: true, width: '20%' },
        ],
    });
}

function BindEntity() {
    var a = {
        year_id: $("#year_id").val(),
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
            subjectList: subjectList
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
    var year_id = $(ctrl).data("id");
    console.log("menu_id =>" + year_id);
    $("#state").val("Modified");
    $("#year_id").prop('disabled', true);
    $("#btnSave").val("Update");
    GetSubjecListByAcademic(year_id);
}

$('#btnNew').click(function () {
    GetSubjecListByAcademic("");
});

$('#btnSave').click(function () {
    check_list = [];
    $.each($("input[name='chk_Subject']:checked"), function () {
        var a = {
            subject_id: $(this).val(),
            yearsubj_id: $(this).attr('data-id')
        };
        check_list.push(a);        
    });
    console.log(subjectList);
    if (check_list.length > 0) {
        var a = BindEntity();
        Merge(a);
    }
});

function Check_Subject(subject_id, yearsubj_id, e) {
    $.each(subjectList, function (index, obj) {
        if (obj.subject_id == subject_id) {
            console.log("Same + " + yearsubj_id)
            //obj.isDelete = subject_id != 0 ? !($(e).is(':checked')) : false;
            obj.Sstate = !($(e).is(':checked')) ? "Deleted" : "";
            obj.state_status = (yearsubj_id != null && yearsubj_id != "" && yearsubj_id != "0" )? obj.state_status : "Added";
            obj.isCheck = $(e).is(':checked');
        }
    });
    console.log(subjectList);
}