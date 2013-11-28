<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test-json.aspx.cs" Inherits="FundalyticsChart.pages._tmp.test_json" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/json2.js"></script>
    <script type="text/javascript" src="/scripts/linq.js"></script>

    <script type="text/javascript">

        var seriesMeta = new Array();
        var seriesTags = new Array();

        $(function () {

            alert('here');

            $.ajax({

                dataType: "json",
                url: "/data/json/test-tags.json",
                async: false,
                success: function (data, textStatus, jqXHR) {
                    alert(data);
                    seriesTags = data;
                    alert(seriesTags.length);
                }
            });

            /*
            $.ajax({

            dataType: "json",
            url: "/data/json/series/meta-flattened.aspx",
            async: false,
            success: function (data, textStatus, jqXHR) {
            seriesMeta = data;
            }
            });*/

        });

        
        
        
        function getGASSCO() {

            var ix = 0;

            var result =
                Enumerable.from(seriesMeta)
                    .where(function (meta) { return meta.series.tagtype == "DATA_SOURCE" })
                    .where(function (meta) { return meta.series.tagvalue == "GASSCO" })
                    .orderBy(function (meta) { return meta.series.name })
                .toArray();

            alert(result.length);
            
            for (ix = 0; ix < 10; ix++) {

                alert(result[ix].series.name);
                alert(result[ix].series.datasource);
            };
            
            /*
            Enumerable.from(seriesMeta).forEach(function (meta) {

                
                
                for (ix = 0; ix < 10; ix++) {

                    alert(meta.series.id);
                };
            });*/



        };

    
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 400px; height: 400px; padding: 20px; margin-top: 20px; background-color: #EEE;">
        <input type="button" id="getGASSCO series" value="Get GASSCO nodes" onclick="javascript:getGASSCO()" />
    </div>
    </form>
</body>
</html>
