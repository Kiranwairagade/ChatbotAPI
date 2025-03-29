using Microsoft.ML.Data;

namespace ChatbotApi.Models
{
    public class ChatbotData
    {
        [LoadColumn(0)]
        public string Intent { get; set; } = string.Empty;
        
        [LoadColumn(1)]
        public string Text { get; set; } = string.Empty;
    }
    
    public class ChatbotPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Intent { get; set; } = string.Empty;
        
        [ColumnName("Score")]
        public float[] Confidence { get; set; } = Array.Empty<float>();
    }
    
    public class ChatbotQuery
    {
        public string Text { get; set; } = string.Empty;
    }
    
    public class ChatbotResponse
    {
        public string Intent { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public string Response { get; set; } = string.Empty;
    }
}