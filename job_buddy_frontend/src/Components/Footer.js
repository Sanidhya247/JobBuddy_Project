import React from "react";
import "../assets/css/footer.css";

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="about-section">
          <h3>About</h3>
          <p>
            JobBuddy is an online career resource and platform created to help
            job seekers find that perfect job. They don’t just match employees
            and employers but also provide tools for the users to make
            professional profiles and improve job applications. With such
            features as keeping users updated about suitable positions and
            helping them in their professional development, JobBuddy streamlines
            and speeds up the process of job hunting.
          </p>
        </div>

        <div className="resources-section">
          <h3>Resources</h3>
          <ul>
            <li>Home</li>
            <li>About Us</li>
            <li>Job Search</li>
          </ul>
        </div>

        <div className="social-section">
          <h3>Social</h3>
          <div className="social-icons">
            <i className="fab fa-facebook"></i>
            <i className="fab fa-google"></i>
            <i className="fab fa-twitter"></i>
            <i className="fab fa-instagram"></i>
          </div>
        </div>
      </div>
      <div className="footer-bottom">
        <p>© 2024 - Jobbuddy</p>
      </div>
    </footer>
  );
};

export default Footer;
