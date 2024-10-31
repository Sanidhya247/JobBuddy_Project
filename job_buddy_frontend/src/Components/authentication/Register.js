import React, { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthContext from "../../context/AuthContext";
import "./Auth.css";
import { toast } from "react-toastify";

const Register = () => {
  const { register, setError } = useContext(AuthContext);
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    fullName: "",
    email: "",
    password: "",
    confirmPassword: "",
    role: "Job Seeker",
    phoneNumber: "",
  });
  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const validateForm = () => {
    let formErrors = {};

    //validation logic for the registration form
    if (!formData.fullName) formErrors.fullName = "Full name is required.";
    if (!formData.email) {
      formErrors.email = "Email is required.";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      formErrors.email = "Email address is invalid.";
    }
    if (!formData.password) {
      formErrors.password = "Password is required.";
    } else if (formData.password.length < 6) {
      formErrors.password = "Password must be at least 6 characters.";
    } else if (!/[A-Z]/.test(formData.password)) {
      formErrors.password =
        "Password must contain at least one uppercase letter.";
    } else if (!/[0-9]/.test(formData.password)) {
      formErrors.password = "Password must contain at least one number.";
    } else if (!/[!@#$%^&*]/.test(formData.password)) {
      formErrors.password =
        "Password must contain at least one special character.";
    }
    if (formData.password !== formData.confirmPassword) {
      formErrors.confirmPassword = "Passwords do not match.";
    }

    setErrors(formErrors);
    return Object.keys(formErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (validateForm()) {
      try {
        await register(formData);
        toast.success(
          "Registration successful! Please check your email to confirm your account."
        );
        navigate("/verify-email");
      } catch (err) {
        console.log(err?.message);
        const errorMessage = err?.message || "Registration failed.";
        setErrors({ server: errorMessage });
        setError(errorMessage);
        toast.error(errorMessage);
      }
    }
  };

  return (
    <div className="auth-container">
      <h2>Create Account</h2>
      {errors.server && <div className="error">{errors.server}</div>}
      <form onSubmit={handleSubmit} className="auth-form">
        <div className="form-group">
          <label>Role</label>
          <select
            name="role"
            value={formData.role}
            onChange={handleChange}
            className={errors.role ? "error-input" : ""}
          >
            <option name='role' value="Job Seeker">Job Seeker</option>
            <option name='role' value="Employer">Employer</option>
          </select>
          {errors.role && <span className="error-text">{errors.role}</span>}
        </div>
        <div className="form-group">
          <label>Full Name</label>
          <input
            type="text"
            name="fullName"
            value={formData.fullName}
            onChange={handleChange}
            className={errors.fullName ? "error-input" : ""}
          />
          {errors.fullName && (
            <span className="error-text">{errors.fullName}</span>
          )}
        </div>

        <div className="form-group">
          <label>Email</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className={errors.email ? "error-input" : ""}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>

        <div className="form-group">
          <label>Password</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            className={errors.password ? "error-input" : ""}
          />
          {errors.password && (
            <span className="error-text">{errors.password}</span>
          )}
        </div>

        <div className="form-group">
          <label>Confirm Password</label>
          <input
            type="password"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            className={errors.confirmPassword ? "error-input" : ""}
          />
          {errors.confirmPassword && (
            <span className="error-text">{errors.confirmPassword}</span>
          )}
        </div>

        <div className="form-group">
          <label>Phone Number (Optional)</label>
          <input
            type="text"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
          />
        </div>
        <button type="submit" className="auth-button">
          Sign Up
        </button>
        <p>
          Already have an account?{" "}
          <Link to="/login" className="switch-auth-link">
            Log In
          </Link>
        </p>
      </form>
    </div>
  );
};

export default Register;
