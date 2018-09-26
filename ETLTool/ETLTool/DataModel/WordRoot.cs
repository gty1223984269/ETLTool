using System.Collections.Generic;

namespace ETLTool.DataModel
{
    public class WordRoot
    {
        public string wordRoot { set; get; }

        public string wordRootMeaning { set; get; }

        public List<RelatedWord> relatedWord { set; get; }
    }
}