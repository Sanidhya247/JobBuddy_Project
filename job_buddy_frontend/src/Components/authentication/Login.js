import React, { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../../context/AuthContext";
import "./Auth.css";
import { toast } from "react-toastify";

const Login = () => {
  const { login, setError } = useContext(AuthContext);
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false); // To toggle password visibility
  const [localError, setLocalError] = useState("");
  const [fieldErrors, setFieldErrors] = useState({});

  // Form submission handler
  const handleSubmit = async (e) => {
    e.preventDefault();

    const errors = validateInputs();
    if (Object.keys(errors).length > 0) {
      setFieldErrors(errors);
      return;
    }

    try {
      await login({ email, password });
      toast.success("Login Successful! Welcome to JobBuddy.");
      setTimeout(() => {
        navigate("/");
      }, 1000);
    } catch (err) {
      const errorMessage = err.message || "Invalid email or password. Please try again.";
      setLocalError(errorMessage);
      setError(errorMessage);
      setFieldErrors({});
      toast.error(errorMessage);
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
          <div className="password-container">
            <input
              type={showPassword ? "text" : "password"}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className={fieldErrors.password ? "error-input" : ""}
            />
            <span
              className="toggle-password"
              onClick={() => setShowPassword(!showPassword)}
              title={showPassword ? "Hide Password" : "Show Password"}
            >
              {showPassword ? "👁️‍🗨️" : "👁️"}
            </span>
          </div>
          {fieldErrors.password && <span className="error-text">{fieldErrors.password}</span>}
        </div>
        <button type="submit" className="auth-button">Sign In</button>
        <div className="auth-options">
          <Link to="/forgot-password" className="forgot-password">Forgot your password?</Link>
          <p>Don’t have an account? <Link to="/register" className="switch-auth-link">Sign Up</Link></p>
        </div>
      </form>
    </div>
  );
};

export default Login;
