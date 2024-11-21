import React, { useState, useEffect } from 'react';
import { FaTimes } from 'react-icons/fa';
import apiService from '../../utils/apiService';
import "../../assets/css/ChatModal.css";

function ChatModal({ closeModal }) {
  const [activeTab, setActiveTab] = useState('chats');
  const [chats, setChats] = useState([]);
  const [friendRequests, setFriendRequests] = useState([]);

  useEffect(() => {
    if (activeTab === 'chats') {
      fetchChats();
    } else {
      fetchFriendRequests();
    }
  }, [activeTab]);

  const fetchChats = async () => {
    try {
      const response = await apiService.get('/api/chat/getChats');
      setChats(response.data.data || []);
    } catch (error) {
      console.error('Error fetching chats:', error);
    }
  };

  const fetchFriendRequests = async () => {
    try {
      const response = await apiService.get('/api/connection/user/friend-requests');
      setFriendRequests(response.data.data || []);
    } catch (error) {
      console.error('Error fetching friend requests:', error);
    }
  };

  const handleAcceptRequest = async (requestId) => {
    if (!requestId) return;
    try {
      await apiService.put(`/api/connection/accept/${requestId}`);
      setFriendRequests((prevRequests) =>
        prevRequests.filter((request) => request.id !== requestId)
      );
    } catch (error) {
      console.error(`Error accepting request ${requestId}:`, error);
    }
  };

  const handleRejectRequest = async (requestId) => {
    if (!requestId) return;
    try {
      await apiService.put(`/api/connection/reject/${requestId}`);
      setFriendRequests((prevRequests) =>
        prevRequests.filter((request) => request.id !== requestId)
      );
    } catch (error) {
      console.error(`Error rejecting request ${requestId}:`, error);
    }
  };

  return (
    <div className="chat-modal">
      <div className="chat-modal-header">
        <h3>{activeTab === 'chats' ? 'Chats' : 'Friend Requests'}</h3>
        <button onClick={closeModal} className="close-button">
          <FaTimes />
        </button>
      </div>
      <div className="chat-modal-tabs">
        <button
          className={activeTab === 'chats' ? 'active' : ''}
          onClick={() => setActiveTab('chats')}
        >
          Chats
        </button>
        <button
          className={activeTab === 'requests' ? 'active' : ''}
          onClick={() => setActiveTab('requests')}
        >
          Friend Requests
        </button>
      </div>
      <div className="chat-modal-content">
        {activeTab === 'chats' ? (
          <div className="chats-list">
            {chats.length > 0 ? (
              chats.map((chat) => (
                <div key={chat.chatID} className="chat-item">
                  <p>{chat.userName}</p>
                  <p>{chat.lastMessage}</p>
                </div>
              ))
            ) : (
              <p>No active chats yet.</p>
            )}
          </div>
        ) : (
          <div className="requests-list">
            {friendRequests.length > 0 ? (
              friendRequests.map((request) => (
                <div key={request.id} className="request-item">
                  <p>{request.userName}</p>
                  <button
                    onClick={() => handleAcceptRequest(request.id)}
                    className="accept-button"
                  >
                    Accept
                  </button>
                  <button
                    onClick={() => handleRejectRequest(request.id)}
                    className="decline-button"
                  >
                    Decline
                  </button>
                </div>
              ))
            ) : (
              <p>No pending friend requests.</p>
            )}
          </div>
        )}
      </div>
    </div>
  );
}

export default ChatModal;
