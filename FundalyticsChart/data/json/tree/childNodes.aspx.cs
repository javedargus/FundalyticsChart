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
    public partial class childNodes : System.Web.UI.Page
    {

        const int MAX_LEAF_NODES = 50;

        SeriesTagList    _parentNodes;
        SeriesMetaList   _seriesMeta;

        protected void Page_Load(object sender, EventArgs e) {


            if ((!Request.QueryString.AllKeys.Contains("tagtype")) || (!Request.QueryString.AllKeys.Contains("tagvalue"))) { return; };

            _parentNodes = ParentNodes();
            _seriesMeta = SeriesManager.GetSeriesMeta(GetTagTypes(), _parentNodes);

            if (_seriesMeta.Count > MAX_LEAF_NODES) {

                Response.Write(ChildNodesJSON(_seriesMeta, _parentNodes));
            } else {

                Response.Write(LeafNodesJSON(_seriesMeta));
            };
        }

        private IList<string> GetTagTypes()
        {

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

        private SeriesTagList ParentNodes() {

            SeriesTagList parentNodes = new SeriesTagList();
            
            string [] tag_type = Request.QueryString["tagtype"].Split(new char[] { ',' });
            string [] tag_value = Request.QueryString["tagvalue"].Split(new char[] { ',' });

            for (int ix = 0; ix < tag_type.Length; ix++) {

                parentNodes.Add(new SeriesTag(tag_type[ix], tag_value[ix]));
            };

            return parentNodes;
        }

        private string ChildNodesJSON(SeriesMetaList meta, SeriesTagList parentNodes)
        {

            string json = string.Empty;
            StringBuilder sb = new StringBuilder();

            string type = string.Empty;
            
            foreach (SeriesTag tag in _seriesMeta.DistinctTagList()) {

                if (!parentNodes.ContainsTagType(tag)) {
                
                    if (tag.Type != type) {

                        type = tag.Type;
                        if (sb.Length > 0) {

                            json = sb.ToString();
                            json = json.Remove(json.Length - 1); json += "]},";
                            sb = new StringBuilder(); sb.Append(json);
                        };

                        sb.Append("{\"title\": \"" + tag.Type + "\", \"isFolder\": true, \"unselectable\": true, \"key\": \"" + tag.Type + "\", \"children\": [");
                    };

                    sb.Append("{\"title\": \"" + tag.Value + "\",  \"isFolder\": true,  \"unselectable\": true, \"isLazy\": true, \"key\": \"" + "?");
                    foreach (SeriesTag node in parentNodes) {

                        sb.Append("&tagtype=" + node.Type + "&tagvalue=" + node.Value);
                    };
                    sb.Append("&tagtype=" + tag.Type + "&tagvalue=" + tag.Value + "\"},");
                };
            };

            json = sb.ToString();
            
            json = json.Remove(json.Length - 1);
            json = "{\"title\": \"" + meta.Count.ToString() + " matching series\"}," + json;
            json = "[" + json + "]}]";

            return json;
        }

        private string LeafNodesJSON(SeriesMetaList meta)
        {

            string json = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (SeriesMeta item in meta)
            {

                sb.Append("{\"title\": \"" + item.Name + "\", \"key\": \"" + item.Id + "\"},");
            };

            json = sb.ToString();
            
            json = json.Remove(json.Length - 1);
            json = "[" + json + "]";

            return json;
        }
    }
}