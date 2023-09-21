namespace One.Toolbox.Models.Dashboard
{
    internal class YiyanAPIM
    {
        public int id { get; set; }
        public string uuid { get; set; }

        /// <summary> 正文 utf-8 </summary>
        public string hitokoto { get; set; }

        public string type { get; set; }

        /// <summary> 出处 </summary>
        public string from { get; set; }

        public string from_who { get; set; }
        public string creator { get; set; }
        public int creator_uid { get; set; }
        public int reviewer { get; set; }
        public string commit_from { get; set; }
        public string created_at { get; set; }
        public int length { get; set; }
    }
}