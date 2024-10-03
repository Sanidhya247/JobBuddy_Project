import React from 'react';
import '../assets/css/Home.css';
import profileImage from '../assets/imgs/buildProfile.jpg'

const Home = () => {
    return (
        <div>
            <div className="banner">
                <div className="banner-text">
                    <h1>We help you find the right job</h1>
                    <button className="search-btn">Search Job</button>
                </div>
                <div className="overlay"></div>
            </div>

            <div className="profile-section">
                <div className="profile-image-container">
                    <div className="background-div"></div>
                    <img src={profileImage} alt="Profile Builder" className="profile-image" />
                </div>

                <div className="profile-text">
                    <h3>Build Your Profile</h3>
                    <p>
                        JobBuddy is an online career resource and platform created to help job seekers find the perfect job. 
                        They don’t just match employees and employers but also provide tools for users to make professional 
                        profiles and improve their job applications. With features like keeping users updated about suitable 
                        positions and assisting in their professional development, JobBuddy streamlines and speeds up the 
                        process of job hunting.
                    </p>
                    <button className="create-btn">Create</button>
                </div>
            </div>
        </div>
    );
};

export default Home;
