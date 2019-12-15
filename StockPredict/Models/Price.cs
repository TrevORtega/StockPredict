using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockPredict.Models
{
    public class Price
    {
        int year = 2022;
        int month = 5;
        private double getPrice(int year)
        {
            // Year not included in current data
            if (year <= System.DateTime.Now.Year && month <= System.DateTime.Now.Month)
            {
                int months = ((year - 1915) * 12) + month;
                return predictPrice(months);
            }
            else
            {
                throw new ArgumentException("Date must be in the future");
            }
          
        }
        private double predictPrice(int months)
        {   
            int curPrice = 0;
        
            double fin = Math.Round(12.315 * (1.54 * months + 20.73 * Math.Sin((2 * 3.14159 / 51) * months)) + 1315.88, 2);
            return fin;
        }
    }
}