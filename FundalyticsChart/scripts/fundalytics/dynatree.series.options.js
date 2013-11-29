var series_tree_options = {
    checkbox: false,
    selectMode: 2,
    initAjax: {
        url: "/data/dynatree/seriesRoot.aspx"
    },
    onSelect: function (select, node) {

        if (node.data.isFolder) { return false; };
        if (select) {
            chartbuilder.loadseries(node);
        } else {
            chartbuilder.removeseries(node.data.key, false);
        };
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

        getSeriesTreeChildNodes(node);
        //addSeriesNodes(node);
        /*
        ajaxloadtree = true,
                    node.appendAjax({
                        url: "/data/json/tree/childNodes.aspx" + node.data.key,
                        debugLazyDelay: 750
                    });*/
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