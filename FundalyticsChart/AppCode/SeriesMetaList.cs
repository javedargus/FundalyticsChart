using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundalyticsChart.AppCode
{
    
    public class SeriesMetaList : List<SeriesMeta> {
    
        
        public SeriesMetaList() {

        }

        public IList<SeriesTag> DistinctTagList() {

            SeriesTagList distinctTags = new SeriesTagList();
            
            foreach(SeriesMeta metaItem in this) {

                foreach(SeriesTag tagItem in metaItem.TagList) {

                    if (!distinctTags.ContainsTag(tagItem)) {

                        distinctTags.Add(tagItem);
                    };
                };
            };

            List<SeriesTag> sortedTags = distinctTags.OrderBy(x => x.Value).OrderBy(x => x.Type).ToList();

            return sortedTags;
        }
    }
}