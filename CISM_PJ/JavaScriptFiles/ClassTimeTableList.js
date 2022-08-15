var SubjectList = [];
var ClassList = [];
GetYearList("");

$('#grade_id').change(function () {
    GetSubjecListByAcademic($(this).val());
});

function GetSubjecListByAcademic(grade_id) {
    const actionUrl = '/SetupMaster/GetSubjecListByAcademic';
    $.post(actionUrl,
        {
            grade_id: grade_id,
            year: new Date().getFullYear()
        },
        function (data, status) {
            SubjectList = data;
            showSubjectTable();
        });
}

function SubjectList(selected) {
    if (SubjectList !== null) {
        console.log("response.length == > " + SubjectList.length);
        const $select = $('#subject_id');
        $select.find('option').remove();
        //]$select.append('<option value="">-- Please Select --</option>');

        for (let i = 0; i < response.length; i++) {
            $select.append(`<option value="${SubjectList[i].subject_id}">${
                SubjectList[i].subject_name}</option>`);
        }
        if (selected !== "") {
            $select.val(selected);
        }
    }
}

function ClassList(selected) {
    if (ClassList !== null) {
        console.log("response.length == > " + ClassList.length);
        const $select = $('#class_id');
        $select.find('option').remove();
        //]$select.append('<option value="">-- Please Select --</option>');

        for (let i = 0; i < response.length; i++) {
            $select.append(`<option value="${ClassList[i].grade_id}">${
                ClassList[i].year_name}</option>`);
        }
        if (selected !== "") {
            $select.val(selected);
        }
    }
}

function GetYearList(selected) {
    $.get(
        "/SetupMaster/GetYearList",
        function (response) {
            if (response !== null) {
                console.log("response.length == > " + response.length);
                const $select = $('#grade_id');
                $select.find('option').remove();
                //]$select.append('<option value="">-- Please Select --</option>');

                for (let i = 0; i < response.length; i++) {
                    $select.append(`<option value="${response[i].grade_id}">${
                        response[i].year_name}</option>`);
                }
                if (selected !== "") {
                    $select.val(selected);
                }
            }
        });
}