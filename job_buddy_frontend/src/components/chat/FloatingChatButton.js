import React, { useState } from 'react';
import { FaComments } from 'react-icons/fa';
import ChatModal from './ChatModal';
import "../../assets/css/FloatingChatButton.css";

function FloatingChatButton() {
  const [isOpen, setIsOpen] = useState(false);

  const toggleChat = () => {
    setIsOpen(!isOpen);
  };

  return (
    <>
      <button className="floating-chat-button" onClick={toggleChat}>
        <FaComments size={24} />
      </button>
      {isOpen && <ChatModal closeModal={toggleChat} />}
    </>
  );
}

export default FloatingChatButton;
