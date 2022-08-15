$("#myform").validate({
    rules: {
        roleName: {
            required: true,
            remote: {
                url: "/Admin/Role/CheckCode",
                type: "post",
                data: {
                    checkData: function () {
                        return $("#roleName").val();
                    },
                    checkDataID: function () {
                        return urlParam("id");
                    }
                }
            }
        }
    },
    messages: {
        roleName: {
            required: "Name is required!",
            remote: jQuery.validator.format("{0} is already in use")
        }       
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


var Per_Type_List = [];
var id = urlParam("id");
console.log("Role Permission");
if (id != "" && id != null) {
    let _isedit = $("#hid_isAut_Edit").val();
    var isEdit = _isedit === "True" ? 1 : 0;
    if (isEdit) {
        $("#btnSave").show();
    }
    else {
        $("#btnSave").hide();
    }
    $('#state').val("Modified");
    GetPermissionTypeList(id);
    $('#btnSave').text("Update");
}
else {
    console.log("Flae");
    GetPermissionTypeList(null);
}

$('#btnCancel').click(function () {
    window.location.href = "/Admin/Role/RoleList?MenuID=" + urlParam("MenuID");
});

function BindEntity() {
    var a = {
        roleId: urlParam("id"),
        roleName: $("#roleName").val(),
        description: $("#description").val(),
        state: $('#state').val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/Role/RoleMerge",
        type: "POST",
        data: {
            model: a,
            detailList: Per_Type_List
        },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () {
                window.location.href = "/Admin/Role/RoleList?MenuID=" + urlParam("MenuID");
            }, 2500), e.preventDefault()) :
                (toastr.error(d.message));
        },
        error: function () { }
    });
}

function GetPermissionTypeList(roleId) {
    const actionUrl = '/Role/GetPermissionDetail';
    $.post(actionUrl,
        {
            roleId: roleId
        },
        function (data, status) {
            $("#divMenuList").empty();
            console.log(data);
            if (data.length > 0) {
                $("#roleName").val(data[0].roleName);
                $("#description").val(data[0].description);
                $("#RoleId").val(roleId);
                Per_Type_List = data;
                PermissionTypeData_Loading();
            }
        });
}

function PermissionTypeData_Loading() {
    var prg_list = "";
    var ind = 0;
    //var div = '<div class="form-group row mx-0">';
    var div = '<div class="col-sm-12 row mx-0">';

    var div_end = "</div>";
    var div_sm2_start = '<div class="col-sm-2 row mx-0">';

    $.each(Per_Type_List, function (mindex) {
        var data = Per_Type_List[mindex].perTypeList;
        prg_list = "";
        var menuId = Per_Type_List[mindex].menuId;


        prg_list += div_sm2_start + '<label id="lbl' + Per_Type_List[mindex].menuName + '" class="col-form-label px-0">' + Per_Type_List[mindex].menuName + ' </label>' + div_end;
        $.each(data, function (index) {

            if (data[index].isCheck) {
                prg_list += div_sm2_start +
                    '<input type="checkbox" data-id="' + data[index].PermissionTypeId + '" class="col-sm-4 custom-checkbox align-self-center" name="chk_permsType" value="' + data[index].PermissionTypeName + '" id="chkm_' + menuId + "pt_" + data[index].PermissionTypeId + '" onclick="Check_PermissionType(' + menuId + "," + data[index].PermissionTypeId + ",'" + data[index].PermissionTypeName + "',this)" + '" checked/>'
                    + '&nbsp;<label for="chkm_' + menuId + "pt_" + data[index].PermissionTypeId + '" id="lbl' + data[index].PermissionTypeName + '" class="col-form-label px-0">' + data[index].PermissionTypeName + ' </label>' + div_end;
            }
            else {
                prg_list += div_sm2_start +
                    '<input type="checkbox" data-id="' + data[index].PermissionTypeId + '" class="col-sm-4 custom-checkbox align-self-center" name="chk_permsType" value="' + data[index].PermissionTypeName + '" id="chkm_' + menuId + "pt_" + data[index].PermissionTypeId + '" onclick="Check_PermissionType(' + menuId + "," + data[index].PermissionTypeId + ",'" + data[index].PermissionTypeName + "',this)" + '"/>'
                    + '&nbsp;<label for="chkm_' + menuId + "pt_" + data[index].PermissionTypeId + '" id="lbl' + data[index].PermissionTypeName + '" class="col-form-label px-0">' + data[index].PermissionTypeName + ' </label>' + div_end;
            }
        });
        $("#divMenuList").append(div + prg_list + div_end);
    });
}

function Check_PermissionType(menuId, permissionTypeId, permissionTypeName, e) {

    $.each(Per_Type_List, function (mindex, obj) {
        if (obj.menuId == menuId) {
            $.each(obj.perTypeList, function (dindex, dobj) {
                if (dobj.PermissionTypeId == permissionTypeId) {
                    console.log("Same + " + permissionTypeName)
                    dobj.isDelete = permissionTypeId != 0 ? !($(e).is(':checked')) : false;
                    dobj.isCheck = $(e).is(':checked');
                }
            });

            console.log("Same + " + permissionTypeName);
            obj.isDelete = permissionTypeId != 0 ? !($(e).is(':checked')) : false;
            obj.isCheck = $(e).is(':checked');
        }
    });

    console.log(Per_Type_List);
}

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