import React, { useContext, useEffect, useState } from "react";
import "../assets/css/navbar.css";
import logo from "../assets/imgs/logos/logo-transparent.svg";
import Button from "./commons/Button";
import { Link, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleUser } from "@fortawesome/free-regular-svg-icons";

const Navbar = () => {
  const { user, logout } = useContext(AuthContext);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
    document.body.classList.toggle("navbar-active", !isMenuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/">
          <img src={logo} alt="JobBuddy Logo" className="logo-img" />
        </Link>
      </div>

      <div className="hamburger" onClick={toggleMenu}>
        <span></span>
        <span></span>
        <span></span>
      </div>
      
      <ul className={`navbar-links ${isMenuOpen ? 'active' : ''}`}>
        <li><Link onClick={toggleMenu} className="nav-link" to="/" aria-label="Home">Home</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/Job" aria-label="Jobs">Jobs</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/chat" aria-label="Messages">Messages</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/connections" aria-label="Connections">Connections</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/about" aria-label="About">About</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/contact" aria-label="Contact">Contact</Link></li>
        {user?.role === "Employer" && (
          <li><Link onClick={toggleMenu} className="nav-link" to="/post" aria-label="Post Job">Post Job</Link></li>
        )}
        {user?.role === "Admin" && (
          <li><Link onClick={toggleMenu} className="nav-link" to="/admin-dashboard" aria-label="Admin Dashboard">Admin Dashboard</Link></li>
        )}
      </ul>

      <div className={`navbar-right ${isMenuOpen ? 'active' : ''}`}>
        { user ? (<Link to="/profile" aria-label="View Profile">
          <FontAwesomeIcon className="profile-icon" icon={faCircleUser} />
        </Link>) : null}
        {user ? (
          <Button label="Logout" className="btn-submit" onClick={handleLogout} />
        ) : (
          <Link to="/login" aria-label="Login">
            <Button label={"Login"} className={"btn-submit"} />
          </Link>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
