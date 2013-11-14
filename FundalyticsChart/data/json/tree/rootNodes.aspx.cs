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

            _tags = SeriesManager.GetSeriesTags(GetTagTypes());

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

        private IList<string> GetTagTypes() {

            IList<string> _tagTypes = new List<string>();

            _tagTypes.Add("COMMODITY");
            _tagTypes.Add("COUNTRY");
            _tagTypes.Add("DATA_EXPLORER_L1");
            _tagTypes.Add("DATA_EXPLORER_L2");
            _tagTypes.Add("DATA_EXPLORER_L3");
            _tagTypes.Add("DATA_EXPLORER_L4");
            _tagTypes.Add("DATA_SOURCE");
            _tagTypes.Add("DATE_TYPE");
            _tagTypes.Add("DEMAND_GRANULARITY");
            _tagTypes.Add("DEMAND_TYPE");
            _tagTypes.Add("DIRECTION_NAME");
            _tagTypes.Add("DIRECTION_VALUE");
            _tagTypes.Add("LOCATION");
            _tagTypes.Add("PRODUCTION_GRANULARITY");
            _tagTypes.Add("PUBLICATION_LAG");
            _tagTypes.Add("REGION");
            _tagTypes.Add("RELEASE_STATUS");
            _tagTypes.Add("ROUTE_CONNECTED_TSO");
            _tagTypes.Add("ROUTE_CONNECTION");
            _tagTypes.Add("ROUTE_NAME");
            _tagTypes.Add("ROUTE_TYPE");
            _tagTypes.Add("SOURCE_MEASURE_NAME");
            _tagTypes.Add("STORAGE_GRANULARITY");
            _tagTypes.Add("STORAGE_TYPE");
            _tagTypes.Add("SUPPLY_GRANULARITY");
            _tagTypes.Add("SUPPLY_TYPE");
            _tagTypes.Add("TSO");
            _tagTypes.Add("TSO_DIRECTION");

            return _tagTypes;
        }
    }
}