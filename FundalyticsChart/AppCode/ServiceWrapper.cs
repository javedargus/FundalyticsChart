using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using System.Web;
using FundalyticsChart.AppCode;

namespace FundalyticsChart.AppCode
{
    
    public class ServiceWrapper {

        public static SeriesMeta GetSeriesMeta(int id) {

            const string URLBASE = "https://fundalytics.argusmedia.com/datatypes/";
            
            SeriesMeta meta;
            SeriesTagList metaTagList = new SeriesTagList();
            
            string data = string.Empty;

            HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URLBASE + id.ToString());
            req.Credentials = new NetworkCredential("montgomery", "westbere47");
            req.Method = "GET";

            System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {

                System.IO.Stream responseStream = resp.GetResponseStream();
                System.IO.StreamReader myStreamReader = new System.IO.StreamReader(responseStream);

                data = myStreamReader.ReadToEnd();
            }
            resp.Close();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            foreach (XmlNode xmlTag in xml.SelectNodes("/ArgusFundamentals/DataType/DataTypeTags/DataTypeTag")) {

                metaTagList.Add(new SeriesTag(xmlTag.ChildNodes[0].InnerText, xmlTag.ChildNodes[1].InnerText));
            };

            meta = 
                new SeriesMeta(
                    Convert.ToInt32(xml.SelectSingleNode("/ArgusFundamentals/DataType/Id").InnerText),
                    xml.SelectSingleNode("/ArgusFundamentals/DataType/Source").InnerText,        
                    xml.SelectSingleNode("/ArgusFundamentals/DataType/Name").InnerText,
                    metaTagList 
                );

            return meta;
        }

        public static XmlNodeList GetSeries(int id, DateTime fromDate) {

            const string URLBASE = "https://fundalytics.argusmedia.com/datatypes/";
            
            string data = string.Empty;

            HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URLBASE + id.ToString() + "/data/" + fromDate.ToString("yyyy-MM-dd"));
            req.Credentials = new NetworkCredential("montgomery", "westbere47");
            req.Method = "GET";

            System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {

                System.IO.Stream responseStream = resp.GetResponseStream();
                System.IO.StreamReader myStreamReader = new System.IO.StreamReader(responseStream);

                data = myStreamReader.ReadToEnd();
            }
            resp.Close();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            return xml.SelectNodes("/ArgusFundamentals/Data/DataPoint");
        }
    }
}