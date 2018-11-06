using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
using System.IO;

using ShirlyStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.ML.Runtime.Api;
using ColumnAttribute = Microsoft.ML.Runtime.Api.ColumnAttribute;

namespace ShirlyStudio.Services
{
    public class WorkshopClusterService
    {
        private readonly ShirlyStudioContext _context;

        public WorkshopClusterService(ShirlyStudioContext context)
        {
            _context = context;
        }

        //write Workshop to file
        public void WriteWorkshopToFile(string cust)
        {
            using (StreamWriter writer = File.AppendText("WorkshopClusters.csv"))
            {
                writer.WriteLine(cust);
            }
        }


        //Convert Workshop to Vector
        public string WorkshopToVector(Workshop ws)
        {
            //pre-proccessing
            float price = ws.Price;
            float duration = (float)ws.Duration;
            float day = (float)ws.FullData.DayOfWeek;
            float time = (float)((double)ws.FullData.Hour + (double)ws.FullData.Minute*0.01);
            float teacher = (float)ws.TeacherId;

            //[ genre, date , price ] featuresSet
            double[] temp = { price, duration, day, time, teacher};
            return string.Join(",", temp);
            
        }

        //reconvert all workshops
        public void PreproccessingAllWorkshops()
        {
            if (File.Exists(@"WorkshopClusters.csv"))
            {
                File.Delete(@"WorkshopClusters.csv");
            }
            //get all workshops
            var workshops = _context.Workshop;
            foreach(Workshop ws in workshops)
            {
                WriteWorkshopToFile(WorkshopToVector(ws));
            }

        }

        public WorkshopData CreateDataObject(Workshop ws)
        {
            // Prepare BookItem as BookData (featuresSet)
            string convertedData = WorkshopToVector(ws);
            List<string> WorkshopFeaturesSet = convertedData.Split(',').ToList();
            return new WorkshopData
            {
                price = float.Parse(WorkshopFeaturesSet[0]),
                duration = float.Parse(WorkshopFeaturesSet[1]),
                day = float.Parse(WorkshopFeaturesSet[2]),
                time = float.Parse(WorkshopFeaturesSet[3]),
                teacher = float.Parse(WorkshopFeaturesSet[4])
            };

        }


        public class WorkshopData
        {
            [Column("0")]
            public float price;

            [Column("1")]
            public float duration;

            [Column("2")]
            public float day;

            [Column("3")]
            public float time;

            [Column("4")]
            public float teacher;
        }

        public class ClusterPrediction
        {
            [ColumnName("PredictedLabel")]
            public uint PredictedClusterId;

            [ColumnName("Score")]
            public float[] Distances;
        }

        public class WorkshopClustering
        {

            static readonly string _dataPath = "WorkshopClusters.csv";
            static readonly string _modelPath = "ClusteringModel.zip";

            private static PredictionModel<WorkshopData, ClusterPrediction> Train()
            {
                var pipeline = new LearningPipeline();
                // pipeline.Add(new TextLoader(_dataPath).CreateFrom<BookData>(separator: ','));

                //building dataset of BookData
                List<WorkshopData> data = new List<WorkshopData>();
                string line;
                using (var reader = File.OpenText(_dataPath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string convertedData = line;
                        List<string> WorkshopFeaturesSet = convertedData.Split(',').ToList();
                        WorkshopData bd = new WorkshopData
                        {
                            price = float.Parse(WorkshopFeaturesSet[0]), 
                            duration = float.Parse(WorkshopFeaturesSet[1]),
                            day = float.Parse(WorkshopFeaturesSet[2]),
                            time = float.Parse(WorkshopFeaturesSet[3]),
                            teacher = float.Parse(WorkshopFeaturesSet[4])
                        };
                        data.Add(bd);
                    }
                }

                var collection = CollectionDataSource.Create(data);
                pipeline.Add(collection);
                pipeline.Add(new ColumnConcatenator(
                    "Features",
                    "price",
                    "duration",
                    "day",
                    "time",
                    "teacher")
                    );

                pipeline.Add(new KMeansPlusPlusClusterer() { K = 3 });


                var model = pipeline.Train<WorkshopData, ClusterPrediction>();

                return model;
            }

            public ClusterPrediction Predict(WorkshopData wsData)
            {
                var model = Train();
                return model.Predict(wsData);
            }

        }
    }
}
