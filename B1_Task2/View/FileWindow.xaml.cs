using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using B1_Task2.Controllers;

namespace B1_Task2
{
    /// <summary>
    /// Логика взаимодействия для FileWindow.xaml
    /// </summary>
    public partial class FileWindow : Window
    {
        static string filesTableName = "Files";
        public FileWindow()
        {
            InitializeComponent();
            FileData file = new FileData();
            file.ShowFiles(dataGrid, filesTableName);
        }

    }
}
