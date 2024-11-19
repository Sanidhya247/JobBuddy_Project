import React, { useEffect, useState } from 'react';
import { FaEnvelope } from 'react-icons/fa';
import apiService from '../../utils/apiService';
import NotificationSound from '../../assets/sounds/notification.mp3';

const NotificationIcon = () => {
  const [unreadCount, setUnreadCount] = useState(0);

  useEffect(() => {
    const fetchUnreadCount = async () => {
      try {
        const response = await apiService.get('/api/chat/unread-counts');
        setUnreadCount(response.data.data);
      } catch (error) {
        console.error('Error fetching unread messages:', error);
      }
    };

    fetchUnreadCount();

    if (unreadCount > 0) {
      const sound = new Audio(NotificationSound);
      sound.play();
    }
  }, [unreadCount]);

  return (
    <div className="notification-icon">
      <FaEnvelope />
      {unreadCount > 0 && <span className="unread-badge">{unreadCount}</span>}
    </div>
  );
};

export default NotificationIcon;
