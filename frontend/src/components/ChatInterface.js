import React, { useState, useEffect, useRef } from 'react';
import '../styles/ChatInterface.css';

const ChatInterface = () => {
  const [messages, setMessages] = useState([]);
  const [inputText, setInputText] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const messageEndRef = useRef(null);

  useEffect(() => {
    // Scroll to bottom when messages change
    messageEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  useEffect(() => {
    // Add welcome message when component mounts
    setMessages([
      {
        text: 'Hello! I\'m your AI assistant. How can I help you today?',
        sender: 'bot',
        timestamp: new Date().toISOString()
      }
    ]);
  }, []);

  const handleSendMessage = async (e) => {
    e.preventDefault();
    
    if (!inputText.trim()) return;
    
    const userMessage = {
      text: inputText,
      sender: 'user',
      timestamp: new Date().toISOString()
    };
    
    setMessages(prev => [...prev, userMessage]);
    setInputText('');
    setIsLoading(true);
    
    try {
      const response = await fetch('http://localhost:5275/api/chatbot/query', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ text: inputText })
      });
      
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      
      const data = await response.json();
      
      const botMessage = {
        text: data.response,
        sender: 'bot',
        intent: data.intent,
        confidence: data.confidence,
        timestamp: new Date().toISOString()
      };
      
      setMessages(prev => [...prev, botMessage]);
    } catch (error) {
      console.error('Error:', error);
      
      const errorMessage = {
        text: 'Sorry, I encountered an error processing your request. Please try again later.',
        sender: 'bot',
        error: true,
        timestamp: new Date().toISOString()
      };
      
      setMessages(prev => [...prev, errorMessage]);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="chat-container">
      <div className="chat-header">
        <h2>AI Assistant</h2>
      </div>
      
      <div className="messages-container">
        {messages.map((message, index) => (
          <div 
            key={index} 
            className={`message ${message.sender === 'user' ? 'user-message' : 'bot-message'} ${message.error ? 'error-message' : ''}`}
          >
            <div className="message-content">{message.text}</div>
            <div className="message-timestamp">
              {new Date(message.timestamp).toLocaleTimeString()}
              {message.intent && (
                <span className="message-intent">
                  Intent: {message.intent} ({(message.confidence * 100).toFixed(1)}%)
                </span>
              )}
            </div>
          </div>
        ))}
        
        {isLoading && (
          <div className="message bot-message loading-message">
            <div className="loading-dots">
              <span></span>
              <span></span>
              <span></span>
            </div>
          </div>
        )}
        
        <div ref={messageEndRef} />
      </div>
      
      <form className="chat-input-form" onSubmit={handleSendMessage}>
        <input
          type="text"
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          placeholder="Type your message here..."
          disabled={isLoading}
        />
        <button type="submit" disabled={isLoading || !inputText.trim()}>
          Send
        </button>
      </form>
    </div>
  );
};

export default ChatInterface;