using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

namespace MLModelTrainer
{
    public class ChatbotModelTrainer
    {
        private readonly string _dataPath;
        private readonly string _modelPath;
        
        public ChatbotModelTrainer(string dataPath, string modelPath = "chatbot_model.zip")
        {
            _dataPath = dataPath;
            _modelPath = modelPath;
        }
        
        public void TrainAndSaveModel()
        {
            // Create MLContext
            var mlContext = new MLContext(seed: 1);
            
            // Load data
            IDataView dataView = mlContext.Data.LoadFromTextFile<ChatbotData>(
                _dataPath, 
                hasHeader: true,
                separatorChar: '\t');
            
            // Split dataset for training and evaluation
            var dataSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainingData = dataSplit.TrainSet;
            var testData = dataSplit.TestSet;
            
            // Create and train the model
            Console.WriteLine("Building and training the model...");
            var pipeline = mlContext.Transforms.Text.NormalizeText("NormalizedText", "Text")
                .Append(mlContext.Transforms.Text.TokenizeIntoWords("Words", "NormalizedText"))
                .Append(mlContext.Transforms.Text.RemoveDefaultStopWords("FilteredWords", "Words"))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("FilteredWordsKey", "FilteredWords")) // Convert to key type
                .Append(mlContext.Transforms.Text.ProduceNgrams("NGrams", "FilteredWordsKey"))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", "Intent"))
                .Append(mlContext.Transforms.Text.FeaturizeText("Features", "NormalizedText"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            
            var model = pipeline.Fit(trainingData);
            
            // Evaluate the model
            var predictions = model.Transform(testData);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions);
            
            Console.WriteLine($"Micro Accuracy: {metrics.MicroAccuracy:F2}");
            Console.WriteLine($"Macro Accuracy: {metrics.MacroAccuracy:F2}");
            Console.WriteLine($"Log Loss: {metrics.LogLoss:F2}");
            
            // Save the model
            mlContext.Model.Save(model, trainingData.Schema, _modelPath);
            Console.WriteLine($"Model saved to {_modelPath}");
        }
        
        public static void Main(string[] args)
        {
            string dataPath = args.Length > 0 ? args[0] : Path.Combine("Data", "data.tsv");
            string modelPath = args.Length > 1 ? args[1] : "chatbot_model.zip";
            
            var trainer = new ChatbotModelTrainer(dataPath, modelPath);
            trainer.TrainAndSaveModel();
        }
    }
    
    public class ChatbotData
    {
        [LoadColumn(0)]
        public string Intent { get; set; } = string.Empty;  // Fix nullability issue
        
        [LoadColumn(1)]
        public string Text { get; set; } = string.Empty;  // Fix nullability issue
    }
    
    public class ChatbotPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Intent { get; set; } = string.Empty;  // Fix nullability issue
        
        [ColumnName("Score")]
        public float[] Confidence { get; set; } = Array.Empty<float>();  // Fix nullability issue
    }
}
