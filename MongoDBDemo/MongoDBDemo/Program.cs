using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDBDemo
{
    class Program
    {
        private static bool GEN_POINT_DATA = false;
        private static bool GEN_LINE_DATA = false;
        private static bool SHOW_HELP = false;

        private static string COLLECTION_NAME = "default";
        private static string DATABASE = "mongodb://localhost/geo";
        private static string CARDINALITY = "100";
        private static string CENTER_POINT = "-117.6,34.07";

        private static double LON = -117.6;
        private static double LAT = 34.07;

        private static Random rand = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {
            OptionSet opts = new OptionSet()
            {
                {"points", (x) => {GEN_POINT_DATA = x != null; }},
                {"lines", (x) => {GEN_LINE_DATA = x != null; }},
                {"collection-name=", (x) => {COLLECTION_NAME = x ?? COLLECTION_NAME; }},
                {"db=", (x) => {DATABASE = x ?? DATABASE; }},
                {"cardinality=", (x) => {CARDINALITY = x ?? CARDINALITY; }},
                {"center=", (x) => {CENTER_POINT = x ?? CENTER_POINT; }},
                {"help", (x) => {SHOW_HELP = x != null; }},
            };

            List<string> extra;
            try
            {
                extra = opts.Parse(args);
            }
            catch(OptionException e)
            {
                ShowHelp(opts);
                return;
            }

            if(SHOW_HELP)
            {
                ShowHelp(opts);
                return;
            }

            if(!(GEN_LINE_DATA || GEN_POINT_DATA))
            {
                Console.WriteLine("Please select a data type.");
                return;
            }

            int cardinality = 0;
            if(!int.TryParse(CARDINALITY, out cardinality))
            {
                Console.WriteLine("Please enter a valid integer.");
                return;
            }

            try
            {
                ParseBBox();
            }
            catch(Exception)
            {
                Console.WriteLine("Failed to parse the bounding box.");
                return;
            }

            MongoClient client;
            MongoUrl url;

            try
            {
                url = new MongoUrl(DATABASE);
                client = new MongoClient(new MongoUrl(DATABASE));
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception encountered while connecting to the database.");
                Console.WriteLine(e.Message);
                return;
            }

            IMongoDatabase db = client.GetDatabase(url.DatabaseName);

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>(COLLECTION_NAME);

            List<BsonDocument> documents = new List<BsonDocument>();
            for(int i = 0; i < cardinality; i++)
            {
                BsonDocument doc = GenerateDocument();
                collection.InsertOneAsync(doc);
            }

            collection.Indexes.CreateOneAsync((new { geo = "2dsphere" }).ToBsonDocument());
        }

        private static BsonDocument GenerateDocument()
        {
            BsonDocument result = new BsonDocument();

            if(GEN_POINT_DATA)
            {
                result = GeneratePointDocument();
            }
            if(GEN_LINE_DATA)
            {
                result = GenerateLineDocument();
            }

            return result;
        }

        private static BsonDocument GenerateLineDocument()
        {
            double[][] line = GenerateRandomLine();
            BsonDocument attributes = GenerateRandomAttributes();

            return (new {
                geo = new {
                    type = "Line",
                    coordinates = line,
                },
                attributes = attributes,
            }).ToBsonDocument();
        }

        private static double[][] GenerateRandomLine()
        {
            return new double[][] { GenerateRandomPoint(), GenerateRandomPoint() };
        }

        private static BsonDocument GeneratePointDocument()
        {
            double[] point = GenerateRandomPoint();
            BsonDocument attributes = GenerateRandomAttributes();

            return (new {
                geo = new { 
                    type = "Point",
                    coordinates =  point,
                },
                attributes = attributes,
            }).ToBsonDocument();
        }

        private static BsonDocument GenerateRandomAttributes()
        {
            string[] colors = new string[] { "blue", "green", "orange", "red" };
            int maxPointValue = 10;

            return (new {
                color = colors[rand.Next(colors.Length)],
                value = rand.Next(maxPointValue) + 1,
            }).ToBsonDocument();
        }

        private static double[] GenerateRandomPoint()
        {
            double longitude = ((rand.NextDouble() >= 0.5) ? -1 : 1.0) * rand.NextDouble() + LON;
            double latitude = ((rand.NextDouble() >= 0.5) ? -1 : 1.0) * rand.NextDouble() + LAT;

            return new double[] { longitude, latitude };
        }

        private static void ParseBBox()
        {
            string[] parts = CENTER_POINT.Split(',');

            if (parts.Length != 4 ||
                !double.TryParse(parts[0], out LON) ||
                !double.TryParse(parts[1], out LAT))
                throw new Exception();
        }

        private static void ShowHelp(OptionSet opts)
        {
            Console.WriteLine("Help dialog for demo data generator.");
            Console.WriteLine("Options:");
            opts.WriteOptionDescriptions(Console.Out);
        }
    }
}
