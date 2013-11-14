using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.pages
{
    
    public partial class chart : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e) {

            /*
            string _js = string.Empty;
            int days = 0;
            int seriesId = 0;

            try {
                days = int.Parse(Request["days"]);
            }
            catch { days = -90; };
        
            try {

                foreach (string qs in Request.QueryString.GetValues("id")) {

                    seriesId = int.Parse(qs); 
                    
                    _js += "hc_chart.addseries(" + 
                                seriesId.ToString() + ", " +
                                "\"" + 
                                ServiceWrapper.GetSeriesMeta(seriesId).Name + 
                                "\", " + 
                                GetSeriesJSON(ServiceWrapper.GetSeries(seriesId, DateTime.Now.AddDays(days))) +
                            ");";
                };
            } catch { };

            this.InitChartJS.Text =
                "<script type=\"text/javascript\">" +
                    "$(document).ready(function(){" +
                        "var days = " + days + ";" +
                        "hc_chart = new hc_chart();" +
                        "hc_chart.initialise('Argus Fundalytics Chart');" +
                        "hc_chart.initialisechart();" +
                        _js +
                        "var timer = window.setInterval(function () { hc_chart.refresh() }, 30000);" +
                    "});" +
                "</script>";
            */
        }

        private string GetSeriesJSON(XmlNodeList seriesXml) {

            StringBuilder sb = new StringBuilder();
            
            foreach (XmlNode point in seriesXml)
            {

                DateTime point_datetime = DateTime.Parse(point["AppliesToDateTime"].InnerText);
                string point_value = point["Value"].InnerText;

                sb.Append(
                    "[" +
                        (point_datetime - DateTime.Parse("1 Jan 1970 00:00")).TotalMilliseconds.ToString() +
                        "," +
                        point_value +
                    "],");
            };

            string json = sb.ToString();
            if (json.Length > 0) { json = json.Remove(json.Length - 1); }
            json = "[" + json + "]";

            return json;
        }
    }
}