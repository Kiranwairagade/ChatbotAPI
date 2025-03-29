import React from 'react';
import { FaComments, FaKey, FaCog, FaSignOutAlt } from 'react-icons/fa';
import '../styles/Sidebar.css';

const Sidebar = ({ activeView, setActiveView }) => {
  return (
    <div className="sidebar">
      <div className="sidebar-logo">
        <h2>AI Chat</h2>
      </div>
      <ul className="sidebar-menu">
        <li 
          className={activeView === 'chat' ? 'active' : ''}
          onClick={() => setActiveView('chat')}
        >
          <FaComments /> <span>Chat Interface</span>
        </li>
        <li 
          className={activeView === 'keys' ? 'active' : ''}
          onClick={() => setActiveView('keys')}
        >
          <FaKey /> <span>API Keys</span>
        </li>
        <li>
          <FaCog /> <span>Settings</span>
        </li>
      </ul>
      <div className="sidebar-footer">
        <button className="logout-button">
          <FaSignOutAlt /> <span>Logout</span>
        </button>
      </div>
    </div>
  );
};

export default Sidebar;