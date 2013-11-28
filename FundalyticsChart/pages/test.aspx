<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="FundalyticsChart.pages.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <style type="text/css">
        .isloading {
            font-family: Arial, Helvetica;
            font-size: 11px;
            font-weight: bold;
            letter-spacing: .5px;
        }
        .isloading-wrapper.isloading-right {
            margin-left: 10px;
        }
        .isloading-overlay {
            position: relative;
            text-align: center;
        }
        .isloading-overlay .isloading-wrapper {
            font-family: Arial, Helvetica;
            font-size: 12px;
            font-weight: bold;
            letter-spacing: 1px;
            color: #005DAB;
            background: #FC911A;
            -webkit-border-radius: 6px;
            -webkit-background-clip: padding-box;
            -moz-border-radius: 6px;
            -moz-background-clip: padding;
            border-radius: 6px;
            background-clip: padding-box;
            display: inline-block;
            margin: 0 auto;
            padding: 10px 20px;
            top: 10%;
            z-index: 9000;
        }
    </style>
    
    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/jquery.isloading.min.js"></script>
    <script type="text/javascript">

        var bsComparator = function (a, b) {
            if (a.x < b.x) { return -1; }
            if (a.x > b.x) { return 1; }
            return 0;
        };
        var binarySearch = function (series_data, point) {
            var low = 0, high = series_data.length - 1,
        i, comparison;
            while (low <= high) {
                i = Math.floor((low + high) / 2);
                comparison = bsComparator(series_data[i], point);
                if (comparison < 0) { low = i + 1; continue; }
                if (comparison > 0) { high = i - 1; continue; }
                return i;
            }
            return null;
        };

        //alert(seriesTo.toString());

        //hc_chart.reloadallseries(seriesFrom, seriesFromHour);
        //alert(seriesFrom.getTime());
        //alert(hc_chart.chart.xAxis[0].max);
        //var minIndex = binarySearch(hc_chart.chart.series[0].data, seriesFrom.getTime());
        //var maxIndex = binarySearch(hc_chart.chart.series[0].data, hc_chart.chart.xAxis[0].max);
        //alert(minIndex);

        //var fromDate = new Date.UTC(seriesFrom.getFullYear(), seriesFrom.getMonth(), seriesFrom.getDate());
        //alert(fromDate);


        //hc_chart.chart.xAxis[0].zoom(100, 200);
        
        $(document).ajaxStart(function () {

            $(".log").text("ajax start event triggered");
            $.isLoading({ text: "Loading data ..." });
        });
        $(document).ajaxStop(function () {

            $(".log").text("ajax stop event triggered");
            $.isLoading("hide");
        });

        function ajaxcommand() {

            var seriesFrom = new Date();
            
            alert("executing ...");

            $.ajax({



                dataType: "json",
                url: "/data/json/series/data.aspx?seriesid=31103&seriesfrom=" + seriesFrom.toString("d-MMM-yyyy") + "&seriesfromhour=0",
                success: function (data) {
                    alert(data);
                    //$(".result").text(data);
                },
                async: false
            });
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="trigger" onclick="javascript:ajaxcommand();">Trigger</div>
        <div class="result"></div>
        <div class="log"></div>

        <div style="float: left; clear: both; margin-top: 40px;">

            <pre>
                defaults = {
                    'position': "right",        // right | inside | overlay
                    'text': "",                 // Text to display next to the loader
                    'class': "icon-refresh",    // loader CSS class
                    'tpl': '&lt;span class="isloading-wrapper %wrapper%"&gt;%text%&lt;i class="%class% icon-spin"&gt;&lt;/i&gt;&lt;/span&gt;',
                    'disableSource': true,      // true | false
                    'disableOthers': []
                };
            </pre>

            <!-- OVERLAY -->
            <div id="load-overlay">
                
                <a name="overlay"></a>

                <h3>Overlay</h3>

                <div class="demo well">
                    <p class="alert">container</p>
                    <span class="btn">Test</span>
                </div>

                <pre class="example syntax javascript"></pre>

                <script>
                    $(function () {

                        // Action on Click
                        $("#load-overlay .btn").click(function () {

                            $.isLoading({ text: "Loading series data ..." });

                            // Setup Loading plugin
                            $("#load-overlay .demo p").removeClass("alert-success");

                            // Re-enabling event
                            setTimeout(function () {
                                $.isLoading("hide");
                                $("#load-overlay .demo p").html("Content Loaded")
                                                        .addClass("alert-success");
                            }, 2000);

                        });

                    });
                </script>

            </div>

        </div>

    </div>
    </form>
</body>
</html>
