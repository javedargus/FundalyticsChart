// series meta: CONST -- maximum series leaf nodes to display.
var MAX_LEAF_NODES = 30;
// series: meta: all series meta descriptions.
var seriesmeta;

// chart: profiles: build tree.
function getChartTreeProfiles(tree, profiles) {

    var root = tree.getNodeByKey("chart_profiles");

    Enumerable.from(profiles).forEach(function (profile) {

        root.addChild({ title: profile.name, isFolder: false, key: profile.id });
    });

    
    //tree.addChild({ title: "new folder", isFolder: true, unselectable: true, key: "1" });
    
    

    //var thenode = $("#charttree").dynatree("getTree").getNodeByKey("chart_profiles");
    
    //alert(thenode.data.title);
    //alert(node.data.title);
};

// series: load series meta.
function getSeriesMeta() {

    $.ajax({

        dataType: "json",
        url: "/data/json/series/meta-flattened.aspx", 
        async: false,
        success: function (data, textStatus, jqXHR) { seriesmeta = data; }
    });
}

// series: get series tree child nodes.
function getSeriesTreeChildNodes(node) {

    var nodeTags = new Array();
    var series;

    // get node hierarchy tags.
    var tagsJSON = replaceQuotes(node.data.key);
    nodeTags = JSON.parse(tagsJSON);

    // get matching series.
    series = seriesmeta;
    for (var ix = 0; ix < nodeTags.length; ix++) {

        if (ix > 0) {

            var series = 
                Enumerable.from(seriesmeta)
                    .join(series, "$.id", "$.id", function (a, b) {

                        return { id: a.id, datasource: a.datasource, name: a.name, tagtype: a.tagtype, tagvalue: a.tagvalue }
                    })
                .toArray();
        };

        series =
            Enumerable.from(series)
            .where(     function (meta) { return meta.tagtype == nodeTags[ix].type })
            .where(     function (meta) { return meta.tagvalue == nodeTags[ix].value })
            .orderBy(   function (meta) { return meta.name })
        .toArray();
    };

    // display series (leaf nodes).
    if (series.length <= MAX_LEAF_NODES) {

        Enumerable.from(series).forEach(function (item) {

            node.addChild({ title: item.name, isFolder: false, key: item.id });
        });
        return;
    };

    // display distinct sub nodes.
    if (series.length > MAX_LEAF_NODES) {

        // get all the tags that match the selected series.
        var matchedTags = 
            Enumerable.from(seriesmeta)
                .join(series, "$.id", "$.id", function (a, b) {

                    return { type: a.tagtype, value: a.tagvalue }
                })
            .toArray();

        // iterate the tags and build appropriate child nodes and sub nodes.
        var type = ""; var typeNode;
        Enumerable.from(matchedTags)
            .orderBy("$.value").orderBy("$.type")
            .distinct("$.type + '-' + $.value")
            .forEach(
                function (tag) {

                    // check type node not already traversed.
                    if (!matchedTagType(nodeTags, tag.type)) {

                        // add type node.
                        if (tag.type != type) {

                            type = tag.type;
                            typeNode = node.addChild({ title: tag.type, isFolder: true, unselectable: true, key: tag.type });
                        };

                        // add value (child) node.
                        typeNode.addChild({ title: tag.value, isFolder: true, unselectable: true, isLazy: true, key: getValueNodeKey(nodeTags, tag) });
                    };
                });

        return;
    };
}

// series: tags: get tags for current tree folder in hierarchy.
function getValueNodeKey(parentTags, tag) {

    var key = "";

    Enumerable.from(parentTags).forEach(function (item) {

        key = key + "{&quot;type&quot;: &quot;" + item.type + "&quot;, &quot;value&quot;: &quot;" + item.value + "&quot;},";
    });
    key = key + "{&quot;type&quot;: &quot;" + tag.type + "&quot;, &quot;value&quot;: &quot;" + tag.value + "&quot;}";

    return "[" + key + "]";
};

// series: tags: check if tags array contains a given type.
function matchedTagType(list, tagType) {

    var matched = false;
    Enumerable.from(list).forEach(function (tag) {

        if (tag.type == tagType) { matched = true; }
    });
    return matched;
};

// general: replace &quot; in string with quote (").
function replaceQuotes(str) {

    var quot = '&quot;';

    return str.replace(new RegExp(quot, 'g'), '"');
};

