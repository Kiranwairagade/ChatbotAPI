import React, { useState, useRef, useEffect } from "react";
import { Send } from "lucide-react";
import axios from "axios";
import "../styles/ChatInterface.css";

const ChatbotInterface = () => {
  const [messages, setMessages] = useState([
    { id: 1, text: "Hi there! How can I help you today?", sender: "bot" },
  ]);
  const [inputMessage, setInputMessage] = useState("");
  const [loading, setLoading] = useState(false);
  const messagesEndRef = useRef(null);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const fetchBotResponse = async (message) => {
    try {
      const response = await axios.post("http://localhost:5275/api/chatbot", {
        message,
      });
      return response.data.reply;
    } catch (error) {
      console.error("Error fetching bot response:", error);
      return "Sorry, I couldn't process that.";
    }
  };

  const handleSendMessage = async () => {
    if (inputMessage.trim() === "" || loading) return;

    const userMessage = { id: messages.length + 1, text: inputMessage, sender: "user" };
    setMessages((prev) => [...prev, userMessage]);
    setInputMessage("");
    setLoading(true);

    const botReply = await fetchBotResponse(inputMessage);
    const botMessage = { id: messages.length + 2, text: botReply, sender: "bot" };

    setMessages((prev) => [...prev, botMessage]);
    setLoading(false);
  };

  return (
    <div className="chat-container">
      <div className="chat-header">Chatbot</div>
      <div className="chat-messages">
        {messages.map((msg) => (
          <div
            key={msg.id}
            className={`chat-message ${msg.sender === "bot" ? "bot-message" : "user-message"}`}
          >
            {msg.text}
          </div>
        ))}
        <div ref={messagesEndRef} />
      </div>
      <div className="chat-footer">
        <input
          value={inputMessage}
          onChange={(e) => setInputMessage(e.target.value)}
          placeholder="Type your message..."
          onKeyDown={(e) => e.key === "Enter" && handleSendMessage()}
          className="chat-input"
          disabled={loading}
        />
        <button onClick={handleSendMessage} className="chat-button" disabled={loading}>
          {loading ? "..." : <Send className="h-4 w-4" />}
        </button>
      </div>
    </div>
  );
};

export default ChatbotInterface;
