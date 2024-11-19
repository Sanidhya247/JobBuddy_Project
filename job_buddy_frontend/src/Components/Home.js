import React from "react";
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

const Home = () => {
  return (
    <div>
      <div className="banner">
        <div className="banner-text">
          <h1>We help you find the right job</h1>
          <button className="search-btn">Search Job</button>
        </div>
        <div className="overlay"></div>
      </div>

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
      </div>
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
      <div className="top-recruiters-section">
        <h2>Top Recruiters</h2>
        <div className="top-recruiters">
          <div className="slider">
            <div className="slide-track">
              <div className="slide">
                <img src={BlackberryLogo} alt="Blackberry" />
              </div>
              <div className="slide">
                <img src={LogitechLogo} alt="Logitech" />
              </div>
              <div className="slide">
                <img src={TimberlandLogo} alt="Timberland" />
              </div>
              <div className="slide">
                <img src={AmazonLogo} alt="Amazon" />
              </div>
              <div className="slide">
                <img src={GoogleLogo} alt="Google" />
              </div>
              <div className="slide">
                <img src={GithubLogo} alt="GitHub" />
              </div>
              <div className="slide">
                <img src={MetaLogo} alt="Meta" />
              </div>
              <div className="slide">
                <img src={FordLogo} alt="Ford" />
              </div>
            </div>
          </div>
        </div>
      </div>
      
    </div>
  );
};

export default Home;
