$("#myform").validate({
    rules: {
        txtuser_name: {
            required: true,
            remote: {
                url: "/Admin/User/CheckCode",
                type: "post",
                data: {
                    checkData: function () {
                        return $("#txtuser_name").val();
                    },
                    checkDataID: function () {
                        return $("#user_id").val();
                    }
                }
            }
        },
        txt_user_password: { required: true },
        roleId: { required: true }

    },
    messages: {
        txtuser_name: { required: "Name is required!", remote: jQuery.validator.format("{0} is already in use") },
        txt_user_password: { required: "Password is required!" },
        roleId: { required: "Role is required!" },
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
        console.log(a);
        Merge(a, event);
    }
});

$("#btnAdd").on("click", function (e) {
    BindRole();
    clearCache();
});

function BindEntity() {
    var a = {
        user_id: $("#user_id").val(),
        user_name: $("#txtuser_name").val(),
        user_pwd: $("#txt_user_password").val(),
        roleId: $("#roleId").val(),
        description: $("#description").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a, e) {
    $.ajax({
        url: "/Admin/User/UserMerge",
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

function clearCache() {
    $("#txtuser_name").val("");
    $("#txt_user_password").val("");
    $("#description").val("");
}

$("#myTable").on("click", "#btnEdit", function () {
    clearCache();
    var id = $(this).attr("data-id");
    $("#user_id").val(id);
    console.log("id ==> " + id);
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
    GetDatabyID(id);
});

$("#myTable").on("click", "#btnDelete", function () {
    let code = $(this).attr("data-id");
    $("#txtID").val(code);
});

$("#btnDeletePopup").on("click", function (e) {
    let employee_id = $("#txtID").val();
    e.preventDefault();
    Delete(employee_id);
});

function GetDatabyID(id) {
    $.post(
        "/Admin/User/GetUserbyID", { user_id: id },
        function (data) {
            bindDataToControl(data);
        });
}

function bindDataToControl(data) {
    if (data.length > 0) {
        $("#txtuser_name").val(data[0].user_name);
        $("#txt_user_password").val(data[0].user_pwd);
        //$("#roleId").val(data[0].roleId);
        BindRole(data[0].roleId);
        $("#description").val(data[0].description);
        $("#state").val("Modified");
        $("#btnEntry").val("Update");
    }
}

function Delete(login_id) {
    $.ajax({
        url: "/Admin/User/DeleteUser",
        type: "POST",
        data: { user_id: login_id },
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
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})
$('#btnClose').click(function () {
    var validator = $("#myform").validate();
    validator.resetForm();
    location.reload();
})

function BindRole(selected) {
    $.get(
        "/Admin/User/GetRoleList",
        function (response) {
            if (response.length > 0) {
                const $select = $('#roleId');
                $select.find('option').remove();
                $select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].roleId}">${
                        response[i].roleName}</option>`);
                }
                if (selected != "") {
                    $select.val(selected);
                }
            }
        });
}