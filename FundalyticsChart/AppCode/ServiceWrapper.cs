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

        public static IList<SeriesTag> GetTags() {

            SeriesTagList tags = new SeriesTagList();

            string data = GetRequest("https://fundalytics.argusmedia.com/tags/");

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            foreach (XmlNode xmlTag in xml.SelectNodes("/ArgusFundamentals/Tags/Tag")) {

                tags.Add(new SeriesTag(xmlTag.ChildNodes[1].InnerText, xmlTag.ChildNodes[2].InnerText));
            };

            List<SeriesTag> sortedTags = tags.OrderBy(x => x.Value).OrderBy(x => x.Type).ToList();
            return sortedTags;
        }

        public static SeriesMetaList GetSeriesMetaList() {

            SeriesMetaList list = new SeriesMetaList();

            string data = GetRequest("https://fundalytics.argusmedia.com/datatypes/2013-01-01/00.00");

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            foreach (XmlNode xmlMeta in xml.SelectNodes("/ArgusFundamentals/DataTypes/DataType")) {

                data = GetRequest("https://fundalytics.argusmedia.com/datatypes/" + xmlMeta.ChildNodes[0].InnerText);

                XmlDocument tagsXML = new XmlDocument();
                tagsXML.LoadXml(data);

                SeriesTagList tags = new SeriesTagList();
                foreach (XmlNode xmlTag in tagsXML.SelectNodes("/ArgusFundamentals/DataType/DataTypeTags/DataTypeTag")) {

                    tags.Add(new SeriesTag(xmlTag.ChildNodes[0].InnerText, xmlTag.ChildNodes[1].InnerText));
                };

                list.Add(new SeriesMeta(Convert.ToInt32(xmlMeta.ChildNodes[0].InnerText), xmlMeta.ChildNodes[1].InnerText, xmlMeta.ChildNodes[2].InnerText, tags));
            };
            
            return list;
        }
        
        public static SeriesMeta GetSeriesMeta(int id) {

            SeriesMeta meta;
            SeriesTagList metaTagList = new SeriesTagList();

            string data = GetRequest("https://fundalytics.argusmedia.com/datatypes/" + id.ToString());

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

            string data = GetRequest("https://fundalytics.argusmedia.com/datatypes/" 
                            + id.ToString() + "/data/" + fromDate.ToString("yyyy-MM-dd"));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            return xml.SelectNodes("/ArgusFundamentals/Data/DataPoint");
        }

        public static XmlNodeList GetSeries(int id, DateTime fromDate, int fromHour) {

            string hours = fromHour.ToString();
            hours = (hours.Length == 1) ? "0" + hours : hours;

            string data = GetRequest("https://fundalytics.argusmedia.com/datatypes/" 
                            + id.ToString() + "/data/" + fromDate.ToString("yyyy-MM-dd") + "/" + hours + ".00");

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            return xml.SelectNodes("/ArgusFundamentals/Data/DataPoint");
        }

        protected static string GetRequest(string path) {

            string data = string.Empty;

            HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(path);
            req.Credentials = new NetworkCredential("montgomery", "westbere47");
            req.Method = "GET";

            System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {

                System.IO.Stream responseStream = resp.GetResponseStream();
                System.IO.StreamReader myStreamReader = new System.IO.StreamReader(responseStream);

                data = myStreamReader.ReadToEnd();
            }
            else {

                Console.Write("");
            };
            resp.Close();

            return data;
        }
    }
}