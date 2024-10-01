import React, { useContext } from "react";
import "../assets/css/navbar.css";
import logo from '../assets/imgs/logos/logo-transparent.svg';
import Button from "./commons/Button";
import { Link, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";

const Navbar = () => {
  const { user, logout } = useContext(AuthContext); // Access user and logout information from AuthContext
  const navigate = useNavigate();

  const handleLogout = () => {
    logout(); // Call logout function from context
    navigate("/"); // Redirect to home after logout
  };

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/">
          <img src={logo} alt="JobBuddy Logo" className="logo-img" />
        </Link>
      </div>
      <ul className="navbar-links">
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
        <Link to="/contact">Contact</Link>
      </ul>
      <div className="navbar-icon">
        {user ? (
          // If the user is logged in, show the Logout button
          <Button label={"Logout"} className={"btn-submit"} onClick={handleLogout} />
        ) : (
          // If user is not logged in, show the Login button
          <Link to="/login">
            <Button label={"Login"} className={"btn-submit"} />
          </Link>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
