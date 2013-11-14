using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundalyticsChart.AppCode
{
    
    public class SeriesTag {

        private string _type;
        private string _value;

        public string Type {

            get { return _type; }
        }
        public string Value {

            get { return _value; }
        }

        public SeriesTag(string type, string value) {

            this._type = type;
            this._value = value;
        }
    }
}