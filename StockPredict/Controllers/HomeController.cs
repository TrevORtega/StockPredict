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
        public ActionResult Index()
        {
            List<DataPoint> dataPoints = new List<DataPoint>
            {
                new DataPoint(10, 22),
                new DataPoint(20, 36),
                new DataPoint(30, 42),
                new DataPoint(40, 51),
                new DataPoint(50, 46),
            };
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}