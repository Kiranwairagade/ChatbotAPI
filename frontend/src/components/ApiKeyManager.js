import React, { useState, useEffect } from 'react';
import '../styles/ApiKeyManager.css';

const ApiKeyManager = () => {
  const [apiKeys, setApiKeys] = useState([]);
  const [newKeyName, setNewKeyName] = useState('');
  
  useEffect(() => {
    fetchApiKeys();
  }, []);
  
  const fetchApiKeys = async () => {
    try {
      const response = await fetch('api/keys');
      if (response.ok) {
        const data = await response.json();
        setApiKeys(data);
      }
    } catch (error) {
      console.error('Error fetching API keys:', error);
    }
  };
  
  const generateApiKey = async () => {
    if (!newKeyName.trim()) return;
    
    try {
      const response = await fetch('api/keys', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          name: newKeyName,
        }),
      });
      
      if (response.ok) {
        const data = await response.json();
        setApiKeys([...apiKeys, data]);
        setNewKeyName('');
      }
    } catch (error) {
      console.error('Error generating API key:', error);
    }
  };
  
  const revokeApiKey = async (keyId) => {
    try {
      const response = await fetch(`api/keys/${keyId}`, {
        method: 'DELETE',
      });
      
      if (response.ok) {
        setApiKeys(apiKeys.filter(key => key.id !== keyId));
      }
    } catch (error) {
      console.error('Error revoking API key:', error);
    }
  };
  
  return (
    <div className="api-key-manager">
      <div className="key-manager-header">
        <h2>API Key Management</h2>
      </div>
      <div className="new-key-form">
        <input
          type="text"
          placeholder="Key Name"
          value={newKeyName}
          onChange={(e) => setNewKeyName(e.target.value)}
        />
        <button onClick={generateApiKey}>Generate New Key</button>
      </div>
      <div className="keys-list">
        <div className="keys-table-header">
          <div className="key-name-column">Name</div>
          <div className="key-value-column">API Key</div>
          <div className="key-created-column">Created</div>
          <div className="key-actions-column">Actions</div>
        </div>
        {apiKeys.map(key => (
          <div key={key.id} className="key-item">
            <div className="key-name-column">{key.name}</div>
            <div className="key-value-column">
              <span className="key-value">{key.value}</span>
              <button 
                className="copy-button"
                onClick={() => navigator.clipboard.writeText(key.value)}
              >
                Copy
              </button>
            </div>
            <div className="key-created-column">
              {new Date(key.created).toLocaleDateString()}
            </div>
            <div className="key-actions-column">
              <button 
                className="revoke-button"
                onClick={() => revokeApiKey(key.id)}
              >
                Revoke
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ApiKeyManager;