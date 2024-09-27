import React from 'react';
import "../assets/css/job_search_page.css";

const Job_Search_Page = () => {
  return (
    <div className="search-container">
      <div className="search-bar">
        <div className="search-input">
          <span className="search-icon">Icon</span>
          <input type="text" placeholder="Job Title" />
        </div>
        <button className="search-button">Search</button>
      </div>
    </div>
  )
}

export default Job_Search_Page;
