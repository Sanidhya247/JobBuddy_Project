import React from "react";
import "../assets/css/navbar.css";
import logo from '../assets/imgs/logos/logo-transparent.svg'; 
import Button from "./commons/Button";
import { Link } from "react-router-dom";
// reference : https://www.npmjs.com/package/@fortawesome/free-regular-svg-icons
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from '@fortawesome/free-regular-svg-icons';

const Navbar = () => {
  return (
    <>
      <nav className="navbar">
        <div className="navbar-logo">
          <Link to={"/"}><img src={logo} alt="JobBuddy Logo" className="logo-img" /></Link>
        </div>
        <ul className="navbar-links full-width content-center">
          <Link className="nav-link" to={"/"}>Home</Link>
          <Link className="nav-link" to={"/Job"}>Jobs</Link>
          <Link className="nav-link" to={"/about"}>About</Link>
          <Link className="nav-link" to={"/contact"}>Contact</Link>
          <Link className="nav-link" to={"/post"}>Post Job</Link>
        </ul>
        <div className="navbar-right">
          <Link to={"/profile"}><FontAwesomeIcon className="profile-icon" icon={faCircleUser} /></Link>
          <Link to={"/login"}><Button label={"Login"} className={"btn-submit"} /></Link>
        </div>
      </nav>
    </>
  );
};

export default Navbar;
