import React from 'react';
import "../assets/css/job_search_page.css";

const Job_Search_Page = () => {
  return (

    <div className="main-container">

      <div className="search-bar">
        <div className="search-input">
          <span className="search-icon">Icon</span>
          <input type="text" placeholder="Job Title" />
        </div>
        <button className="search-button">Search</button>
      </div>

      <div className="content-container">

        <div className="filter-sidebar">
          <h2>Filters</h2>
          <div className="filter-group">
            <select>
              <option value="">Title</option>
              <option value="pharmacy-assistant">Pharmacy Assistant</option>
              <option value="pharmacist">Pharmacist</option>
            </select>
            <select>
              <option value="">Company</option>
              <option value="company1">Company 1</option>
              <option value="company2">Company 2</option>
            </select>
            <select>
              <option value="">Job Type</option>
              <option value="full-time">Full Time</option>
              <option value="part-time">Part Time</option>
            </select>
            <select>
              <option value="">Position</option>
              <option value="employee">Employee</option>
              <option value="manager">Manager</option>
            </select>
            <button className="apply-btn">Apply</button>
            <button className="clear-btn">Clear All</button>
          </div>
        </div>

        <div className="job-list">
            <div className="job-card">
                <div className="company-logo">
                  <img src="#" alt="Company Logo" />
                </div>
                <div className="company-details">
                  <h3>Pharmacy Assistant | Grimsby Pharmacy</h3>
                  <p>Kitchener, ON | Full Time | Employee | $15 / hour</p>
                  <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry.</p>
                  <button className="apply-now-btn">Quick Apply</button>
                </div>
              </div>

              <div className="job-card">
                <div className="company-logo">
                  <img src="#" alt="Company Logo" />
                </div>
                <div className="company-details">
                  <h3>Pharmacy Assistant | Grimsby Pharmacy</h3>
                  <p>Kitchener, ON | Full Time | Employee | $15 / hour</p>
                  <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry.</p>
                  <button className="apply-now-btn">Quick Apply</button>
                </div>
            </div>
        </div>
        
      </div>

      <div className="pagination">
        <div className="pagination-button active">1</div>
        <div className="pagination-button">2</div>
        <div className="pagination-button">3</div>
        <div className="pagination-dots">.....</div>
        <div className="pagination-button">
          <i className="pagination-next">100</i>
        </div>
      </div>

    </div>
    
  )
}

export default Job_Search_Page;
