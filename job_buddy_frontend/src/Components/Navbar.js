import React, { useContext, useEffect, useState } from "react";
import "../assets/css/navbar.css";
import logo from '../assets/imgs/logos/logo-transparent.svg';
import Button from "./commons/Button";
import { Link } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from '@fortawesome/free-regular-svg-icons';

const Navbar = () => {
  const { user, logout } = useContext(AuthContext); // Directly access `user` context state
  const [display, setDisplay] = useState(!!user);
  //const navigate = useNavigate();

  

  useEffect(() => {
    setDisplay(!!user);
  }, [user]);

  // Handle logout operation and navigate to login page
  const handleLogout = () => {
    logout();
    setDisplay(false);
  };

  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/">
          <img src={logo} alt="JobBuddy Logo" className="logo-img" />
        </Link>
      </div>
      <ul className="navbar-links full-width content-center">
        <Link className="nav-link" to={"/"}>Home</Link>
        <Link className="nav-link" to={"/Job"}>Jobs</Link>
        <Link className="nav-link" to={"/about"}>About</Link>
        <Link className="nav-link" to={"/contact"}>Contact</Link>
        <Link className="nav-link" to={"/post"}>Post Job</Link>
      </ul>
      <div className="navbar-right">
        {/* Show profile icon only if the user is logged in */}
        {display && (
          <Link to={"/profile"}>
            <FontAwesomeIcon className="profile-icon" icon={faCircleUser} />
          </Link>
        )}
        {display ? (
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
