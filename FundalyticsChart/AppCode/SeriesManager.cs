using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FundalyticsChart.AppCode
{
    
    public class SeriesManager {

        public static IList<string> GetSeriesTagTypes() {

            IList<string> _tagTypes = new List<string>();

            _tagTypes.Add("COMMODITY");
            _tagTypes.Add("COUNTRY");
            _tagTypes.Add("DATA_SOURCE");
            _tagTypes.Add("DEMAND_TYPE");
            _tagTypes.Add("LOCATION");
            _tagTypes.Add("REGION");
            _tagTypes.Add("ROUTE_CONNECTED_TSO");
            _tagTypes.Add("ROUTE_CONNECTION");
            _tagTypes.Add("ROUTE_NAME");
            _tagTypes.Add("STORAGE_TYPE");
            _tagTypes.Add("SUPPLY_TYPE");
            _tagTypes.Add("TSO");

            return _tagTypes;
        }
        
        public static SeriesTagList GetSeriesTags(IList<string> tagTypes) {

            SeriesTagList _tags = new SeriesTagList();

            var db = new Database();
            string sql =
                "SELECT " +
                    "DISTINCT TAG_TYPE, TAG_VALUE " +
                    "FROM " +
                        "CORE.DATUM_TYPE_TAG " +
                    "WHERE " +
                        "TAG_TYPE IN " + GetTagTypeInClauseValues(tagTypes) + " " +
                    "ORDER BY " +
                        "TAG_TYPE, TAG_VALUE ";

            DataTable table = db.GetDataTable(sql);
            foreach (DataRow record in table.Rows) {

                _tags.Add(new SeriesTag(record["TAG_TYPE"].ToString(), record["TAG_VALUE"].ToString()));
            };

            return _tags;
        }

        public static SeriesMetaList GetSeriesMeta(IList<string> tagTypes) {

            SeriesMetaList _meta = new SeriesMetaList();

            var db = new Database();
            string sql =
                "SELECT " +
                    "DISTINCT CORE_DATUM_TYPE_ID, DATA_SOURCE, NAME, TAG_TYPE, TAG_VALUE, TAG_DESCRIPTION " +
                    "FROM " +
                        "CORE.DATUM_TYPE_TAG " +
                    "WHERE " +
                        "TAG_TYPE IN " + GetTagTypeInClauseValues(tagTypes) + " " +
                    "ORDER BY " +
                        "CORE_DATUM_TYPE_ID, TAG_TYPE, TAG_VALUE ";

            DataTable table = db.GetDataTable(sql);

            int id = 0;
            string dataSource = string.Empty;
            string name = string.Empty;
            SeriesTagList metaTags = new SeriesTagList();

            foreach (DataRow record in table.Rows) {

                if (Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]) != id) {

                    if (id != 0) { _meta.Add(new SeriesMeta(id, dataSource, name, metaTags)); };

                    id = Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]);
                    dataSource = record["DATA_SOURCE"].ToString();
                    name = record["NAME"].ToString();

                    metaTags = new SeriesTagList();
                }

                metaTags.Add(new SeriesTag(record["TAG_TYPE"].ToString(), record["TAG_VALUE"].ToString()));
            };

            return _meta;
        }
        
        public static SeriesMetaList GetSeriesMeta(IList<string> tagTypes, SeriesTag tag) {

            SeriesMetaList _meta = new SeriesMetaList();

            var db = new Database();
            string sql =
                "SELECT " +
                    "DISTINCT CORE_DATUM_TYPE_ID, DATA_SOURCE, NAME, TAG_TYPE, TAG_VALUE, TAG_DESCRIPTION " +
                    "FROM " +
                        "CORE.DATUM_TYPE_TAG " +
                    "WHERE " +
                        "TAG_TYPE IN " + GetTagTypeInClauseValues(tagTypes) + " " +
                    "ORDER BY " + 
                        "CORE_DATUM_TYPE_ID, TAG_TYPE, TAG_VALUE ";

            DataTable table = db.GetDataTable(sql);

            int id = 0;
            string dataSource = string.Empty;
            string name = string.Empty;
            SeriesTagList metaTags = new SeriesTagList();

            foreach (DataRow record in table.Rows)
            {

                if (Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]) != id) {

                    if ((id != 0) && (metaTags.ContainsTag(tag)))
                    {
                        _meta.Add(new SeriesMeta(id, dataSource, name, metaTags));
                    };

                    id = Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]);
                    dataSource = record["DATA_SOURCE"].ToString();
                    name = record["NAME"].ToString();

                    metaTags = new SeriesTagList();
                }

                metaTags.Add(new SeriesTag(record["TAG_TYPE"].ToString(), record["TAG_VALUE"].ToString()));
            };

            return _meta;
        }

        public static SeriesMetaList GetSeriesMeta(IList<string> tagTypes, SeriesTagList tags) {

            SeriesMetaList _meta = new SeriesMetaList();

            var db = new Database();
            string sql =
                "SELECT " +
                    "DISTINCT CORE_DATUM_TYPE_ID, DATA_SOURCE, NAME, TAG_TYPE, TAG_VALUE, TAG_DESCRIPTION " +
                    "FROM " +
                        "CORE.DATUM_TYPE_TAG " +
                    "WHERE " +
                        "TAG_TYPE IN " + GetTagTypeInClauseValues(tagTypes) + " " +
                    "ORDER BY " +
                        "CORE_DATUM_TYPE_ID, TAG_TYPE, TAG_VALUE ";

            DataTable table = db.GetDataTable(sql);

            int id = 0;
            string dataSource = string.Empty;
            string name = string.Empty;
            SeriesTagList metaTags = new SeriesTagList();

            foreach (DataRow record in table.Rows)
            {

                if (Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]) != id)
                {

                    if (id != 0) {

                        bool matched = true;
                        foreach(SeriesTag tag in tags) {

                            if (!metaTags.ContainsTag(tag)) {
                                matched = false;
                                continue;
                            };
                        };
                        
                        if (matched) _meta.Add(new SeriesMeta(id, dataSource, name, metaTags));
                    };

                    id = Convert.ToInt32(record["CORE_DATUM_TYPE_ID"]);
                    dataSource = record["DATA_SOURCE"].ToString();
                    name = record["NAME"].ToString();

                    metaTags = new SeriesTagList();
                }

                metaTags.Add(new SeriesTag(record["TAG_TYPE"].ToString(), record["TAG_VALUE"].ToString()));
            };

            return _meta;
        }

        private static string GetTagTypeInClauseValues(IList<string> tagTypes) {

            string sql = string.Empty;
            foreach (string tag in tagTypes)
            {

                sql += "'" + tag + "',";
            };
            sql = sql.Remove(sql.Length - 1);
            sql = "(" + sql + ")";

            return sql;
        }
    }
}