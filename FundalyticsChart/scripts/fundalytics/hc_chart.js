// class : hc_chart.
function hc_chart() {

    // properties.
    this.chart_options;
    this.chart;
    this.series = new Array();
    this.timer;
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
    this.initialise = function (title) {

        this.chart_options = {
            chart: {
                renderTo: 'hc_chart',
                type: 'spline',
                zoomType: 'x',
                x: -20
            },
            rangeSelector: {
                buttons: [
                            { type: 'day', count: 3, text: '3d' },
                            { type: 'week', count: 1, text: '1w' },
                            { type: 'month', count: 1, text: '1m' },
                            { type: 'month', count: 6, text: '6m' },
                            { type: 'year', count: 1, text: '1y' },
                            { type: 'all', text: 'All' }
                        ]
            },
            title: {
                text: '',
                style: {
                    display: 'none'
                }
            },
            subtitle: {
                text: '',
                style: {
                    display: 'none'
                }
            },
            xAxis: {
                type: 'datetime',
                dateTimeLabelFormats: {
                    month: '%e. %b',
                    year: '%b'
                }
            },
            plotOptions: {
                line: {
                    marker: {
                        enabled: false
                    }
                },
                spline: {
                    marker: {
                        enabled: false
                    }
                }
            }
        };
    };

    // event : initialisechart.
    this.initialisechart = function () {

        this.chart = new Highcharts.Chart(this.chart_options);
    };

    // event : add series.
    this.addseries = function (node, data) {

        this.series[this.series.length] = new hc_series(node.data.key, node.data.title, data, node);
        this.chart.addSeries({ name: '' + node.data.title + '', data: data });
        $("#series_row_" + node.data.key).removeClass("series_row_loading");
    };

    // add series: row.
    this.addseriesrow = function (id, name, data) {

        var html =
                    "<div id='series_row_" + id + "' class='series_row series_row_loading'>" +
                        "<div class='series_row_title'>" + name + "</div>" +
                        "<div class='series_row_remove'>" +
                            "<a href='javascript:hc_chart.removeseries(" + id + ", true)'>remove</a>" +
                        "</div>" +
                        "<div class='series_row_series_type'>" +
                            "<select class='series_type' style='width: 100px;' onchange='hc_chart.changeseries(" + id + ", this.value);'>" +
                                "<option>area</option>" +
                                "<option>areaspline</option>" +
                                "<option>column</option>" +
                                "<option>line</option>" +
                                "<option>pie</option>" +
                                "<option selected>spline</option>" +
                            "</select>" +
                        "</div>" +
                    "</div>"
        $("#hc_series").append(html);
    };

    // event : change series [type].
    this.changeseries = function (id, seriesType) {

        this.chart.series[this.seriesIndex(id)].update({ type: seriesType });
    };

    // event : load series.
    this.loadseries = function (node, seriesFrom, seriesFromHour) {

        if (this.series.length > 100) { return false; };

        hc_chart.addseriesrow(node.data.key, node.data.title);
        
        $.getJSON("/data/json/series/data.aspx?seriesid=" + node.data.key + "&seriesfrom=" + seriesFrom.toString("d-MMM-yyyy") + "&seriesfromhour=" + seriesFromHour, function (data) {
            hc_chart.addseries(node, data);
        })
        .done(function () { })
        .fail(function (jqXHR, textStatus, errorThrown) { alert(errorThrown); })
        .always(function () { });
    };

    // event : reload all series.
    this.reloadallseries = function (seriesFrom, seriesFromHour) {

        if (this.series.length == 0) { return; };

        var seriesidx = -1; var seriesdata; var flush = {};

        //this.chart.series[0].setData(flush, true); 
        this.chart.series[0].remove();

        for (var ix = 0; ix < this.series.length; ix++) {

            $("#series_row_" + this.series[ix].id).addClass("series_row_loading");


            $.getJSON("/data/json/series/data.aspx?seriesid=" + this.series[ix].id + "&seriesfrom=" + seriesFrom.toString("d-MMM-yyyy") + "&seriesfromhour=" + seriesFromHour, function (data) {
                seriesidx = ix; seriesdata = data;
            })
            .done(function () { alert(seriesdata);  /*this.timer = setInterval(function () { alert('reloading ...'); hc_chart.donevent(seriesdata); }, 500);*/ })
            .fail(function (jqXHR, textStatus, errorThrown) { alert(errorThrown); })
            .always(function () { });
        };
    };

    this.donevent = function (seriesdata) {

        alert("*" + seriesdata + "*");
        if (seriesdata == "") { alert('nowt'); return; };

        this.chart.addSeries({ name: '' + "new title" + '', data: seriesdata });
        clearInterval(this.timer);
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
};

// class : hc_series.
function hc_series(id, name, data, node) {

    // properties.
    this.id = id;
    this.name = name;
    this.data = data;
    this.node = node;
};