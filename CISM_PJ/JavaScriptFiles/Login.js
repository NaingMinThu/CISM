
var validator = $("#myform").validate({
    rules: {
       // company_name: { required: true },
        user_name: { required: true },
        user_pwd: { required: true }
    },
    messages: {
       // company_name: { required: "Company Name is required!" },
        user_name: { required: "User Name is required!" },
        user_pwd: { required: "Password is required!" }
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
        Process(a, event);
    }
});

$body = $("body");
//$(document).ajaxStart(function () {
//    $body.addClass("loading");
//});
//$(document).ajaxStop(function () {
//    $body.removeClass("loading");
//});
$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

function BindEntity() {
    var a = {
        company_name: $("#company_name").val(),
        user_name: $("#user_name").val(),
        user_pwd: $("#user_pwd").val()
    };
    return a;
}

function Process(a, e) {
    $.ajax({
        url: "/Login/Login",
        type: "POST",
        data: { model: a },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () {
                window.location.href = '/Admin/Dashboard/Index?MenuID=';
            }, 0), e.preventDefault()) :
                (toastr.error("An error [" + d.message + "] occured in the solution!", "Please contact with system administrator."),
                    setTimeout(function () { }, 1e4));
        },
        error: function () { }
    });
}