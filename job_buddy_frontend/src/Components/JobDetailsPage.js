import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import apiService from '../utils/apiService';
import "../assets/css/job_details_page.css";

const JobDetailsPage = () => {
  const { jobId } = useParams();
  const [job, setJob] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchJobDetails = async () => {
      try {
        const response = await apiService.get(`/api/JobListing/${jobId}`);
        setJob(response.data.data);
        setLoading(false);
      } catch (error) {
        setError("Failed to fetch job details. Please try again later.");
        setLoading(false);
      }
    };

    fetchJobDetails();
  }, [jobId]);

  if (loading) return <p>Loading job details...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="job-details-container">
      <h2>{job.jobTitle}</h2>
      
      <div className="section-title">Description</div>
      <p><strong>Short Description:</strong> {job.shortJobDescription}</p>
      <p><strong>Full Description:</strong> {job.jobDescription}</p>
      
      <div className="section-title">Location & Salary</div>
      <div className="location"><strong>Location:</strong> <span className="value">{job.city}, {job.province}</span></div>
      <div className="location"><strong>Zip Code:</strong> <span className="value">{job.zipCode}</span></div>
      <div className="salary"><strong>Salary Range:</strong> <span className="value">{job.salaryRange}</span></div>
      <div className="salary"><strong>Pay Rate (Per Year):</strong> <span className="value">${job.payRatePerYear?.toLocaleString() || "Not specified"}</span></div>
      <div className="salary"><strong>Pay Rate (Per Hour):</strong> <span className="value">${job.payRatePerHour || "Not specified"}</span></div>
      
      <div className="section-title">Job Details</div>
      <div className="job-type"><strong>Job Type:</strong> <span className="value">{job.jobType}</span></div>
      <div className="work-type"><strong>Work Type:</strong> <span className="value">{job.workType}</span></div>
    </div>
  );
};

export default JobDetailsPage;
