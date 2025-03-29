import React from 'react';
import './styles/App.css';
import ChatInterface from './components/ChatInterface';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Chatbot Demo</h1>
      </header>
      <main>
        <ChatInterface />
      </main>
      <footer className="App-footer">
        <p>© 2025 - AI Chatbot Demo</p>
      </footer>
    </div>
  );
}

export default App;