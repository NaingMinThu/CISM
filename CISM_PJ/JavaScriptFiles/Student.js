initialize();
var ClassList = [];
var PreviousSchools = [];
var validator = $("#mywoform").validate({
    rules: {
        name: { required: true },
        dob: { required: true },
        //class_id: { required: true },
        citizen_id: { required: true },
        contract_no: { required: true },
        address: { required: true },
        grade_id: { required: true },
        year_id: { required: true },
        date_of_join: { required: true },
        email: {
            required: true,
            regex: /^\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b$/i
        }
    },
    messages: {
        name: { required: "Name is required!" },
        dob: { required: "DOB is required!" },
        //class_id: { required: "Class is required!" },
        citizen_id: { required: "CitizenShip is required!" },
        contract_no: { required: "Contract No is required!" },
        address: { required: "Address is required!" },
        email: { required: "Invalid Email Format!" },
        grade_id: { required: "Grade is required!" },
        year_id: { required: "Year is required!" },
        date_of_join: { required: "Date of Join is required!" },
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
        Merge(a, event);
    }
});
$('#BackBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/StudentsInfo/Students/StudentList?MenuID=' + urlParam('MenuID');
});
$('#CancelBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/StudentsInfo/Students/StudentList?MenuID=' + urlParam('MenuID');
});

$("#dob").keypress(function (event) { event.preventDefault(); });

$('#grade_id').change(function () {
    GetClassListByGrade($(this).val());
});

function initialize() {   
    setDateCtrl("#dob");
    setDateCtrl("#date_of_join");
    setDateCtrl("#date_of_exit");
    setDateCtrl("#year_from");
    setDateCtrl("#year_to");
    setToday("#year_from");
    setToday("#year_to");
    $("#dob").keypress(function (event) { event.preventDefault(); });
    $("#date_of_exit").keypress(function (event) { event.preventDefault(); });
    $("#date_of_join").keypress(function (event) { event.preventDefault(); });
    $("#year_from").keypress(function (event) { event.preventDefault(); });
    $("#year_to").keypress(function (event) { event.preventDefault(); });
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
        GetStudentByID();
        $("#btnSchoolRecord").show();
    }
    else {
        setToday("#dob");
        setToday("#date_of_join");
        $("#btnSchoolRecord").hide();
        BindPaymentMode("");
        BindCitizenShip("");
        GetGradeList("", "");
        GetYearList("");
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

function BindPaymentMode(selected) {
    $.get(
        "/StudentsInfo/Students/GetPaymentMode",
        function (response) {
            if (response.length > 0) {
                const $select = $('#paymentmode_id');
                $select.find('option').remove();
                //$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].paymentmode_id}">${
                        response[i].pm_name}</option>`);
                }
                if (selected != "") {
                    $select.val(selected);
                }
            }
        });
}

function BindCitizenShip(selected) {
    $.get(
        "/StudentsInfo/Students/GetCitizenShip",
        function (response) {
            if (response.length > 0) {
                const $select = $('#citizen_id');
                $select.find('option').remove();
                //$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].citizen_id}">${
                        response[i].citizen_name}</option>`);
                }
                if (selected != "") {
                    $select.val(selected);
                }
            }
        });
}

function GetClassListByGrade(grade_id, class_id) {
    const actionUrl = '/SetupMaster/GetClassListByGrade';
    $.post(actionUrl,
        {
            grade_id: grade_id
        },
        function (data, status) {
            ClassList = data;
            GetClassList(class_id);
        });
}

function GetClassList(selected) {
    if (ClassList !== null) {
        console.log("response.length == > " + ClassList.length);
        const $select = $('#class_id');
        $select.find('option').remove();
        //]$select.append('<option value="">-- Please Select --</option>');

        for (let i = 0; i < ClassList.length; i++) {
            $select.append(`<option value="${ClassList[i].class_id}">${
                ClassList[i].class_name}</option>`);
        }
        if (selected !== "") {
            $select.val(selected);
        }
    }
}

function GetGradeList(selected,class_selected) {
    $.get(
        "/SetupMaster/GetGradeList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#grade_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');
                if (urlParam("id") == "" || urlParam("id") == null) {
                    GetClassListByGrade(response[0].grade_id, class_selected);
                }
                else {
                    GetClassListByGrade(selected, class_selected);
                }
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

function BindClass(selected) {
    $.get(
        "/StudentsInfo/Students/GetClass",
        function (response) {
            if (response.length > 0) {
                const $select = $('#class_id');
                $select.find('option').remove();
                //$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].class_id}">${
                        response[i].calss_name}</option>`);
                }
                if (selected != "") {
                    $select.val(selected);
                }
            }
        });
}

function Merge(a, e) {
    $.ajax({
        url: "/StudentsInfo/Students/StudentMerge",
        type: "POST",
        data: {
            model: a,
            previous_school_details: PreviousSchools
        },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () {
              
                window.location.href = '/StudentsInfo/Students/StudentList?MenuID=' + urlParam('MenuID');
               
            }, 2500), e.preventDefault()) :
                (toastr.error("An error occured in the solution!", "Please contact with system administrator." + d.message),
                    setTimeout(function () { }, 1e4));
        },
        error: function () { }
    });
}

function GetStudentByID() {
    const actionUrl = '/StudentsInfo/Students/GetStudentByID';
    $.post(actionUrl,
        {
            student_id: urlParam('id')
        },
        function (data, status) {
            bindDataToControl(data);
            PreviousSchools = data.PreviousSchool;
            $("#PreviousSchoolTable tbody").empty();
            showPreviousTable();
        });
}

function bindDataToControl(data) {
    if (data.STU.length > 0) {
        $('#student_id').val(data.STU[0].student_id);
        $('#name').val(data.STU[0].name);
        $('#reg_no').val(data.STU[0].reg_no);
        $('#prv_reg_no').val(data.STU[0].reg_no);        
        //$('#dob').datepicker('setDate', new Date(data.STU[0].dob));
        $('#dob').val(data.STU[0].sdob);
        $('#email').val(data.STU[0].email);
        BindPaymentMode(data.STU[0].paymentmode_id);
        BindCitizenShip(data.STU[0].citizen_id);    
        GetGradeList(data.STU[0].grade_id, data.STU[0].class_id);
        GetYearList(data.STU[0].year_id);
        $('#father_name').val(data.STU[0].father_name);
        $('#mother_name').val(data.STU[0].mother_name);
        $('#contract_no').val(data.STU[0].contract_no);
        $('#contract_no1').val(data.STU[0].contract_no1);
        $('#address').val(data.STU[0].address);
        $('#remark').val(data.STU[0].remark);        
        $('#date_of_join').val(data.STU[0].sdate_of_join);
        $('#date_of_exit').val(data.STU[0].sdate_of_exit);
        //$('#date_of_join').datepicker('setDate', new Date(data.STU[0].date_of_join));
        //if (data.STU[0].date_of_exit != null && data.STU[0].date_of_exit != "")
        //    $('#date_of_exit').datepicker('setDate', new Date(data.STU[0].date_of_exit));
        $('#nationality').val(data.STU[0].nationality);
        $("#state").val("Modified");
        $('#lblAction').text('Update');
    }
}

function BindEntity() {
    var a = {
        student_id: $("#student_id").val(),
        name: $("#name").val(),
        reg_no: $("#reg_no").val(),
        dob: formatDate($("#dob").datepicker("getDate")),
        email: $("#email").val(),
        class_id: $("#class_id").val(),
        citizen_id: $("#citizen_id").val(),
        paymentmode_id: $('#paymentmode_id').val(), 
        father_name: $("#father_name").val(),
        mother_name: $("#mother_name").val(),
        contract_no: $("#contract_no").val(),
        contract_no1: $('#contract_no1').val(),
        address: $('#address').val(), 
        remark: $('#remark').val(), 
        state : $("#state").val(),
        grade_id: $("#grade_id").val(),
        year_id: $("#year_id").val(),
        date_of_join : $("#date_of_join").val(),
        date_of_exit: $("#date_of_exit").val(),
        nationality: $("#nationality").val()
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


