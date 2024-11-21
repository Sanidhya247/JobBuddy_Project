import React, { useEffect, useState, useCallback } from "react";
import apiService from "../utils/apiService";
import JobDetailsCard from "./JobDetailsCard";
import "../assets/css/job_search_page.css";
import Loader from './commons/Loader';

const JobSearchPage = () => {
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [searchText, setSearchText] = useState("");

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Number of jobs per page

  // Filter state
  const [filters, setFilters] = useState({
    city: "",
    province: "",
    jobType: "",
    workType: "",
    experienceLevel: "",
    industry: "",
    minSalary: "",
    maxSalary: ""
  });

  const fetchJobs = useCallback(async (queryParams = {}) => {
    setLoading(true);
    try {
      const response = await apiService.get("/api/JobListing/filter", {
        params: { ...queryParams, page: currentPage, pageSize }
      });
      setJobs(response.data.data || []);
      setLoading(false);
    } catch (error) {
      setError("Error fetching jobs. Please try again later.");
      setLoading(false);
    }
  }, [currentPage]);

  // Handle search functionality
  const handleSearch = async () => {
    setLoading(true);
    try {
      const response = await apiService.get("/api/JobListing/search", {
        params: { title: searchText, page: 1, pageSize }
      });
      setJobs(response.data.data || []);
      setSearchText(""); 
      setCurrentPage(1); 
      setLoading(false);
    } catch (error) {
      setError("Error fetching jobs. Please try again later.");
      setLoading(false);
    }
  };

  // Apply filters
  const applyFilters = () => {
    const queryParams = {
      ...Object.fromEntries(Object.entries(filters).filter(([_, value]) => value))
    };
    setCurrentPage(1);
    fetchJobs(queryParams);
  };

  // Clear filters and reset to fetch all jobs
  const clearFilters = () => {
    setFilters({
      city: "",
      province: "",
      jobType: "",
      workType: "",
      experienceLevel: "",
      industry: "",
      minSalary: "",
      maxSalary: ""
    });
    setSearchText("");
    setCurrentPage(1);
    fetchJobs(); // Fetch all jobs
  };


  const handleInputChange = (e) => {
    setFilters({ ...filters, [e.target.name]: e.target.value });
  };

  const handleSearchChange = (e) => {
    setSearchText(e.target.value);
  };

  const handleNextPage = () => {
    setCurrentPage((prevPage) => prevPage + 1);
  };

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage((prevPage) => prevPage - 1);
    }
  };

  useEffect(() => {
    fetchJobs();
  }, [fetchJobs]);

  if (loading) return <Loader/>;
  if (error) return <p>{error}</p>;

  return (
    <div className="main-container">
      <h2>Job Listings</h2>

      {/* Search Bar */}
      <div className="search-bar-wrapper">
        <div className="search-bar">
          <div className="search-input">
            <input
              type="text"
              placeholder="Job Title"
              value={searchText}
              onChange={handleSearchChange}
            />
          </div>
          <button className="search-button" onClick={handleSearch}>
            Search
          </button>
        </div>
        <button className="view-all-button" onClick={clearFilters}>
          View All Jobs
        </button>
      </div>

      {/* Filter and Job List Section */}
      <div className="content-container">
        <div className="filter-sidebar">
          <h3>Filter Jobs</h3>
          <div className="filter-group">
            <input
              type="text"
              name="city"
              placeholder="City"
              value={filters.city}
              onChange={handleInputChange}
            />
            <input
              type="text"
              name="province"
              placeholder="Province"
              value={filters.province}
              onChange={handleInputChange}
            />
            <input
              type="text"
              name="jobType"
              placeholder="Job Type"
              value={filters.jobType}
              onChange={handleInputChange}
            />
            <input
              type="text"
              name="workType"
              placeholder="Work Type"
              value={filters.workType}
              onChange={handleInputChange}
            />
            <input
              type="text"
              name="experienceLevel"
              placeholder="Experience Level"
              value={filters.experienceLevel}
              onChange={handleInputChange}
            />
            <input
              type="text"
              name="industry"
              placeholder="Industry"
              value={filters.industry}
              onChange={handleInputChange}
            />
            <input
              type="number"
              name="minSalary"
              placeholder="Min Salary"
              value={filters.minSalary}
              onChange={handleInputChange}
            />
            <input
              type="number"
              name="maxSalary"
              placeholder="Max Salary"
              value={filters.maxSalary}
              onChange={handleInputChange}
            />
          </div>
          <button className="apply-btn" onClick={applyFilters}>
            Apply Filters
          </button>
          <button className="clear-btn" onClick={clearFilters}>
            Clear Filters
          </button>
        </div>

        {/* Job Listings */}
        <div className="job-list">
          {jobs.length > 0 ? (
            jobs.map((job) => <JobDetailsCard key={job.jobID} job={job} />)
          ) : (
            <p>No jobs found matching the criteria.</p>
          )}
        </div>
      </div>

      {/* Pagination Controls - moved to the bottom */}
      <div className="pagination">
        <button
          onClick={handlePreviousPage}
          disabled={currentPage === 1}
          className="pagination-button"
        >
          Previous
        </button>
        <span>Page {currentPage}</span>
        <button 
        onClick={handleNextPage} 
        className="pagination-button"
      >
          Next
        </button>
      </div>
    </div>
  );
};

export default JobSearchPage;
