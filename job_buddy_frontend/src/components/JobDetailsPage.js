import React, { useEffect, useState, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import apiService from "../utils/apiService";
import AuthContext from "../context/AuthContext";
import Loader from "./commons/Loader";
import { toast } from "react-toastify";
import "../assets/css/job_details_page.css";

const JobDetailsPage = () => {
  const { jobId } = useParams();
  const navigate = useNavigate();
  const { user } = useContext(AuthContext); // Getting user from context
  const [job, setJob] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [isConnected, setIsConnected] = useState(false);
  const [isRequestSent, setIsRequestSent] = useState(false); 
  const [hasApplied, setHasApplied] = useState(false); 

  useEffect(() => {
    const role = localStorage.getItem("role");
    if (!user) return;
    if (role !== "Job Seeker") {
      toast.error("Employers cannot apply for jobs.");
      navigate("/");
      return;
    }
    fetchJobDetails();
  }, [jobId, user]);

  useEffect(() => {
    if (job && user) {
      checkConnectionStatus(user.userID, job.employerID);
      checkExistingApplication(user.userID, jobId);
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
      const { exists, status } = response.data.data;

      if (exists) {
        if (status === "Accepted") {
          setIsConnected(true); // Connection is active
          setIsRequestSent(false); // No pending request
        } else if (status === "Pending") {
          setIsConnected(false);
          setIsRequestSent(true); // Request is pending
        } else {
          setIsConnected(false);
          setIsRequestSent(false); 
        }
      } else {
        setIsConnected(false);
        setIsRequestSent(false); 
      }
    } catch (error) {
      console.error("Error checking connection status:", error);
    }
  };

  const checkExistingApplication = async (userID, jobID) => {
    try {
      const response = await apiService.get(`/api/applications/${userID}/${jobID}`);
    if (response.data && response.data.success) {
      if (response.data.data) {
        console.log("Existing application found:", response.data.data);
        setHasApplied(true);
      } else {
        console.log("No existing application found for this user and job.");
      }
    } else if (!response.data.success) {
      console.log("Application not found:", response.data.errors?.$values);
    }
  }catch (error) {
      toast.error("Failed to check existing application.");
    }
  };

  const handleStartConversation = async () => {
    try {
      if (!isConnected) {
        toast.info("Connection request is pending acceptance.");
        return;
      }

      const response = await apiService.get("/api/chat/getChats");
      const existingChat = response.data.data.find(
        (chat) =>
          (chat.jobSeekerID === user.userID &&
            chat.employerID === job.employerID) ||
          (chat.employerID === user.userID &&
            chat.jobSeekerID === job.employerID)
      );

      if (existingChat) {
        navigate(`/chat/${existingChat.chatID}`);
      } else {
        const newChatResponse = await apiService.post("/api/chat/create", {
          jobSeekerID: user.userID,
          employerID: job.employerID,
          jobID: job.jobID,
        });
        const newChat = newChatResponse.data.data;
        navigate(`/chat/${newChat.chatID}`);
      }
    } catch (error) {
      toast.error("Error starting conversation. Please try again.");
      console.error("Error starting conversation:", error);
    }
  };

  const handleConnect = async () => {
    try {
      await apiService.post(`/api/connection/request`, {
        requestorId: user.userID,
        requesteeId: job.employerID,
      });
      setIsRequestSent(true); 
      toast.success("Connection request sent to the employer.");
    } catch (error) {
      toast.error("Error sending connection request.");
      console.error("Error sending connection request:", error);
    }
  };

  const handleApply = () => {
    navigate(`/apply/${jobId}`, { state: { userId: user.userID, jobId } });
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
            <button
              className="start-conversation-button"
              onClick={handleStartConversation}
            >
              <i className="fas fa-comment"></i> Start Conversation
            </button>
          ) : isRequestSent ? (
            <button className="friend-request-sent-button" disabled>
              <i className="fas fa-hourglass-half"></i> Friend Request Sent
            </button>
          ) : (
            <button className="connect-button" onClick={handleConnect}>
              <i className="fas fa-user-plus"></i> Connect with Employer
            </button>
          )}
          <button
            className="apply-button"
            onClick={handleApply}
            disabled={hasApplied}
          >
            {hasApplied ? "Already Applied" : "Apply for Job"}
          </button>
        </div>
      </div>

      <div className="section-title">Description</div>
      <p>
        <strong>Short Description:</strong> {job.shortJobDescription}
      </p>
      <p>
        <strong>Full Description:</strong> {job.jobDescription}
      </p>

      <div className="section-title">Details</div>
      <div className="location">
        <strong>Location:</strong>{" "}
        <span className="value">
          {job.city}, {job.province}
        </span>
      </div>
      <div className="location">
        <strong>Zip Code:</strong> <span className="value">{job.zipCode}</span>
      </div>
      <div className="salary">
        <strong>Salary Range:</strong>{" "}
        <span className="value">{job.salaryRange}</span>
      </div>
    </div>
  );
};

export default JobDetailsPage;
