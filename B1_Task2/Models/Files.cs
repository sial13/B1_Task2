using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1_Task2.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

    }
}
