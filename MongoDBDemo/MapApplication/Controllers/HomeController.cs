using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MapApplication.Controllers
{
    public class HomeController : AsyncController
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
        public async Task<JsonResult> Layer(string id)
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(id);

            var features = await collection.Find<BsonDocument>(new BsonDocument()).ToListAsync();
            features = features.Select(x => { x.Remove("_id"); return x; }).ToList();

            return Json(features.ToJson(),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> Near(string id, [System.Web.Http.FromUri]double[] point,  double maxDistance, double minDistance)
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(id);

            var nearQuery = new BsonDocument();
            nearQuery["$near"] = new BsonDocument();
            nearQuery["$near"]["$geometry"] = (new
            {
                type = "Point",
                coordinates = point,
            }).ToBsonDocument();
            nearQuery["$near"]["$maxDistance"] = maxDistance;
            nearQuery["$near"]["$minDistance"] = minDistance;

            var query = new BsonDocument("geometry", nearQuery);

            var features = await collection.Find(query).ToListAsync();
            features = features.Select(x => { x.Remove("_id"); return x; }).ToList();

            return Json(features.ToJson(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> WithinBox(string id, [System.Web.Http.FromUri] double[] leftCorner, [System.Web.Http.FromUri] double[] rightCorner)
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(id);

            var withinQuery = new BsonDocument();
            withinQuery["$geoWithin"] = new BsonDocument();
            var bsonLeftCorner = new BsonArray(leftCorner);
            var bsonRightCorner = new BsonArray(rightCorner);
            var bsonCorners = new BsonArray() { bsonLeftCorner, bsonRightCorner };
            withinQuery["$geoWithin"]["$box"] = bsonCorners;

            var query = new BsonDocument("geometry", withinQuery);

            var temp = query.ToJson();
            var features = await collection.Find(query.ToBsonDocument()).ToListAsync();
            features = features.Select(x => { x.Remove("_id"); return x; }).ToList();

            return Json(features.ToJson(), JsonRequestBehavior.AllowGet);
        }
    }
}
