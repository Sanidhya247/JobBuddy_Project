import React, { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiService from '../utils/apiService';
import AuthContext from "../context/AuthContext";
import Loader from './commons/Loader';
import {toast} from 'react-toastify';
import "../assets/css/job_details_page.css";

const JobDetailsPage = () => {
  const { jobId } = useParams();
  const navigate = useNavigate();
  const { user } = useContext(AuthContext); // Getting user from context
  const [job, setJob] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    if (!user) return; // Avoid API calls if user is not logged in
    fetchJobDetails();
  }, [jobId, user]);

  useEffect(() => {
    if (job && user) {
      checkConnectionStatus(user.userID, job.employerID);
    }
  }, [job, user]);

  const fetchJobDetails = async () => {
    try {
      const response = await apiService.get(`/api/JobListing/${jobId}`);
      setJob(response.data.data);
    } catch (error) {
      setError("Failed to fetch job details. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  const checkConnectionStatus = async (userID, employerID) => {
    try {
      const response = await apiService.get(`/api/connection/check/${userID}/${employerID}`);
      console.log("check connection", response.data.data);
      setIsConnected(response.data.data); 
    } catch (error) {
      console.error("Error checking connection status:", error);
    }
  };

  const handleStartConversation = async () => {
    try {
      const response = await apiService.get('/api/chat/getChats');
      const existingChat = response.data.data.find(chat =>
        (chat.jobSeekerID === user.userID && chat.employerID === job.employerID) ||
        (chat.employerID === user.userID && chat.jobSeekerID === job.employerID)
      );

      if (existingChat) {
        navigate(`/chat/${existingChat.chatID}`);
      } else {
        const newChatResponse = await apiService.post('/api/chat/create', {
          jobSeekerID: user.userID,
          employerID: job.employerID,
        });
        const newChat = newChatResponse.data.data;
        navigate(`/chat/${newChat.chatID}`);
      }
    } catch (error) {
      toast.error("Error starting conversation. Please try again.");
      console.error('Error starting conversation:', error);
    }
  };

  const handleConnect = async () => {
    try {
      await apiService.post(`/api/connection/request`, {
        requestorId: user.userID,
        requesteeId: job.employerID,
      });
      setIsConnected(true);
      toast.success("Connection request sent to the employer.");
    } catch (error) {
      toast.error("Error sending connection request.");
      console.error("Error sending connection request:", error);
    }
  };

  const handleApply = async () => {
    try {
      const applicationData = {
        jobId: jobId,
        userId: user.userID,
        resumeId: user.resumeID,
      };
      await apiService.post(`/api/applications`, applicationData);
      toast.success("Application submitted successfully!");
    } catch (error) {
      toast.error("Error submitting application.");
      console.error("Error submitting application:", error);
    }
  };

  if (loading) return <Loader />;
  if (error) return <p>{error}</p>;
  if (!user) return <p>Please log in to view job details.</p>; 

  return (
    <div className="job-details-container">
      <div className="header">
        <h2>{job.jobTitle}</h2>
        <div className="action-buttons">
          {isConnected ? (
            <button className="start-conversation-button" onClick={handleStartConversation}>
              <i className="fas fa-comment"></i> Start Conversation
            </button>
          ) : (
            <button className="connect-button" onClick={handleConnect}>
              <i className="fas fa-user-plus"></i> Connect with Employer
            </button>
          )}
          <button className="apply-button" onClick={handleApply}>
            <i className="fas fa-paper-plane"></i> Apply
          </button>
        </div>
      </div>
      
      <div className="section-title">Description</div>
      <p><strong>Short Description:</strong> {job.shortJobDescription}</p>
      <p><strong>Full Description:</strong> {job.jobDescription}</p>

      <div className="section-title">Details</div>
      <div className="location"><strong>Location:</strong> <span className="value">{job.city}, {job.province}</span></div>
      <div className="location"><strong>Zip Code:</strong> <span className="value">{job.zipCode}</span></div>
      <div className="salary"><strong>Salary Range:</strong> <span className="value">{job.salaryRange}</span></div>
    </div>
  );
};

export default JobDetailsPage;
