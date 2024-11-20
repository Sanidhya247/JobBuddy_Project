import React, { useEffect, useState, useContext } from 'react';
import apiService from '../../utils/apiService';
import AuthContext from '../../context/AuthContext';
import Loader from '../commons/Loader'; 
import "../../assets/css/FriendRequestsPage.css";
import ProfileModal from './ProfileModal';
import { useNavigate } from 'react-router-dom';

function FriendRequestsPage() {
  const navigate = useNavigate();
  const { user } = useContext(AuthContext);
  const [requests, setRequests] = useState([]);
  const [activeFriends, setActiveFriends] = useState([]);
  const [selectedProfile, setSelectedProfile] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [loadingRequests, setLoadingRequests] = useState(false);
  const [loadingFriends, setLoadingFriends] = useState(false);
  const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

  useEffect(() => {
    fetchFriendRequests();
    fetchActiveFriends();
  }, []);

  const fetchFriendRequests = async () => {
    setLoadingRequests(true);
    try {
      const response = await apiService.get('/api/connection/user/friend-requests');
      const userRequests = await Promise.all(
        response.data.data
          .filter(request => request.requesteeID === user.userID)
          .map(async (request) => {
            const profileResponse = await apiService.get(`/api/userprofile/${request.requestorID}`);
            return { ...request, userProfile: profileResponse.data.data };
          })
      );
      setRequests(userRequests);
    } catch (error) {
      console.error('Error fetching friend requests:', error);
    } finally {
      setLoadingRequests(false);
    }
  };

  const fetchActiveFriends = async () => {
    setLoadingFriends(true);
    try {
      const response = await apiService.get(`/api/connection/user/${user.userID}`);
      const friendsData = await Promise.all(
        response.data.data.map(async (friend) => {
          const profileId = friend.requestorID === user.userID ? friend.requesteeID : friend.requestorID;
          const profileResponse = await apiService.get(`/api/userprofile/${profileId}`);
          return { ...friend, userProfile: profileResponse.data.data };
        })
      );
      setActiveFriends(friendsData);
    } catch (error) {
      console.error('Error fetching active friends:', error);
    } finally {
      setLoadingFriends(false);
    }
  };

  const handleAcceptRequest = async (requestId) => {
    if (!requestId) return;
    try {
      await apiService.put(`/api/connection/accept/${requestId}`);
      setRequests(prevRequests => prevRequests.filter(request => request.connectionID !== requestId));
      fetchActiveFriends();
    } catch (error) {
      console.error(`Error accepting request ${requestId}:`, error);
    }
  };

  const handleRejectRequest = async (requestId) => {
    if (!requestId) return;
    try {
      await apiService.put(`/api/connection/reject/${requestId}`);
      setRequests(prevRequests => prevRequests.filter(request => request.connectionID !== requestId));
    } catch (error) {
      console.error(`Error rejecting request ${requestId}:`, error);
    }
  };

  const handleRemoveFriend = async (friendId) => {
    if (!friendId) return;
    try {
      await apiService.delete(`/api/connection/remove/${friendId}`);
      setActiveFriends(prevFriends => prevFriends.filter(friend => friend.connectionID !== friendId));
    } catch (error) {
      console.error(`Error removing friend ${friendId}:`, error);
    }
  };

  const handleSendMessage = async (friend) => {
    try {
      const response = await apiService.get('/api/chat/getChats');
      const existingChat = response.data.data.find(chat =>
        (chat.jobSeekerID === user.userID && chat.employerID === friend.userProfile.userID) ||
        (chat.employerID === user.userID && chat.jobSeekerID === friend.userProfile.userID)
      );
      if (existingChat) {
        navigate(`/chat/${existingChat.chatID}`);
      } else {
        const newChatResponse = await apiService.post('/api/chat/create', {
          jobSeekerID: user.userID,
          employerID: friend.userProfile.userID,
        });
        const newChat = newChatResponse.data.data;
        navigate(`/chat/${newChat.chatID}`);
      }
    } catch (error) {
      console.error('Error sending message or creating chat:', error);
    }
  };

  const viewUserProfile = (profile) => {
    setSelectedProfile(profile);
    setShowModal(true);
  };

  return (
    <div className="friend-requests-page">
      <div className="friend-requests-content">
        <h2>Friend Requests</h2>
        {loadingRequests ? (
          <Loader />
        ) : (
          <div className="requests-list">
            {requests.length > 0 ? (
              requests.map((request) => (
                <div key={request.connectionID} className="request-item card">
                  <img
                    src={request.userProfile?.profilePictureUrl ? `${BASE_URL}${request.userProfile.profilePictureUrl}` : '/default-avatar.png'}
                    alt={`${request.userProfile?.fullName}'s avatar`}
                    className="profile-image"
                    onClick={() => viewUserProfile(request.userProfile)}
                  />
                  <div className="request-info">
                    <p><strong>{request.userProfile?.fullName}</strong></p>
                    <p>{request.userProfile?.jobTitle}</p>
                    <button onClick={() => handleAcceptRequest(request.connectionID)} className="accept-button">Accept</button>
                    <button onClick={() => handleRejectRequest(request.connectionID)} className="decline-button">Decline</button>
                  </div>
                </div>
              ))
            ) : (
              <p>No pending friend requests.</p>
            )}
          </div>
        )}

        <h2>Active Friends</h2>
        {loadingFriends ? (
          <Loader />
        ) : (
          <div className="active-friends-list">
            {activeFriends.length > 0 ? (
              activeFriends.map((friend) => (
                <div key={friend.connectionID} className="friend-item card">
                  <img
                    src={friend.userProfile?.profilePictureUrl ? `${BASE_URL}${friend.userProfile.profilePictureUrl}` : '/default-avatar.png'}
                    alt={`${friend.userProfile?.fullName}'s avatar`}
                    className="profile-image"
                    onClick={() => viewUserProfile(friend.userProfile)}
                  />
                  <div className="friend-info">
                    <div className="friend-header">
                      <p><strong>{friend.userProfile?.fullName}</strong></p>
                      <button onClick={() => handleRemoveFriend(friend.connectionID)} className="remove-button">Remove Friend</button>
                    </div>
                    <p>{friend.userProfile?.email}</p>
                    <p>{friend.userProfile?.jobTitle}</p>
                    <button onClick={() => handleSendMessage(friend)} className="message-button">Send Message</button>
                  </div>
                </div>
              ))
            ) : (
              <p>No active friends.</p>
            )}
          </div>
        )}

        {showModal && selectedProfile && (
          <ProfileModal
            profile={selectedProfile}
            onClose={() => setShowModal(false)}
          />
        )}
      </div>
    </div>
  );
}

export default FriendRequestsPage;
