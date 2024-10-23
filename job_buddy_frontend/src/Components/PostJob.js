import React, { useState } from "react";
import "../style.css";
import "../assets/css/post_job.css";
import Input from "./commons/Input";
import { Link } from "react-router-dom";

const PostJob = () => {
  const [formData, setFormData] = useState({
    jobTitle: "",
    jobDescription: "",
    location: "",
    employmentType: "",
    salaryRange: "",
    experienceLevel: "",
    jobCategory: "",
    minExperience: "",
    qualifications: "",
    preferredQualifications: "",
    remoteOption: "",
    requiredDocuments: "",
    companyWebsite: "",
    industry: "",
    interviewProcess: "",
    workingHours: "",
    applicationDeadline: "",
    termsAccepted: false,
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Form data submitted:", formData);
  };

  return (
    <div className="container">
      <div className="job-application-header">
        <h2>Post a Job</h2>
        <p>Please fill out the form below to create a job posting.</p>
      </div>

      <form onSubmit={handleSubmit}>
        {/* Job Title */}
        <div className="form-group">
          <Input
            type="text"
            className="text-field-input"
            name="jobTitle"
            id="jobTitle"
            value={formData.jobTitle}
            onChange={handleChange}
            placeholder="Job Title"
            required
          />
        </div>

        {/* Job Description */}
        <div className="form-group">
          <Input
            className="text-field-input"
            name="jobDescription"
            id="jobDescription"
            type="textarea"
            value={formData.jobDescription}
            onChange={handleChange}
            placeholder="Job Description"
            required
          />
        </div>

        {/* Location */}
        <div className="form-group">
          <Input
            type="text"
            className="text-field-input"
            name="location"
            id="location"
            value={formData.location}
            onChange={handleChange}
            placeholder="Location (City/Remote)"
            required
          />
        </div>

        {/* Employment Type */}
        <div className="form-group">
          <Input
            type="text"
            className="text-field-input"
            name="employmentType"
            id="employmentType"
            value={formData.employmentType}
            onChange={handleChange}
            placeholder="Full-time, Part-time"
            required
          />
        </div>

        {/* Salary Range */}
        <div className="form-group">
          <Input
            type="text"
            className="text-field-input"
            name="salaryRange"
            id="salaryRange"
            value={formData.salaryRange}
            onChange={handleChange}
            placeholder="Salary Range"
          />
        </div>

        {/* Minimum Years of Experience */}
        <div className="form-group">
          <Input
            type="number"
            className="text-field-input"
            name="minExperience"
            id="minExperience"
            value={formData.minExperience}
            onChange={handleChange}
            placeholder="Minimum Experience"
          />
        </div>

        {/* Experience Level */}
        <div className="form-group text-field-container">
          <label htmlFor="experienceLevel" className="text-field-label">Experience Level</label>
          <select
            name="experienceLevel"
            value={formData.experienceLevel}
            onChange={handleChange}
            className="text-field-input"
            required
          >
            <option value="">Select Experience Level</option>
            <option value="entry">Entry Level</option>
            <option value="mid">Mid Level</option>
            <option value="senior">Senior Level</option>
          </select>
        </div>

        {/* Company Website */}
        <div className="form-group">
          <Input
            type="text"
            className="text-field-input"
            name="companyWebsite"
            id="companyWebsite"
            value={formData.companyWebsite}
            onChange={handleChange}
            placeholder="Company Website URL"
          />
        </div>

        {/* Industry */}
        <div className="form-group">
          <Input
            id="industry"
            type="text"
            className="text-field-input"
            name="industry"
            value={formData.industry}
            onChange={handleChange}
            placeholder="Industry"
            required
          />
        </div>

        {/* Qualifications */}
        <div className="form-group">
          <Input
            className="text-field-input"
            name="qualifications"
            type="textarea"
            id="qualifications"
            value={formData.qualifications}
            onChange={handleChange}
            placeholder="Required Qualifications"
          />
        </div>

        {/* Preferred Qualifications */}
        <div className="form-group">
          <Input
            type="textarea"
            className="text-field-input"
            name="preferredQualifications"
            id="preferredQualifications"
            value={formData.preferredQualifications}
            onChange={handleChange}
            placeholder="Preferred Qualifications (Optional)"
          />
        </div>

        {/* Interview Process */}
        <div className="form-group">
          <Input
            className="text-field-input"
            name="interviewProcess"
            id="interviewProcess"
            value={formData.interviewProcess}
            onChange={handleChange}
            placeholder="Interview Process"
          />
        </div>

        {/* Application Deadline */}
        <div className="form-group">
          <Input
            type="date"
            placeholder = "Application Deadline"
            className="text-field-input"
            name="applicationDeadline"
            id="applicationDeadline"
            value={formData.applicationDeadline}
            onChange={handleChange}
            required
          />
        </div>

         {/* Remote or On-site */}
         <div className="form-radio">
          <label className="radio-label">Remote or On-site</label>
          <div className="radio-buttons">
            <label>
              <input
                type="radio"
                name="remoteOption"
                value="on-site"
                checked={formData.remoteOption === "on-site"}
                onChange={handleChange}
              />
              On-site
            </label>
            <label>
              <input
                type="radio"
                name="remoteOption"
                value="work-from-home"
                checked={formData.remoteOption === "work-from-home"}
                onChange={handleChange}
              />
              Work from Home
            </label>
            <label>
              <input
                type="radio"
                name="remoteOption"
                value="hybrid"
                checked={formData.remoteOption === "hybrid"}
                onChange={handleChange}
              />
              Hybrid
            </label>
          </div>
        </div>

        {/* Terms and Conditions */}
        <div className="form-group-checkbox">
          <input
            type="checkbox"
            name="termsAccepted"
            id="termsAccepted"
            checked={formData.termsAccepted}
            onChange={handleChange}
            required
          />
          <label>
            Accept <Link to="#">Terms of Use </Link> and{" "}
            <Link to="#">Privacy Policy</Link>
          </label>
        </div>

        {/* Submit Button */}
        <button type="submit" className="btn-submit full-width">
          Submit
        </button>
      </form>
    </div>
  );
};

export default PostJob;
