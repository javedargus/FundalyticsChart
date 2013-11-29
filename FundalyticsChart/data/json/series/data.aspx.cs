using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.data.json.series
{
    public partial class data : System.Web.UI.Page
    {

        private int _seriesId;
        private DateTime _seriesFrom; private int _seriesFromHour;

        protected void Page_Load(object sender, EventArgs e) {

            try {
                _seriesId = int.Parse(Request["seriesid"]);
            } catch {
                Response.Write("[]"); return; };

            try {
                _seriesFrom = DateTime.Parse(Request["seriesfrom"]);
            } catch {
                Response.Write("[]"); return; };
            
            try {
                _seriesFromHour = int.Parse(Request["seriesfromhour"]);
            } catch {
                _seriesFromHour = 0; };

            if (_seriesFromHour != 0) {
                _seriesFrom = _seriesFrom.AddHours(_seriesFromHour);
            } else {
                _seriesFrom = _seriesFrom.AddHours(_seriesFromHour);
            };

            XmlNodeList series;
            StringBuilder sb = new StringBuilder();

            if (_seriesFromHour != 0)
            {
                series = ServiceWrapper.GetSeries(_seriesId, _seriesFrom, _seriesFromHour);
            } else {

                series = ServiceWrapper.GetSeries(_seriesId, _seriesFrom);
            };
            
            foreach (XmlNode point in series) {

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

            Response.Write(json);
        }
    }
}