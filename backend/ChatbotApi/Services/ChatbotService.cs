using System;
using System.Collections.Generic;
using Microsoft.ML;
using ChatbotApi.Models;

namespace ChatbotApi.Services
{
    public class ChatbotService
    {
        private readonly string _modelPath;
        private readonly PredictionEngine<ChatbotData, ChatbotPrediction> _predictionEngine;
        private readonly Dictionary<string, Func<string, string>> _responseGenerators;

        public ChatbotService(string modelPath)
        {
            _modelPath = modelPath;
            
            // Load the model
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(_modelPath, out var modelInputSchema);
            
            // Create prediction engine
            _predictionEngine = mlContext.Model.CreatePredictionEngine<ChatbotData, ChatbotPrediction>(model);
            
            // Initialize response generators for each intent
            _responseGenerators = new Dictionary<string, Func<string, string>>
            {
                ["Greeting"] = _ => "Hello! How can I help you today?",
                ["Farewell"] = _ => "Goodbye! Have a great day!",
                ["Help"] = _ => "I can help you with information about our services, products, and more. What would you like to know?",
                ["ProductInfo"] = query => $"Here's information about our products that might answer your question: '{query}'",
                ["ServiceInfo"] = query => $"Our services are designed to help you with: '{query}'",
                ["SupportRequest"] = _ => "I'll connect you with our support team. Please provide more details about your issue.",
                ["None"] = _ => "I'm not sure I understand. Could you rephrase your question?"
            };
        }

        public (string Intent, float Confidence) ProcessQuery(string query)
        {
            // Create input data
            var input = new ChatbotData { Text = query };
            
            // Make prediction
            var prediction = _predictionEngine.Predict(input);
            
            // Get the highest confidence score
            var maxScore = prediction.Confidence.Max();
            var maxIndex = Array.IndexOf(prediction.Confidence, maxScore);
            
            return (prediction.Intent, maxScore);
        }

        public string GenerateResponse(string intent, string query)
        {
            if (_responseGenerators.TryGetValue(intent, out var responseGenerator))
            {
                return responseGenerator(query);
            }
            
            return "I'm not sure how to respond to that.";
        }

        public IEnumerable<string> GetSupportedIntents()
        {
            return _responseGenerators.Keys;
        }
    }
}