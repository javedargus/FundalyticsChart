using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.data.json.tree
{
    public partial class rootNodes : System.Web.UI.Page
    {

        SeriesTagList _tags;

        protected void Page_Load(object sender, EventArgs e) {

            _tags = SeriesManager.GetSeriesTags(SeriesManager.GetSeriesTagTypes());

            string json = string.Empty;
            StringBuilder sb = new StringBuilder();
            
            string type = string.Empty;
            
            foreach (SeriesTag tag in _tags) {

                if (tag.Type != type) {

                    type = tag.Type;
                    if (sb.Length > 0) {

                        json = sb.ToString();
                        json = json.Remove(json.Length - 1); json += "]},";
                        sb = new StringBuilder(); sb.Append(json);
                    };
                    
                    sb.Append("{\"title\": \"" + tag.Type + "\", \"isFolder\": true, \"unselectable\": true, \"expand\": " + (tag.Type == "DATA_SOURCE" ? "true": "false") + ", \"key\": \"" + tag.Type + "\", \"children\": [");
                };

                sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true, \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "?tagtype=" + tag.Type + "&tagvalue=" + tag.Value + "\"},");
            };

            json = sb.ToString();
            json = json.Remove(json.Length - 1);
            json = "[" + json + "]}]";

            Response.Write(json);
        }
    }
}