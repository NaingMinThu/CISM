$("#myform").validate({
    rules: {
        allowance_type_id: { required: true },
        employee_id: { required: true },
        date: { required: true },
        amount: { required: true }
    },
    messages: {
        allowance_type_id: { required: "Type is required!" },
        employee_id: { required: "Employee is required!" },
        date: { required: "Date is required!" },
        amount: { required: "Amount is required!" }
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
GetEmployee("");
GetAllowanceList("");
$("#date").keypress(function (event) { event.preventDefault(); });
setDateCtrl("#date");
setToday("#date");
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
    GetDataByIdAllowanceSetup(id);
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
        allowance_id: $("#allowance_id").val(),
        employee_id: $("#employee_id").val(),
        allowance_type_id: $("#allowance_type_id").val(),
        date: $("#date").val(),
        amount: $("#amount").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/AllowanceModule/AllowanceInfo/AllowancePeriodMerge",
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

function GetDataByIdAllowanceSetup(id) {
    $.post(
        "/AllowanceModule/AllowanceInfo/GetDataByIdAllowanceSetup", { allowance_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#allowance_id").val(data[0].allowance_id);
        $("#employee_id").val(data[0].employee_id);
        $("#allowance_type_id").val(data[0].allowance_type_id);
        $("#date").val(data[0].sdate);
        $("#amount").val(data[0].amount);     
        $("#state").val("Modified");
        $("#allowance_type_id").prop('disabled', true);
        $("#employee_id").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(allowance_id) {
    $.ajax({
        url: "/AllowanceModule/AllowanceInfo/DeleteDataAllowanceSetup",
        type: "POST",
        data: { allowance_id: allowance_id },
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

function GetEmployee(selected) {
    var actionUrl = '/AllowanceModule/AllowanceInfo/GetEmployee';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            var $select = $('#employee_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].employee_id}">
                                     ${response[i].name}</option>`);
            }
            if (selected !== "") {
                $select.val(selected);
            }
        }
    });
}

function GetAllowanceList(selected) {
    var actionUrl = '/AllowanceModule/AllowanceInfo/GetAllowanceList';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            var $select = $('#allowance_type_id');
            $select.find('option').remove();
            $select.append(`<option value="">-- Please Select --</option>`);
            for (let i = 0; i < response.length; i++) {
                $select.append(`<option value="${response[i].allowance_type_id}">
                                     ${response[i].name}</option>`);
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

$("#amount").keypress(function (e) {
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
