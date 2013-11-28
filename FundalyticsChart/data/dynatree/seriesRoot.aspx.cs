using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.data.dynatree
{
    
    public partial class seriesRoot : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e) {

            IList<string> types = SeriesManager.GetSeriesTagTypes();
            IList<SeriesTag> tags = ServiceWrapper.GetTags();

            string json = string.Empty;
            StringBuilder sb = new StringBuilder();

            string type = string.Empty;
            
            foreach (SeriesTag tag in tags)
            {

                if (types.Contains(tag.Type)) {

                    if (tag.Type != type) {

                        type = tag.Type;
                        if (sb.Length > 0) {

                            json = sb.ToString();
                            json = json.Remove(json.Length - 1); json += "]},";
                            sb = new StringBuilder(); sb.Append(json);
                        };

                        sb.Append("{\"title\": \"" + tag.Type + "\", \"isFolder\": true, \"unselectable\": true, \"expand\": " + (tag.Type == "DATA_SOURCE" ? "true" : "false") + ", \"key\": \"" + tag.Type + "\", \"children\": [");
                    };

                    //sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true, \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "?tagtype=" + tag.Type + "&tagvalue=" + tag.Value + "\"},");
                    //sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true, \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "[{\"type\": \"" + tag.Type + "\", \"value\": \"" + tag.Value + "\"}]\"},");
                    //sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true, \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "[tag: {type: " + tag.Type + ", value: " + tag.Value + "}]\"},");
                    sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true, \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "[{&quot;type&quot;: &quot;" + tag.Type + "&quot;, &quot;value&quot;: &quot;" + tag.Value + "&quot;}]\"},");
                };
            };

            json = sb.ToString(); 
            json = json.Remove(json.Length - 1); json = "[" + json + "]}]";

            Response.Write(json);
        }
    }
}