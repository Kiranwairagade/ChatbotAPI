using Microsoft.ML;
using System;
using System.IO;

namespace ChatbotAPI.Services
{
    public class ChatbotService
    {
        private readonly PredictionEngine<ChatbotData, ChatbotPrediction> _predictionEngine;

        public ChatbotService()
        {
            var mlContext = new MLContext();
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "MLModel", "chatbot_model.zip");
            var loadedModel = mlContext.Model.Load(modelPath, out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<ChatbotData, ChatbotPrediction>(loadedModel);
        }

        public string GetAnswer(string question)
        {
            var prediction = _predictionEngine.Predict(new ChatbotData { Question = question });
            return string.IsNullOrEmpty(prediction.Answer) ? "Sorry, I couldn't process that." : prediction.Answer;
        }
    }

    public class ChatbotData
    {
        public string Question { get; set; }
    }

    public class ChatbotPrediction
    {
        public string Answer { get; set; }
    }
}
