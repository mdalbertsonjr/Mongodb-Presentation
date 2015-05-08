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

            return Json(features.ToJson(),JsonRequestBehavior.AllowGet);
        }

        //http://localhost:57212/home/near/default?point=-130.83901183425886&point=40.355523246501896&maxDistance=500&minDistance=250
        [HttpGet]
        public async Task<JsonResult> Near(string id, [System.Web.Http.FromUri] double[] point, double maxDistance, double minDistance)//Make a model for this argument
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

            var query = new BsonDocument("geo", nearQuery);

            var features = await collection.Find(query).ToListAsync();
            features = features.Take(3).ToList();

            return Json(features.ToJson(), JsonRequestBehavior.AllowGet);
        }

        //http://docs.mongodb.org/manual/reference/operator/query/geoWithin/#op._S_geoWithin
        [HttpGet]
        public async Task<JsonResult> Within(double[] point, string collectionName)//Make a model for this argument
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(collectionName);
            //set up the query
            //query the database
            //reformat the response
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> Intersects(double[] point, string collectionName)//Make a model for this argument
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(collectionName);
            //set up the query
            //query the database
            //reformat the response
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //Intersects with line?
        //Intersects with polygon?
    }
}
