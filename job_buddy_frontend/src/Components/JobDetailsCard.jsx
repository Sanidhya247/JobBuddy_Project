import React from "react";
import { useNavigate } from "react-router-dom";
import "../assets/css/job_search_page.css";

const JobDetailsCard = ({ job}) => {
        const navigate = useNavigate();
        const handleNavigation = () => {
            navigate("/job-apply")
        }
        const handleViewDetails = () => {
            navigate(`/job/${job.jobID}`); // Navigate to the details page with the job ID
      };
        return (
            <div className="job-card">
              <div className="company-logo">
                <img src={job.companyLogo || "default-logo.png"} alt="Company Logo" />
              </div>
              <div className="company-details">
                <h3>{job.jobTitle}</h3>
                <p>{job.shortJobDescription}</p>
                <p>Location: {job.city}, {job.province}</p>
                <p>Salary: {job.salaryRange || "Not specified"}</p>
                
              </div>
              <button onClick={handleNavigation} className="apply-now-btn">Quick Apply</button>
              <button className="apply-now-btn" onClick={handleViewDetails}>
                View Details
              </button>
            </div>
          );
    
}

export default JobDetailsCard
