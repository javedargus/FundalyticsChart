<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chart.aspx.cs" Inherits="FundalyticsChart.pages.chart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <meta http-equiv="content-type" content="text/html; charset=ISO-8859-1">
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1990 12:00:00 GMT" />

    <title>Fundalytics Chart Prototype - Tree</title>

    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui.custom.js"></script>
    <script type="text/javascript" src="/scripts/jquery.dynatree.js"></script>
    <script type="text/javascript" src="/scripts/highcharts.js"></script>
    <script type="text/javascript" src="/scripts/date.js"></script>
    <script type="text/javascript" src="/scripts/fundalytics/hc_chart.js"></script>

    <link rel="stylesheet" type="text/css" href="/styles/jquery/jquery-ui.css" >
    <link rel="stylesheet" type="text/css" href="/styles/dynatree/skin-vista/ui.dynatree.css" >
    <link rel="stylesheet" type="text/css" href="/styles/fundalytics/style.css" >

    <style type="text/css">
        ul.dynatree-container       { border: none; font-family: Arial, Helvetica; font-size: 11px; }
        span.dynatree-selected a    { color: #005DAB; font-weight: bold; font-style: italic; }
    </style>

    <script type="text/javascript">
        $(function () {

            // chart (high charts wrapper) object initialisation. 
            hc_chart = new hc_chart();
            hc_chart.initialise("Fundalytics Chart Builder");
            hc_chart.initialisechart();

            // series from date / time globals.
            var seriesFrom = (90).days().ago();
            var seriesFromHour = "0";

            // datepicker init and datepicker change date handler.
            $("#datepicker").datepicker({
                onSelect: function (date) {
                    seriesFrom = date;
                    seriesFromHour = 0;
                    hc_chart.reloadallseries(seriesFrom, seriesFromHour);
                },
                changeMonth: true,
                changeYear: true,
                showButtonPanel: false
            });
            $("#datepicker").datepicker("setDate", new Date(seriesFrom.getFullYear(), seriesFrom.getMonth(), seriesFrom.getDate()));

            // timepicker click event handler.
            $(".timepicker").bind({
                click: function () {

                    $(".timepicker").removeClass("timepickerselected");
                    $(this).addClass("timepickerselected");

                    var picked = $(this).val();
                    var unit = parseInt(picked.replace(/\D/g, ''));

                    if (picked.indexOf("hr") > 0) { seriesFrom = unit.hours().ago(); };
                    if (picked.indexOf("d") > 0) { seriesFrom = unit.days().ago(); };
                    if (picked.indexOf("m") > 0) { seriesFrom = unit.months().ago(); };
                    if (picked.indexOf("y") > 0) { seriesFrom = unit.years().ago(); };

                    seriesFromHour = seriesFrom.getHours();

                    $("#datepicker").datepicker("setDate", new Date(seriesFrom.getFullYear(), seriesFrom.getMonth(), seriesFrom.getDate()));
                }
            });

            // series tree (dynatree).
            $("#tree").dynatree({
                checkbox: false,
                selectMode: 2,
                initAjax: {
                    url: "/data/json/tree/rootNodes.aspx"
                },
                onSelect: function (select, node) {
                    if (node.data.isFolder) { return false; };
                    if (select) {
                        hc_chart.loadseries(node, seriesFrom, seriesFromHour);
                    } else {
                        hc_chart.removeseries(node.data.key, false);
                    };
                },
                onDblClick: function (node, event) {
                    node.toggleSelect();
                },
                onKeydown: function (node, event) {
                    if (event.which == 32) {
                        node.toggleSelect();
                        return false;
                    }
                },
                onLazyRead: function (node) {
                    node.appendAjax({
                        url: "/data/json/tree/childNodes.aspx" + node.data.key,
                        // We don't want the next line in production code:
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
                        //alert(node.data.title);
                    }
                }
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
        });
    </script>
        
</head>

<body class="example">

    <div style="width: 1360px;">
        <div style="width: 1358px; height: 58px; border: solid 1px #CCC; background-color: #005DAB; display: none;">
        </div>
        <div style="float: left; clear: both; width: 1360px; margin-top: 15px;">
            <div style="float: left; width: 478px; height: 859px; padding: 10px; border: solid 1px #FC911A; overflow: scroll;">
                <div style="float: left; clear: both; width: 438px; height: 30px; padding: 10px; background-color: #FC911A; color: #FFF; font-size: 11px; font-weight: bold; line-height: 16px;">
                    Either double click the required timeseries leaf, or drag and drop into the RHS top panel to add a series to the current chart. 
                </div>
                <div id="tree" style="float: left; width: 458px; margin-top: 10px;"></div>
            </div>
            <div id="hc_container">
                <div id="hc_chart_panel">
                    <div style="float: left; clear: both; width: 100%; height: 190px; color: #FFF;">
                        
                        <!-- timeseries period selection -->
                        <div style="float: left; clear: both; width: 100%;">
                            <div style="float: left; width: 70px;">
                                Quick timeseries historic picker:
                            </div>
                            <div style="float: left; padding: 5px; margin: 10px 0px 0px 15px; background-color: #FFF;">
                                <div style="float: left;">
                                    <input type="button" class="timepicker" id="1hr" value="1hr" />  <br />
                                    <input type="button" class="timepicker" value="3hrs" /> <br />
                                    <input type="button" class="timepicker" value="6hrs" /> <br />
                                    <input type="button" class="timepicker" value="12hrs" />
                                </div>
                                <div style="float: left; margin-left: 6px;">
                                    <input type="button" class="timepicker" value="1d" />   <br />
                                    <input type="button" class="timepicker" value="3d" />   <br />
                                    <input type="button" class="timepicker" value="7d" />  <br />
                                    <input type="button" class="timepicker timepickerselected" value="90d" />
                                </div>
                                <div style="float: left; margin-left: 6px;">
                                    <input type="button" class="timepicker" value="1m" />   <br />
                                    <input type="button" class="timepicker" value="2m" />   <br />
                                    <input type="button" class="timepicker" value="3m" />   <br />
                                    <input type="button" class="timepicker" value="6m" />
                                </div>
                                <div style="float: left; margin-left: 6px;">
                                    <input type="button" class="timepicker" value="1y" />   <br />
                                    <input type="button" class="timepicker" value="2y" />   <br />
                                    <input type="button" class="timepicker" value="3y" />   <br />
                                    <input type="button" class="timepicker" value="5y" />
                                </div>
                            </div>
                            <div style="float: left; width: 70px; margin-left: 25px;">
                                Timeseries from:
                            </div>
                            <div style="float: left; padding: 5px; padding-top: 0px; margin: 10px 0px 0px 15px; background-color: #FFF;">
                                <div id="datepicker" style="font-family: Arial; font-size: 9px;"></div>
                            </div>
                            <div style="float: left; width: 130px; margin-top: 10px; margin-left: 25px;">
                                <div style="float: left; clear: both;">
                                    Default series type:
                                </div>
                                <div style="float: left; clear: both; margin-top: 5px;">
                                    <select style="font-size: 11px; color: #005DAB; font-weight: bold; width: 120px;">
                                        <option value="spline" selected>spline</option>
                                    </select>
                                </div>
                                <div style="float: left; clear: both; margin-top: 10px;">
                                    Line marker:
                                </div>
                                <div style="float: left; clear: both; margin-top: 5px;">
                                    <div style="float: left; margin-top: 1px; font-size: 10px; font-weight: bold; color: #FC911A;">
                                        DISABLED
                                    </div>
                                    <div style="float: left;margin-left: 5px;">
                                        <input type="checkbox" checked />
                                    </div>
                                </div>
                                <div style="float: left; clear: both; width: 120px; height: 30px; margin-top: 0px; padding-top: 10px; border-top: dotted 1px #FFF;">
                                    <!--<input type="button" value="Clear all series" />-->
                                </div>
                            </div>
                        </div>
                    </div>


                    <div style="float: left; clear: both;">
                        <div style="float: left; clear: both; color: #FC911A;">Current series:</div>
                        <div style="float: left; clear: both; padding: 5px; margin-top: 5px; background-color: #FFF; min-height: 100px;">
                            <div id="hc_series"></div>
                        </div>
                    </div>
                    

                    

                </div>
                <div id="hc_chart"></div>    
                
                
            </div>
        </div>
    </div>

     <!-- chart : javascript intialisation -->
    <asp:Literal runat="server" ID="InitChartJS" />

</body>

</html>