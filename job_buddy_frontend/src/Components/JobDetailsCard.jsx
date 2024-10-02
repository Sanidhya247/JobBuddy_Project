import React from 'react'
import "../assets/css/job_search_page.css";

const JobDetailsCard = ({ title,
    location,
    type,
    position,
    salary,
    description }) => {
    return (
        <div className="job-card">
            <div className="company-logo">
                <img src="#" alt="Company Logo" />
            </div>
            <div className="company-details">
                <h3>{title}</h3>
                <p>{`${location} | ${type} | ${position} | ${salary}`}</p>
                <p>{description}</p>
                <button className="apply-now-btn">Quick Apply</button>
            </div>
        </div>

    )
}

export default JobDetailsCard
