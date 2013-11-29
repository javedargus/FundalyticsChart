<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="builder.aspx.cs" Inherits="FundalyticsChart.pages.builder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Fundalytics: Chart Builder</title>

    <!-- jquery & jquery plug-ins -->
    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui.custom.js"></script>
    <script type="text/javascript" src="/scripts/jquery.isloading.min.js"></script>
    <script type="text/javascript" src="/scripts/date.js"></script>
    <script type="text/javascript" src="/scripts/jquery.dynatree.js"></script>
    <!-- json2 -->
    <script type="text/javascript" src="/scripts/json2.js"></script>
    <!-- linq -->
    <script type="text/javascript" src="/scripts/linq.js"></script>
    <!-- highcharts -->
    <script type="text/javascript" src="/scripts/highcharts/highcharts.js"></script>
    <script type="text/javascript" src="/scripts/highcharts/highcharts.theme.grey.js"></script>
    <script type="text/javascript" src="/scripts/highcharts/export-csv.js"></script>
    <!-- fundalytics: dynatree: default chart tree and series tree options -->
    <script type="text/javascript" src="/scripts/fundalytics/dynatree.chart.options.js"></script>
    <script type="text/javascript" src="/scripts/fundalytics/dynatree.series.options.js"></script>
    <!-- fundalytics: dynatree: loading functions -->
    <script type="text/javascript" src="/scripts/fundalytics/dynatree.js"></script>
    <!-- fundalytics: highcharts: default chart options -->
    <script type="text/javascript" src="/scripts/fundalytics/hc_chart.options.full.js"></script>
    <!-- fundalytics: highcharts: highcharts wrapper -->
    <script type="text/javascript" src="/scripts/fundalytics/hc_chart.js"></script>
    <!-- fundalytics: highcharts: chartbuilder wrapper -->
    <script type="text/javascript" src="/scripts/fundalytics/chart.builder.js"></script>

    <!-- stylesheets -->
    <link rel="stylesheet" type="text/css" href="/styles/jquery/jquery-ui.css">
    <link rel="stylesheet" type="text/css" href="/styles/dynatree/skin-vista/ui.dynatree.css">
    <link rel="stylesheet" type="text/css" href="/styles/fundalytics/ui.dynatree.css">
    <link rel="stylesheet" type="text/css" href="/styles/fundalytics/chart.builder.css">
    <link rel="stylesheet" type="text/css" href="/styles/fundalytics/ui.isloading.css">
    <link href="http://netdna.bootstrapcdn.com/font-awesome/3.0.2/css/font-awesome.css" rel="stylesheet">

    <script type="text/javascript">
        
        // ajax start stop (loading series) dialog handling.
        var initprofiles = true; var initseries = false; 
        $(document).ajaxStart(function () {

            $("body").append("<div id='loadingseries' style='background-color: #005DAB; opacity: .2; position: absolute; top: 0; left: 0; height: 100%; width: 100%; z-index: 999;'></div>");

            var msg = ((initseries) || (initprofiles)) ? "Initialiasing Chart Builder. Please wait ..." : "Retrieving series data";
            $.isLoading({ text: msg });
        });
        $(document).ajaxStop(function () {

            if (initseries) { initseries = false; };
            if (initprofiles) { initprofiles = false; initseries = true; };

            $.isLoading("hide");
            $("#loadingseries").remove();
            $(".series_row").removeClass("series_row_loading");
        });

        // defaults for jquery isloading dialog. 
        defaults = {
            'position': "right",
            'text': "",
            'class': "icon-refresh",
            'tpl': '&lt;span class="isloading-wrapper %wrapper%"&gt;%text%&lt;i class="%class% icon-spin"&gt;&lt;/i&gt;&lt;/span&gt;',
            'disableSource': true,
            'disableOthers': []
        };

        


        function addSeriesNodes(toNode) {

            var tags = new Array();

            var tagsJSON = replaceQuotes(toNode.data.key);
            tags = JSON.parse(tagsJSON);

            var series =
                Enumerable.from(seriesmeta)
                    .where(function (meta)  { return meta.series.tagtype == tags[0].type })
                    .where(function (meta) { return meta.series.tagvalue == tags[0].value })
                    .orderBy(function (meta) { return meta.series.name })
                .toArray();

            Enumerable.from(series).forEach( function(item) {

                toNode.addChild({ title: item.series.name, isFolder: false, key: item.series.id });
            });

            
        };

        function replaceQuotes(str) {

            var quot = '&quot;';

            return str.replace(new RegExp(quot, 'g'), '"');
        };

    </script>

</head>

<body>
    <div class="main">

        <!-- page LHS -->    
        <div class="lhs">
            <!-- series tree -->
            <div class="panel">
                <div class="message">
                    <span style="display: block; color: #DDD; font-size: 12px; letter-spacing: 1px;">CHART SERIES SELECTOR</span>
                    Either double click the required timeseries leaf, 
                    or drag and drop into the builder panel to add 
                    a series to the active chart. 
                </div>
                <div class="seriestree">
                    <!-- series: dynatree container -->
                    <div id="seriestree"></div>
                </div>
            </div>
            <!-- chart tree -->
            <div class="panel" style="margin-top: 10px; border-color: #50A6C2">
                <div class="message" style="background-color: #50A6C2;">
                    <span style="display: block; color: #DDD; font-size: 12px; letter-spacing: 1px;">CHART SELECTOR</span>
                    To load a chart profile double click the required profile node.
                </div>
                <div class="charttree">
                    <!-- chart: dynatree container -->
                    <div id="charttree"></div>
                </div>
            </div>
        </div>

        <!-- page RHS -->
        <div class="rhs" id="hc_container">
            <!-- control panel header -->
            <div style="float: left; clear: both; width: 801px; padding: 7px 10px 5px 10px; margin-top: 00px; background-color: #DDDDDD; font-size: 11px; font-weight: bold; letter-spacing: 1px; color: #005DAB; border: dotted 1px #005DAB; border-radius: 12px;">
                FUNDALYTICS CHART BUILDER: CONTROL PANEL
            </div>
            <!-- chart control panel -->
            <div class="chartcontrols">
                <!--#include virtual="chartcontrols.htm" --> 
            </div>
            <!-- chart header -->
            <div style="float: left; clear: both; width: 801px; padding: 7px 10px 5px 10px; margin-top: 10px; background-color: #50A6C2; font-size: 11px; font-weight: bold; letter-spacing: 1px; color: #FFFFFF; border: dotted 1px #005DAB; border-radius: 12px;">
                CURRENT CHART DISPLAY
            </div>
            <!-- chart (highcharts) -->
            <div id="hc_chart"></div>  
        </div>

    </div>
</body>

</html>
