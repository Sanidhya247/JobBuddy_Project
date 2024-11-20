import React, { useState } from 'react';
import AtsScoringComponent from './AtsScoringComponent';
import '../assets/css/Home.css';
import profileImage from '../assets/imgs/buildProfile.jpg';
import Modal from 'react-modal';

Modal.setAppElement('#root');  // Set the app element for accessibility

const Home = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const openModal = () => {
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

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
                    <h3>Check Your ATS Score</h3>
                    <p>
                        JobBuddy is not just a job search platform but also an extensive tool that helps job seekers improve their job applications.
                        Our ATS Score feature allows you to see how well your resume matches a specific job description.
                        This way, you can optimize your resume to improve your chances of getting noticed by recruiters.
                    </p>
                    <button className="create-btn" onClick={openModal}>Check ATS Score</button>
                </div>
            </div>

            <Modal
                isOpen={isModalOpen}
                onRequestClose={closeModal}
                contentLabel="ATS Scoring Modal"
                className="ats-modal"
                overlayClassName="ats-modal-overlay"
            >
                <div className="ats-modal-content">
                    <button onClick={closeModal} className="close-modal-btn">Close</button>
                    <AtsScoringComponent />
                </div>
            </Modal>
        </div>
    );
};

export default Home;
