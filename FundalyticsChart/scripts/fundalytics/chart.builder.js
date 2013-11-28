// ** chartbuilder: class. 
function chartbuilder() {

    // ** properties.
    this.hc_chart;                          // -- highcharts wrapper instance.
    this.seriesDefaultType = "spline";      // -- default chart series type.
    this.seriesRows = $("#seriesrows");     // -- chart series rows container.
    this.seriesTree = $("#seriestree");     // -- series tree (dynatree).

    // ** tmp //
    this.tmp = function () {

        var series = new Array();
        series[series.length] = new hc_series(34000, "GASSCO, Dornum Flows MSm3/d", "spline", null);
        series[series.length] = new hc_series(34003, "GASSCO, Dunkerque Flows MSm3/d", "spline", null);
        series[series.length] = new hc_series(34005, "GASSCO, Easington Flows MSm3/d", "spline", null);

        this.hc_chart.fromdate = new Date("26 Nov 2013");
        this.hc_chart.fromhour = 0;
        this.hc_chart.series = series;
        this.hc_chart.refreshchart();
    };

    
    
    // ** chartbuilder: initialise object.
    this.initialise = function (options, theme) {

        // create instance of hc_chart and initialise.
        this.hc_chart = new hc_chart();
        this.hc_chart.initialise(options, theme);
    };

    // chart: flush chart and related series.
    this.clear = function () {

        var nodes = $("#seriestree").dynatree("getSelectedNodes");
        if (nodes.length > 0) {
            
            for (var ix = 0; ix < nodes.length; ix++) { nodes[ix].select(false); };
        };

        this.seriesRows.html("");
        this.hc_chart.clearchart();
    };

    // ** chart: launch in new window.
    this.launch = function () {

        var theme = $("#themedarkgrey").attr('checked') ? "true" : "false";
        var markers = $("#seriesmarkers").attr('checked') ? "false" : "true";

        var href =
            "/pages/pop-up.aspx" +
                "?greytheme=" + theme +
                "&markers=" + markers +
                "&fromdate=" + $.datepicker.formatDate('d M yy', chartbuilder.hc_chart.fromdate) +
                "&fromhour=" + chartbuilder.hc_chart.fromhour +
                "&series=" + chartbuilder.hc_chart.seriesToJSON();

        window.open(href, "", "width=820,height=425");
    };

    // ** series: load a new series.
    this.loadseries = function (node) {

        this.addSeriesRow(node.data.key, node.data.title)
        this.hc_chart.loadseries(node.data.key, node.data.title, chartbuilder.seriesDefaultType);
        $("#series_row_" + node.data.key).removeClass("series_row_loading");
    };

    // ** series: remove a series.
    this.removeseries = function (id, toggle) {

        if (toggle) {   // deselect series tree node.

            var node = this.seriesTree.dynatree("getTree").getNodeByKey(id.toString());
            node.toggleSelect();
        };
        $("#series_row_" + id).remove();    // remove series from row list.
        this.hc_chart.removeseries(id);     // remove series from chart.
    };

    // ** series: reload (all) series.
    this.reloadseries = function () {

        this.hc_chart.refreshchart();
    };
    
    // ** series: row: add a series row.
    this.addSeriesRow = function (id, title) {

        var html =
                "<div id='series_row_" + id + "' class='series_row series_row_loading'>" +
                    "<div class='series_row_title'>" + title + "</div>" +
                    "<div class='series_row_remove'>" +
                        "<a href='javascript:chartbuilder.removeseries(" + id + ", true)'>remove</a>" +
                    "</div>" +
                    "<div class='series_row_series_type'>" +
                        "<select class='series_type' style='width: 100px;' onchange='chartbuilder.hc_chart.changeseriestype(" + id + ", this.value);'>" +
                            "<option" + ((this.seriesDefaultType == 'area') ? " selected>" : ">") + "area</option>" +
                            "<option" + ((this.seriesDefaultType == 'areaspline') ? " selected>" : ">") + "areaspline</option>" +
                            "<option" + ((this.seriesDefaultType == 'column') ? " selected>" : ">") + "column</option>" +
                            "<option" + ((this.seriesDefaultType == 'line') ? " selected>" : ">") + "line</option>" +
                            "<option" + ((this.seriesDefaultType == 'spline') ? " selected>" : ">") + "spline</option>" +
                        "</select>" +
                    "</div>" +
                "</div>"; 
        this.seriesRows.append(html);
    };
};

// initialise objects and bind ui interface elements.
$(function () {

    chartbuilder = new chartbuilder();                              // init: chart builder.
    chartbuilder.initialise(chart_options, {});

    $("#seriestree").dynatree(series_tree_options);                 // init: series tree.
    $("#charttree").dynatree(chart_tree_options);                   // init: chart tree.
    setTimeout(function () { getSeriesMeta(); }, 2000);             // init: load all series meta descriptions.

    // initialise date picker.
    $("#datepicker").datepicker({

        /* event handler: on select */
        onSelect: function (date) {

            var fromDate = new Date(Date.parse(date));              // set the new date and hour.
            var fromHour = 0;

            $(".quickpicker").removeClass("quickpickerselected");   // disassociate quick date picker.
            chartbuilder.hc_chart.changeDate(fromDate, fromHour);   // trigger change date in the chart object.
        },
        changeMonth: true,
        changeYear: true,
        showButtonPanel: false
    });

    // drag drop series initialisation.
    $("#hc_container").droppable({
        hoverClass: "drophover",
        addClasses: true,
        over: function (event, ui) {

        },
        drop: function (event, ui) {
            var node = ui.helper.data("dtSourceNode");
            node.toggleSelect();
        }
    });

    // sync initial date.
    $("#datepicker").datepicker("setDate",
                new Date(
                    chartbuilder.hc_chart.fromdate.getFullYear(),
                    chartbuilder.hc_chart.fromdate.getMonth(),
                    chartbuilder.hc_chart.fromdate.getDate()
                )
            );

    // initialise quick date picker.
    $(".quickpicker").bind({
        click: function () {

            // get picked unit. 
            var picked = $(this).val();
            var unit = parseInt(picked.replace(/\D/g, ''));
            // set newly selected date and time.
            if (picked.indexOf("hr") > 0) { var fromDate = unit.hours().ago(); };
            if (picked.indexOf("d") > 0) { var fromDate = unit.days().ago(); };
            if (picked.indexOf("m") > 0) { var fromDate = unit.months().ago(); };
            if (picked.indexOf("y") > 0) { var fromDate = unit.years().ago(); };
            var fromHour = fromDate.getHours();

            // current quicker button selected.
            $(".quickpicker").removeClass("quickpickerselected");
            $(this).addClass("quickpickerselected");
            // sync datepicker date/time.
            $("#datepicker").datepicker("setDate", new Date(fromDate.getFullYear(), fromDate.getMonth(), fromDate.getDate()));

            // trigger change date in the chart object.
            chartbuilder.hc_chart.changeDate(fromDate, fromHour);
        }
    });

    // toggle chart grey theme or default theme.
    $("#themedarkgrey").bind({
        click: function () {

            var bw = ($(this).attr('checked')) ? 0 : 1;
            $("#hc_chart").css("borderWidth", bw);

            chartbuilder.hc_chart.theme = $(this).attr('checked') ? grey_theme : {};
            chartbuilder.hc_chart.settheme();
        }
    });

    // toggle chart series line markers.
    $("#seriesmarkers").bind({
        click: function () {

            chartbuilder.hc_chart.markers = $(this).attr('checked') ? false : true;
            chartbuilder.hc_chart.setmarkers();
        }
    });

    // set default series type.
    $("#defaultseriestype").bind({
        change: function () {

            chartbuilder.seriesDefaultType = $(this).val();
        }
    });

});




