using ETLTool.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

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

          
            var newFile = @"C:\temp\gDict.xlsx";
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 0));

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("wordRoot");
                //row.CreateCell(1).SetCellValue("wordRootMeaning");
                row.CreateCell(2).SetCellValue("relatedWord");
                row.CreateCell(3).SetCellValue("releateWordMeaning");
                sheet.AutoSizeColumn(0);
                workbook.Write(fs);
            }

           

            int k = 0;
            try
            {
                for (int i =1; i <=8; i++)
                {
                    string wordRootContent = HttpTool.GetHttpResponse(_wordRootsUrl + i, 500000);
                    if (wordRootContent.Equals(null) || wordRootContent.Equals(""))
                    {
                        continue;
                    }
                    var wordrootList = HtmlParser.ParseHtmlData(wordRootContent);

                    foreach (string wordroot in wordrootList)
                    {
                        WordRoot wordRootModel = new WordRoot();
                        string word = wordroot.TrimEnd('-').TrimStart('-');

                        var relatedWordResult = HttpTool.GetHttpResponse(_relatedWordsUrl + word, 50000);

                        HtmlParser.ParseHtmlDataContent(relatedWordResult, out List<WordRoot> outRelatedWordList, word);
                        ExcelTool.strat(outRelatedWordList, _logger, sheet, workbook, newFile);
                        _logger.LogError(word + "##" + k++);
                    }

                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
          
           
        }
    }
}