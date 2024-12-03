import React, { useState } from 'react';
import axios from 'axios';
import '../assets/css/contactus.css';
import contact_us from '../assets/imgs/contact_us.jpg';
import user from '../assets/imgs/user.png';

const Contact = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    subject: '',
    message: '',
  });

  const [responseMessage, setResponseMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('https://localhost:7113/api/contact/submit', formData);
      setResponseMessage(response.data); 
      setFormData({ name: '', email: '', subject: '', message: '' }); // Clear form
    } catch (error) {
      console.error(error);
      setResponseMessage('Failed to submit the form. Please try again.');
    }
  };

  return (
    <div className="contact-page">
      <div className="hero-section">
        <div className="hero-text">
          <h1>Get in touch</h1>
          <p>
            Got a question? "Reach Out to Us – Your JobBuddy is Here to Help!"
          </p>
        </div>
        <div className="hero-image">
          <img src={contact_us} alt="contact_us" />
          {/* Photo by MART  PRODUCTION from Pexels: https://www.pexels.com/photo/people-working-as-call-center-agents-7709296/ */}
        </div>
      </div>

      <div className="contact-options">
        <div className="option-card">
          <img src={user} alt="contact_us" />
          <h3>Job Seekers</h3>
          <p>
            Are you looking for job opportunities or career guidance? Reach out to us for support and resources.
          </p>
        </div>
        <div className="option-card">
          <img src={user} alt="contact_us" />
          <h3>Employers</h3>
          <p>
            Want to post job openings or find the right candidates? Get in touch with our team to help you recruit talent.
          </p>
        </div>
        <div className="option-card">
          <img src={user} alt="contact_us" />
          <h3>Partners</h3>
          <p>
            Interested in collaborating with us to empower job seekers and employers? Let's work together!
          </p>
        </div>
      </div>
      
      <div className="contact-form-container">
        <h2>Contact Us</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              placeholder="Enter your name"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              placeholder="Enter your email"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="subject">Subject</label>
            <input
              type="text"
              id="subject"
              name="subject"
              value={formData.subject}
              onChange={handleChange}
              placeholder="Enter the subject"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="message">Message</label>
            <textarea
              id="message"
              name="message"
              value={formData.message}
              onChange={handleChange}
              placeholder="Enter your message"
              rows="5"
              required
            ></textarea>
          </div>
          <button type="submit" className="submit-button">
            Submit
          </button>
        </form>
        {responseMessage && <p className="response-message">{responseMessage}</p>}
      </div>
    </div>
  );
};

export default Contact;
