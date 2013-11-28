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
    
    public partial class tags : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e) {

            IList<string> types = SeriesManager.GetSeriesTagTypes();
            IList<SeriesTag> tags = ServiceWrapper.GetTags();

            StringBuilder sb = new StringBuilder();
            foreach(SeriesTag tag in tags) {

                if (types.Contains(tag.Type)) {
                    
                    sb.Append("[" + tag.Type + "," + tag.Value + "],");
                };
            };

            string json = sb.ToString();

            if (json.Length > 0) { json = json.Remove(json.Length - 1); }
            json = "[" + json + "]";

            Response.Write(json);
        }
    }
}