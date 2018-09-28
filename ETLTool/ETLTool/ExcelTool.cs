using ETLTool.DataModel;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace ETLTool
{
    public class ExcelTool
    {
        public static void strat(List<WordRoot> wordRootList, ILogger<string> _logger, ISheet sheet, IWorkbook workbook,string newFile)
        {
            foreach (WordRoot wordRoot in wordRootList)
            {


                int relatedWordCount = wordRoot.relatedWord.Count;
                int executeTime = 0;
                foreach (RelatedWord relatedWord in wordRoot.relatedWord)
                {
                    

                    using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                    {
                        IRow row2 = sheet.CreateRow(Staticflag.flagCount + 1);
                        if (executeTime == 0)
                        {
                            ICellStyle cellstyle = workbook.CreateCellStyle();
                            cellstyle.VerticalAlignment = VerticalAlignment.Center;
                            cellstyle.Alignment = HorizontalAlignment.Center;
                             sheet.AddMergedRegion(new CellRangeAddress(Staticflag.flagCount+1, Staticflag.flagCount + relatedWordCount, 0, 0));
                            var cell = row2.CreateCell(0);
                            cell.CellStyle = cellstyle;
                            cell.SetCellValue(wordRoot.wordRoot + wordRoot.wordRootMeaning);


                        }

                        

                        row2.CreateCell(1).SetCellValue(relatedWord.relatedWord);
                        var relatedWordCell = row2.CreateCell(2);
                        relatedWordCell.SetCellValue(relatedWord.releateWordMeaning);
                        workbook.Write(fs);
                        _logger.LogError("write to excel " + wordRoot.wordRoot);
                    }

                    Staticflag.flagCount++;

                    executeTime++;

                }

            }
        }
    }
}