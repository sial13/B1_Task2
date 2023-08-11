using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1_Task2.Models
{
    public class Balances
    {
        public string? Bch { get; set; }
        public double IncomeAssets { get; set; }
        public double IncomeLiabilities { get; set; }
        public double TurnoverDebet { get; set; }
        public double TurnoverCredit { get; set; }

        public double OutcomeAssets { get; set; }

        public double OutcomeLiabilities { get; set; }
        /*public double OutcomeAssets
        {
            get
            {
                if (IncomeAssets == 0)
                {
                    return 0;
                }
                else
                {
                    return IncomeAssets + TurnoverDebet - TurnoverCredit;
                }
            }
        }

        public double OutcomeLiabilities
        {
            get
            {
                if (IncomeLiabilities == 0)
                {
                    return 0;
                }
                else
                {
                    return IncomeLiabilities + TurnoverCredit - TurnoverDebet;
                }
            }
        }*/
    }
}
