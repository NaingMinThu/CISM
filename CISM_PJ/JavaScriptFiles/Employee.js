initialize();
var Hobbies = [];
var Qualifications = [];
var Hisotries = [];
var Family = [];
var Skill = [];
var validator = $("#mywoform").validate({
    rules: {
        name: { required: true },
        dob: { required: true },
        nrc_passport: { required: true },
        emp_type_Id: { required: true },
        contract_no: { required: true },
        address: { required: true },
        grade_id: { required: true },
        email: {
            required: true,
            regex: /^\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b$/i
        }
    },
    messages: {
        name: { required: "Name is required!" },
        dob: { required: "DOB is required!" },
        nrc_passport: { required: "NRC/Passport is required!" },
        grade_id: { required: "Grade is required!" },
        emp_type_Id: { required: "Type is required!" },
        contract_no: { required: "Contract No is required!" },
        address: { required: "Address is required!" },
        email: { required: "Email is required!", regex: "Invalid Email Format!" },
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
        console.log(Hobbies);
        console.log(Qualifications);        
        Merge(a, event);
    }
});
$('#BackBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/EmployeeInfo/Employee/EmployeeList?MenuID=' + urlParam('MenuID');
});
$('#CancelBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/EmployeeInfo/Employee/EmployeeList?MenuID=' + urlParam('MenuID');
});

$("#dob").keypress(function (event) { event.preventDefault(); });
$("#qual_year_from").keypress(function (event) { event.preventDefault(); });
$("#qual_year_to").keypress(function (event) { event.preventDefault(); });
$("#his_year_from").keypress(function (event) { event.preventDefault(); });
$("#his_year_to").keypress(function (event) { event.preventDefault(); });

function initialize() {   
    setDateCtrl("#dob");
    setDateCtrl("#qual_year_from");
    setDateCtrl("#qual_year_to");
    setDateCtrl("#his_year_from");
    setDateCtrl("#his_year_to");
    GetSkill();
    $("#dob").keypress(function (event) { event.preventDefault(); });
    let _id = urlParam('id');
    //console.log(_ops +"_ops");
    if (_id != "" && _id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit == "True" ? 1 : 0;
        if (isEdit) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
        GetEmployeeByID();
        $("#btnHobbies").show();
        $("#btnQualification").show();
        $("#btnEmployment").show(); 
        $("#btnFamily").show(); 
        $("#btnSkill").show(); 
    }
    else {
        setToday("#dob");
        setToday("#qual_year_from");
        setToday("#qual_year_to");
        setToday("#his_year_from");
        setToday("#his_year_to");
        GetGradeList("", "");
        GetEmployeeType("");
    }
}

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

function GetEmployeeType(selected) {
    $.get(
        "/Employee/GetEmployeeType",
        function (response) {
            if (response !== null) {
                const $select = $('#emp_type_Id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].emp_type_Id}">${
                        response[i].emp_type}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

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

function GetSkill() {
    $.get(
        "/EmployeeInfo/Employee/GetSkill",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#skill_id');
                $select.find('option').remove();
                $select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].skill_id}">${
                        response[i].name}</option>`);
                }
            }
        });
}

function Merge(a, e) {
    $.ajax({
        url: "/EmployeeInfo/Employee/EmployeeMerge",
        type: "POST",
        data: {
            model: a,
            hobbies: Hobbies,
            educations: Qualifications,
            employmentList: Hisotries,
            familyList: Family,
            skillList: Skill
        },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () {              
                window.location.href = '/EmployeeInfo/Employee/EmployeeList?MenuID=' + urlParam('MenuID');               
            }, 2500), e.preventDefault()) :
                (toastr.error("An error occured in the solution!", "Please contact with system administrator." + d.message),
                    setTimeout(function () { }, 1e4));
        },
        error: function () { }
    });
}

function GetEmployeeByID() {
    const actionUrl = '/EmployeeInfo/Employee/GetEmployeeByID';
    $.post(actionUrl,
        {
            employee_id: urlParam('id')
        },
        function (data, status) {
            bindDataToControl(data);
            Hobbies = data.Hobbies;
            $("#HobbiesTable tbody").empty();
            showHobbiesTable();

            Qualifications = data.Qualification;
            $("#QualificationTable tbody").empty();
            showQualification();

            Hisotries = data.Hisotries;
            $("#HistoriesTable tbody").empty();
            showHistoriesTable();

            Family = data.Family;
            $("#FamilyTable tbody").empty();
            showFamilyTable();

            Skill = data.Skill;
            $("#PersonalSkillTable tbody").empty();
            showPersonalSkill();
        });
}

function bindDataToControl(data) {
    console.log(data.EMP);
    if (data.EMP.length > 0) {
        $('#employee_id').val(data.EMP[0].employee_id);
        $('#name').val(data.EMP[0].name);
        $('#staff_no').val(data.EMP[0].staff_no);
        $('#hobby_reg_no').val(data.EMP[0].staff_no);
        $('#qual_reg_no').val(data.EMP[0].staff_no);     
        $('#his_reg_no').val(data.EMP[0].staff_no);  
        $('#fm_reg_no').val(data.EMP[0].staff_no); 
        $('#skill_reg_no').val(data.EMP[0].staff_no);
        
        GetEmployeeType(data.EMP[0].emp_type_Id);
        GetGradeList(data.EMP[0].grade_id);
        $('#dob').val(data.EMP[0].sdob);
        $('#email').val(data.EMP[0].email);        
        $('#nrc_passport').val(data.EMP[0].nrc_passport);
        $('#nationality').val(data.EMP[0].nationality);
        $("input[name=gender][value='" + (data.EMP[0].gender == "true" ? 1 : 0) + "']").prop("checked", true);
        $("input[name=marital_status][value='" + (data.EMP[0].marital_status == "true" ? 1 : 0) + "']").prop("checked", true);
        $('#contract_no').val(data.EMP[0].contract_no);
        $('#contract_no1').val(data.EMP[0].contract_no1);
        $('#address').val(data.EMP[0].address);
        $('#remark').val(data.EMP[0].remark);  
        $("#state").val("Modified");
        $('#lblAction').text('Update');
    }
}

function BindEntity() {
    var a = {
        employee_id: $("#employee_id").val(),
        name: $("#name").val(),
        staff_no: $("#staff_no").val(),
        dob: formatDate($("#dob").datepicker("getDate")),
        email: $("#email").val(),
        emp_type_Id: $("#emp_type_Id").val(),
        grade_id: $("#grade_id").val(),
        nrc_passport: $("#nrc_passport").val(),
        nationality: $("#nationality").val(),
        gender: $('input[name="gender"]:checked').val(),
        marital_status: $('input[name="marital_status"]:checked').val(),
        contract_no: $("#contract_no").val(),
        contract_no1: $('#contract_no1').val(),
        address: $('#address').val(), 
        remark: $('#remark').val(), 
        state: $("#state").val(),     
    };
    return a;
}

function setTimeCtrl(ctrl) {
    $(ctrl).timepicker({
        timeFormat: 'HH:mm',
        interval: 30,
        minTime: '8',
        // maxTime: '6:00pm',
        defaultTime: '8',
        startTime: '8',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
}

function setDateCtrl(ctrl) {
    $(ctrl).datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        //showButtonPanel: true,
        beforeShow: function () {
            $(".ui-datepicker").css('font-size', 14);
        }
    });
}

function setToday(ctrl) {
    $(ctrl).datepicker('setDate', new Date());
}

function checkDate(type) {
    //console.log(type);
    if (type == "date") {
        if (validator.element("#start_date") && validator.element("#end_date")) {
            var startdate = moment($('#start_date').val(), "DD/MM/YYYY");
            var enddate = moment($('#end_date').val(), "DD/MM/YYYY");
            //console.log(startdate);
            //console.log(enddate);
            if (startdate > enddate) {
                //console.log("Must be greater than");
                $("#lblstartDate_errmsg").text("Must be greater than or equal end date");
                $("#btnSave").attr('disabled', true);
            } else {
                $("#lblstartDate_errmsg").text("");
                $("#btnSave").attr('disabled', false);
            }
        }
    }
    else {
        //console.log(type);
        var startTime = $('#start_time').val();
        var endTime = $('#end_time').val();

        var start_time = moment(startTime, 'HH:mm');
        var end_Time = moment(endTime, 'HH:mm');
        if (start_time > end_Time) {
            //console.log("Must be greater than");
            $("#lblstartDate_errmsg").text("Must be greater than or equal end time");
            $("#btnSave").attr('disabled', true);
        }
        else {
            $("#lblstartDate_errmsg").text("");
            $("#btnSave").attr('disabled', false);
        }
    }
}
// for Update
function urlParam(name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href); // -- ?parameter
    // var results = new RegExp('/Entry/([^&#]*)').exec(window.location.href);
    if (results === null) {
        return "";
    }
    else {
        return results[1];
    }
}

function postAndRedirect(url, postData) {
    var postFormStr = "<form method='POST' action='" + url + "'>\n";

    for (var key in postData) {
        if (postData.hasOwnProperty(key)) {
            postFormStr += "<input type='hidden' name='" + key + "' value='" + postData[key] + "'></input>";
        }
    }

    postFormStr += "</form>";

    var formElement = $(postFormStr);

    $('body').append(formElement);
    $(formElement).submit();
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


