using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockPredict.Models
{
    public class Price
    {
        /* Predict the price of every month from 1915 to the current year */
        public List<DataPoint> initializePredictions(int year, int month)
        {
            List<DataPoint> list = new List<DataPoint>();
            int maxMonths = month + (year - 1915) * 12;

            for (int i = 1; i < maxMonths; i++)
            {
                double price = predictPrice(i);
                string date = createDate(i);
                list.Add(new DataPoint(i, price, date));
            }
            return list;
        }

       

        /* Ensures a given year is valid, then predicts the price for that given year */
        public double getPrice(int year, int month)
        {
            if (year <= 0)
                return 0;
            int months = ((year - 1915) * 12) + month;
            DJIA djia = new DJIA();

            // Year included in post 1985 data
            if ((year >= 1985 && year <= 2018) && month <= 10)
            {
                return djia.findPriceInDJIA(year, month, "DJIA185.txt");
            }
            // Year included in post 1915 data
            else if (year >= 1915 && year < 1969){
                return djia.findPriceInDJIA(year, month, "DJIA1915.txt");
            }
            else
            {
                return predictPrice(months);
            }
          
        }

        /* Implements the price prediction formula with the given months since 1915 */
        private double predictPrice(int months)
        { 
            return Math.Round(12.315 * (1.54 * months + 20.73 * Math.Sin((2 * 3.14159 / 51) * months)) + 1315.88, 2);
        }

        /* Creates a string date in the format mm/dd/yyyy. Must be given the amount of months since 1915 */
        public string createDate(int months)
        {
            int year = (months / 12) + 1915;
            int month = months < 12 ? months : ((months % 12) + 1);
            return Convert.ToString(month) + "/1/" + Convert.ToString(year);
        }

        /* Returns true if market is increasing(based on cyclical market patterns), else returns false.
         * The average interval cycle is 51 months: 36 month increase followed by 15 month decrease */
        public bool marketIsIncreasing(int monthsSince)
        {
            if (monthsSince < 0)
                return false;
            int intervalLength = 51;
            int intervalIncreaseLen = 36;

            int monthsSinceInterval = monthsSince % intervalLength;

            // This calculates the EXACT VALUE of the derivative of the  price prediction function.
            // double result = 34.04 * System.Math.Cos(.1232 * monthsSince) + 20.53;

            return monthsSinceInterval <= intervalIncreaseLen;
              
        }


    }
}