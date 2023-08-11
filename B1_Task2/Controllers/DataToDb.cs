using B1_Task2.Models;
using B1_Task2.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.HSSF.UserModel;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using static Azure.Core.HttpHeader;

namespace B1_Task2.Controllers
{
    public class DataToDb
    {
        private string? _filePath;
        private string? _target;
        private List<string> tableNames;

        public DataToDb(string? filePath, string? target)
        {
            _filePath = filePath;
            _target = target;
        }

        public void DeleteAllTables(List<string> names)
        {

                foreach (var name in names)
                {
                    var context = new AppDbContext(name);
                    var tableNames = context.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
                    context.Database.ExecuteSqlRaw($"DROP TABLE {name}");
                    
                }
            MessageBox.Show("Old Data Deleted");

        }

        public void AddData()
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
                {
                    var workbook = new HSSFWorkbook(fs);
                    var worksheet = workbook.GetSheetAt(0);
                    Dictionary<int, int> numberOfTablse = GetNumberOfTables(_filePath, _target);
                tableNames = GetTableNames();
                DeleteAllTables(tableNames);
                int nameCount = 0;

                    foreach (var segment in numberOfTablse)
                    {
                        string name = tableNames[nameCount++];
                        using (var context = new AppDbContext(name))
                        {

                            var entityType = typeof(Balances);
                            var newEntity = Activator.CreateInstance(entityType) as Balances;

                            var modelBuilder = new ModelBuilder();
                            modelBuilder.Entity<Balances>().ToTable(name);

                            context.Database.ExecuteSqlRaw($"CREATE TABLE {name} (" +
                            $"Bch nvarchar(max), " +
                            $"IncomeAssets float, " +
                            $"IncomeLiabilities float, " +
                            $"TurnoverDebet float, " +
                            $"TurnoverCredit float, " +
                            $"OutcomeAssets float, " +
                            $"OutcomeLiabilities float)");


                            for (int row = segment.Key; row < segment.Value; row++)
                            {
                                var currentRow = worksheet.GetRow(row);
                                newEntity.Bch = currentRow.GetCell(0).ToString();
                                newEntity.IncomeAssets = currentRow.GetCell(1).NumericCellValue;
                                newEntity.IncomeLiabilities = currentRow.GetCell(2).NumericCellValue;
                                newEntity.TurnoverDebet = currentRow.GetCell(3).NumericCellValue;
                                newEntity.TurnoverCredit = currentRow.GetCell(4).NumericCellValue;
                                newEntity.OutcomeAssets = newEntity.IncomeAssets + newEntity.TurnoverDebet - newEntity.TurnoverCredit;
                                newEntity.OutcomeLiabilities = newEntity.IncomeLiabilities + newEntity.TurnoverCredit - newEntity.TurnoverDebet;
                                if (newEntity.IncomeAssets == 0)
                                {
                                    newEntity.OutcomeAssets = 0;
                                }

                                if (newEntity.IncomeLiabilities == 0)
                                {
                                    newEntity.OutcomeLiabilities = 0;
                                }

                                string insertSql = $"INSERT INTO {name}" +
                        $" VALUES (N'{newEntity.Bch}',{newEntity.IncomeAssets.ToString().Replace(",", ".")},{newEntity.IncomeLiabilities.ToString().Replace(",", ".")},{newEntity.TurnoverDebet.ToString().Replace(",", ".")},{newEntity.TurnoverCredit.ToString().Replace(",", ".")}, {newEntity.OutcomeAssets.ToString().Replace(",", ".")}, {newEntity.OutcomeLiabilities.ToString().Replace(",", ".")})";

                                context.Database.ExecuteSqlRaw(insertSql);

                            }

                            context.SaveChanges();

                        }
                    }
                }

                MessageBox.Show("Data loaded");
            

        }

        private Dictionary<int, int> GetNumberOfTables(string path, string target)
        {
            Dictionary<int, int> segments = new Dictionary<int, int>();

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fs); // Для формата XLS
                //XSSFWorkbook workbook = new XSSFWorkbook(fs); // Для формата XLSX

                HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
                //XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);

                int lastRow = sheet.LastRowNum;
                int startOfFirst = 0;

                for (int row = 0; row <= lastRow; row++)
                {
                    HSSFRow xlsRow = (HSSFRow)sheet.GetRow(row); // Для формата XLS
                    //XSSFRow xlsRow = (XSSFRow)sheet.GetRow(row); // Для формата XLSX

                    if (xlsRow != null)
                    {
                        HSSFCell cell = (HSSFCell)xlsRow.GetCell(0); // Для формата XLS
                        //XSSFCell cell = (XSSFCell)xlsRow.GetCell(0); // Для формата XLSX

                        if (cell != null)
                        {
                            string cellValue;

                            if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
                            {
                                cellValue = cell.NumericCellValue.ToString();
                            }
                            else
                            {
                                cellValue = cell.StringCellValue;
                            }

                            if (cellValue != null && cellValue.StartsWith(target))
                            {
                                startOfFirst = row + 2;
                                break;
                            }
                        }
                    }
                }
                //starting from first
                for (int row = startOfFirst; row <= lastRow; row++)
                {
                    HSSFRow xlsRow = (HSSFRow)sheet.GetRow(row); // Для формата XLS
                    //XSSFRow xlsRow = (XSSFRow)sheet.GetRow(row); // Для формата XLSX

                    if (xlsRow != null)
                    {
                        HSSFCell cell = (HSSFCell)xlsRow.GetCell(0); // Для формата XLS
                        //XSSFCell cell = (XSSFCell)xlsRow.GetCell(0); // Для формата XLSX

                        if (cell != null)
                        {
                            string cellValue;

                            if (cell.CellType == NPOI.SS.UserModel.CellType.Numeric)
                            {
                                cellValue = cell.NumericCellValue.ToString();
                            }
                            else
                            {
                                cellValue = cell.StringCellValue;
                            }

                            if (cellValue != null && (cellValue.StartsWith(target) || row == sheet.LastRowNum))
                            {
                                segments.Add(startOfFirst - 1, row);
                                startOfFirst = row + 2;
                            }
                        }
                    }
                }
                return segments;
            }
        }

        public List<string> GetTableNames()
        {

            List<string> names = new List<string>();
            int count = GetNumberOfTables(_filePath, _target).Count;
            for (int i = 1; i <= count; i++)
            {
                names.Add($"Class{i}");
            }

            return names;
           
        }
    }
}
