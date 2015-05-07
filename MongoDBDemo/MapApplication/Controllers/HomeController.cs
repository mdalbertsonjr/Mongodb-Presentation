using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapApplication.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private MongoUrl _url;
        private IMongoClient _client;
        private IMongoDatabase _db;

        public HomeController()
        {
            _url = new MongoUrl("mongodb://localhost/geo"); 
            _client = new MongoClient(_url);
            _db = _client.GetDatabase(_url.DatabaseName);
        }

        public ActionResult Index()
        {
            return View();
        }
        

        [HttpGet]
        public JsonResult Layer(string id)
        {
            //Get the collection named by the id
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(id);
            //Get all of the features in the collection
            var features = collection.Find<BsonDocument>((x) => true);
            //Reformat as necessary for returning the collection as json. GeoJSON?
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Near(double[] point, string collectionName)//Make a model for this argument
        {
            //set up the query
            //query the database
            //reformat the response
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Within(double[] point, string collectionName)//Make a model for this argument
        {
            //set up the query
            //query the database
            //reformat the response
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Intersects(double[] point, string collectionName)//Make a model for this argument
        {
            //set up the query
            //query the database
            //reformat the response
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //Intersects with line?
        //Intersects with polygon?
    }
}
