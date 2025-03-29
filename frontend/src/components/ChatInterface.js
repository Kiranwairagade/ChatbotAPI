import React, { useState, useRef, useEffect } from "react";
import { Send } from "lucide-react";
import "../styles/ChatInterface.css";

const ChatbotInterface = () => {
  const [messages, setMessages] = useState([
    { id: 1, text: "Hi there! How can I help you today?", sender: "bot" },
  ]);
  const [inputMessage, setInputMessage] = useState("");
  const messagesEndRef = useRef(null);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const handleSendMessage = () => {
    if (inputMessage.trim() === "") return;

    const userMessage = { id: messages.length + 1, text: inputMessage, sender: "user" };
    const botResponse = { id: messages.length + 2, text: `You said: ${inputMessage}`, sender: "bot" };

    setMessages([...messages, userMessage, botResponse]);
    setInputMessage("");
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
          onKeyPress={(e) => e.key === "Enter" && handleSendMessage()}
          className="chat-input"
        />
        <button onClick={handleSendMessage} className="chat-button">
          <Send className="h-4 w-4" />
        </button>
      </div>
    </div>
  );
};

export default ChatbotInterface;
