import React from 'react';
import "../../assets/css/ProfileModal.css";

function ProfileModal({ profile, onClose }) {
    const BASE_URL = process.env.REACT_APP_API_URL || "https://localhost:7113";

    return (
        <div className="profile-modal">
            <div className="modal-content">
                <button className="close-button" onClick={onClose}>X</button>
                
                <img
                    src={profile.profilePictureUrl ? `${BASE_URL}${profile.profilePictureUrl}` : '/default-avatar.png'}
                    alt="Profile"
                    className="modal-profile-image"
                />
                <h2>{profile.fullName}</h2>
                <p><strong>Email:</strong> {profile.email}</p>
                <p><strong>Phone:</strong> {profile.phoneNumber}</p>
                <p><strong>Address:</strong> {profile.address}</p>
                <p><strong>Date of Birth:</strong> {new Date(profile.dateOfBirth).toLocaleDateString()}</p>
                <p><strong>Nationality:</strong> {profile.nationality}</p>
                <p><strong>LinkedIn:</strong> <a href={`https://www.linkedin.com/in/${profile.linkedInUrl}`} target="_blank" rel="noopener noreferrer">{profile.linkedInUrl}</a></p>
                <p><strong>About:</strong> {profile.about}</p>
            </div>
        </div>
    );
}

export default ProfileModal;
