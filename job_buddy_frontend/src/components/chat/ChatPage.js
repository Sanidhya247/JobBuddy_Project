import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiService from '../../utils/apiService';
import AuthContext from '../../context/AuthContext';
import "../../assets/css/ChatPage.css";

const ChatPage = () => {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  const { chatID } = useParams(); 
  const [chats, setChats] = useState([]);
  const [activeChat, setActiveChat] = useState(null);
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [unreadCounts, setUnreadCounts] = useState({});
  const [isBlocked, setIsBlocked] = useState(false);

  useEffect(() => {
    if (user?.userID) {
      fetchChats(user.userID); // Fetch chats for the logged-in user
    }
  }, [user?.userID]);

  useEffect(() => {
    if (chats.length > 0) {
      const defaultChat = chatID
        ? chats.find(chat => chat.chatID === parseInt(chatID))
        : chats[0]; 

      if (defaultChat) {
        handleChatSelect(defaultChat); 
      }
    }
  }, [chatID, chats]);

  const fetchChats = async (userID) => {
    try {
      const response = await apiService.get(`/api/chat/getChats/${userID}`);
      setChats(response.data.data);
    } catch (error) {
      console.error('Error fetching chats:', error);
    }
  };

  const fetchMessages = async (chatID) => {
    try {
      const response = await apiService.get(`/api/chat/messages/${chatID}`);
      setMessages(response.data.data);
    } catch (error) {
      console.error('Error fetching messages:', error);
    }
  };

  const handleChatSelect = (chat) => {
    console.log("check chat", chat);
    setActiveChat(chat);
    navigate(`/chat/${chat.chatID}`); 
    fetchMessages(chat.chatID);
    setIsBlocked(!chat.isActive); // Update blocked status
  };

  const handleSendMessage = async () => {
    if (newMessage.trim() && activeChat) {
      try {
        await apiService.post(`/api/chat/send`, {
          chatID: activeChat.chatID,
          senderID: user.userID,
          content: newMessage,
        });
        setNewMessage('');
        fetchMessages(activeChat.chatID);
      } catch (error) {
        console.error('Error sending message:', error);
      }
    }
  };

  const formatDate = (timestamp) => {
    return timestamp ? new Date(timestamp).toLocaleString() : '';
  };

  return (
    <div className="chat-page">
      <div className="chat-list">
        <h2>Chats</h2>
        {chats.length > 0 ? (
          chats.map((chat) => (
            <div
              key={chat.chatID}
              className={`chat-item ${chat.chatID === activeChat?.chatID ? 'active' : ''}`}
              onClick={() => handleChatSelect(chat)}
            >
              <div className="chat-user">
                <strong>{chat.userName || 'Unknown User'}</strong>
              </div>
              <div className="last-message">{chat.lastMessage || 'No messages yet'}</div>
              <div className="timestamp">{formatDate(chat.lastMessageTime)}</div>
            </div>
          ))
        ) : (
          <p>No chats available.</p>
        )}
      </div>
      <div className="chat-conversation">
        {activeChat ? (
          <>
            <div className="conversation-header">
              <h3>{activeChat.userName || 'Unknown User'}</h3>
              {!activeChat.isActive && <span className="blocked-label">Blocked</span>}
            </div>
            <div className="conversation-messages">
              {messages.length > 0 ? (
                messages.map((message) => (
                  <div
                    key={message.messageID}
                    className={`message ${message.senderID === user.userID ? 'sent' : 'received'}`}
                  >
                    <p>{message.content}</p>
                    <span className="timestamp">{formatDate(message.sentAt)}</span>
                  </div>
                ))
              ) : (
                <p>No messages yet.</p>
              )}
            </div>
            {!isBlocked ? (
              <div className="conversation-input">
                <input
                  type="text"
                  value={newMessage}
                  onChange={(e) => setNewMessage(e.target.value)}
                  placeholder="Type a message..."
                />
                <button onClick={handleSendMessage}>Send</button>
              </div>
            ) : (
              <p className="blocked-message">You cannot send messages to this user.</p>
            )}
          </>
        ) : (
          <p className="no-chat">Select a chat to start messaging</p>
        )}
      </div>
    </div>
  );
};

export default ChatPage;
