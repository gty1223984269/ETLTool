﻿using ETLTool.DataModel;
using Microsoft.Extensions.DependencyInjection;
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

        public void Migrate(Repository repository)
        {

            string sql1 =
@"INSERT INTO[dbo].[related_words]
           ([created_date_time_utc]
           ,[created_by]
           ,[updated_date_time_utc]
           ,[updated_by]
           ,[is_active]
           ,[root_id]
           ,[word]
           ,[chinese_meaning]
           ,[remember_logic])
     VALUES
           (<created_date_time_utc, datetime2(7),>
           ,<created_by, nvarchar(64),>
           ,<updated_date_time_utc, datetime2(7),>
           ,<updated_by, nvarchar(64),>
           ,<is_active, bit,>
           ,<root_id, bigint,>
           ,<word, nvarchar(100),>
           ,<chinese_meaning, nvarchar(300),>
           ,<remember_logic, nvarchar(400),>)'";

           


            var newFile = @"C:\temp\gDict.xlsx";
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {
                ICellStyle cellstyle = workbook.CreateCellStyle();
                cellstyle.VerticalAlignment = VerticalAlignment.Center;
                cellstyle.Alignment = HorizontalAlignment.Center;
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.SetColumnWidth(2, 200);
                IRow row = sheet.CreateRow(0);
                var wordCell = row.CreateCell(0);
                wordCell.SetCellValue("wordRoot");
                wordCell.CellStyle = cellstyle;

                var relatedWordCell = row.CreateCell(1);
                relatedWordCell.SetCellValue("relatedWord");
                relatedWordCell.CellStyle = cellstyle;
                var relatedWordMeaningCell = row.CreateCell(2);
                relatedWordMeaningCell.SetCellValue("releateWordMeaning");
                relatedWordMeaningCell.CellStyle = cellstyle;
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

                       
                        repository.Add(outRelatedWordList);
                        //ExcelTool.strat(outRelatedWordList, _logger, sheet, workbook, newFile);
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