using Microsoft.EntityFrameworkCore;
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
using System.Data;
using Microsoft.Data.SqlClient;
using B1_Task2.Models;
using NPOI.SS.Formula.Functions;
using System.Windows.Controls.Primitives;
using B1_Task2.Models.Data;

namespace B1_Task2
{
    /// <summary>
    /// Логика взаимодействия для DataTable.xaml
    /// </summary>
    public partial class DataTable : Window
    {

        private AppDbContext context;
        public DataTable()
        {
            InitializeComponent();
        }


        public void LoadData(string name)
        {
            var lblName = new Label();
            lblName.Content = name;
            lblName.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblName.VerticalAlignment = VerticalAlignment.Center;
            lblName.Height = 50;
            lblName.FontSize = 35;
            stackPanel.Children.Add(lblName);
            var lblText = new Label();
            var dataGrid = new DataGrid();
            dataGrid.AutoGenerateColumns = true; 
            dataGrid.VerticalAlignment = VerticalAlignment.Center;
            dataGrid.HorizontalAlignment = HorizontalAlignment.Center;
            context = new AppDbContext(name);
            var query = $"SELECT * FROM {name}";
            var data = context.Balances.FromSqlRaw(query).ToList();
            dataGrid.ItemsSource = data;
            foreach (var column in dataGrid.Columns)
            {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            stackPanel.Children.Add(dataGrid);
        }


    }
}
