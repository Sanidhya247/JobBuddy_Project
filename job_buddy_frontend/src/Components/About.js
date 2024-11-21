import React from 'react';
import '../assets/css/about_us.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBriefcase, faShieldAlt, faFileAlt, faSmile, faUsers,faUser, faBuilding, faClipboard } from '@fortawesome/free-solid-svg-icons';
import logo from '../assets/imgs/logos/logo-transparent.png'; 

const AboutUs = () => {
  return (
    <div>
      {/* About Us Section */}
      <section className="about-section">
        <div className="about-content">
          <h1>ABOUT US</h1>
        </div>
      </section>

      {/* Welcome to Jobb Section */}
      <section className="content-section">
        <div className="image-container">
          <img src={logo} alt="Job Buddy Logo" className="logo-style" />
        </div>
        <div className="text-container">
          <h2>Welcome To Job buddy</h2>
          <p>
            Job Buddy is your ultimate partner in navigating the job market. Whether you're looking to start your career, secure your dream job, or take the next big step, we are here to help. With personalized tools and resources, we make your journey smoother and more efficient. Job Buddy empowers you with the confidence to tackle challenges and create a brighter future in the professional world.
          </p>
        </div>
      </section>

      {/* Our Services Section */}
      <section className="services-section">
        <h2>Our Services</h2>
        <div className="services-container">
          {[
            { title: 'Search a Job', desc: 'Easily explore thousands of job opportunities tailored to your skills and preferences. Our advanced filters ensure that you find the perfect match for your career aspirations.', icon: faBriefcase },
            { title: 'Job Security', desc: 'Gain access to expert advice and resources to ensure long-term success in your career. Job Buddy is dedicated to helping you secure stable and rewarding employment.', icon: faShieldAlt },
            { title: 'Build Your Resume', desc: 'Create a professional and impactful resume with our step-by-step guidance. Highlight your achievements and stand out to potential employers.', icon: faFileAlt },
            { title: 'Happy Customer', desc: 'Join our community of satisfied users who have successfully found their dream jobs with Job Buddy. Your success is our priority, and we are here to celebrate it with you!', icon: faSmile },
          ].map((service, index) => (
            <div className="service-box" key={index}>
              <FontAwesomeIcon icon={service.icon} size="3x" className="service-icon" />
              <h3>{service.title}</h3>
              <p>{service.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Statistics Section */}
      <section className="stats-section">
        <div className="stats-overlay"></div>
        <div className="stats-container">
          {[
            { stat: '1967', label: 'Members', icon: faUsers },
            { stat: '667', label: 'Jobs', icon: faClipboard },
            { stat: '475', label: 'Resumes', icon: faFileAlt },
            { stat: '475', label: 'Companies', icon: faBuilding },
          ].map((statItem, index) => (
            <div className="stat-box" key={index}>
              <FontAwesomeIcon icon={statItem.icon} size="3x" className="stat-icon" />
              <h3>{statItem.stat}</h3>
              <p>{statItem.label}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Our Team Section */}
      <section className="team-section">
        <h2>Our Team</h2>
        <div className="team-container">
          {[
            { name: 'Hemant Cherry', role: 'Web Developer', icon: faUser },
            { name: 'Zankhna Mayani', role: 'Front-End Developer', icon: faUser },
            { name: 'Tushar Dagar', role: 'Web Developer', icon: faUser },
            { name: 'Sanidhya', role: 'Full Stack Developer', icon: faUser },
          ].map((teamMember, index) => (
            <div className="team-member" key={index}>
              <FontAwesomeIcon icon={teamMember.icon} size="5x" className="team-icon" />
              <h3>{teamMember.name}</h3>
              <p>{teamMember.role}</p>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
};

export default AboutUs;
