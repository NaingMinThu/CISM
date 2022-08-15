
//Ref --https://www.jqueryscript.net/table/Minimal-Tree-Table-jQuery-Plugin-For-Bootstrap-TreeTable.html

$(document).ready(function () {
    var
        $table = $('#tree-table'),
        rows = $table.find('tr');

    rows.each(function (index, row) {
        var
            $row = $(row),
            level = $row.data('level'),
            id = $row.data('id'),
            $columnName = $row.find('td[data-column="name"]'),
            children = $table.find('tr[data-parent="' + id + '"]');
        if (children.length) {
            $columnName.prepend('' +
                '<i class="fa fa-plus-circle parent" onclick="expandIcon__(this)" data-exp="close"></i>' +
                '');
            children.hide();
        }
        $columnName.prepend('' +
            '<span class="treegrid-indent" style="width:' + 5 * level + '%"></span>' +
            '');
    });
})


// Reverse hide all elements
reverseHide = function (table, element) {
    var
        $element = $(element),
        id = $element.data('id'),
        children = table.find('tr[data-parent="' + id + '"]');

    if (children.length) {
        children.each(function (i, e) {
            var $icon = $(e).find('i');
            $icon
            .removeClass('fa fa-minus-circle')
            .addClass('fa fa-plus-circle');

            $icon.data('exp', 'close');
            reverseHide(table, e);
        });

        children.hide(0, { direction: "up" });
    }
};

//For Icon
function expandIcon__(e) {
    var
        $table = $('#tree-table'),
        tr = $(e).closest('tr'),
        id = tr.data('id'),
        children = $table.find('tr[data-parent="' + id + '"]'),
        exp = $(e).data('exp');
    //console.log(exp);

    if (exp == "close") {
        $(e)
            .removeClass('fa fa-plus-circle')
            .addClass('fa fa-minus-circle');
        $(e).data('exp', 'open')
        children.show(0, { direction: "up" });
    }
    else {
        $(e)
            .removeClass('fa fa-minus-circle')
            .addClass('fa fa-plus-circle');
        $(e).data('exp', 'close')
        reverseHide($table, tr);
    }
}

//For Row 
function expandRow__(e) {
    var
        $table = $('#tree-table'),
        tr = $(e).closest('tr'),
        id = tr.data('id'),
        children = $table.find('tr[data-parent="' + id + '"]'),
        exp = $(e).data('exp');
    console.log(exp);

    if (exp == "close") {
        $(e).find("i")
            .removeClass('fa fa-plus-circle')
            .addClass('fa fa-minus-circle');
        $(e).data('exp', 'open')
        children.show();
    }
    else {
        $(e).find("i")
            .removeClass('fa fa-minus-circle')
            .addClass('fa fa-plus-circle');
        $(e).data('exp', 'close')
        reverseHide($table, tr);
    }
}
