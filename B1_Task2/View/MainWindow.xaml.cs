using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using B1_Task2.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.Storage;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System.Globalization;
using NPOI.OpenXmlFormats.Shared;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.Util;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Data;
using Microsoft.Win32;
using B1_Task2.Models.Data;
using B1_Task2.Controllers;
using FileData = B1_Task2.Controllers.FileData;

namespace B1_Task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext context;
        string target = "КЛАСС";
        string? path;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            FileData file = new FileData();
            path = file.ChooseFilePath();
            
            DataToDb readData = new DataToDb(path, target);
            readData.AddData();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            FileData file = new FileData();
            path = file.GetLastColumnValue();
            DataToDb readData = new DataToDb(path, target);
            List<string> names = readData.GetTableNames();
            DataTable table = new DataTable();
            foreach (var name in names)
            {
                table.LoadData(name);
            }
            table.Show();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            FileWindow fileShow = new FileWindow();
            fileShow.Show();
        }
    }
}
