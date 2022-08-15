$("#myform").validate({
    rules: {
        pm_name: { required: true }
    },
    messages: {
        pm_name: { required: "Payment Mode is required!" }
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

    GetDataByIdPM(id);
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
        paymentmode_id: $("#paymentmode_id").val(),
        pm_name: $("#pm_name").val(),
        description: $("#description").val(),
        isactive: $("#isactive").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/PaymentModeMerge",
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

function GetDataByIdPM(id) {
    $.post(
        "/SetupMaster/GetDataByIdPM", { paymentmode_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#paymentmode_id").val(data[0].paymentmode_id);
        $("#pm_name").val(data[0].pm_name);
        $("#description").val(data[0].description);       
        $("#state").val("Modified");
        $("#pm_name").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(paymentmode_id) {
    $.ajax({
        url: "/SetupMaster/DeleteDataPM",
        type: "POST",
        data: { paymentmode_id: paymentmode_id },
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

$('#btnCancel').click(function () {
    $('#pm_name').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

