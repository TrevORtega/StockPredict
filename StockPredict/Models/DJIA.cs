using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using StockPredict.Models;
using System.IO;

namespace StockPredict.Models
{
    public class DJIA
    {
        /* Initializes the process that creates a list of DataPoints  */
        public List<DataPoint> initializeDJIA()
        {
            List <DataPoint> dataPoints = new List<DataPoint>();
            dataPoints = initializeFromFiles(dataPoints);
            return dataPoints;
        }

        /* Gets a list of DataPoints from 2 data files */
        private List <DataPoint> initializeFromFiles(List<DataPoint> list)
        {
            list = readFrom1915(list);
            list = readFrom1985(list);
            return list;
        }

        /* Extract DataPoint objects from the DJIA data in the 1914 txt file */
        private List<DataPoint> readFrom1915(List<DataPoint> list)
        {
            string line;
            var path = HttpContext.Current.Server.MapPath(@"\DJIA_Data\DJIA1915.txt");
            //string[] paths = Directory.GetFiles(@"D:/home/site/wwwroot");
            System.IO.StreamReader file;
            //try
            //{
                file = new System.IO.StreamReader(path);
            //}
            //catch(DirectoryNotFoundException e)
            //{
            //    return list;
            //}

            // Convert data into a DataPoint object and append it to the list
            while ((line = file.ReadLine()) != null)
            {
                string[] data = line.Split('\t');
                string date = data[0];
                double months = getMonthsSince1915(date);
                double price = Convert.ToDouble(data[2]);
                list.Add(new DataPoint(months, price, date));

            }
            return list;
        }

        /* Extract DataPoint objects from the DJIA data in the 1985 txt file */
        private List<DataPoint> readFrom1985(List<DataPoint> list)
        {
            string line;

            var path = HttpContext.Current.Server.MapPath(@"\DJIA_Data\DJIA1985.txt");
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader(path);

            }
            catch(DirectoryNotFoundException e)
            {
                return list;
            }
            // Convert data into a DataPoint object and append it to the list
            while ((line = file.ReadLine()) != null)
            {
                string[] data = line.Split('\t');
                string date = data[0];
                double months = getMonthsSince1915(date);
                double price = Convert.ToDouble(data[1]);
                list.Add(new DataPoint(months, price, date));

            }
            return list;
        }

        /* Takes in a date in mm/dd/yyyy format and returns the amount of months since 1915 */
        private double getMonthsSince1915(string date)
        {
            string[] dates = date.Split('/');
            double year = Convert.ToDouble(dates[2]);
            double month = Convert.ToDouble(dates[0]);

            return month + (year - 1915) * 12; 
        }

        /* Finds the price for a given year and month. Year must be between 1915 and 1968, 
         * month must be 0-12, file name must be in format [name].[extension] */
        public double findPriceInDJIA(int year, int month, string filename)
        {
            int linesToSkip;
            if (string.Compare(filename, "DJIA1915.txt") == 0)
            {
                linesToSkip = (12 * (year - 1915) + month) - 1;

            }
            else if (string.Compare(filename, "DJIA1985.txt") == 0)
            {
                linesToSkip = (12 * (year - 1985) + month) - 1;

            }
            else
                return 0;

            string path = HttpContext.Current.Server.MapPath(@"\DJIA_Data\" + filename);
            string line = File.ReadLines(path).Skip(linesToSkip).Take(1).FirstOrDefault();

            string[] data = line.Split('\t');
            // last element in data is the price
            return Convert.ToDouble(data[data.Length - 1]);

        }
    }
}