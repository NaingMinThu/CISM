$("#myform").validate({
    rules: {
        from_currency_id: { required: true },
        //to_currency_id: { required: true },
        date: { required: true },
        rate: { required: true }
    },
    messages: {
        from_currency_id: { required: "From Currency Code is required!" },
        //to_currency_id: { required: "To Currency Code is required!" },
        date: { required: "Date is required!" },
        rate: { required: "Rate is required!" }
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
$("#date").keypress(function (event) { event.preventDefault(); });
setDateCtrl("#date");
setToday("#date");
GetCurrency_From("");
GetCurrency_To("");
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
    GetDataByIdRate(id);
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
        exchange_rate_id: $("#exchange_rate_id").val(),
        from_currency_id: $("#from_currency_id").val(),
        //to_currency_id: $("#to_currency_id").val(),
        date: $("#date").val(),
        rate: $("#rate").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/ExchangeRateMerge",
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

function GetDataByIdRate(id) {
    $.post(
        "/SetupMaster/GetDataByIdRate", { exchange_rate_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#exchange_rate_id").val(data[0].exchange_rate_id);
        $("#from_currency_id").val(data[0].from_currency_id);
        //$("#to_currency_id").val(data[0].to_currency_id);
        console.log(data[0].date);
        $('#date').val(data[0].sdate);
       // $('#date').datepicker('setDate', new Date(data[0].date));
        $("#rate").val(data[0].rate);    
        $("#state").val("Modified");
        $("#from_currency_id").prop('disabled', true);
        //$("#to_currency_id").prop('disabled', tssrue);
        $("#btnEntry").val("Update");
    }
}

function Delete(exchange_rate_id) {
    $.ajax({
        url: "/SetupMaster/DeleteDataRate",
        type: "POST",
        data: { exchange_rate_id: exchange_rate_id },
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

function GetCurrency_From(selected) {
    var actionUrl = '/SetupMaster/GetCurrency';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            GradesList = response;
            var $select = $('#from_currency_id');
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

function GetCurrency_To(selected) {
    var actionUrl = '/SetupMaster/GetCurrency';
    $.getJSON(actionUrl, (response) => {
        if (response !== null) {
            GradesList = response;
            var $select = $('#to_currency_id');
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

$('#btnCancel').click(function () {
    $('#pm_name').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

$("#rate").keypress(function (e) {
    return isNumber(e, this);
});

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