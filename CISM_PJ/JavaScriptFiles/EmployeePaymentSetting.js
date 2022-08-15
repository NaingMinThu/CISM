$("#myform").validate({
    rules: {
        emp_type_id: { required: true },
        emp_grade_id: { required: true },
        basic_pay: { required: true },
        probation: { required: true },
        increment: { required: true },
        currency_id: { required: true }
    },
    messages: {
        emp_type_id: { required: "Type is required!" },
        emp_grade_id: { required: "Grade is required!" },
        basic_pay: { required: "Basic pay is required!" },
        probation: { required: "Probation is required!" },
        increment: { required: "Increment is required!" },
        currency_id: { required: "Currency is required!" }
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
GetEmployeeGrade("");
GetEmployeeType("");
GetCurrency("");
$("#myTable").on("click", "#btnEdit", function () {
    var id = $(this).attr("data-Id");
    if (id != "" && id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit === "True" ? 1 : 0;
        if (isEdit) {
            $("#btnEntry").show();
        }
        else {
            $("#btnEntry").hide();
        }
    }
    GetEmployeePaymentSetting(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-Id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let paymentmode_id = $("#txtID").val();
    e.preventDefault();
    Delete(paymentmode_id);
});

function BindEntity() {
    var a = {
        emp_payment_setting_id: $("#emp_payment_setting_id").val(),
        emp_type_id: $("#emp_type_id").val(),
        emp_grade_id: $("#emp_grade_id").val(),
        basic_pay: $("#basic_pay").val(),
        probation: $("#probation").val(),
        increment: $("#increment").val(),
        currency_id: $("#currency_id").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/EmployeePaymentSettingMerge",
        type: "POST",
        data: { model: a },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () { location.reload() }, 2500), e.preventDefault()) :
                (toastr.error(d.message));
        },
        error: function () { }
    });
}

function GetEmployeePaymentSetting(id) {
    $.post(
        "/SetupMaster/GetEmployeePaymentSetting", { emp_payment_setting_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#emp_payment_setting_id").val(data[0].emp_payment_setting_id);
        console.log("data[0].emp_type_id " + data[0].emp_type_id);
        $("#emp_type_id").val(data[0].emp_type_Id);
        $("#emp_grade_id").val(data[0].emp_grade_id);
        $("#basic_pay").val(data[0].basic_pay);
        $("#probation").val(data[0].probation);
        $("#increment").val(data[0].increment);
        $("#currency_id").val(data[0].currency_id);       
        $("#state").val("Modified");
        $("#emp_type_id").prop('disabled', true);
        $("#emp_grade_id").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(emp_payment_setting_id) {
    $.ajax({
        url: "/SetupMaster/DeleteEmployeePaymentSetting",
        type: "POST",
        data: { emp_payment_setting_id: emp_payment_setting_id },
        async: !0,
        dataType: "json",
        success: function (d) {
            if (d.errorcode === -1) {
                toastr.error(d.message);
                setTimeout(function () { location.reload(); }, 2500), e.preventDefault();
            }
            else if (d.errorcode === 1) {
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

function GetCurrency(selected) {
    var actionUrl = '/SetupMaster/GetCurrency';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            GradesList = response;
            var $select = $('#currency_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].currency_id}">
                                     ${response[i].currency_code}</option>`);
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

$('#btnCancel').click(function () {
    $('#pm_name').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

$("#basic_pay").keypress(function (e) {
    return isNumber(e, this);
});

$("#probation").keypress(function (e) {
    return isNumber(e, this);
});

$("#increment").keypress(function (e) {
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