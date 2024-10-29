import React, { useState } from "react";
import "../style.css";
import "../assets/css/post_job.css";
import Input from "./commons/Input";
import { Link } from "react-router-dom";

const JobApplication = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    city: "",
    province: "",
    resume: null,
    coverLetter: "",
    jobType: "",
    workType: "",
    termsAccepted: false,
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const handleChange = (e) => {
    const { name, value, type, checked, files } = e.target;
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : files ? files[0] : value,
    });
    setErrors((prevErrors) => ({ ...prevErrors, [name]: "" })); // Clear error on change
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.firstName) newErrors.firstName = "First name is required.";
    if (!formData.lastName) newErrors.lastName = "Last name is required.";
    if (!formData.email) newErrors.email = "Email is required.";
    if (!formData.phone) newErrors.phone = "Phone number is required.";
    if (!formData.city) newErrors.city = "City is required.";
    if (!formData.province) newErrors.province = "Province is required.";
    if (!formData.resume) newErrors.resume = "Resume is required.";
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

    const formDataObj = new FormData();
    Object.keys(formData).forEach((key) => formDataObj.append(key, formData[key]));

    try {
      const response = await fetch("https://localhost:7113/api/JobApplication", {
        method: "POST",
        body: formDataObj,
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to submit application.");
      }

      setSuccessMessage("Application submitted successfully!");
      setFormData({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        city: "",
        province: "",
        resume: null,
        coverLetter: "",
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
        <h2>Apply for a Job</h2>
        <p>Please fill out the form below to apply for the position.</p>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="form-group text-field-container">
          <label htmlFor="firstName" className="text-field-label">First Name</label>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            placeholder="First Name"
            className={`text-field-input ${errors.firstName ? "input-error" : ""}`}
          />
          {errors.firstName && <span className="error-text">{errors.firstName}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="lastName" className="text-field-label">Last Name</label>
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            placeholder="Last Name"
            className={`text-field-input ${errors.lastName ? "input-error" : ""}`}
          />
          {errors.lastName && <span className="error-text">{errors.lastName}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="email" className="text-field-label">Email</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="Email"
            className={`text-field-input ${errors.email ? "input-error" : ""}`}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="phone" className="text-field-label">Phone</label>
          <input
            type="tel"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            placeholder="Phone Number"
            className={`text-field-input ${errors.phone ? "input-error" : ""}`}
          />
          {errors.phone && <span className="error-text">{errors.phone}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="resume" className="text-field-label">Upload Resume</label>
          <input
            type="file"
            name="resume"
            onChange={handleChange}
            className={`text-field-input ${errors.resume ? "input-error" : ""}`}
          />
          {errors.resume && <span className="error-text">{errors.resume}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="coverLetter" className="text-field-label">Cover Letter (Optional)</label>
          <textarea
            name="coverLetter"
            value={formData.coverLetter}
            onChange={handleChange}
            placeholder="Cover Letter"
            className="text-field-input"
          />
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="jobType" className="text-field-label">Job Type</label>
          <select
            name="jobType"
            value={formData.jobType}
            onChange={handleChange}
            className={`text-field-input ${errors.jobType ? "input-error" : ""}`}
          >
            <option value="">Select Job Type</option>
            <option value="full-time">Full-Time</option>
            <option value="part-time">Part-Time</option>
            <option value="contract">Contract</option>
          </select>
          {errors.jobType && <span className="error-text">{errors.jobType}</span>}
        </div>

        <div className="form-group text-field-container">
          <label htmlFor="workType" className="text-field-label">Work Type</label>
          <select
            name="workType"
            value={formData.workType}
            onChange={handleChange}
            className={`text-field-input ${errors.workType ? "input-error" : ""}`}
          >
            <option value="">Select Work Type</option>
            <option value="on-site">On-Site</option>
            <option value="remote">Remote</option>
            <option value="hybrid">Hybrid</option>
          </select>
          {errors.workType && <span className="error-text">{errors.workType}</span>}
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
          {loading ? "Submitting..." : "Submit Application"}
        </button>
      </form>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </div>
  );
};

export default JobApplication;
