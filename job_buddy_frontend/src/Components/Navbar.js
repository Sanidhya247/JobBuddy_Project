import React from "react";
import "../assets/css/navbar.css";
import logo from '../assets/imgs/logos/logo-transparent.svg'; 
import Button from "./commons/Button";
import { Link } from "react-router-dom";

const Navbar = () => {
  return (
    <>
      <nav className="navbar">
        <div className="navbar-logo">
          <img src={logo} alt="JobBuddy Logo" className="logo-img" />
        </div>
        <ul className="navbar-links">
          <Link to={"/"}>Home</Link>
          <Link to={"/Job"}>Jobs</Link>
          <Link to={"/about"}>About</Link>
          <Link to={"/contact"}>Contact</Link>
          
        </ul>
        <div className="navbar-icon">
          {/* <img
            src="profile-icon.png"
            alt="Profile Icon"
            className="profile-img"
          /> */}
          <Link to={"/login"}><Button label={"Login"} className={"btn-submit"} /></Link>
        </div>
      </nav>
    </>
  );
};

export default Navbar;
