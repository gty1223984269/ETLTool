using ETLTool.DataModel;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ETLTool
{
    public class HtmlParser
    {
        public static List<string> ParseHtmlData(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var div = doc.DocumentNode.SelectNodes("//h2[@class='clh2']");

            if (div != null)
            {
                return div.Descendants("a")
                               .Select(a => a.InnerText)
                               .ToList();
            }
            return null;
        }

        public static void ParseHtmlDataContent(string html, out string wordrootMeaning, out List<RelatedWord> relatedWordList)
        {
            string _wordrootMeaning = "";
            List<RelatedWord> _relatedWordList = new List<RelatedWord>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var div = doc.DocumentNode.SelectNodes("//div[@class='wdef']").FirstOrDefault();
            var collectionNode = div.ChildNodes["ol"].ChildNodes;
            if (collectionNode != null)
            {

                foreach (var co in collectionNode)
                {

                    if (co.Name!="#text")
                    {
                        _wordrootMeaning = co.Descendants("p").Select(a => a.InnerText).ToList()[0].Replace("表示", "").Replace("&nbsp", "").Replace(";", "");

                        var list = co.Descendants("li").Select(a => a.InnerText.Replace(";", "").Replace("&nbsp", "").Replace("\t", "").Replace("\n", "")).ToList();
                    }
                   

                }
              
                //foreach (string item in list)
                //{
                //    RelatedWord relatedWord = new RelatedWord();
                //    if (string.IsNullOrWhiteSpace(item)||item=="")
                //    { break; }
                //  var arrayStr=SplitChineseEnglish(item);

                //    if (arrayStr.Length > 1)
                //    {
                //        relatedWord.relatedWord = arrayStr[0];
                //        relatedWord.releateWordMeaning = arrayStr[1];

                //    }
                //    else
                //    {
                //        break;
                //    }
                //    _relatedWordList.Add(relatedWord);
                   
                //}
               
            }

            relatedWordList = _relatedWordList;
            wordrootMeaning = _wordrootMeaning;
        }

        public static string[] SplitChineseEnglish(string originString)
        {
        string str = originString;
        string strSplit1 = Regex.Replace(str, "[^\x00-\xff]", "", RegexOptions.IgnoreCase);
        string strSplit2 = Regex.Replace(str, "[a-z]", "", RegexOptions.IgnoreCase);
           return new string[] { strSplit1.Trim(), strSplit2.Trim()};
        }

     
    }
}