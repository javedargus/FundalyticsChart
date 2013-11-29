// ** class : hc_chart.
function hc_chart() {

    // ** properties.
    this.chart_options;     // -- highseries chart options.
    this.theme = "{}";      // -- highcharts theme.
    this.markers = false;   // -- display series point markers (toggle).
    this.chart;             // -- highcharts instance.
    this.fromdate = (90).days().ago();  // -- series from date.
    this.fromhour = 0;                  // -- series from hour.
    this.series = new Array();          // -- series (properties) array (id, name, type, data).
        
    this.seriesIndex = function (seriesId) {    // -- find the series index of a given highcharts series.

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

    // ** chart (instance): -- initialise.
    this.initialise = function (options, theme, markers, fromdate) {

        if (typeof fromdate != "undefined") { this.fromdate = fromdate; };

        if (markers) {

            options.plotOptions.area.marker.enabled = true;
            options.plotOptions.areaspline.marker.enabled = true;
            options.plotOptions.column.marker.enabled = true;
            options.plotOptions.line.marker.enabled = true;
            options.plotOptions.spline.marker.enabled = true;
        };

        this.chart_options = options;
        this.theme = theme;

        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));

    };

    // ** chart: load and display (series).
    this.load = function (series) {

        this.series = series;

        for (var ix = 0; ix < this.series.length; ix++) {

            this.chartseries(this.series[ix], this.chart);
        };
    };
    
    // ** chart -- change the (from) date and time.
    this.changeDate = function (date, hour) {

        var prevDate = this.fromdate;   // store prev date & time.
        var prevTime = this.fromhour;

        this.fromdate = date;           // set new date and time.
        this.fromhour = hour;

        // no chart series to sync.
        if (this.series.length == 0) {
            return;
        };

        // sync chart series to new date. 
        if (this.fromdate >= prevDate) {

            // date > prev: zoom the chart.
            var todate = new Date(this.chart.xAxis[0].max);
            this.chart.xAxis[0].setExtremes(this.fromdate, todate);
            this.chart.showResetZoom();

        } else {

            // date < prev: reload all chart series.
            this.refreshchart();
        };
    };
    
    // ** chart: -- clear.
    this.clearchart = function () {

        $("#tree").dynatree("getSelectedNodes")

        this.series = new Array();
        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));
    };

    // ** chart: -- refresh & reload related series.
    this.refreshchart = function () {

        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));

        for (var ix = 0; ix < this.series.length; ix++) {

            this.chartseries(this.series[ix], this.chart);
        };
    };

    // ** chart: series: -- add a series to the chart.
    this.chartseries = function (series, chart) {

        $.ajax({

            dataType: "json",
            url: "/data/json/series/data.aspx?seriesid=" + series.id + "&seriesfrom=" + this.fromdate.toString("d-MMM-yyyy") + "&seriesfromhour=" + this.fromhour,
            async: true,
            success: function (data, textStatus, jqXHR) {
                chart.addSeries({ name: '' + replaceComma(series.name) + '', type: series.type, data: data })     // add to chart.
                series.data = data;     // store the series data.
            }
        })
    };

    // ** chart: series: -- load a new series.
    this.loadseries = function (id, name, type) {

        var series = new hc_series(id, name, type, null);   // store the series.
        this.series[this.series.length] = series;

        // add the series to the chart.
        this.chartseries(series, this.chart);
    };

    // ** chart: series: -- remove an existing series.
    this.removeseries = function (id) {

        var newseries = new Array();
        
        this.chart.series[this.seriesIndex(id)].remove();
        for (var ix = 0; ix < this.series.length; ix++) {

            if (this.series[ix].id != id) { newseries[newseries.length] = this.series[ix]; };
        };

        this.series = newseries;
        if (this.series.length == 0) { this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme)); };
    };

    // ** chart: series: -- to JSON string.
    this.seriesToJSON = function () {

        var series = this.series;
        for (var ix = 0; ix < series.length; ix++) {

            series[ix].data = null;
        };

        return JSON.stringify(this.series);
    };

    // ** chart: set theme.
    this.settheme = function () {

        // set theme and re-init chart.
        this.chart = new Highcharts.Chart(Highcharts.merge(this.chart_options, this.theme));

        // re-add any existing series to new chart.
        if (this.series.length == 0) { return; };
        for (var ix = 0; ix < this.series.length; ix++) {

            this.chart.addSeries({ name: '' + replaceComma(this.series[ix].name) + '', type: this.series[ix].type, data: this.series[ix].data });
        };
    };
    
    // ** chart: series: -- set line markers (toogle).
    this.setmarkers = function () {

        if (this.series.length == 0) { return; };
        for (var ix = 0; ix < this.series.length; ix++) {

            this.chart.series[ix].update({ marker: { enabled: this.markers} });
        };
    };

    // ** chart: series: type: -- change the highcharts series type.
    this.changeseriestype = function (id, type) {

        this.series[this.seriesIndex(id)].type = type;
        this.chart.series[this.seriesIndex(id)].update({ type: type });
    };
};

// ** class : hc_series.
function hc_series(id, name, type, data) {

    // properties.
    this.id = id;
    this.name = name;
    this.type = type;
    this.data = data;
};

// general: replace comma in string with " -".
function replaceComma(str) {

    var comma = ',';

    return str.replace(new RegExp(comma, 'g'), ' -');
};
