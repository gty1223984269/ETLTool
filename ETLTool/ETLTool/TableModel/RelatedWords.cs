using System;

namespace ETLTool.TableModel
{
    public class RelatedWords
    {
        public int id { set; get; }
        public DateTime created_date_time_utc { set; get; }
        public string created_by { set; get; }
        public DateTime updated_date_time_utc { get; set; }
        public string updated_by { set; get; }
        public int is_active { set; get; }
        public int root_id { set; get; }
        public string word { set; get;}
        public string chinese_meaning { set; get; }
        public string remember_logic { set; get; }
    }

}