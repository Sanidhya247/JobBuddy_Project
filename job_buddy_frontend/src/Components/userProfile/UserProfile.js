import React, { useEffect, useState, useContext } from "react";
import "../../assets/css/UserProfile.css";
import Loader from "../commons/Loader";
import { toast } from "react-toastify";
import EditProfile from "./EditProfile";
import apiService from "../../utils/apiService";
import AuthContext from "../../context/AuthContext";
import { CircularProgressbar, buildStyles } from "react-circular-progressbar";
import "react-circular-progressbar/dist/styles.css";

function UserProfile() {
  const { user } = useContext(AuthContext);
  const [profile, setProfile] = useState({});
  const [loading, setLoading] = useState(true);
  const [editMode, setEditMode] = useState(false);
  const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

  useEffect(() => {
    async function fetchProfile() {
      try {
        const response = await apiService.get(`/api/UserProfile/${user.userID}`);
        setProfile(response.data.data);
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
        <div className="profile-picture-wrapper">
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
          <h2>{profile.fullName}</h2>
          <p className="headline">{profile.headline}</p>
          <p className="email">{profile.email}</p>
          <button className="edit-profile-btn" onClick={() => setEditMode(!editMode)}>
            {editMode ? "Cancel" : "Edit Profile"}
          </button>
        </div>
        <div className="profile-completeness-wrapper">
          <CircularProgressbar
            value={profile.profileCompletenessPercentage}
            text={`${Math.round(profile.profileCompletenessPercentage)}%`}
            styles={buildStyles({
              pathColor: profile.profileCompletenessPercentage === 100 ? "green" : "orange",
              textColor: "#0366d6",
              trailColor: "#d6d6d6",
            })}
          />
        </div>
      </div>
      {editMode ? (
        <EditProfile profile={profile} setProfile={setProfile} userId={user.id} setEditMode={setEditMode} />
      ) : (
        <div className="profile-details">
          <div className="detail-card">
            <h3>About</h3>
            <p>{profile.about || "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Contact Info</h3>
            <p><strong>Address:</strong> {profile.address || "N/A"}</p>
            <p><strong>Nationality:</strong> {profile.nationality || "N/A"}</p>
            <p><strong>LinkedIn:</strong> <a href={profile.linkedInUrl}>{profile.linkedInUrl}</a></p>
            <p><strong>Date of Birth:</strong> {profile.dateOfBirth ? new Date(profile.dateOfBirth).toLocaleDateString() : "N/A"}</p>
          </div>
          <div className="detail-card">
            <h3>Education</h3>
            {profile.educations && profile.educations.length > 0 ? (
              profile.educations.map((edu) => (
                <div key={edu.userEducationID}>
                  <p><strong>Degree:</strong> {edu.degree}</p>
                  <p><strong>Institution:</strong> {edu.institution}</p>
                  <p><strong>Graduation Date:</strong> {new Date(edu.graduationDate).toLocaleDateString()}</p>
                </div>
              ))
            ) : (
              <p>No education details available.</p>
            )}
          </div>
          <div className="detail-card">
            <h3>Experience</h3>
            {profile.experiences && profile.experiences.length > 0 ? (
              profile.experiences.map((exp) => (
                <div key={exp.userExperienceID}>
                  <p><strong>Job Title:</strong> {exp.jobTitle}</p>
                  <p><strong>Company:</strong> {exp.company}</p>
                  <p><strong>Start Date:</strong> {new Date(exp.startDate).toLocaleDateString()}</p>
                  <p><strong>End Date:</strong> {exp.endDate ? new Date(exp.endDate).toLocaleDateString() : "Present"}</p>
                </div>
              ))
            ) : (
              <p>No experience details available.</p>
            )}
          </div>
          <div className="detail-card">
            <h3>Projects</h3>
            {profile.projects && profile.projects.length > 0 ? (
              profile.projects.map((proj) => (
                <div key={proj.userProjectID}>
                  <p><strong>Title:</strong> {proj.projectTitle}</p>
                  <p><strong>Description:</strong> {proj.description}</p>
                </div>
              ))
            ) : (
              <p>No project details available.</p>
            )}
          </div>
          <div className="detail-card">
            <h3>Certifications</h3>
            {profile.certifications && profile.certifications.length > 0 ? (
              profile.certifications.map((cert) => (
                <div key={cert.userCertificationID}>
                  <p><strong>Title:</strong> {cert.title}</p>
                  <p><strong>Issued By:</strong> {cert.issuedBy}</p>
                </div>
              ))
            ) : (
              <p>No certifications available.</p>
            )}
          </div>
        </div>
      )}
    </div>
  );
}

export default UserProfile;
