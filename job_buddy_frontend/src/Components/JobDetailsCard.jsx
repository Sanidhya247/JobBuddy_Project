import React from "react";
import { useNavigate } from "react-router-dom";
import "../assets/css/job_search_page.css";
import { useNavigate } from 'react-router-dom';

const JobDetailsCard = ({ title,
    location,
    type,
    position,
    salary,
    description }) => {
        const navigate = useNavigate();
        const handleNavigation = () => {
            navigate("/job-apply")
        }
    return (
        <div className="job-card">
            <div className="company-logo">
                <img src="#" alt="Company Logo" />
            </div>
            <div className="company-details">
                <h3>{title}</h3>
                <p>{`${location} | ${type} | ${position} | ${salary}`}</p>
                <p>{description}</p>
                <button onClick={handleNavigation} className="apply-now-btn">Quick Apply</button>
            </div>
        </div>

    )
}

export default JobDetailsCard
