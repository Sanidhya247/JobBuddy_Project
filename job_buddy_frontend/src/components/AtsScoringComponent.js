import React, { useState } from 'react';
import apiService from '../utils/apiService';
import '../assets/css/Home.css';
import '../assets/css/ATSScoring.css';

const AtsScoringComponent = () => {
  const [resume, setResume] = useState(null);
  const [jobDescription, setJobDescription] = useState('');
  const [score, setScore] = useState(null);
  const [errorMessage, setErrorMessage] = useState('');

  const handleResumeUpload = (event) => {
    setResume(event.target.files[0]);
  };

  const handleJobDescriptionChange = (event) => {
    setJobDescription(event.target.value);
  };

  const handleSubmit = async () => {
    if (!resume || jobDescription.trim() === '') {
      setErrorMessage('Please upload a resume and enter a job description.');
      return;
    }

    setErrorMessage('');

    const formData = new FormData();
    formData.append('Resume', resume);
    formData.append('JobDescription', jobDescription);

    try {
      const response = await apiService.post('/api/atsscoring/score', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      console.log(response.data.score);
      setScore(Math.round(response.data.score));
    } catch (error) {
      setErrorMessage('Error calculating ATS score. Please try again later.');
    }
  };

  return (
    <div className="ats-scoring-container">
      <h2>Check Your ATS Score</h2>
      <div className="ats-form-group">
        <label htmlFor="jobDescription">Job Description:</label>
        <textarea
          id="jobDescription"
          value={jobDescription}
          onChange={handleJobDescriptionChange}
          placeholder="Paste the job description here..."
        />
      </div>
      <div className="ats-form-group">
        <label htmlFor="resumeUpload">Upload Your Resume:</label>
        <input
          type="file"
          id="resumeUpload"
          accept=".txt,.pdf,.doc,.docx"
          onChange={handleResumeUpload}
        />
      </div>
      {errorMessage && <div className="ats-error-message">{errorMessage}</div>}
      <button onClick={handleSubmit} className="ats-submit-button">Check ATS Score</button>
      {score !== null && (
        <div className="ats-score-result">
          <h3>Your ATS Score: {score}%</h3>
        </div>
      )}
    </div>
  );
};

export default AtsScoringComponent;
