<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prototype.aspx.cs" Inherits="FundalyticsChart.pages.prototype" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html; charset=ISO-8859-1">
    <title>Fundalytics Chart Prototype - Tree</title>

    <script type="text/javascript" src="/scripts/jquery.1.8.2.js"></script>
    <script type="text/javascript" src="/scripts/jquery-ui.custom.js"></script>
    <script type="text/javascript" src="/scripts/jquery.dynatree.js"></script>
  
    <link rel="stylesheet" type="text/css" href="/styles/dynatree/skin-vista/ui.dynatree.css" >

    <script type="text/javascript">

        $(function () {

            $("#tree").dynatree({
                checkbox: true,
                selectMode: 3,
                //children: rootNodes,
                initAjax: {
                    url: "/data/json/tree/rootNodes.aspx"
                },
                onSelect: function (select, node) {
                    // Get a list of all selected nodes, and convert to a key array:
                    var selKeys = $.map(node.tree.getSelectedNodes(), function (node) {
                        return node.data.key;
                    });
                    $("#echoSelection3").text(selKeys.join(", "));

                    // Get a list of all selected TOP nodes
                    var selRootNodes = node.tree.getSelectedNodes(true);
                    // ... and convert to a key array:
                    var selRootKeys = $.map(selRootNodes, function (node) {
                        return node.data.key;
                    });
                    $("#echoSelectionRootKeys3").text(selRootKeys.join(", "));
                    $("#echoSelectionRoots3").text(selRootNodes.join(", "));
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
                }
            });

        });

    </script>
</head>

<body class="example">
    <div id="tree">  </div>
</body>

</html>