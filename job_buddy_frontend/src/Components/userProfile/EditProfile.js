import React, { useState } from 'react';
import { FaChevronDown, FaChevronUp, FaPlus } from 'react-icons/fa';
import Loader from '../commons/Loader';
import { toast } from 'react-toastify';
import '../../assets/css/EditProfile.css';
import apiService from '../../utils/apiService';

function EditProfile({ profile, setProfile, userId, setEditMode }) {
  const [formData, setFormData] = useState({
    ...profile,
    educations: profile.educations || [{}],
    experience: profile.experience || [{}],
    projects: profile.projects || [{}],
    certifications: profile.certifications || [{}],
  });
  const [loading, setLoading] = useState(false);

  const [expandSections, setExpandSections] = useState({
    education: true,
    experience: false,
    projects: false,
    certifications: false,
  });

  const toggleSection = (section) => {
    setExpandSections((prev) => ({
      ...prev,
      [section]: !prev[section],
    }));
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSectionChange = (section, index, field, value) => {
    const updatedSection = formData[section].map((item, idx) =>
      idx === index ? { ...item, [field]: value } : item
    );
    setFormData({ ...formData, [section]: updatedSection });
  };

  const addNewEntry = (section) => {
    setFormData({
      ...formData,
      [section]: [...formData[section], {}],
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      await apiService.put(`/api/UserProfile/${userId}/update-profile`, formData);
      setProfile(formData);
      setEditMode(false);
      toast.success('Profile updated successfully!');
    } catch (error) {
      toast.error('Error updating profile. Please try again.');
      console.error('Error updating profile:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <form className="edit-profile-form" onSubmit={handleSubmit}>
      {/* Basic Information */}
      <label>
        Full Name:
        <input type="text" name="fullName" value={formData.fullName || ''} onChange={handleInputChange} />
      </label>
      <label>
        Email:
        <input type="email" name="email" value={formData.email || ''} onChange={handleInputChange} disabled />
      </label>
      <label>
        Address:
        <input type="text" name="address" value={formData.address || ''} onChange={handleInputChange} />
      </label>

      {/* Education Section */}
      <div className="toggle-section" onClick={() => toggleSection('education')}>
        <h3>Education {expandSections.education ? <FaChevronUp /> : <FaChevronDown />}</h3>
      </div>
      {expandSections.education &&
        formData.educations.map((education, index) => (
          <div key={index} className="education-entry">
            <label>
              Degree:
              <input
                type="text"
                value={education.degree || ''}
                onChange={(e) => handleSectionChange('educations', index, 'degree', e.target.value)}
              />
            </label>
            <label>
              Institution:
              <input
                type="text"
                value={education.institution || ''}
                onChange={(e) => handleSectionChange('educations', index, 'institution', e.target.value)}
              />
            </label>
            <label>
              Graduation Date:
              <input
                type="date"
                value={education.graduationDate || ''}
                onChange={(e) => handleSectionChange('educations', index, 'graduationDate', e.target.value)}
              />
            </label>
          </div>
        ))}
      <button type="button" className="add-btn" onClick={() => addNewEntry('educations')}>
        <FaPlus /> Add Education
      </button>

      {/* Experience Section */}
      <div className="toggle-section" onClick={() => toggleSection('experience')}>
        <h3>Experience {expandSections.experience ? <FaChevronUp /> : <FaChevronDown />}</h3>
      </div>
      {expandSections.experience &&
        formData.experience.map((experience, index) => (
          <div key={index} className="experience-entry">
            <label>
              Job Title:
              <input
                type="text"
                value={experience.jobTitle || ''}
                onChange={(e) => handleSectionChange('experience', index, 'jobTitle', e.target.value)}
              />
            </label>
            <label>
              Company:
              <input
                type="text"
                value={experience.company || ''}
                onChange={(e) => handleSectionChange('experience', index, 'company', e.target.value)}
              />
            </label>
            <label>
              Start Date:
              <input
                type="date"
                value={experience.startDate || ''}
                onChange={(e) => handleSectionChange('experience', index, 'startDate', e.target.value)}
              />
            </label>
            <label>
              End Date:
              <input
                type="date"
                value={experience.endDate || ''}
                onChange={(e) => handleSectionChange('experience', index, 'endDate', e.target.value)}
              />
            </label>
          </div>
        ))}
      <button type="button" className="add-btn" onClick={() => addNewEntry('experience')}>
        <FaPlus /> Add Experience
      </button>

      {/* Projects Section */}
      <div className="toggle-section" onClick={() => toggleSection('projects')}>
        <h3>Projects {expandSections.projects ? <FaChevronUp /> : <FaChevronDown />}</h3>
      </div>
      {expandSections.projects &&
        formData.projects.map((project, index) => (
          <div key={index} className="project-entry">
            <label>
              Project Name:
              <input
                type="text"
                value={project.projectName || ''}
                onChange={(e) => handleSectionChange('projects', index, 'projectName', e.target.value)}
              />
            </label>
            <label>
              Description:
              <textarea
                value={project.description || ''}
                onChange={(e) => handleSectionChange('projects', index, 'description', e.target.value)}
              />
            </label>
          </div>
        ))}
      <button type="button" className="add-btn" onClick={() => addNewEntry('projects')}>
        <FaPlus /> Add Project
      </button>

      {/* Certifications Section */}
      <div className="toggle-section" onClick={() => toggleSection('certifications')}>
        <h3>Certifications {expandSections.certifications ? <FaChevronUp /> : <FaChevronDown />}</h3>
      </div>
      {expandSections.certifications &&
        formData.certifications.map((certification, index) => (
          <div key={index} className="certification-entry">
            <label>
              Certification Name:
              <input
                type="text"
                value={certification.certificationName || ''}
                onChange={(e) => handleSectionChange('certifications', index, 'certificationName', e.target.value)}
              />
            </label>
            <label>
              Issued By:
              <input
                type="text"
                value={certification.issuedBy || ''}
                onChange={(e) => handleSectionChange('certifications', index, 'issuedBy', e.target.value)}
              />
            </label>
            <label>
              Date Issued:
              <input
                type="date"
                value={certification.dateIssued || ''}
                onChange={(e) => handleSectionChange('certifications', index, 'dateIssued', e.target.value)}
              />
            </label>
          </div>
        ))}
      <button type="button" className="add-btn" onClick={() => addNewEntry('certifications')}>
        <FaPlus /> Add Certification
      </button>

      <button type="submit" className="save-profile-btn">
        Save Changes
      </button>
    </form>
  );
}

export default EditProfile;
