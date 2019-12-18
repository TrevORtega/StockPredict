using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockPredict.Models;

namespace StockPredict.Models
{
    /* An object that contains predicted market information
     * made from a mm/dd/yyyy date string */
    public class FormModel
    {
        public string Date { get; set; }
        public int MonthsSince1915 { get; set; }
        public double Price { get; set; }
        // True = increasing, False = decreasing
        public bool MarketState { get; set; }

        /* Creates a new FormModel with a null string */
        public FormModel()
        {
            this.Date = null;
        }

        /* Sets the values for all attributes in the class */
        public FormModel(string date)
        {
            this.Date = date;
            setPrice();
            // sets the Market State as well as the months since 1915
            setMarketState();
        }

        /* Takes in a string date in the form yyyy/1/dd and sets predicted price
         * of DJIA*/
        public void setPrice()
        {
            Price Price = new Price();
            string[] dates = this.Date.Split('-');
            this.Price = Price.getPrice(Convert.ToInt32(dates[0]), Convert.ToInt32(dates[1]));

        }

        /* Returns whether or not the market is increasing, given a # of months since 1915 */
        public void setMarketState()
        {
            string[] dates = this.Date.Split('-');
            int month = Convert.ToInt32(dates[1]);
            int year = Convert.ToInt32(dates[0]);
            this.MonthsSince1915 = month + (year - 1915) * 12;
            Price p = new Price();
            this.MarketState = p.marketIsIncreasing(this.MonthsSince1915);
        }
    }
}