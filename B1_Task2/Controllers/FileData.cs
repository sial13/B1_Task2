using B1_Task2.Models;
using B1_Task2.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NPOI.HSSF.Record.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace B1_Task2.Controllers
{
    public class FileData
    {
        private FileDbContext context;
        public string? ChooseFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string path = openFileDialog.FileName;

                using (var context = new FileDbContext())
                {
                    var fileRecord = new Files
                    {
                        FileName = System.IO.Path.GetFileName(path),
                        FilePath = path
                    };

                    context.Files.Add(fileRecord);
                    context.SaveChanges();
                }

                return path;
            }

            return null;
        }

        public void ShowFiles(DataGrid grid, string name)
        {
            context = new FileDbContext();
            var query = $"SELECT * FROM {name}";
            var data = context.Files.FromSqlRaw(query).ToList();
            grid.ItemsSource = data;
        }

        public string GetLastColumnValue()
        {
            using (var context = new FileDbContext())
            {
                var lastRow = context.Files.OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastRow != null)
                {
                    return lastRow.FilePath; 
                }
                return null;
            }
        }
    }
}
