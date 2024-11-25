import React, { useState } from "react";
import "../style.css";
import "../assets/css/post_job.css";
import Input from "./commons/Input";
import { Link } from "react-router-dom";
import SpeechInput from "./SpeechInput";

const PostJob = () => {
  const [formData, setFormData] = useState({
    jobTitle: "",
    jobDescription: "",
    shortJobDescription: "",
    city: "",
    province: "",
    zipCode: "",
    salaryRange: "",
    payRatePerYear: 0,
    payRatePerHour: 0,
    jobType: "",
    workType: "",
    termsAccepted: false,
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : value,
    });
    setErrors((prevErrors) => ({ ...prevErrors, [name]: "" })); // Clear error on change
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.jobTitle) newErrors.jobTitle = "Job title is required.";
    if (!formData.jobDescription) newErrors.jobDescription = "Description is required.";
    if (!formData.shortJobDescription) newErrors.shortJobDescription = "Short description is required.";
    if (!formData.city) newErrors.city = "City is required.";
    if (!formData.province) newErrors.province = "Province is required.";
    if (!formData.zipCode) newErrors.zipCode = "Zip code is required.";
    if (!formData.salaryRange) newErrors.salaryRange = "Select a salary range.";
    if (!formData.jobType) newErrors.jobType = "Job type is required.";
    if (!formData.workType) newErrors.workType = "Work type is required.";
    if (!formData.termsAccepted) newErrors.termsAccepted = "You must accept the terms.";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    setLoading(true);
    setErrorMessage("");
    setSuccessMessage("");
    const authToken = localStorage.getItem("authToken") || "";

    try {
      const response = await fetch("https://localhost:7113/api/JobListing", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${authToken}`,
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to post job listing.");
      }

      setSuccessMessage("Job posted successfully!");
      setFormData({
        jobTitle: "",
        jobDescription: "",
        shortJobDescription: "",
        city: "",
        province: "",
        zipCode: "",
        salaryRange: "",
        payRatePerYear: 0,
        payRatePerHour: 0,
        jobType: "",
        workType: "",
        termsAccepted: false,
      });
    } catch (error) {
      setErrorMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <div className="job-application-header">
        <h2>Post a Job</h2>
        <p>Please fill out the form below to create a job posting.</p>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="form-group text-field-container">
          <label htmlFor="jobTitle" className="text-field-label">Job Title</label>
          {/* <input
            type="text"
            name="jobTitle"
            value={formData.jobTitle}
            onChange={handleChange}
            placeholder="Job Title"
            className={`text-field-input ${errors.jobTitle ? "input-error" : ""}`}
          /> */}

          <SpeechInput 
            type="text"
            name="jobTitle"
            value={formData.jobTitle}
            onChange={handleChange}
            placeholder="Job Title"
            className={`text-field-input ${errors.jobTitle ? "input-error" : ""}`}
          />
          {errors.jobTitle && <span className="error-text">{errors.jobTitle}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="jobDescription" className="text-field-label">Job Description</label>
          <textarea
            name="jobDescription"
            value={formData.jobDescription}
            onChange={handleChange}
            placeholder="Job Description"
            className={`text-field-input ${errors.jobDescription ? "input-error" : ""}`}
          />
          {errors.jobDescription && <span className="error-text">{errors.jobDescription}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="shortJobDescription" className="text-field-label">Short Job Description</label>
          <textarea
            name="shortJobDescription"
            value={formData.shortJobDescription}
            onChange={handleChange}
            placeholder="Short Job Description"
            className={`text-field-input ${errors.shortJobDescription ? "input-error" : ""}`}
          />
          {errors.shortJobDescription && <span className="error-text">{errors.shortJobDescription}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="city" className="text-field-label">City</label>
          <input
            type="text"
            name="city"
            value={formData.city}
            onChange={handleChange}
            placeholder="City"
            className={`text-field-input ${errors.city ? "input-error" : ""}`}
          />
          {errors.city && <span className="error-text">{errors.city}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="province" className="text-field-label">Province</label>
          <input
            type="text"
            name="province"
            value={formData.province}
            onChange={handleChange}
            placeholder="Province"
            className={`text-field-input ${errors.province ? "input-error" : ""}`}
          />
          {errors.province && <span className="error-text">{errors.province}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="zipCode" className="text-field-label">Zip Code</label>
          <input
            type="text"
            name="zipCode"
            value={formData.zipCode}
            onChange={handleChange}
            placeholder="Zip Code"
            className={`text-field-input ${errors.zipCode ? "input-error" : ""}`}
          />
          {errors.zipCode && <span className="error-text">{errors.zipCode}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="salaryRange" className="text-field-label">Salary Range</label>
          <select
            name="salaryRange"
            value={formData.salaryRange}
            onChange={handleChange}
            className={`text-field-input ${errors.salaryRange ? "input-error" : ""}`}
          >
            <option value="">Select Salary Range</option>
            <option value="30,000 - 50,000">$30,000 - $50,000</option>
            <option value="50,000 - 70,000">$50,000 - $70,000</option>
            <option value="70,000 - 90,000">$70,000 - $90,000</option>
            <option value="90,000 - 110,000">$90,000 - $110,000</option>
            <option value="110,000+">$110,000+</option>
          </select>
          {errors.salaryRange && <span className="error-text">{errors.salaryRange}</span>}
        </div>

         {/* Job Type */}
         <div className="form-group text-field-container">
            <label htmlFor="jobType" className="text-field-label">
              Job Type
            </label>
            <select
              name="jobType"
              value={formData.jobType}
              onChange={handleChange}
              className="text-field-input"
              required
            >
              <option value="">Select Type Of Job</option>
              <option value="part-time">Part - Time</option>
              <option value="full-time">Full - Time</option>
              <option value="flex">Flex Timing</option>
            </select>
        </div>

        <div className="form-group text-field-container">
            <label htmlFor="workType" className="text-field-label">
              Work Type
            </label>
            <select
              name="workType"
              value={formData.workType}
              onChange={handleChange}
              className="text-field-input"
              required
            >
              <option value="">Select Type Of Work</option>
              <option value="on-site">On-Site</option>
              <option value="remote">Remote</option>
              <option value="hybrid">Hybrid</option>
            </select>
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="termsAccepted">
            <input
              type="checkbox"
              name="termsAccepted"
              checked={formData.termsAccepted}
              onChange={handleChange}
            />
            Accept <Link to="#">Terms of Use</Link> and <Link to="#">Privacy Policy</Link>
          </label>
          {errors.termsAccepted && <span className="error-text">{errors.termsAccepted}</span>}
        </div>

        <button type="submit" className="btn-submit full-width" disabled={loading}>
          {loading ? "Submitting..." : "Submit"}
        </button>
      </form>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </div>
  );
};

export default PostJob;
