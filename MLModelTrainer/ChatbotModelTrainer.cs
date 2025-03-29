using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MLModelTrainer
{
    class Program
    {
        static void Main()
        {
            var mlContext = new MLContext();

            string dataPath = "Data/data.tsv";
            var data = mlContext.Data.LoadFromTextFile<ChatbotData>(dataPath, separatorChar: '\t', hasHeader: true);

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(ChatbotData.Question))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ChatbotData.Answer)))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "Label"))
                .Append(mlContext.Transforms.CopyColumns("Answer", "PredictedLabel"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"));

            var model = pipeline.Fit(data);
            mlContext.Model.Save(model, data.Schema, "chatbot_model.zip");

            Console.WriteLine("Model trained and saved successfully.");
        }

        public class ChatbotData
        {
            [LoadColumn(0)] public string Question { get; set; }
            [LoadColumn(1)] public string Answer { get; set; }
        }
    }
}
