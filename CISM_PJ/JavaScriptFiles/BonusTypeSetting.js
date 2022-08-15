//showData_Table();
var GradeList = [];
var SettingList = [];
GetEmployeeGrade("");
GetEmployeeType("");
GetBonusList("");
showData_Table();
function BonusTypeSettingByBonusID(bonus_setting_id) {
    const actionUrl = '/SetupMaster/BonusTypeSettingByBonusID';
    $.post(actionUrl,
        {
            bonus_setting_id: bonus_setting_id
        },
        function (data, status) {
            $("#divSubjectList").empty();
            SettingList = data;
            $("#bonus_id").val(SettingList[0].bonus_id);
            $("#emp_type_id").val(SettingList[0].emp_type_Id);
            $("#emp_grade_id").val(SettingList[0].emp_grade_id);
            showTable();             
        });
}

function showData_Table() {
    var actionurl = '/SetupMaster/BonusTypeSettingAsPage';

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
                targets: [0,1,2],
                searchable: true,
                orderable: true
                
            },
           
            {
                "aTargets": [3],
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
            { data: 'bonus_name', name: 'bonus_name', title: 'Bonus', autoWidth: true },
            { data: 'emp_type', name: 'emp_type', title: 'Type', autoWidth: true },
            { data: 'emp_grade_name', name: 'emp_grade_name', title: 'Grade', autoWidth: true },
            //{ data: 'year', name: 'year', title: 'Year', autoWidth: true, width: '40%' },
            { data: 'bonus_setting_id', name: 'bonus_setting_id', title: '', autoWidth: true},
        ],
    });
}

function BindEntity() {
    var a = {
        bonus_setting_id: $("#bonus_setting_id").val(),
        emp_grade_id: $("#emp_grade_id").val(),
        emp_type_id: $("#emp_type_id").val(),
        bonus_id: $("#bonus_id").val(),
        state : $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Setup/SetupMaster/BonusTypeSettingMerge",
        type: "POST",
        data: {
            model: a,
            detailList: SettingList
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
    var bonus_setting_id = $(ctrl).data("id");
    if (bonus_setting_id != "" && bonus_setting_id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit === "True" ? 1 : 0;
        if (isEdit) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }
    $("#bonus_setting_id").val(bonus_setting_id);
    $("#state").val("Modified");
    $("#bonus_id").prop('disabled', true);
    $("#emp_type_id").prop('disabled', true);
    $("#emp_grade_id").prop('disabled', true);
    $("#btnSave").val("Update");
    BonusTypeSettingByBonusID(bonus_setting_id);
}

$('#btnSave').click(function () {
    if (SettingList.length > 0) {
        var a = BindEntity();
        Merge(a);
    }
});

function GetEmployeeGrade(selected) {
    var actionUrl = '/SetupMaster/GetEmployeeGrade';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            GradesList = response;
            var $select = $('#emp_grade_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].emp_grade_id}">
                                     ${response[i].emp_grade_name}</option>`);
            }
            if (selected !== "") {
                $select.val(selected);
            }
        }
    });
}

function GetEmployeeType(selected) {
    var actionUrl = '/SetupMaster/GetEmployeeType';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            var $select = $('#emp_type_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].emp_type_Id}">
                                     ${response[i].emp_type}</option>`);
            }
            if (selected !== "") {
                $select.val(selected);
            }
        }
    });
}

function GetBonusList(selected) {
    var actionUrl = '/SetupMaster/GetBonusList';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            var $select = $('#bonus_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].bonus_id}">
                                     ${response[i].bonus_name}</option>`);
            }
            if (selected !== "") {
                $select.val(selected);
            }
        }
    });
}

$("#from_amt").keypress(function (e) {
    return isNumber(e, this);
});

$("#to_amt").keypress(function (e) {
    return isNumber(e, this);
});

$("#from_year").keypress(function (e) {
    return isNumber(e, this);
});

$("#to_year").keypress(function (e) {
    return isNumber(e, this);
});

function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var dotindex = $(element).val().indexOf('.');
    var decimalplacelen = (dotindex == -1 ? dotindex : $(element).val().substring($(element).val().indexOf('.')).length);

    if (
        // (charCode != 45 || $(element).val().indexOf('-') != -1) &&   // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        ((charCode < 48 || charCode > 57) || (decimalplacelen > 2 && dotindex < evt.target.selectionStart))) // CHECK TWO DECIMAL-PLACE
        return false;
    return true;
}
