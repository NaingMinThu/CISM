
//Ref --https://bootsnipp.com/snippets/featured/bootstrap-30-treeview

$.fn.extend({
    treed: function (o) {

        var openedClass = 'fa-minus-circle';
        var closedClass = 'fa-plus-circle';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        };

        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').has("ul").each(function () {
            var branch = $(this); //li with children ul
            branch.prepend("<i class='indicator fa " + closedClass + "'></i>");
            branch.addClass('branch');
            branch.on('click', function (e) {
                if (this == e.target) {
                    var icon = $(this).children('i:first');
                    icon.toggleClass(openedClass + " " + closedClass);
                    $(this).children().children().toggle();
                }
            })
            branch.children().children().toggle();
        });

        //fire event from the dynamically added icon
        tree.find('.indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });
    }
});


