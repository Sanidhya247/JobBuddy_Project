import React from 'react';
import "../assets/css/About_us.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch, faShieldAlt, faFileAlt, faSmile, faUser, faBuilding, faList } from "@fortawesome/free-solid-svg-icons"; 
import aboutUsPage from '../assets/imgs/About_us.png'; 

const About = () => {
  return (
    <div>
      <section className="aboutpage_section">
        <div className="aboutcontent">
          <h1>ABOUT US</h1>
        </div>
      </section>

      <section className="content_section">
        <div className="image-container">
          <img src={aboutUsPage} alt="Team working" className="responsive-image" />
        </div>
        <div className="text-container">
          <h2>Welcome To JobBuddy</h2>
          <p>JobBuddy is an online career resource and platform created to help job seekers find that perfect job.
            They don't just match employees and employers but also provide tools for the users to make professional
            profiles and improve job applications. With such features as keeping users updated about suitable positions
            and helping them in their professional development, JobBuddy streamlines and speeds up the process of job hunting.</p>
        </div>
      </section>

      <div className="section-service">
      <section className="services-section">
        <h2>Our Services</h2>
        <div className="services-container">
          <div className="service-box">
          <FontAwesomeIcon icon={faSearch} className="fa-icon" />

            <h3>Search a Job</h3>
            <p>Effortlessly find your ideal job with advanced search features tailored to your skills.</p>
          </div>
          <div className="service-box">
          <FontAwesomeIcon icon={faShieldAlt} className="fa-icon" />

            <h3>Job Security</h3>
            <p>Explore job opportunities with stable and secure employment options.</p>
          </div>
          <div className="service-box">
          <FontAwesomeIcon icon={faFileAlt} className="fa-icon" />

            <h3>Build Your Resume</h3>
            <p>Create a professional resume to enhance your job applications and stand out.</p>
          </div>
          <div className="service-box">
          <FontAwesomeIcon icon={faSmile} className="fa-icon" />

            <h3>Happy Customer</h3>
            <p>Experience a job search platform designed to make your journey enjoyable and rewarding.</p>
          </div>
        </div>
      </section>
      </div>

    <section className="stats-section">
        <div className="stats-overlay"></div>
        <div className="stats-container">
            <div className="stat-box">
            <FontAwesomeIcon icon={faUser} className="fa-icon" />
                <h3>1967</h3>
                <p>Members</p>
            </div>
            <div className="stat-box">
            <FontAwesomeIcon icon={faList} className="fa-icon" />
                <h3>667</h3>
                <p>Jobs</p>
            </div>
            <div className="stat-box">
            <FontAwesomeIcon icon={faFileAlt} className="fa-icon" />
                <h3>475</h3>
                <p>Resumes</p>
            </div>
            <div className="stat-box">
            <FontAwesomeIcon icon={faBuilding} className="fa-icon" />
                <h3>475</h3>
                <p>Companies</p>
            </div>
        </div>
    </section>

    <section className="team-section">
        <h2>Our Team</h2>
        <div className="team-container">
            <div className="team-member">
                <img src={aboutUsPage} alt="Hemant" />
                <h3>Hemant Cherry</h3>
                <p>Web Developer</p>
            </div>
            <div className="team-member">
                <img src={aboutUsPage} alt="Hemant" />
                <h3>Sanidhya</h3>
                <p>Full Stack Developer</p>
            </div>
            <div className="team-member">
                <img src={aboutUsPage} alt="Hemant" />
                <h3>Zankhna Mayani</h3>
                <p>Front_End Developer</p>
            </div>
            <div className="team-member">
                <img src={aboutUsPage} alt="Hemant" />
                <h3>Tushar Dagar</h3>
                <p>Web Developer</p>
            </div>
        </div>
    </section>

    </div>
  );
};

export default About;
