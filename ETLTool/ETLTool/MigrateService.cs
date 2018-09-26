using ETLTool.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace ETLTool
{
    public class MigrateService
    {
        private readonly ILogger<string> _logger;
        private string _relatedWordsUrl;
        private string _wordRootsUrl;

        public MigrateService(

            ILogger<string> logger,
            IOptions<ConnectionStrings> connectionStrings,

            IOptions<Endpoint> endpoint
            )
        {
            _logger = logger;

            _relatedWordsUrl = endpoint.Value.relatedWordsUrl;
            _wordRootsUrl = endpoint.Value.wordRootsUrl;
        }

        public void Migrate()
        {
            try
            {
                for (int i = 0; i < +8; i++)
                {
                    string wordRootContent = HttpTool.GetHttpResponse(_wordRootsUrl + i, 50000);
                    if (wordRootContent.Equals(null) || wordRootContent.Equals(""))
                    {
                        break;
                    }
                    var wordrootList = HtmlParser.ParseHtmlData(wordRootContent);

                    foreach (string wordroot in wordrootList)
                    {
                        WordRoot wordRootModel = new WordRoot();
                        string word = wordroot.TrimEnd('-').TrimStart('-');

                        var relatedWordResult = HttpTool.GetHttpResponse(_relatedWordsUrl + "re", 50000);

                        HtmlParser.ParseHtmlDataContent(relatedWordResult, out string outwordrootMeaning, out List<RelatedWord> outRelatedWordList);

                        wordRootModel.wordRoot = word;

                        wordRootModel.wordRootMeaning = outwordrootMeaning;

                        wordRootModel.relatedWord = outRelatedWordList;

                        _logger.LogInformation(word);
                    }

                    List<WordRoot> WordRootList = new List<WordRoot>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
            //WordRootList.Add(new WordRoot { relatedWord = "1", releateWordMeaning = "eree", wordRoot = "llll", wordRootMeaning = "3333" });
            //ExcelTool.strat(WordRootList, _logger);
        }
    }
}