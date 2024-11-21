import React, { useEffect, useState } from "react";
import "../assets/css/Home.css";
import profileImage from "../assets/imgs/buildProfile.jpg";
import BlackberryLogo from "../assets/imgs/logos/Blackberry-logo.png";
import LogitechLogo from "../assets/imgs/logos/Logitech-Logo.png";
import TimberlandLogo from "../assets/imgs/logos/Timberland-Logo.png";
import AmazonLogo from "../assets/imgs/logos/Amazon-Logo.png";
import GoogleLogo from "../assets/imgs/logos/Google-logo.png";
import GithubLogo from "../assets/imgs/logos/GitHub-logo.png";
import MetaLogo from "../assets/imgs/logos/Meta-Logo.png";
import FordLogo from "../assets/imgs/logos/Ford-Logo.png";
import Button from "./commons/Button";
import Modal from "react-modal";
import AtsScoringComponent from "./AtsScoringComponent";
import apiService from "../utils/apiService";
import JobDetailsCard from "./JobDetailsCard";

Modal.setAppElement("#root");

const Home = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const openModal = () => setIsModalOpen(true);
  const closeModal = () => setIsModalOpen(false);
  const [jobs,setJobs] = useState([]);
  const [error,setError] = useState("");

  const [currentIndex, setCurrentIndex] = useState(0);
  const jobsPerPage = 5;

  const handlePrevious = () => {
    setCurrentIndex((prevIndex) => Math.max(prevIndex - 1, 0));
  };

  const handleNext = () => {
    setCurrentIndex((prevIndex) =>
      Math.min(prevIndex + 1, Math.ceil(jobs.length / jobsPerPage) - 1)
    );
  };

  const start = currentIndex * jobsPerPage;
  const end = start + jobsPerPage;
  const visibleJobs = jobs.slice(start, end);


  const fetchJobs = async() => {
    try {
      const response = await apiService.get("/api/JobListing/");
      setJobs(response.data.data || []);
    } catch (error) {
      setError("Error fetching jobs. Please try again later.");
    }
  };

  useEffect(() => {
    fetchJobs();
  },[])

  return (
    <div>
      {/* Banner Section */}
      <div className="banner">
        <div className="banner-text">
          <h1>We help you find the right job</h1>
          <button className="search-btn">Search Job</button>
        </div>
        <div className="overlay"></div>
      </div>

      <section className="explore-jobs">
      <h2>Explore Jobs</h2>
      <div className="job-cards">
        {visibleJobs.map((job, index) => (
          <JobDetailsCard job={job}/>
        ))}
      </div>
      <div className="pagination-buttons">
        <button
          className="previous-button"
          onClick={handlePrevious}
          disabled={currentIndex === 0}
        >
          Previous
        </button>
        <button
          className="next-button"
          onClick={handleNext}
          disabled={end >= jobs.length}
        >
          Next
        </button>
      </div>
    </section>

      {/* Profile Section - ATS Score */}
      <div className="profile-section">
        <div className="profile-image-container">
          <div className="background-div"></div>
          <img
            src={profileImage}
            alt="Profile Builder"
            className="profile-image"
          />
        </div>
        <div className="profile-text">
          <h3>Check Your ATS Score</h3>
          <p>
            JobBuddy is not just a job search platform but also an extensive
            tool that helps job seekers improve their job applications. Our ATS
            Score feature allows you to see how well your resume matches a
            specific job description. This way, you can optimize your resume to
            improve your chances of getting noticed by recruiters.
          </p>
          <button className="create-btn" onClick={openModal}>
            Check ATS Score
          </button>
        </div>
      </div>

      {/* ATS Modal */}
      <Modal
        isOpen={isModalOpen}
        onRequestClose={closeModal}
        contentLabel="ATS Scoring Modal"
        className="ats-modal"
        overlayClassName="ats-modal-overlay"
      >
        <div className="ats-modal-content">
          <button onClick={closeModal} className="close-modal-btn">
            Close
          </button>
          <AtsScoringComponent />
        </div>
      </Modal>

      {/* Profile Section - Build Your Profile
      <div className="profile-section">
        <div className="profile-image-container">
          <div className="background-div"></div>
          <img
            src={profileImage}
            alt="Profile Builder"
            className="profile-image"
          />
        </div>
        <div className="profile-text">
          <h3>Build Your Profile</h3>
          <p>
            JobBuddy is an online career resource and platform created to help
            job seekers find the perfect job. They don’t just match employees
            and employers but also provide tools for users to make professional
            profiles and improve their job applications. With features like
            keeping users updated about suitable positions and assisting in
            their professional development, JobBuddy streamlines and speeds up
            the process of job hunting.
          </p>
          <button className="create-btn">Create</button>
        </div>
      </div> */}

      {/* Numbers Section */}
      <div className="numbers-container">
        <h1 className="numbers-heading">
          <span className="logo-txt"> JobBuddy </span> by the numbers
        </h1>
        <div className="stats-container">
          <div className="stat-card">
            <p className="stat-number">105,000</p>
            <p className="stat-description">
              Job postings advertised (monthly average)
            </p>
          </div>
          <div className="stat-card">
            <p className="stat-number">14.7 million</p>
            <p className="stat-description">
              Job postings views (monthly average)
            </p>
          </div>
          <div className="stat-card">
            <p className="stat-number">300,000</p>
            <p className="stat-description">Employers already registered</p>
          </div>
        </div>
        <Button label={"Learn more"} className={"btn-submit"} />
        <p className="date-modified">Date modified: 2024-09-21</p>
      </div>

      {/* Top Recruiters Section */}
      <div className="top-recruiters-section">
        <h2>Top Recruiters</h2>
        <div className="top-recruiters">
          <div className="slider">
            <div className="slide-track">
              {[BlackberryLogo, LogitechLogo, TimberlandLogo, AmazonLogo, GoogleLogo, GithubLogo, MetaLogo, FordLogo].map(
                (logo, index) => (
                  <div className="slide" key={index}>
                    <img src={logo} alt={`Logo ${index + 1}`} />
                  </div>
                )
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
