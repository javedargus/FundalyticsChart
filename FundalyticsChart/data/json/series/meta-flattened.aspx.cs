using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.data.json.series
{
    
    public partial class meta_flattened : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e) {

            SeriesMetaList meta = SeriesManager.GetSeriesMeta(SeriesManager.GetSeriesTagTypes());

            StringBuilder sb = new StringBuilder();
            foreach (SeriesMeta item in meta) {

                foreach (SeriesTag tag in item.TagList) {

                    sb.Append(
                        "{" +
                            //"\"series\": {" +
                                "\"id\": \"" + item.Id + "\"," +
                                "\"datasource\": \"" + item.DataSource + "\"," +
                                "\"name\": \"" + item.Name + "\"," +
                                "\"tagtype\": \"" + tag.Type + "\"," +
                                "\"tagvalue\": \"" + tag.Value + "\"" +
                            //"}" +
                        "},"
                    );
                };
            };

            string json = sb.ToString();

            json = json.Remove(json.Length - 1);
            json = "[" + json + "]";

            Response.Write(json);
        }
    }
}