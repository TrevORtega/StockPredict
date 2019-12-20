using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockPredict.Models;

namespace StockPredict.Controllers
{
    public class HomeController : Controller
    {
        /* Get Protocall - load historical data, set all predictions to null */
        public ActionResult Index()
        {
            initHistoricalData();
            ViewBag.predictedPoints = JsonConvert.SerializeObject(null);
            initPredictionPoint();
            return View(new FormModel());
        }

        /* Post Protocall - load historical data, all prediction data up to the 
         * given date are loaded, including the predicted point */
        [HttpPost]
        public ActionResult Index(string formName)
        {
            FormModel point;
            if (formName == null || formName == "")
                point = new FormModel();
            else
                point = new FormModel(formName);

            initPredictionPoint(point);
            initPredictedData(formName);
            initHistoricalData();
            
            return View(point);
        }

        /* Initializes the process to convert historical data into JSON data for ViewBag(not accessible in a model class) */
        public void initHistoricalData()
        {
            DJIA djia = new DJIA();
            List<DataPoint> dataPoints = djia.initializeDJIA();
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
        }

        /* Initializes the process to predict data for ViewBag(not accessible in a model class).
         * Date must come in format yyyy-mm-dd*/
        public void initPredictedData(string date)
        {
            string[] dates = date.Split('-');
            int year = Convert.ToInt32(dates[0]);
            int month = Convert.ToInt32(dates[1]);
            Price price = new Price();
            List<DataPoint> dataPoints = price.initializePredictions(year, month);
            ViewBag.predictedPoints = JsonConvert.SerializeObject(dataPoints);
        }

        /* Initializes a new predicted DataPoint */
        public void initPredictionPoint(FormModel model)
        {
            bool marketIsIncreasing = model.MarketState;
            double price = model.Price;
            DataPoint point = new DataPoint(model.MonthsSince1915, price, model.Date);
            ViewBag.Prediction = JsonConvert.SerializeObject(new List<DataPoint> { point });
        }
        public void initPredictionPoint()
        {
            ViewBag.Prediction = JsonConvert.SerializeObject(null);
        }
    }
}