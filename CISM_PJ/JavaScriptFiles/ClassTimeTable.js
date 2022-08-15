var SubjectList = [];
var ClassList = [];
var ClassTimeTable = [];
var validator = $("#mywoform").validate({
    rules: {
        grade_id: { required: true },
        class_id: { required: true },
        affected_date: { required: true },
        weekday: { required: true },
        subject_id: { required: true },
        from_time: { required: true },
        to_time: { required: true }
    },
    messages: {
        grade_id: { required: "Grade is required!" },
        class_id: { required: "Class is required!" },
        affected_date: { required: "Affected Date is required!" },
        weekday: { required: "Week Day is required!" },
        subject_id: { required: "Subject is required!" },
        from_time: { required: "Start Time is required!" },
        to_time: { required: "End Time is required!" }
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

$("#affected_date").keypress(function (event) { event.preventDefault(); });
    
initialize();
function initialize() {
    setDateCtrl("#affected_date");
    setToday("#affected_date");
    var id = urlParam("id");
    if (id != "" && id != null) {
        let _isedit = $("#hid_isAut_Edit").val();
        var isEdit = _isedit === "True" ? 1 : 0;
        if (isEdit) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }
    GetTimeTable();
}

function GetTimeTable() {
    const actionUrl = '/TimeTable/ClassTimeTable/GetClassTimeTable';
    $.post(actionUrl,
        {
            time_table_id: urlParam("id")
        },
        function (data, status) {
            var Master = data.Master;
            console.log(Master);
            if (Master.length > 0) {
                GetGradeList(Master[0].grade_id, new Date(Master[0].affected_date).getFullYear());
                $('#affected_date').datepicker('setDate', new Date(Master[0].affected_date));

                console.log("new Date(Master[0].affected_date).getFullYear() ==> " + $('#affected_date').val() + "dd" + new Date($('#affected_date').val()));
                GetSubjecListByAcademic(Master[0].grade_id, Master[0].year);
                GetClassListByAcademic(Master[0].grade_id, Master[0].class_id);
                //$('#class_id').val(Master[0].class_id);
                $('#grade_id').attr('disabled', true);
                $('#affected_date').attr('disabled', true);
                $('#class_id').attr('disabled', true);
                $('#state').val("Modified");
                $('#btnSave').val("Update");
            }
            else {
                $('#state').val("Added");
                $('#btnSave').val("Save");
                GetGradeList("", (new Date()).getFullYear());
            }
            if (data.Detail.length > 0) {
                ClassTimeTable = data.Detail;
                showTimeTable();
            }
        });
}

function BindEntity() {
    var a = {
        time_table_id : urlParam("id"),
        grade_id: $("#grade_id").val(),
        class_id: $("#class_id").val(),
        affected_date: $("#affected_date").val(),
        state: $("#state").val()
    };
    return a;
}

function Merge(a) {
    $.ajax({
        url: "/TimeTable/ClassTimeTable/ClassTimeTableMerge",
        type: "POST",
        data: {
            model: a,
            detailList: ClassTimeTable
        },
        async: !0,
        dataType: "json",
        success: function (d) {
            d.errorcode === 0 ? (toastr.success(d.message), setTimeout(function () {
                window.location.href = '/TimeTable/ClassTimeTable/ClassTimeTable?MenuID=' + urlParam('MenuID');
            }, 2500)) :
                (toastr.error(d.message));
        },
        error: function () { }
    });
}

$('#grade_id').change(function () {
    GetSubjecListByAcademic($(this).val(), new Date($('#affected_date').val()).getFullYear());
    GetClassListByAcademic($(this).val(),'');
});

function GetSubjecListByAcademic(grade_id,year) {
    const actionUrl = '/TimeTable/ClassTimeTable/GetSubjecListByAcademic';
    $.post(actionUrl,
        {
            grade_id: grade_id,
            year: year
        },
        function (data, status) {
            SubjectList = data;
            GetSubjectList('');
        });
}

function GetClassListByAcademic(grade_id,selected) {
    const actionUrl = '/TimeTable/ClassTimeTable/GetClassListByAcademic';
    $.post(actionUrl,
        {
            grade_id: grade_id
        },
        function (data, status) {
            ClassList = data;
            GetClassList(selected);
        });
}

function GetSubjectList(selected) {
    if (SubjectList !== null) {
        console.log("response.length == > " + SubjectList.length);
        const $select = $('#subject_id');
        $select.find('option').remove();
        //]$select.append('<option value="">-- Please Select --</option>');

        for (let i = 0; i < SubjectList.length; i++) {
            $select.append(`<option value="${SubjectList[i].subject_id}">${
                SubjectList[i].subject_name}</option>`);
        }
        if (selected !== "") {
            $select.val(selected);
        }
    }
}

function GetClassList(selected) {
    if (ClassList !== null) {
        console.log("response.length == > " + ClassList.length);
        const $select = $('#class_id');
        $select.find('option').remove();
        //]$select.append('<option value="">-- Please Select --</option>');

        for (let i = 0; i < ClassList.length; i++) {
            $select.append(`<option value="${ClassList[i].class_id}">${
                ClassList[i].class_name}</option>`);
        }
        if (selected !== "") {
            $select.val(selected);
        }
    }
}

function GetGradeList(selected , year) {
    $.get(
        "/SetupMaster/GetGradeList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#grade_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');
                if (urlParam("id") == "" || urlParam("id") == null) {
                    GetSubjecListByAcademic(response[0].grade_id, year);
                    GetClassListByAcademic(response[0].grade_id,'');
                }              
                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].grade_id}">${
                        response[i].grade_name}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}

function showTimeTable() {
    $("#TimeTableDataBody").empty();
    for (var i = 0; i < ClassTimeTable.length; i++) {
        ////Re order index after delelting or inserting
        ClassTimeTable[i]["index"] = i + 1;
        var tr = "<tr>";
        var td1 = "<td hidden>" + ClassTimeTable[i]["period_id"] + "</td>";
        var td2 = "<td>" + ClassTimeTable[i]["time_period"] + "</td>";
        var td3 = "<td>" + ClassTimeTable[i]["monday_List_data"] + "</td>";
        var td4 = "<td>" + ClassTimeTable[i]["tue_List_data"] + "</td>";
        var td5 = "<td>" + ClassTimeTable[i]["wed_List_data"] + "</td>";
        var td6 = "<td>" + ClassTimeTable[i]["thur_List_data"] + "</td>";
        var td7 = "<td>" + ClassTimeTable[i]["fri_List_data"] + "</td>";
        $("#TimeTableDataBody").append(tr + td1 + td2 + td3 + td4 + td5 + td6 + td7 + "</tr>");

        console.log(ClassTimeTable);
    }
}



//$("#btnAdd").click(function () {
//    var sub_index = $("#index").val();  
//    if (sub_index != null && sub_index != "") {
//        var ext_list = ClassTimeTable.filter(x => x.subject_id == $("#subject_id").val() && x.index != sub_index
//            && x.weekday == $("#weekday").val() && x.from_time == $("#from_time").val() && x.to_time == $("#to_time").val());
//        console.log("ext_list ==>" + ext_list);
//        if (ext_list.length > 0) {
//            toastr.error("This period for subject and weekday is already added!", "Duplicate Data"),
//                setTimeout(function () { }, 1e4);
//        }
//        else {
//            $.each(ClassTimeTable, function (index, obj) {
//                if (obj.index == sub_index) {
//                    BindJsonClasTimeTable(obj);
//                    return false;
//                }
//            });
//        }
//    }
//    else {
//        var exit_list = ClassTimeTable.filter(x => x.subject_id == $("#subject_id").val()
//            && x.weekday == $("#weekday").val() && x.from_time == $("#from_time").val() && x.to_time == $("#to_time").val());
//        if (exit_list.length > 0) {
//            toastr.error("This period for subject and weekday is already added!", "Duplicate Data"),
//                setTimeout(function () { }, 1e4);
//        }
//        else {
//            if (($("#subject_id").val() != "" && $("#subject_id").val() != null)
//                && ($("#weekday").val() != "" && $("#weekday").val() != null)
//                && ($("#from_time").val() != "" && $("#from_time").val() != "")
//                && ($("#to_time").val() != "" && $("#to_time").val() != "")) {
//                var data_info = BindJsonClasTimeTable({});
//                ClassTimeTable.push(data_info);
//            }
//        }
//    }
//    console.log(ClassTimeTable);
//    showTimeTable();
//    clearDataSubejctInfo();
//});

//function BindJsonClasTimeTable(obj) {
//    obj.index = $("#index").val();
//    obj.subject_id = $("#subject_id").val();
//    obj.weekday = $("#weekday").val();
//    obj.weekday_name = $("#weekday option:selected").text();
//    obj.subject_name = $("#subject_id option:selected").text();
//    obj.from_time = $("#from_time").val();
//    obj.to_time = $("#to_time").val();
//    obj.isDelete = false;
//    return obj;
//}

//$("#btnClear_UOMCon").click(function () {
//    clearSubject();
//});

//function EditData(con_index) {
//    $.each(ClassTimeTable, function (index, obj) {
//        if (obj.index == con_index) {
//            BindControlsClassTimeTable(obj);
//            $("#btnAdd").val("Update");
//            return false;
//        }
//    });
//}

//function DeleteData(con_index) {
//    $.each(ClassTimeTable, function (index, obj) {
//        if (obj.index == con_index) {
//            obj.index = 0;
//            obj.isDelete = true;
//            return false;
//        }
//    });
//    showTimeTable();
//    clearDataSubejctInfo();
//}

//function BindControlsClassTimeTable(obj) {
//    $("#index").val(obj.index);
//    $("#subject_id").val(obj.subject_id);
//    $("#weekday").val(obj.weekday);
//    $("#from_time").val(obj.from_time);
//    $("#to_time").val(obj.to_time);
//    $("#weekday").val(obj.weekday);
//    $("#btnAdd").val("Update");
//}


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
// for Update
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

$('#BackBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/TimeTable/ClassTimeTable/ClassTimeTable?MenuID=' + urlParam('MenuID');
});
$('#CancelBtn').click(function (e) {
    e.preventDefault();
    window.location.href = '/TimeTable/ClassTimeTable/ClassTimeTable?MenuID=' + urlParam('MenuID');
});