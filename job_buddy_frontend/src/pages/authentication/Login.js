import React, { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../../context/AuthContext";
import Toaster from "../../components/commons/Toaster"; 
import "./Auth.css";

const Login = () => {
  const { login, setError } = useContext(AuthContext);
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [localError, setLocalError] = useState(""); 
  const [fieldErrors, setFieldErrors] = useState({}); 
  const [showToaster, setShowToaster] = useState(false); 

  // Form submission handler
  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent default form submission

    const errors = validateInputs(); // Validate inputs
    if (Object.keys(errors).length > 0) {
      setFieldErrors(errors);
      return;
    }

    try {
      await login({ email, password }); // Call the login function from context
      setShowToaster(true); // Show success toaster when login is successful
      setTimeout(() => {
        navigate("/home"); 
      }, 2000); 
    } catch (err) {
      const errorMessage = err.message || "Invalid email or password. Please try again.";
      setLocalError(errorMessage);
      setError(errorMessage); 
      setFieldErrors({});
    }
  };

  // Validate input fields
  const validateInputs = () => {
    const errors = {};
    if (!email) {
      errors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      errors.email = "Invalid email address.";
    }

    if (!password) {
      errors.password = "Password is required.";
    }

    return errors;
  };

  return (
    <div className="auth-container">
      <h2>Sign In</h2>
      {localError && <p className="error">{localError}</p>}
      <form onSubmit={handleSubmit} className="auth-form">
        <div className="form-group">
          <label>Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className={fieldErrors.email ? "error-input" : ""}
          />
          {fieldErrors.email && <span className="error-text">{fieldErrors.email}</span>}
        </div>
        <div className="form-group">
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className={fieldErrors.password ? "error-input" : ""} 
          />
          {fieldErrors.password && <span className="error-text">{fieldErrors.password}</span>}
        </div>
        <button type="submit" className="auth-button">Sign In</button>
        <div className="auth-options">
          <Link to="/forgot-password" className="forgot-password">Forgot your password?</Link>
          <p>Don’t have an account? <Link to="/register" className="switch-auth-link">Sign Up</Link></p>
        </div>
      </form>
      
      {showToaster && (
        <Toaster
          message="Login Successful! Redirecting to home..."
          type="success"
          onClose={() => setShowToaster(false)} // Function to hide the toaster
        />
      )}
    </div>
  );
};

export default Login;
