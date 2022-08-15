$("#myform").validate({
    rules: {
        emp_type: { required: true }
    },
    messages: {
        emp_type: { required: "Type is required!" }
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
    GetDataByEmpType(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-Id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let emp_type_Id = $("#txtID").val();
    e.preventDefault();
    Delete(emp_type_Id);
});

function BindEntity() {
    var a = {
        emp_type_Id: $("#emp_type_Id").val(),
        emp_type: $("#emp_type").val(),
        description: $("#description").val(),
        isactive: $("#isactive").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/StaffTypeMerge",
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

function GetDataByEmpType(id) {
    $.post(
        "/SetupMaster/GetDataByEmpType", { emp_type_Id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#emp_type_Id").val(data[0].emp_type_Id);
        $("#emp_type").val(data[0].emp_type);
        $("#description").val(data[0].description);       
        $("#state").val("Modified");
        $("#emp_type").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(emp_type_Id) {
    $.ajax({
        url: "/SetupMaster/DeleteEmpType",
        type: "POST",
        data: { emp_type_Id: emp_type_Id },
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
    $('#emp_type').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

