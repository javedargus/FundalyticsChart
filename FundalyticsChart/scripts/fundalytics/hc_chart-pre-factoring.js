// class : hc_chart.
function hc_chart() {

    // properties.
    this.chart_options;
    this.theme = "{}";
    this.markers = false;
    this.chart;
    this.fromdate = (90).days().ago();
    this.fromhour = 0;
    this.series = new Array();

    this.seriesIndex = function (seriesId) {

        var seriesName;

        for (var ix = 0; ix < this.series.length; ix++) {

            if (this.series[ix].id == seriesId) {
                seriesName = this.series[ix].name;
            };
        };
        for (var iy = 0; iy < this.chart.series.length; iy++) {

            if (this.chart.series[iy].name == seriesName) {
                return iy;
            };
        };
        return false;
    };

    // event : initialise.
    this.initialise = function (options, theme) {

        this.chart_options = options;
        this.theme = theme;
        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));
    };

    // event : add series.
    this.addseries = function (node, seriesType, data) {

        this.series[this.series.length] = new hc_series(node.data.key, node.data.title, seriesType, data, node);
        this.chart.addSeries({ name: '' + node.data.title + '', type: seriesType, data: data });
        $("#series_row_" + node.data.key).removeClass("series_row_loading");
    };

    // add series: row.
    this.addseriesrow = function (id, name, seriesType) {

        var html =
                "<div id='series_row_" + id + "' class='series_row series_row_loading'>" +
                    "<div class='series_row_title'>" + name + "</div>" +
                    "<div class='series_row_remove'>" +
                        "<a href='javascript:hc_chart.removeseries(" + id + ", true)'>remove</a>" +
                    "</div>" +
                    "<div class='series_row_series_type'>" +
                        "<select class='series_type' style='width: 100px;' onchange='hc_chart.changeseries(" + id + ", this.value);'>" +
                            "<option" + ((seriesType == 'area') ? " selected>" : ">") + "area</option>" +
                            "<option" + ((seriesType == 'areaspline') ? " selected>" : ">") + "areaspline</option>" +
                            "<option" + ((seriesType == 'column') ? " selected>" : ">") + "column</option>" +
                            "<option" + ((seriesType == 'line') ? " selected>" : ">") + "line</option>" +
                            "<option" + ((seriesType == 'spline') ? " selected>" : ">") + "spline</option>" +
                        "</select>" +
                    "</div>" +
                "</div>";
        $("#hc_series").append(html);
    };

    // event: clear all series.
    this.clearseries = function () {

        if (this.series.length == 0) { return; };

        var nodes = new Array();
        for (var ix = 0; ix < this.series.length; ix++) {

            nodes[ix] = this.series[ix].node;
        };
        for (var ix = 0; ix < nodes.length; ix++) {

            nodes[ix].toggleSelect();
        };

        this.series = new Array();
        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));
    };

    // event : change series [type].
    this.changeseries = function (id, seriesType) {

        this.chart.series[this.seriesIndex(id)].update({ type: seriesType });
    };

    // event : load series.
    this.loadseries = function (node, seriesType) {

        if (this.series.length > 100) { return false; };

        this.addseriesrow(node.data.key, node.data.title, seriesType);

        $.getJSON("/data/json/series/data.aspx?seriesid=" + node.data.key + "&seriesfrom=" + this.fromdate.toString("d-MMM-yyyy") + "&seriesfromhour=" + this.fromhour, function (data) {
            hc_chart.addseries(node, seriesType, data);
        })
        .done(function () { })
        .fail(function (jqXHR, textStatus, errorThrown) { alert(errorThrown); })
        .always(function () { });
    };

    // event : reload all series.
    this.reloadallseries = function (seriesFrom, seriesFromHour) {

        if (this.series.length == 0) { return; };

        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));

        for (var ix = 0; ix < this.series.length; ix++) {

            $("#series_row_" + this.series[ix].id).addClass("series_row_loading");

            $.ajax({

                dataType: "json",
                url: "/data/json/series/data.aspx?seriesid=" + this.series[ix].id + "&seriesfrom=" + this.fromdate.toString("d-MMM-yyyy") + "&seriesfromhour=" + this.fromhour,
                success: function (data) {
                    hc_chart.series[ix].data = data;
                    hc_chart.chart.addSeries({ name: '' + hc_chart.series[ix].name + '', type: hc_chart.series[ix].type, data: hc_chart.series[ix].data });
                    $("#series_row_" + hc_chart.series[ix].id).removeClass("series_row_loading");
                },
                async: false
            });
        };
    };

    // event : remove series.
    this.removeseries = function (id, toggle) {

        $("#series_row_" + id).remove();
        this.chart.series[this.seriesIndex(id)].remove();

        var newseries = new Array();
        for (var ix = 0; ix < this.series.length; ix++) {

            if (this.series[ix].id == id) {
                if (toggle) { this.series[ix].node.toggleSelect(); };
            };
            if (this.series[ix].id != id) {
                newseries[newseries.length] = this.series[ix];
            };
        };

        this.series = newseries;
        if (this.series.length == 0) {

            this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));
        };
    };

    // event : refresh (all) series.
    this.refresh = function () {

        var ix;
        for (ix = 0; ix < this.series.length; ix++) {

            var refreshchart = chart.series[ix];
            var existingdata = this.series[ix].data;

            try {

                $.getJSON("getargusrefreshjson.aspx?datasourceid=" + this.series[ix].id, function (newdata) {

                    $.each(newdata, function (i, newvalue) {

                        try {

                            var exists = false;

                            $.each(existingdata, function (i, existingvalue) {

                                if (existingvalue[0] == newvalue[1]) { exists = true; }
                            });

                            if (!exists) {

                                alert("new value found");
                                refreshchart.addPoint([newvalue[1], newvalue[2]]);
                            };

                        } catch (err) { alert(err); };
                    });
                });
            } catch (err) { alert(err); };
        };
    };

    // set series line markers.
    this.setmarkers = function () {

        if (this.series.length == 0) { return; };
        for (var ix = 0; ix < this.series.length; ix++) {

            this.chart.series[ix].update({ marker: { enabled: this.markers} });
        };
    };

    // set (custom) theme.
    this.settheme = function (toggle) {

        this.theme = (toggle) ? grey_theme : {};
        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));

        if (this.series.length == 0) { return; };
        for (var ix = 0; ix < this.series.length; ix++) {

            this.chart.addSeries({ name: '' + this.series[ix].name + '', type: this.series[ix].type, data: this.series[ix].data });
        };
    };

};

// class : hc_series.
function hc_series(id, name, type, data, node) {

    // properties.
    this.id = id;
    this.name = name;
    this.type = type;
    this.data = data;
    this.node = node;
};
