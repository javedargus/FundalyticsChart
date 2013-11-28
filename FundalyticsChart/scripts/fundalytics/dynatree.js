// (all) series meta descriptions.
var seriesmeta;

// load series meta.
function getSeriesMeta() {

    $.ajax({

        dataType: "json",
        url: "/data/json/series/meta-flattened.aspx", 
        async: false,
        success: function (data, textStatus, jqXHR) { seriesmeta = data; }
    });
}

