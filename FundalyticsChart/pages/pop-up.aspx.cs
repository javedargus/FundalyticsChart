using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FundalyticsChart.pages
{
    public partial class pop_up : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            bool greyTheme = Boolean.Parse(Request.QueryString["greytheme"]);
            bool markers = Boolean.Parse(Request.QueryString["markers"]);

            DateTime fromDate = DateTime.Parse(Request.QueryString["fromDate"]);
            int fromHour = Int32.Parse(Request.QueryString["fromHour"]);
            
            fromDate = fromDate.AddHours(fromHour);

            string series = Request.QueryString["series"];

            this.InitChartJS.Text =
                "<script type=\"text/javascript\">" +
                    "$(function () {" +
                        "hc_chart = new hc_chart();" +
                        "hc_chart.initialise(chart_options, " + 
                        ((greyTheme) ? "grey_theme" : "[]") + ", " +
                        ((markers) ? "true" : "false") + ", " + 
                        "\"" + fromDate + "\");" +
                        "hc_chart.load(" + series + ");" +
                    "});" +
                "</script>";
        }
    }
}