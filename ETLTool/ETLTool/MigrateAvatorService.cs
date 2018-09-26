using ETLTool.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ETLTool
{
    public class MigrateService
    {
        private readonly ILogger<string> _logger;
        private string _testModel;

        public MigrateService(

            ILogger<string> logger,
            IOptions<ConnectionStrings> connectionStrings,

            IOptions<TestModel> testModel
            )
        {
            _logger = logger;

            _testModel = testModel.Value.ToString();
        }

        public void Migrate()
        {
            List<WordRoot> WordRootList = new List<WordRoot>();
            WordRootList.Add(new WordRoot { relatedWord = "1", releateWordMeaning = "eree", wordRoot = "llll", wordRootMeaning = "3333" });
            ExcelTool.strat(WordRootList, _logger);
        }
    }
}