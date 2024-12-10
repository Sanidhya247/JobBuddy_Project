import React from "react";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import "./Auth.css";

const VerifyEmail = () => {

  // Function to handle resend email (pending for sprint-2)
  const handleResendEmail = () => {
    toast.info("Verification email resent. Please check your inbox.");
  };

  return (
    <div className="auth-container">
      <h2>Email Verification Required</h2>
      <p className="verify-email-text">
        Thank you for registering! We've sent a verification link to your email address. 
        Please check your inbox and follow the link to verify your account.
       <h1>(For development purposes, Please login directly, No verification required.)</h1> 
      </p>
      
      <div className="auth-form">
        <button onClick={handleResendEmail} className="btn-submit resend-button">
          Resend Verification Email
        </button>

        <p>
          Already verified? <Link to="/login" className="switch-auth-link">Log In</Link>
        </p>
      </div>
    </div>
  );
};

export default VerifyEmail;
