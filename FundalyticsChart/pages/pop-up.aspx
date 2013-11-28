<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop-up.aspx.cs" Inherits="FundalyticsChart.pages.pop_up" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Fundalytics Charting - Chart Window</title>

    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/highcharts/highcharts.js"></script>
    <script type="text/javascript" src="/scripts/highcharts/highcharts.theme.grey.js"></script>
    <script type="text/javascript" src="/scripts/date.js"></script>
    <script type="text/javascript" src="/scripts/fundalytics/hc_chart.js"></script>
    <script type="text/javascript" src="/scripts/fundalytics/hc_chart.options.full.js"></script>

    <style type="text/css">
        
        body                { margin: 10px; }
        #hc_chart           { float: left; width: 100%; height: 100%; margin: 0px; border: solid 1px #005DAB; border-radius: 6px; }
        
    </style>

</head>
<body>
    <div id="hc_chart"></div>
    <asp:Literal runat="server" ID="InitChartJS" />
</body>
</html>
