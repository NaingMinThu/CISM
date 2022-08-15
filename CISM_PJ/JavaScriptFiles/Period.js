$("#myform").validate({
    rules: {
        period_name: { required: true }
    },
    messages: {
        period_name: { required: "Name is required!" }
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

setTimeCtrl("#from_time");
setTimeCtrl("#to_time");
$("#from_time").val('08:00');
$("#to_time").val('09:00');

function setTimeCtrl(ctrl) {
    $(ctrl).timepicker({
        timeFormat: 'HH:mm',
        interval: 15,
        minTime: '8',
        // maxTime: '6:00pm',
        defaultTime: '8',
        startTime: '8',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
}

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

    GetDataByIdPeriod(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-Id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let period_id = $("#txtID").val();
    e.preventDefault();
    Delete(period_id);
});

function BindEntity() {
    var a = {
        period_id: $("#period_id").val(),
        period_name: $("#period_name").val(),
        description: $("#description").val(),
        from_time: $("#from_time").val(),
        to_time: $("#to_time").val(),
        isactive: $("#isactive").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/PeriodMerge",
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

function GetDataByIdPeriod(id) {
    $.post(
        "/SetupMaster/GetDataByIdPeriod", { period_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#period_id").val(data[0].period_id);
        $("#period_name").val(data[0].period_name);
        $("#description").val(data[0].description);  
        $("#from_time").val(data[0].from_time); 
        $("#to_time").val(data[0].to_time); 
        $("#state").val("Modified");
        $("#period_name").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(period_id) {
    $.ajax({
        url: "/SetupMaster/DeleteDataPeriod",
        type: "POST",
        data: { period_id: period_id },
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
    $('#period_name').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

