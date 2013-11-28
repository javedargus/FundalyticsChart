var chart_tree_options = {
    checkbox: false,
    selectMode: 1,
    initAjax: {
        url: "/data/json/tree/chart.json"
    },
    onSelect: function (select, node) {
        if (node.data.isFolder) {
            return false;
        };
        if (node.data.key == "_new") {

            chartbuilder.clear();
        };
        if (node.data.key == "3") {
            chartbuilder.clear();
            chartbuilder.tmp();
        };
        /*
        if (select) {
        chartbuilder.loadseries(node);
        } else {
        chartbuilder.removeseries(node.data.key, false);
        };*/
    },
    onDblClick: function (node, event) {
        node.toggleSelect();
    },
    onKeydown: function (node, event) {
        if (event.which == 32) {
            node.toggleSelect();
            return false;
        };
    },
    onLazyRead: function (node) {
        ajaxloadtree = true,
                    node.appendAjax({
                        url: "/data/json/tree/childNodes.aspx" + node.data.key,
                        debugLazyDelay: 750
                    });
    },
    dnd: {
        revert: false,
        onDragStart: function (node) {
            if (node.data.isFolder) { return false; };
            return true;
        },
        onDragStop: function (node) {

        }
    }
};