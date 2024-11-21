import React, { useContext, useEffect, useState } from "react";
import "../assets/css/navbar.css";
import logo from "../assets/imgs/logos/logo-transparent.svg";
import Button from "./commons/Button";
import { Link, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
// reference : https://www.npmjs.com/package/@fortawesome/free-regular-svg-icons
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from '@fortawesome/free-regular-svg-icons';

const Navbar = () => {
  const { user, logout } = useContext(AuthContext); // Access user and logout information from AuthContext
  const [role, setRole] = useState(null);
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [display , setDisplay] = useState();

  

  useEffect(() => {
    setDisplay(!!user);
  }, [user]);

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link onClick={toggleMenu} to="/">
          <img src={logo} alt="JobBuddy Logo" className="logo-img" />
        </Link>
      </div>

      <div
        className={`hamburger ${isMenuOpen ? "active" : ""}`}
        onClick={toggleMenu}
      >
        <span></span>
        <span></span>
        <span></span>
      </div>
      
      <ul className={`navbar-links ${isMenuOpen ? 'active' : ''}`}>
        <li><Link onClick={toggleMenu} className="nav-link" to="/" aria-label="Home">Home</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/Job">Jobs</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/chat">Messages</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/connections">Connections</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/about">About</Link></li>
        <li><Link onClick={toggleMenu} className="nav-link" to="/contact">Contact</Link></li>
        {user?.role === "Employer" && (
          <li><Link onClick={toggleMenu} className="nav-link" to="/post">Post Job</Link></li>
        )}
        {user?.role === "Admin" && (
          <li><Link onClick={toggleMenu} className="nav-link" to="/admin-dashboard">Admin Dashboard</Link></li>
        )}
      </ul>

      <div className={`navbar-right ${isMenuOpen ? 'active' : ''}`}>
        { user ? (<Link to="/profile">
          <FontAwesomeIcon className="profile-icon" icon={faCircleUser} />
        </Link>) : null}
        {user ? (
          <Button label={"Logout"} className={"btn-submit"} onClick={handleLogout} />
        ) : (
          <Link to="/login">
            <Button label={"Login"} className={"btn-submit"} />
          </Link>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
