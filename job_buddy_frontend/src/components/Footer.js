import React from "react";
import "../assets/css/footer.css";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faGoogle,
  faTwitter,
  faInstagram,
  faFacebookF,
} from "@fortawesome/free-brands-svg-icons";

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="footer-about-section">
          <h3>About</h3>
          <p>
            JobBuddy is an online career resource and platform created to help
            job seekers find that perfect job. They don't just match employees
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
            <li>
              <Link className="nav-link" to="/" aria-label="Navigate to Home">
                Home
              </Link>
            </li>
            <li>
              <Link className="nav-link" to="/Job" aria-label="Navigate to Jobs">
                Jobs
              </Link>
            </li>
            <li>
              <Link className="nav-link" to="/about" aria-label="Navigate to About">
                About
              </Link>
            </li>
            <li>
              <Link className="nav-link" to="/contact" aria-label="Navigate to Contact">
                Contact
              </Link>
            </li>
            <li>
              <Link className="nav-link" to="/post" aria-label="Navigate to Post Job">
                Post Job
              </Link>
            </li>
          </ul>
        </div>

        <div className="social-section">
          <h3>Social</h3>
          <div className="social-icons">
            <Link 
              to={"https://www.facebook.com"} 
              target="_blank" 
              aria-label="Visit our Facebook page"
            >
              <FontAwesomeIcon icon={faFacebookF} />
            </Link>
            <Link 
              to={"https://www.google.com"} 
              target="_blank" 
              aria-label="Visit our Google page"
            >
              <FontAwesomeIcon icon={faGoogle} />
            </Link>
            <Link 
              to={"https://www.twitter.com"} 
              target="_blank" 
              aria-label="Visit our Twitter profile"
            >
              <FontAwesomeIcon icon={faTwitter} />
            </Link>
            <Link 
              to={"https://www.instagram.com"} 
              target="_blank" 
              aria-label="Visit our Instagram profile"
            >
              <FontAwesomeIcon icon={faInstagram} />
            </Link>
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
