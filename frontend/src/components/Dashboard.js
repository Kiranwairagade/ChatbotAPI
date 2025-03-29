import React, { useState, useEffect } from 'react';
import Sidebar from './Sidebar';
import ChatInterface from './ChatInterface';
import ApiKeyManager from './ApiKeyManager';
import '../styles/Dashboard.css';

const Dashboard = () => {
  const [activeView, setActiveView] = useState('chat');
  const [chatLogs, setChatLogs] = useState([]);

  useEffect(() => {
    fetchChatLogs();
  }, []);

  const fetchChatLogs = async () => {
    try {
      const response = await fetch('api/chatlogs');
      if (response.ok) {
        const data = await response.json();
        setChatLogs(data);
      }
    } catch (error) {
      console.error('Error fetching chat logs:', error);
    }
  };

  return (
    <div className="dashboard-container"> {/* 🟢 FLEX CONTAINER */}
      <Sidebar activeView={activeView} setActiveView={setActiveView} />
      
      <div className="dashboard-content"> {/* 🟢 RIGHT SIDE CONTENT */}
        {activeView === 'chat' && <ChatInterface chatLogs={chatLogs} />}
        {activeView === 'keys' && <ApiKeyManager />}
      </div>
    </div>
  );
};

export default Dashboard;
