using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundalyticsChart.AppCode
{
    
    public class SeriesTagList : List<SeriesTag> {
    
        public SeriesTagList() {

        }

        public bool ContainsTag(SeriesTag tag) {

            foreach(SeriesTag item in this) {
            
                if ((item.Type == tag.Type) && (item.Value == tag.Value)) {

                    return true;
                };
            };

            return false;
        }

        public bool ContainsTagType(SeriesTag tag) {

            foreach(SeriesTag item in this) {
            
                if (item.Type == tag.Type) {

                    return true;
                };
            };

            return false;
        }
    }
}