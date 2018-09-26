//using ETLTool.DataModel;
//using Microsoft.Extensions.Logging;
//using NPOI.SS.UserModel;
//using NPOI.SS.Util;
//using NPOI.XSSF.UserModel;
//using System.Collections.Generic;
//using System.IO;

//namespace ETLTool
//{
//    public class ExcelTool
//    {
//        public static void strat(List<WordRoot> wordRoot, ILogger<string> _logger)
//        {
//            var newFile = @"C:\temp\gDict.xlsx";
//            IWorkbook workbook = new XSSFWorkbook();
//            ISheet sheet1 = workbook.CreateSheet("Sheet1");
//            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 0));

//            int countLine = wordRoot.Count;
//            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
//            {
//                IRow row = sheet1.CreateRow(0);
//                row.CreateCell(0).SetCellValue("wordRoot");
//                row.CreateCell(1).SetCellValue("wordRootMeaning");
//                row.CreateCell(2).SetCellValue("relatedWord");
//                row.CreateCell(3).SetCellValue("releateWordMeaning");
//                sheet1.AutoSizeColumn(0);
//                workbook.Write(fs);
//            }

//            for (int i = 0; i < wordRoot.Count; i++)
//            {
//                using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
//                {
//                    var ui = wordRoot[i];
//                    IRow row2 = sheet1.CreateRow(i + 1);
//                    row2.CreateCell(0).SetCellValue(ui.wordRoot);
//                    row2.CreateCell(1).SetCellValue(ui.wordRootMeaning);
//                    row2.CreateCell(2).SetCellValue(ui.relatedWord);
//                    row2.CreateCell(2).SetCellValue(ui.releateWordMeaning);
//                    sheet1.AutoSizeColumn(0);
//                    workbook.Write(fs);
//                    _logger.LogError("write to excel " + ui.wordRoot + "rest of count:" + countLine--);
//                }
//            }
//        }
//    }
//}