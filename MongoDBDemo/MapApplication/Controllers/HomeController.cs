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

            #region jsonExample
            //Json example of the query.
            //{
            //    geometry: {
            //        $near: {
            //            $geometry: {
            //                type: "Point",
            //                coordinates: point,
            //            },
            //            $maxDistance: maxDistance,
            //            $minDistance: minDistance
            //        }
            //    }
            //}
            #endregion

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
        public async Task<JsonResult> WithinBox(string id, [System.Web.Http.FromUri] double[] bottomLeft, [System.Web.Http.FromUri]double[] topLeft, [System.Web.Http.FromUri] double[] topRight, [System.Web.Http.FromUri]double[] bottomRight)
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(id);

            #region jsonExample
            //The query as JSON
            //{
            //    geometry: {
            //        $geoWithin: {
            //            $geometry: {
            //                type: "Polygon",
            //                coordinates: [[
            //                    bottomLeft, topLeft, topRight, bottomRight, bottomLeft
            //                ]]
            //            }
            //        }
            //    }
            //}
            #endregion

            var withinQuery = new BsonDocument();
            withinQuery["$geoWithin"] = new BsonDocument();
            var bsonBottomLeft = new BsonArray(bottomLeft);
            var bsonTopLeft = new BsonArray(topLeft);
            var bsonTopRight = new BsonArray(topRight);
            var bsonBottomRight = new BsonArray(bottomRight);
            var bsonCorners = new BsonArray() { bsonBottomLeft, bsonTopLeft, bsonTopRight, bsonBottomRight, bsonBottomLeft };
            var bsonPolygon = new BsonArray() { bsonCorners };
            withinQuery["$geoWithin"]["$geometry"] = new BsonDocument();
            withinQuery["$geoWithin"]["$geometry"]["type"] = "Polygon";
            withinQuery["$geoWithin"]["$geometry"]["coordinates"] = bsonPolygon;

            var query = new BsonDocument("geometry", withinQuery);

            var features = await collection.Find(query.ToBsonDocument()).ToListAsync();
            features = features.Select(x => { x.Remove("_id"); return x; }).ToList();

            return Json(features.ToJson(), JsonRequestBehavior.AllowGet);
        }
    }
}
