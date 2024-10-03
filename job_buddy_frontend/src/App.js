import './App.css';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import "../src/style.css";
import { Routes, Route } from "react-router-dom"; 
import Home from './components/Home';
import Contact from './components/Contact';
import About from './components/About';
import Login from "./components/authentication/Login";
import Register from "./components/authentication/Register";
import PostJob from "./components/PostJob"; 
import Profile from "./components/Profile"; 
import JobSearchPage from "./components/Job_Search_Page";
import VerifyEmail from './components/authentication/VerifyEmail';


function App() {
  return (
    <>
      <Navbar />
      
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/Job" element={<JobSearchPage />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
        <Route path="/post" element={<PostJob />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/register" element={<Register />} />
        <Route path="/verify-email" element={<VerifyEmail />} />
      </Routes>
      <Footer />
    </>
  );
}

export default App;
