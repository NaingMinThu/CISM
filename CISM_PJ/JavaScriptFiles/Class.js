$("#myform").validate({
    rules: {
        calss_name: { required: true }
    },
    messages: {
        calss_name: { required: "Class is required!" }
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

    GetDataByIdClass(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-Id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let class_id = $("#txtID").val();
    e.preventDefault();
    Delete(class_id);
});

function BindEntity() {
    var a = {
        class_id: $("#class_id").val(),
        calss_name: $("#calss_name").val(),
        description: $("#description").val(),
        isactive: $("#isactive").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/SetupMaster/ClassMerge",
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

function GetDataByIdClass(id) {
    $.post(
        "/SetupMaster/GetDataByIdClass", { class_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    let b;
    if (data.length > 0) {
        $("#class_id").val(data[0].class_id);
        $("#calss_name").val(data[0].calss_name);
        $("#description").val(data[0].description);       
        $("#state").val("Modified");
        $("#calss_name").prop('disabled', true);
        $("#btnEntry").val("Update");
    }
}

function Delete(class_id) {
    $.ajax({
        url: "/SetupMaster/DeleteDataClass",
        type: "POST",
        data: { class_id: class_id },
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
    $('#calss_name').val('');
    $('#description').val('');
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

