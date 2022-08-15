$("#myform").validate({
    rules: {
        holiday_date: { required: true },
        name: { required: true }
    },
    messages: {
        holiday_date: { required: "Date is required!" },
        name: { required: "Name is required!" }
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
$("#holiday_date").keypress(function (event) { event.preventDefault(); });
setDateCtrl("#holiday_date");
setToday("#holiday_date");
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

    GetDataByIdHoliday(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-Id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let holiday_id = $("#txtID").val();
    e.preventDefault();
    Delete(holiday_id);
});

function BindEntity() {
    var a = {
        holiday_id: $("#holiday_id").val(),
        name: $("#name").val(),
        description: $("#description").val(),
        holiday_date: $("#holiday_date").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/HolidayMerge",
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

function GetDataByIdHoliday(id) {
    $.post(
        "/SetupMaster/GetDataByIdHoliday", { holiday_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#holiday_id").val(data[0].holiday_id);
        $("#name").val(data[0].name);
        $("#holiday_date").val(data[0].sholiday_date);
        $("#description").val(data[0].description);       
        $("#state").val("Modified");
        $("#name").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(holiday_id) {
    $.ajax({
        url: "/SetupMaster/DeleteDataHoliday",
        type: "POST",
        data: { holiday_id: holiday_id },
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
    $('#name').val('');
    $('#holiday_date').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})


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
