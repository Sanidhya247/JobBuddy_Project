import React, { useEffect, useState, useContext } from "react";
import "../../assets/css/UserProfile.css";
import { toast } from "react-toastify";
import apiService from "../../utils/apiService";
import AuthContext from "../../context/AuthContext";
import { CircularProgressbar, buildStyles } from "react-circular-progressbar";
import "react-circular-progressbar/dist/styles.css";
import { Link } from "react-router-dom";
import EditProfile from "../userProfile/EditProfile";
import Loader from "../commons/Loader";
import EditEmployerProfile from "./EditEmployerProfile";

function EmployerProfile() {
  const { user } = useContext(AuthContext);
  const [profile, setProfile] = useState({});
  const [loading, setLoading] = useState(true);
  const [editMode, setEditMode] = useState(false);
  const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

  useEffect(() => {
    async function fetchProfile() {
      try {
        const response = await apiService.get(`/api/UserProfile/${user.userID}`);
        const userProfile = response.data.data;

        // Set profile and store isPremium in localStorage
        setProfile(userProfile);
        localStorage.setItem("isPremium", userProfile.isPremium);
      } catch (error) {
        toast.error("Error fetching profile. Please try again.");
      } finally {
        setLoading(false);
      }
    }
    fetchProfile();
  }, [user]);

  const handleFileChange = async (e, type) => {
    const file = e.target.files[0];
    if (file) {
      try {
        setLoading(true);
        const formData = new FormData();
        formData.append(type === "profile" ? "profilePicture" : "coverPhoto", file);

        const endpoint = `/api/UserProfile/${user.userID}/upload-${
          type === "profile" ? "profile-picture" : "cover-photo"
        }`;
        const response = await apiService.post(endpoint, formData);

        if (type === "profile") {
          setProfile({ ...profile, profilePictureUrl: response.data.data });
        } else {
          setProfile({ ...profile, coverPhotoUrl: response.data.data });
        }
        toast.success(`${type.charAt(0).toUpperCase() + type.slice(1)} updated successfully!`);
      } catch (error) {
        toast.error(`Error uploading ${type}. Please try again.`);
      } finally {
        setLoading(false);
      }
    }
  };

  if (loading) return <Loader />;

  return (
    <div className="user-profile">
      <div className="cover-photo-wrapper">
        <img
          src={profile.coverPhotoUrl ? `${BASE_URL}${profile.coverPhotoUrl}` : "/default-cover.jpg"}
          alt="Cover"
          className="cover-photo"
        />
        <label className="cover-upload-button">
          <input type="file" onChange={(e) => handleFileChange(e, "cover")} />
          <span>Change Cover</span>
        </label>
      </div>
      <div className="profile-header">
        <div
          className={`profile-picture-wrapper ${
            profile.isPremium ? "premium-border" : ""
          }`}
        >
          <img
            src={profile.profilePictureUrl ? `${BASE_URL}${profile.profilePictureUrl}` : "/default-avatar.png"}
            alt={`${profile.fullName}'s Avatar`}
            className="profile-picture"
          />
          <label className="profile-upload-button">
            <input type="file" onChange={(e) => handleFileChange(e, "profile")} />
            <span>✏️</span>
          </label>
        </div>
        <div className="profile-info">
          <div className="profile-header-row">
            <h2>{profile.fullName}</h2>
            {profile.isPremium ? (
              <button className="premium-user-badge" disabled>
                Premium User
              </button>
            ) : (
              <Link to="/subscription" className="subscribe-button">
                Subscribe Now
              </Link>
            )}
          </div>
          <p className="headline">{profile.headline}</p>
          <p className="email">{profile.email}</p>
          <button className="edit-profile-btn" onClick={() => setEditMode(!editMode)}>
            {editMode ? "Cancel" : "Edit Profile"}
          </button>
        </div>
      </div>
      {editMode ? (
        <EditEmployerProfile profile={profile} setProfile={setProfile} userId={user.userID} setEditMode={setEditMode} />
      ) : (
        <div className="profile-details">
          <div className="detail-card">
            <h3>Company Name</h3>
            <p>{profile.companyName || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Company Website</h3>
            <p>{profile.companyWebsite || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Contact Person or HR</h3>
            <p>{profile.contactPerson || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Contact Email</h3>
            <p>{profile.contactEmail || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Contact Number</h3>
            <p>{profile.phoneNumber || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3></h3>
            <p>{profile.officeAddress || "N/A"}</p>
          </div>

          <div className="detail-card">
            <h3>Office or Company Address</h3>
            <p>
              <strong>Address:</strong> {profile.officeAddress || "N/A"}
            </p>
            <p>
              <strong>City:</strong> {profile.city || "N/A"}
            </p>
            <p>
              <strong>Province:</strong> {profile.province}
            </p>
            <p>
              <strong>Postal Code:</strong> {profile.zipCode}
            </p>
          </div>
          
        </div>
      )}
    </div>
  );
}

export default EmployerProfile;
