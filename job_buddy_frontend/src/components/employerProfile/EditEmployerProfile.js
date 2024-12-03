import React, { useState } from 'react';
import Loader from '../commons/Loader';
import { toast } from 'react-toastify';
import '../../assets/css/EditProfile.css';
import apiService from '../../utils/apiService';

function EditEmployerProfile({ profile, setProfile, userId, setEditMode }) {
  const [formData, setFormData] = useState({
    companyName: profile.companyName || '',
    companyWebsite: profile.companyWebsite || '',
    contactPerson: profile.contactPerson || '',
    contactEmail: profile.contactEmail || '',
    phoneNumber: profile.phoneNumber || '',
    officeAddress: profile.officeAddress || '',
    city: profile.city || '',
    province: profile.province || '',
    zipCode: profile.zipCode || '',
  });

  const [loading, setLoading] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      // Updated API endpoint to api/EmployerProfile
      const response = await apiService.post(`/api/EmployerProfile`, formData);
      setProfile((prevProfile) => ({
        ...prevProfile,
        ...formData,
      }));
      setEditMode(false);
      toast.success('Profile updated successfully!');
    } catch (error) {
      toast.error('Error updating profile. Please try again.');
      console.error('Error updating profile:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <Loader />;

  return (
    <form className="edit-profile-form" onSubmit={handleSubmit}>
      {/* Company Information */}
      <label>
        Company Name:
        <input type="text" name="companyName" value={formData.companyName} onChange={handleInputChange} />
      </label>
      <label>
        Company Website:
        <input type="url" name="companyWebsite" value={formData.companyWebsite} onChange={handleInputChange} />
      </label>
      <label>
        Contact Person:
        <input type="text" name="contactPerson" value={formData.contactPerson} onChange={handleInputChange} />
      </label>
      <label>
        Contact Email:
        <input type="email" name="contactEmail" value={formData.contactEmail} onChange={handleInputChange} />
      </label>
      <label>
        Phone Number:
        <input type="text" name="phoneNumber" value={formData.phoneNumber} onChange={handleInputChange} />
      </label>
      <label>
        Office Address:
        <input type="text" name="officeAddress" value={formData.officeAddress} onChange={handleInputChange} />
      </label>
      <label>
        City:
        <input type="text" name="city" value={formData.city} onChange={handleInputChange} />
      </label>
      <label>
        Province:
        <input type="text" name="province" value={formData.province} onChange={handleInputChange} />
      </label>
      <label>
        Zip Code:
        <input type="text" name="zipCode" value={formData.zipCode} onChange={handleInputChange} />
      </label>

      <button type="submit" className="save-profile-btn">
        Save Changes
      </button>
    </form>
  );
}

export default EditEmployerProfile;
