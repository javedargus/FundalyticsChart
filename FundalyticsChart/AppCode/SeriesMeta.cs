using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundalyticsChart.AppCode
{
    public class SeriesMeta {

        private int _id;

        private string _dataSource;
        private string _name;

        private SeriesTagList _tagList;


        public int Id {

            get { return _id; }
        }

        public string DataSource {

            get { return _dataSource; }
        }
        public string Name {

            get { return _name; }
        }

        public SeriesTagList TagList {

            get { return _tagList; }
        }

        public SeriesMeta(int id, string dataSource, string name, SeriesTagList tagList) {

            this._id = id;
            this._dataSource = dataSource;
            this._name = name;
            this._tagList = tagList;
        }
    }
}