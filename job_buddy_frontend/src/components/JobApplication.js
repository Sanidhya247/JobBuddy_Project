import React, { useEffect, useState, useContext } from "react";
import apiService from "../utils/apiService";
import { toast } from "react-toastify";
import "../assets/css/job_application_page.css";
import { useLocation, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import SpeechInput from "./SpeechInput";

const JobApplication = () => {
  const location = useLocation();
  const { jobId } = location.state || {};
  const { user } = useContext(AuthContext);
  const userID = user?.userID;
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    dob: "",
    phone: "",
    linkedin: "",
    resume: null,
    coverLetter: "",
  });

  const [jobTitle, setJobTitle] = useState("");
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({});
  const [hasApplied, setHasApplied] = useState(false);

  useEffect(() => {
    const role = localStorage.getItem("role");
    if (role !== "Job Seeker") {
      toast.error("Employers cannot apply for jobs.");
      return;
    }

    const fetchJobDetails = async () => {
      try {
        const response = await apiService.get(`/api/JobListing/${jobId}`);
        setJobTitle(response.data.data.jobTitle);
      } catch (error) {
        toast.error("Failed to fetch job details.");
      }
    };

    const fetchUserProfile = async () => {
      try {
        const response = await apiService.get(`/api/UserProfile/${userID}`);
        const profile = response.data.data;
        if (!profile || !profile.fullName) {
          throw new Error("Profile or full name is missing.");
        }
        const [firstName, lastName = ""] = profile.fullName.split(" ", 2);

        setFormData((prev) => ({
          ...prev,
          firstName: firstName || "",
          lastName: lastName || "",
          email: profile.email,
          dob: profile.dateOfBirth?.split("T")[0] || "",
          phone: profile.phoneNumber || "",
          linkedin: profile.linkedInUrl || "",
        }));
      } catch (error) {
        console.error("Error fetching user profile:", error);
        toast.error("Failed to fetch user profile details.");
      }
    };

    fetchUserProfile();
    fetchJobDetails();
  }, [userID, jobId]);

  const handleChange = (e) => {
    const { name, value, files } = e.target;
    setFormData({
      ...formData,
      [name]: files ? files[0] : value,
    });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.resume) newErrors.resume = "Resume is required.";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    setLoading(true);

    try {
      const existingResumesResponse = await apiService.get(
        `/api/resume/${userID}`
      );
      if (
        existingResumesResponse.data.success &&
        existingResumesResponse.data.data
      ) {
        const existingResumes = existingResumesResponse.data.data;
        const existingResume = existingResumes.find(
          (resume) => resume.title === jobTitle
        );

        if (existingResume) {
          const applicationData = {
            jobID: jobId,
            userID: userID,
            firstName: formData.firstName,
            lastName: formData.lastName,
            email: formData.email,
            dob: formData.dob,
            phone: formData.phone,
            linkedin: formData.linkedin,
            resumeID: existingResume.resumeID,
            coverLetter: formData.coverLetter,
          };

          await apiService.post("/api/applications", applicationData);
          toast.success("Application submitted successfully!");
          setHasApplied(true);
          setLoading(false);
          navigate("/job");
          return;
        }
      }

      // Upload the resume
      const resumeData = new FormData();
      resumeData.append("resumeFile", formData.resume);
      resumeData.append("title", jobTitle);
      resumeData.append("userID", userID);

      const resumeResponse = await apiService.post(
        "/api/resume/upload",
        resumeData
      );
      const resumeId = resumeResponse.data.data.resumeID;

      const applicationData = {
        jobId: jobId,
        userId: userID,
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        dob: formData.dob,
        phone: formData.phone,
        linkedin: formData.linkedin,
        resumeId: resumeId,
        coverLetter: formData.coverLetter,
      };

      await apiService.post("/api/applications", applicationData);
      toast.success("Application submitted successfully!");
      setHasApplied(true);
    } catch (error) {
      if (error.response && error.response.data && error.response.data.errors) {
        const apiErrors = error.response.data.errors;
        if (apiErrors.Linkedin) {
          toast.error("LinkedIn URL must be valid.");
        }
        if (apiErrors.ResumeID) {
          toast.error("Resume ID must be a positive number.");
        }
      } else {
        toast.error("Failed to submit application.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="job_application">
      <h2>Job Application Form</h2>
      <form onSubmit={handleSubmit} className="job-application-form">
        <div className="form-group text-field-container">
          <label htmlFor="firstName" className="text-field-label">
            First Name
          </label>
          <SpeechInput
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            className="text-field-input"
            ariaLabel="Enter your first name"
            title="First Name"
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="lastName" className="text-field-label">
            Last Name
          </label>
          <SpeechInput
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            className="text-field-input"
            ariaLabel="Enter your last name"
            title="Last Name"
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="email" className="text-field-label">
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className="text-field-input"
            aria-label="Enter your email address"
            title="Email Address"
            readOnly
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="dob" className="text-field-label">
            Date of Birth
          </label>
          <input
            type="date"
            id="dob"
            name="dob"
            value={formData.dob}
            onChange={handleChange}
            className="text-field-input"
            aria-label="Enter your date of birth"
            title="Date of Birth"
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="phone" className="text-field-label">
            Phone
          </label>
          <SpeechInput
            type="text"
            id="phone"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            className="text-field-input"
            ariaLabel="Enter your phone number"
            title="Phone Number"
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="linkedin" className="text-field-label">
            LinkedIn Address (Optional)
          </label>
          <input
            type="text"
            id="linkedin"
            name="linkedin"
            value={formData.linkedin}
            onChange={handleChange}
            className="text-field-input"
            aria-label="Enter your LinkedIn profile URL"
            title="LinkedIn Profile"
          />
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="resume" className="text-field-label">
            Upload Resume
          </label>
          <input
            type="file"
            id="resume"
            name="resume"
            onChange={handleChange}
            className="text-field-input"
            aria-label="Upload your resume file"
            title="Resume Upload"
          />
          {errors.resume && <span className="error-text">{errors.resume}</span>}
        </div>
        <div className="form-group text-field-container">
          <label htmlFor="coverLetter" className="text-field-label">
            Cover Letter (Optional)
          </label>
          <textarea
            id="coverLetter"
            name="coverLetter"
            value={formData.coverLetter}
            onChange={handleChange}
            className="text-field-input"
            aria-label="Enter your cover letter"
            title="Cover Letter"
          />
        </div>
        <button
          type="submit"
          className="btn-submit full-width"
          disabled={loading || hasApplied}
          aria-label="Submit Job Application"
          title={loading ? "Submitting..." : "Submit Application"}
        >
          {loading ? "Submitting..." : hasApplied ? "Already Applied" : "Submit Application"}
        </button>
      </form>
    </div>
  );
};

export default JobApplication;
